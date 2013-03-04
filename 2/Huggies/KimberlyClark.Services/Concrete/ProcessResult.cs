using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using KimberlyClark.Services.Abstract;
using System.Diagnostics;
using System.Xml.Serialization;

namespace KimberlyClark.Services.Concrete
{
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
#if DEBUG
    [XmlType(Namespace = "")]
    [XmlRoot(Namespace = "", IsNullable = true)]
#else
    [XmlType(Namespace = "")]
    [XmlRoot(Namespace = "", IsNullable = true)]
    //[XmlType(Namespace = "http://schemas.datacontract.org/2004/07/CoRegistrationRestService")]
    //[XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/CoRegistrationRestService", IsNullable = true)]
#endif
    public class ProcessResult : IProcessResult
    {
        private string errorField;
        private bool isSuccessField;
        private bool isSuccessFieldSpecified;
        private string processOutPutField;
 
        [XmlElement(IsNullable = true, Order = 0)]
        public string Error
        {
            get { return this.errorField; }
            set { this.errorField = value; }
        }
      
        [XmlElement(Order = 1)]
        public bool IsSuccess
        {
            get { return this.isSuccessField; }
            set { this.isSuccessField = value; }
        }
        
        [XmlIgnore]
        public bool IsSuccessSpecified
        {
            get { return this.isSuccessFieldSpecified; }
            set { this.isSuccessFieldSpecified = value; }
        }
        
        [XmlElement(IsNullable = true, Order = 2)]
        public string ProcessOutPut
        {
            get { return this.processOutPutField; }
            set { this.processOutPutField = value; }
        }

        public override string ToString()
        {
            return Utilities.ToXmlString(this);
        }
    }
}