using System;

namespace LineCommander
{
    class TypeParser
    {
        public object Parse(string typeName, string textValue)
        {
            object result;
            switch (typeName)
            {
                case "System.DateTime":
                    result = DateTime.Parse(textValue);
                    break;
                case "System.Int32":
                    result = int.Parse(textValue);
                    break;
                default:
                    throw new Exception("unsupported type " + typeName);
            }
            return result;
        }
    }
}
