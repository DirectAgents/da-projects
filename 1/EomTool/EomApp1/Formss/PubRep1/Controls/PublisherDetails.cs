//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//namespace EomApp1.Formss.PubRep1.Controls
//{
//    public partial class PublisherDetails : UserControl
//    {
//        IContainer _components = new Container();
//        MyDataGridView _grid;
//        //MyTableAdapter _adapter;
//        BindingSource _source;

//        public PublisherDetails()
//        {
//            InitializeComponent();
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (_components != null))
//            {
//                _components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private void InitializeComponent()
//        {
//            var factory = new Factory();
//            _grid = factory.CreateDataGridView();
//            // create table adapter
//            _source = factory.CreateBindingSource(_components);
//            // create dataset
//            factory.BeginInit();
//            SuspendLayout();
//            _grid.Setup();
//            factory.EndInit();
//            ResumeLayout();
//            AutoScaleMode = AutoScaleMode.Font;
//        }
//    }
//    //class MyTableAdapter : 
//    class MyDataGridView : DataGridView
//    {
//        public void Setup()
//        {
//            AllowUserToAddRows = false;
//            AllowUserToDeleteRows = false;
//            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            DefaultCellStyle = Factory.CreateDataGridViewStyle();
//            Dock = System.Windows.Forms.DockStyle.Fill;
//            Location = new System.Drawing.Point(0, 0);
//            Name = "_gridView";
//            ReadOnly = true;
//            Size = new System.Drawing.Size(885, 217);
//            TabIndex = 0;
//        }
//    }
//    class Factory
//    {
//        List<object> _objects = new List<object>();
//        IEnumerable<ISupportInitialize> _objectsSupportingInitialize  =
//                from c in _objects
//                where c is ISupportInitialize
//                select c as ISupportInitialize;
//        public MyDataGridView CreateDataGridView()
//        {
//            var dataGridView = new MyDataGridView();
//            _objects.Add(dataGridView);
//            return dataGridView;
//        }
//        public static DataGridViewCellStyle CreateDataGridViewStyle()
//        {
//            var dataGridViewCellStyle = new DataGridViewCellStyle();
//            dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
//            dataGridViewCellStyle.BackColor = SystemColors.Window;
//            dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
//            dataGridViewCellStyle.ForeColor = SystemColors.ControlText;
//            dataGridViewCellStyle.Format = "N2";
//            dataGridViewCellStyle.NullValue = null;
//            dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
//            dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
//            dataGridViewCellStyle.WrapMode = DataGridViewTriState.False;
//            return dataGridViewCellStyle;
//        }
//        public BindingSource CreateBindingSource(IContainer components)
//        {
//            var bindingSource = new BindingSource(components);
//            _objects.Add(bindingSource);
//            return bindingSource;
//        }
//        public void BeginInit()
//        {
//            _objectsSupportingInitialize.ToList().ForEach(c => c.BeginInit());
//        }
//        public void EndInit()
//        {
//            _objectsSupportingInitialize.ToList().ForEach(c => c.EndInit());
//        }
//    }
//}
