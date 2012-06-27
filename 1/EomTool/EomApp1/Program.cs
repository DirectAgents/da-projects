using System;
using System.Windows.Forms;
using DirectTrack.Rest;
using DAgents.Common;
//using Microsoft.Practices.EnterpriseLibrary.Logging.Database;
//using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
//using Microsoft.Practices.Unity;
//using Microsoft.Practices.EnterpriseLibrary.Data;
namespace EomApp1
{
    //class A
    //{
    //    public virtual void F() { }
    //}
    //class B : A
    //{
    //    public void F() { }
    //}
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                DAgents.Common.Properties.Settings.Default.ConnStr =
                    Properties.Settings.Default.DADatabaseR1ConnectionString;

                //Application.Run(new Formss.Campaign2.Campaigns2Form());
                Application.Run(new EOMForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private static void ProcessCommandLine(string[] args)
        {
            try
            {
                ResourceGetter.MaxResources = 5;
                var logger = new ConsoleLogger();
                var a = new ResourceGetter(logger, args[0]);
                a.GetResources();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.InnerException);
                Console.WriteLine(e.StackTrace);
                //Console.WriteLine(e.Message);
            }
            //a.GotResource += new ResourceGetter.GotResourceEventHandler(a_GotResource);
        }

        static void a_GotResource(ResourceGetter sender, string url, System.Xml.Linq.XDocument doc)
        {

        }

        //private static void Foo2()
        //{
        //    //IUnityContainer myContainer = new UnityContainer()
        //        //.Register

        //}
        //#region custom IList
        //class MyIList : IList
        //{
        //    #region ILiForm1.csst Members

        //    int IList.Add(object value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    void IList.Clear()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    bool IList.Contains(object value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    int IList.IndexOf(object value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    void IList.Insert(int index, object value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    bool IList.IsFixedSize
        //    {
        //        get { throw new NotImplementedException(); }
        //    }

        //    bool IList.IsReadOnly
        //    {
        //        get { return true; }
        //    }

        //    void IList.Remove(object value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    void IList.RemoveAt(int index)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    object IList.this[int index]
        //    {
        //        get
        //        {
        //            return "fooo";
        //        }
        //        set
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }

        //    #endregion

        //    #region ICollection Members

        //    void ICollection.CopyTo(Array array, int index)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    int ICollection.Count
        //    {
        //        get { return 2; }
        //    }

        //    bool ICollection.IsSynchronized
        //    {
        //        get { throw new NotImplementedException(); }
        //    }

        //    object ICollection.SyncRoot
        //    {
        //        get { throw new NotImplementedException(); }
        //    }

        //    #endregion

        //    #region IEnumerable Members

        //    IEnumerator IEnumerable.GetEnumerator()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    #endregion
        //}
        //#endregion

        //private static void Foo()
        //{
        //    var dgv = new DataGridView
        //    {
        //        Dock = DockStyle.Fill,
        //        //DataSource = new MyIList(),
        //        //DataSource = new List<string>
        //        //{
        //        //    "foo"
        //        //},
        //        DataSource = new MyListSource(),
        //    };

        //    #region Buttons
        //    var ok = new Button
        //    {
        //        Text = "OK",
        //        //Location = new Point(originX, originY),
        //        Dock = DockStyle.Left,
        //    };
        //    var cancel = new Button
        //    {
        //        Text = "Cancel",
        //        //Location = new Point(ok.Left + ok.Width + horizSpace, ok.Top), // 10 is horiz space between buttons
        //        Dock = DockStyle.Left,

        //    };
        //    cancel.Paint += (s, e) =>
        //    {
        //        var gp = new GraphicsPath();
        //        Rectangle r = cancel.ClientRectangle;
        //        //r.Inflate(-10, -10);
        //        //e.Graphics.DrawEllipse(Pens.Black, r);
        //        //r.Inflate(1, 1);
        //        gp.AddEllipse(r);
        //        cancel.Region = new Region(gp);
        //    };
        //    #endregion

        //    var f = new Form
        //    {
        //        Text = "Hello",
        //        HelpButton = true,
        //        FormBorderStyle = FormBorderStyle.Sizable,
        //        AcceptButton = ok,
        //    };
        //    f.Controls.AddRange(new[] { 
        //        //ok,
        //        //cancel, 
        //        dgv
        //    });
        //    f.ShowDialog();
        //}
    }
}
