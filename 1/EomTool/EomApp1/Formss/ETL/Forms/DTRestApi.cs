using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectTrack.Rest;
using System.IO;
using System.Xml.Xsl;
using EomApp1.Formss.ETL.Controls;

namespace EomApp1.Formss.ETL.Forms
{
    public partial class DTRestApi : Form
    {
        private string _xmlData;

        public DTRestApi()
        {
            InitializeComponent();
        }

        static string Transform()
        {
            XslCompiledTransform myXslTransform;
            myXslTransform = new XslCompiledTransform();
            myXslTransform.Load(@"Formss\ETL\Xsl\showxml.xslt");
            myXslTransform.Transform("scratch1.txt", "transform.xml");
            return File.ReadAllText("transform.xml");
        }

        private void editableDataTable1_GoPressed(object sender, EventArgs e)
        {
            var ev = (EditableDataTable.GoPressedEventArgs)e;
            LoadXml(ev.Row.Url, ev.Row.Element);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadXml(textBox1.Text, textBox2.Text);
        }

        private void LoadXml(string url, string element)
        {
            _xmlData = XmlGetter.GetXml(url);
            File.WriteAllText("scratch1.txt", _xmlData, Encoding.Unicode);
            webBrowser1.DocumentText = Transform();


            //dataGridView1.DataSource = new BindingSource()
            //dataGridView1.AutoGenerateColumns = true;
            ////dataSet1.Reset();
            //dataSet1.ReadXml("scratch1.txt");
            //bindingSource1.DataMember = element;
            ////bindingSource1.ResetBindings(true);
            //dataGridView1.Columns.Insert(0, new DataGridViewCheckBoxColumn
            //{
            //    HeaderText = "Select",
            //    Name = "SelectCol",
            //    HeaderCell = new DataGridViewColumnHeaderCell { Value = "Select" }
            //});
        }

    }
    
}
