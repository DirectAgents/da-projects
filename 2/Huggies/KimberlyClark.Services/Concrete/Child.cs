using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using KimberlyClark.Services.Abstract;

namespace KimberlyClark.Services.Concrete
{
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.datacontract.org/2004/07/CoRegistrationRestService")]
    [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/CoRegistrationRestService", IsNullable = true)]
    public class Child : IChild
    {
        private string birthDateField;
        private string genderField;

        [XmlElement(IsNullable = true, Order = 0)]
        public string BirthDate
        {
            get { return this.birthDateField; }
            set { this.birthDateField = value; }
        }

        [XmlElement(IsNullable = true, Order = 1)]
        public string Gender
        {
            get { return this.genderField; }
            set { this.genderField = value; }
        }
    }
}