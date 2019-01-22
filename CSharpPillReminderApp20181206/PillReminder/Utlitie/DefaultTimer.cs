using PillReminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PillReminder.Utlitie
{
    public class DefaultTimer : ITimer
    {
        Timer timer;

        public DefaultTimer()
        {
            timer = new Timer();
        }
        public DefaultTimer(double interval)
        {
            timer = new Timer(interval);
        }
        
        private bool auutoReset;
        public bool AutoReset
        {
            get { return timer.AutoReset; }
            set { timer.AutoReset = value; }
        }


        public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Interval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event ElapsedEventHandler Elapsed;

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
