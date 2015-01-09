using System;
using System.Linq;
using RestSharp;

namespace AdRoll.Clients
{
    public class ApiRequest
    {
        /// <summary>
        /// Add a parameter with the name and value of each public property of the instance.
        /// </summary>
        public virtual void AddParametersTo(RestRequest restRequest)
        {
            var propertiesWithValue = from property in GetType().GetProperties()
                                      let value = property.GetValue(this)
                                      where value != null
                                      select Tuple.Create(property.Name, value.ToString());

            foreach (var tuple in propertiesWithValue)
                restRequest.AddParameter(tuple.Item1, tuple.Item2);
        }
    }
}
