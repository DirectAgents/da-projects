using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace EomApp1.UI
{
    public class FormSerializer
    {
        public static void Serialize(Form f, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(FormProps));
            using (Stream s = File.Create(path))
            {
                try
                {
                    FormProps props = new FormProps();
                    props.x = f.Location.X;
                    props.y = f.Location.Y;
                    props.ht = f.Size.Height;
                    props.wd = f.Size.Width;
                    xs.Serialize(s, props);
                }
                catch
                {
                }
            }
        }
        public static void Deserialize(Form f, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(FormProps));
            if (File.Exists(path))
            {
                using (Stream s = File.OpenRead(path))
                {
                    try
                    {
                        FormProps props = (FormProps)xs.Deserialize(s);
                        f.Location = new Point(props.x, props.y);
                        f.Size = new Size(props.wd, props.ht);
                    }
                    catch
                    {
                    }
                }
            }
        }
        public class FormProps
        {
            public int x;
            public int y;
            public int ht;
            public int wd;
        }
    }
}