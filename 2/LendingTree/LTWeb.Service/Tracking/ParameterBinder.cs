using RestSharp;

namespace LTWeb.Service.Tracking
{
    public class ParameterBinder
    {
        public void BindParametersToRequest<T>(T parameters, RestRequest request)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var propertyCustomAttributes = property.GetCustomAttributes(typeof(UrlParamAttribute), false);
                if (propertyCustomAttributes.Length > 0)
                {
                    string parameterName = ((UrlParamAttribute)propertyCustomAttributes[0]).ParamName;
                    object propertyValue = property.GetValue(parameters);
                    request.AddParameter(new Parameter
                    {
                        Name = parameterName,
                        Type = ParameterType.GetOrPost,
                        Value = propertyValue
                    });
                }
            }
        }
    }
}
