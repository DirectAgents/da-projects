using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DirectTrack
{
    public class ResourceGetter : IResourceGetter
    {
        private int count = 0;
        private XmlSerializer resourceListDeserializer = new XmlSerializer(typeof(ResourceList));
        private Queue<Task> tasks = new Queue<Task>();
        private string rootURL;
        private object errorLocker = new object();
        private IRestCall directTrackRestCall;
        private int maxResources = int.MaxValue;

        public ResourceGetter(ILogger logger, string url, IRestCall restCall)
        {
            this.logger = logger;
            this.rootURL = url;
            this.directTrackRestCall = restCall;
        }

        public ResourceGetter(ILogger logger, string url, IRestCall restCall, int maxResources)
            : this(logger, url, restCall)
        {
            this.maxResources = maxResources;
        }

        #region Events

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="url"></param>
        /// <param name="doc"></param>
        public delegate void GotResourceEventHandler(ResourceGetter sender, Uri uri, string url, XDocument doc);

        /// <summary>
        /// </summary>
        public event GotResourceEventHandler GotResource;

        private void OnGotResource(Uri uri, string url, XDocument doc)
        {
            if (this.GotResource != null)
            {
                this.GotResource(this, uri, url, doc);
            }
        }

        /// <summary>
        /// Event deletegate for <c>Error</c>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="url"></param>
        /// <param name="doc"></param>
        public delegate void ErrorEventHandler(ResourceGetter sender, Uri uri, Exception ex);

        /// <summary>
        /// This event allows a subscriber to receive errors.
        /// </summary>
        public event ErrorEventHandler Error;

        private void OnError(Uri uri, Exception ex)
        {
            if (this.Error != null)
            {
                this.Error(this, uri, ex);
            }
        }

        #endregion

        #region Resource Retrieval

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public List<XDocument> GetResources()
        {
            this.GetResources(rootURL);

            while (this.NumTasks() > 0)
            {
                this.RemoveTask().Wait();
            }

            return this.results;
        }

        private void GetResources(string url)
        {
            if (++this.count < this.maxResources)
            {
                AddTask(Task.Factory.StartNew(new Action(() => GetResource(url))));
            }
        }

        private void GetResource(string url)
        {
            Uri uri = new Uri("about:blank");
            try
            {
                string xml = this.directTrackRestCall.GetXml(url, out uri);

                XDocument resource = XDocument.Parse(xml);

                if (this.resourceListDeserializer.CanDeserialize(resource.CreateReader()))
                {
                    ResourceList resourceList = (ResourceList)resourceListDeserializer.Deserialize(resource.CreateReader());

                    if (resourceList.HasResources)
                    {
                        // loop over each resource in the list
                        foreach (Resource resourceListResourceURL in resourceList.Resources)
                        {
                            this.GetResources(url + resourceListResourceURL.Location); // recurse
                        }
                    }
                    else
                    {
                        logger.Log("Empty resource list at: " + url);
                    }
                }
                else
                {
                    AddResrouce(uri, url, resource);
                }
            }
            catch (Exception ex)
            {
                lock (this.errorLocker)
                {
                    OnError(uri, ex);
                }
            }
        }

        #endregion

        #region Tasks

        private object tasksLocker = new object();

        private int NumTasks()
        {
            lock (tasksLocker)
            {
                logger.Log(tasks.Count + " tasks left");
                return tasks.Count;
            }
        }

        private Task RemoveTask()
        {
            lock (tasksLocker)
            {
                return tasks.Dequeue();
            }
        }

        private void AddTask(Task task)
        {
            lock (tasksLocker)
            {
                this.tasks.Enqueue(task);
            }
        }

        #endregion

        #region Results

        private List<XDocument> results = new List<XDocument>();

        private object resultsLocker = new object();

        private void AddResrouce(Uri uri, string url, XDocument doc)
        {
            lock (this.resultsLocker)
            {
                this.results.Add(doc);
                this.OnGotResource(uri, url, doc);
            }
        }

        #endregion

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }
        private ILogger logger = new ConsoleLogger();

        public IRestCall DirectTrackRestCall
        {
            get { return directTrackRestCall; }
            set { directTrackRestCall = value; }
        }
    }
}
