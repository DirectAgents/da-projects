using System;
using System.Windows.Forms;

namespace EomAppControls.Filtering
{
    public class RadioButtonPanel<T> : FlowLayoutPanel
    {
        public RadioButtonPanel()
        {
            var graphics = this.CreateGraphics();
            var zeroPadding = new Padding(0);
            var initialPadding = new Padding(10, 0, 0, 0);
            int radioButtonCircleWidth = 30;
            graphics.MeasureString("a", RadioButton.DefaultFont);
            bool first = true;
            foreach (object value in Enum.GetValues(typeof(T)))
            {
                string name = Enum.GetName(typeof(T), value);
                var button = new RadioButton
                { 
                    Text = name, 
                    Checked = first, 
                    Padding = zeroPadding, 
                    Margin = first ? initialPadding : zeroPadding,
                    Width = (int)graphics.MeasureString(name, RadioButton.DefaultFont).Width + radioButtonCircleWidth
                };
                first = false;
                button.CheckedChanged += (s, e) =>
                {
                    if (button.Checked && this.Selected != null)
                    {
                        T current = (T)Enum.Parse(typeof(T), name);
                        this.Current = current;
                        Selected(current);
                    }
                };
                this.Controls.Add(button);
            }
            this.Current = (T)(object)0;
            this.Padding = zeroPadding;
            this.Margin = zeroPadding;
        }
        public event SelectedEvent Selected;
        public delegate void SelectedEvent(T t);
        public T Current { get; set; }
    }
}
