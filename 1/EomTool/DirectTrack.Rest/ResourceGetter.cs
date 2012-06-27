using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using DAgents.Common;
namespace DirectTrack.Rest
{
    public class ResourceGetter
    {
        private int count = 0;
        private ILogger logger = new DAgents.Common.Utilities.NullLogger();
        private XmlSerializer listDeserializer = new XmlSerializer(typeof(resourceList));
        private Queue<Task> tasks = new Queue<Task>();
        private object tasksLocker = new object();
        private List<XDocument> results = new List<XDocument>();
        private object resultsLocker = new object();
        private readonly string rootURL;

        public static int MaxResources { get; set; }

        static ResourceGetter()
        {
            MaxResources = Int32.MaxValue;
        }

        /// <summary>
        /// This constructor takes an event handler and immediately gets the resources.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="handler"></param>
        public ResourceGetter(string url, GotResourceEventHandler handler)
        {
            this.rootURL = url;
            this.GotResource += handler;

            this.GetResources();
        }

        /// <summary>
        /// This constructor takes the logger and the url.  
        /// The client should then call <c>GetResources()</c> after hooking an event to <c>GotResource</c>.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="url"></param>
        public ResourceGetter(ILogger logger, string url)
        {
            this.logger = logger;
            this.rootURL = url;
        }

        /// <summary>
        /// This delegate allows the client to receive the fetched resources.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="url"></param>
        /// <param name="doc"></param>
        public delegate void GotResourceEventHandler(ResourceGetter sender, string url, XDocument doc);

        /// <summary>
        /// This delegate allows the client to receive the fetched resources.
        /// </summary>
        public event GotResourceEventHandler GotResource;
        private void OnGotResource(string url, XDocument doc)
        {
            if (this.GotResource != null)
            {
                this.GotResource(this, url, doc);
            }
        }

        /// <summary>
        /// Begins the process of fetching resources.
        /// </summary>
        /// <returns></returns>
        public List<XDocument> GetResources()
        {
            this.GetResources(rootURL);
            while (this.NumTasks() > 0)
            {
                this.RemoveTask().Wait();
            }
            return results;
        }

        private void GetResources(string url)
        {
            if (++this.count > MaxResources)
            {
                return;
            }

            logger.Log("Getting resources at: " + url);

            AddTask(Task.Factory.StartNew(new Action(() =>
            {
                XDocument fetchedResource = XDocument.Parse(DirectTrackRestCall.GetXml(url));

                if (listDeserializer.CanDeserialize(fetchedResource.CreateReader()))
                {
                    DirectTrack.Rest.resourceList resourceList = (resourceList)listDeserializer.Deserialize(fetchedResource.CreateReader());

                    foreach (DirectTrack.Rest.resourceListResourceURL resourceListResourceURL in resourceList.resourceURL)
                    {
                        this.GetResources(url + resourceListResourceURL.location); // recurse
                    }
                }
                else // since the fetched resource can't be deserialized to a resource list, it is a real resource
                {
                    logger.Log("Got resource at: " + url);

                    AddResrouce(url, fetchedResource);
                }
            })));
        }

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

        private void AddResrouce(string url, XDocument doc)
        {
            lock (this.resultsLocker)
            {
                this.logger.Log("add resource");

                this.results.Add(doc);
                this.OnGotResource(url, doc);
            }
        }
    }
}
