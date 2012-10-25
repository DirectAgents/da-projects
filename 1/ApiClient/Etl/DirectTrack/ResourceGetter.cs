using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using ApiClient.Models.DirectTrack;
using System.Threading;

namespace DirectTrack
{
    public class ResourceGetter : IResourceGetter
    {
        private int count = 0;
        private XmlSerializer resourceListDeserializer = new XmlSerializer(typeof(ResourceList));
        private Queue<Task> tasks = new Queue<Task>();
        private string rootURL;
        private IRestCall directTrackRestCall;
        private int limit = int.MaxValue;
        private ResourceGetterMode mode;
        private bool stop = false;
        private ILogger logger;
        private List<XDocument> results = new List<XDocument>();
        private object resultsLocker = new object();
        private object tasksLocker = new object();
        private object errorLocker = new object();

        public ResourceGetter(ILogger logger, string url, IRestCall restCall, ResourceGetterMode mode)
        {
            this.logger = logger;
            this.rootURL = url;
            this.directTrackRestCall = restCall;
            this.mode = mode;
        }

        public ResourceGetter(ILogger logger, string url, IRestCall restCall, int limit, ResourceGetterMode mode)
            : this(logger, url, restCall, mode)
        {
            this.limit = limit;
        }

        public delegate void GotResourceEventHandler(ResourceGetter sender, Uri uri, string url, XDocument doc, bool cached);
        public event GotResourceEventHandler GotResource;
        void OnGotResource(Uri uri, string url, XDocument doc, bool cached)
        {
            if (this.GotResource != null)
                this.GotResource(this, uri, url, doc, cached);
        }

        public delegate void ErrorEventHandler(ResourceGetter sender, Uri uri, Exception ex);
        public event ErrorEventHandler Error;
        void OnError(Uri uri, Exception ex)
        {
            if (this.Error != null)
                this.Error(this, uri, ex);
        }

        public List<XDocument> Run()
        {
            this.DoGetResources(rootURL);
            while (NumTasks > 0)
            {
                Task task;
                lock (tasksLocker)
                    task = tasks.Dequeue();
                task.Wait();
            }
            return this.results;
        }

        void DoGetResources(string url)
        {
            if (!this.stop)
            {
                Task task = Task.Factory.StartNew(new Action(() => DoGetResource(url)));
                lock (tasksLocker)
                    this.tasks.Enqueue(task);
            }
        }

        void DoGetResource(string url)
        {
            Uri uri = new Uri("about:blank");
            try
            {
                bool cached;
                string xml = this.directTrackRestCall.GetXml(url, out uri, out cached);

                if (!cached) // only count non-cached resources toward the limit
                    if (++this.count < this.limit)
                        this.stop = true;
            
                XDocument resource = XDocument.Parse(xml);
                if (this.resourceListDeserializer.CanDeserialize(resource.CreateReader()))
                {
                    ResourceList resourceList = (ResourceList)resourceListDeserializer.Deserialize(resource.CreateReader());
                    if (this.mode == ResourceGetterMode.Resource)
                    {
                        AddResrouce(uri, url, resource, cached);
                        if (resourceList.HasResources)
                        {
                            // recurse for each resource in the list
                            Array.ForEach(resourceList.Resources, c => this.DoGetResources(url + c.Location));
                        }
                    }
                    else // just return the resource list as the resource and dont recurse
                        AddResrouce(uri, url, resource, cached);
                }
                else
                    AddResrouce(uri, url, resource, cached);
            }
            catch (Exception ex)
            {
                lock (this.errorLocker)
                    OnError(uri, ex);
            }
        }

        private void AddResrouce(Uri uri, string url, XDocument doc, bool cached)
        {
            lock (this.resultsLocker)
            {
                this.results.Add(doc);
                this.OnGotResource(uri, url, doc, cached);
            }

            if (!cached)
            {
                using (var db = new DirectTrackDbContext())
                {
                    int points = db.PointsUsedInLastMinutes(1, ApiInfo.LoginAccessId);
                    logger.Log("Points Used: " + points);
                    if (points > PointsThreshold)
                    {
                        logger.Log("Sleeping for a minute..");
                        Thread.Sleep(60 * 1000);
                    }
                }
            }
        }

        private int NumTasks
        {
            get
            {
                lock (tasksLocker)
                {
                    //logger.Log(tasks.Count + " tasks left");
                    return tasks.Count;
                }
            }
        }

        public static int PointsThreshold { get; set; }
    }
}
