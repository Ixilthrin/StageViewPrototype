using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WPFPlayground
{
    public class StageController
    {
        public delegate void NotifyTimeChanged(string stageLabel, TimeSpan timeSpan);
        public event NotifyTimeChanged TimeChanged;

        DispatcherTimer _updateTimeTimer;

        private DateTime _startTime;
        private DateTime _stopTime;
        private TimeSpan _totalTime;

        public StageController(string stageLabel)
        {
            _startTime = new DateTime();
            _stopTime = new DateTime();
            _totalTime = new TimeSpan();
            StageLabel = stageLabel;

            _updateTimeTimer = new DispatcherTimer();
            _updateTimeTimer.Tick += OnUpdateTime;
            _updateTimeTimer.Interval = TimeSpan.FromSeconds(0.5);
        }
        private void OnUpdateTime(object sender, EventArgs e)
        {
            if (TimeChanged != null)
            {
                TimeChanged(StageLabel, DateTime.Now - _startTime);
            }
        }

        public string StageLabel { get; set; }

        public bool IsRunning { get; set; }

        public TimeSpan TotalTime
        {
            get
            {
                return _totalTime;
            }
        }
        public void StartOrContinue()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                _startTime = DateTime.Now;
                _updateTimeTimer.Start();
            }
            else
            {
                _totalTime = DateTime.Now - _startTime;
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _stopTime = DateTime.Now;
                _totalTime = _stopTime - _startTime;
                _updateTimeTimer.Stop();
            }
        }
    }
}
