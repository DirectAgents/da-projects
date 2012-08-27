using System.Collections.Generic;
namespace DAgents.Common
{
    public interface ITransformText
    {
        string TransformText();
        Dictionary<string, object> Data { get; }
    }
}
