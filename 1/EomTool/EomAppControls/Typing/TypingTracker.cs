using System;
using System.ComponentModel;
using System.Windows.Forms;
using EomApp1.Events;

namespace EomApp1.Components
{
    public partial class TypingTracker : Component
    {
        public event EventHandler StartedTracking;
        public event EventHandler Tracking;
        public event EventHandler StoppedTracking;

        enum TypingState
        {
            Idle,
            Gathering
        }

        Timer _timer;
        TypingState _state;
        DateTime _lastTracked = DateTime.Now;
        string _tracked = string.Empty;

        public TypingTracker()
            : base()
        {
            InitializeComponent();
        }

        public TypingTracker(IContainer container)
            : this()
        {
            container.Add(this);
        }

        void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _timer = new System.Windows.Forms.Timer(components);
            _timer.Tick += new System.EventHandler(timer_Tick);
        }

        public void BindControl(Control source)
        {
            source.KeyPress += new KeyPressEventHandler((s, e) =>
            {
                switch (_state)
                {
                    case TypingState.Idle:
                        StartTracking();
                        Track(e.KeyChar);
                        break;
                    case TypingState.Gathering:
                        Track(e.KeyChar);
                        break;
                    default:
                        break;
                }
            });
        }

        void Track(char p)
        {
            _lastTracked = DateTime.Now;
            _tracked += p;
        }

        private void StartTracking()
        {
            OnStartedTracking();
            _state = TypingState.Gathering;
            _timer.Interval = 350;
            _timer.Start();
        }

        void StopTracking()
        {
            OnStoppedTracking();
            _state = TypingState.Idle;
            _tracked = string.Empty;
            _timer.Stop();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (_state == TypingState.Gathering)
            {
                if (TimeSinceLastTrack > Timeout)
                    StopTracking();
                else
                    OnTracking(_tracked);
            }
        }

        private static TimeSpan Timeout
        {
            get
            {
                return TimeSpan.FromSeconds(1);
            }
        }

        TimeSpan TimeSinceLastTrack
        {
            get
            {
                return DateTime.Now - _lastTracked;
            }
        }

        protected virtual void OnStartedTracking()
        {
            if (StartedTracking != null) StartedTracking(this, EventArgs.Empty);
        }

        protected virtual void OnStoppedTracking()
        {
            if (StoppedTracking != null) StoppedTracking(this, EventArgs.Empty);
        }

        protected virtual void OnTracking(string tracked)
        {
            if (Tracking != null) Tracking(this, new TextEventArgs(tracked));
        }

        IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
