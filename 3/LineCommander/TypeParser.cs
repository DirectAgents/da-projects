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
                case "System.String":
                    result = textValue;
                    break;
                case "System.Char":
                    result = char.Parse(textValue);
                    break;
                case "System.Boolean":
                    result = bool.Parse(textValue);
                    break;
                default:
                    throw new Exception("TypeParser: unsupported type " + typeName);
            }
            return result;
        }
    }
}
