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
            timer.Elapsed += this.Elapsed; 
        }
        public DefaultTimer(double interval)
        {
            timer = new Timer(interval);
            timer.Elapsed += this.Elapsed;
        }
        
        
        public bool AutoReset
        {
            get { return timer.AutoReset; }
            set { timer.AutoReset = value; }
        }


        public bool Enabled
        {
            get { return timer.Enabled; }
            set { timer.Enabled = value; }
        }
        public double Interval
        {
            get { return timer.Interval; }
            set { timer.Interval = value;  }

        }

        public event ElapsedEventHandler Elapsed;

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
