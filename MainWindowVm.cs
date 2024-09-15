using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System;
using GalaSoft.MvvmLight;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Collections.Generic;

namespace WPFPlayground
{
    class MainWindowVm : ViewModelBase
    {
        DispatcherTimer _updateTimer;
        DispatcherTimer _pulseTimer;
        Random _rnd = new Random();
        Canvas _stagesCanvas = new Canvas();
        bool _pulse = false;
        public MainWindowVm()
        {
            StageControllerService.GetService().AddStage("Connect");
            StageControllerService.GetService().AddStage("Stop Program");
            StageControllerService.GetService().AddStage("Build Program");
            StageControllerService.GetService().AddStage("Build VTLP");
            StageControllerService.GetService().AddStage("Get VTLP URL");
            StageControllerService.GetService().AddStage("Reset NVRAM");
            StageControllerService.GetService().AddStage("Upload Program");
            StageControllerService.GetService().AddStage("Upload ECW");
            StageControllerService.GetService().AddStage("Start Program");
            StageControllerService.GetService().AddStage("Disconnect");

            var controllers = StageControllerService.GetService().StageControllers;
            foreach (var controller in controllers)
            {
                controller.TimeChanged += TimeChangedEvent;
            }

            _updateTimer = new DispatcherTimer();
            _updateTimer.Tick += OnUpdate;
            _updateTimer.Interval = TimeSpan.FromSeconds(0.04);
            _updateTimer.Start();

            _pulseTimer = new DispatcherTimer();
            _pulseTimer.Tick += OnUpdatePulse;
            _pulseTimer.Interval = TimeSpan.FromSeconds(0.5);
            _pulseTimer.Start();

            // This builder simulates stage events running normally within the app
            FakeBuilder builder = new FakeBuilder();
            builder.Run();

            builder.BuildStateChanged += BuildEvent;
        }

        private void TimeChangedEvent(string stageLabel, TimeSpan timespan)
        {
            var stage = StageControllerService.GetService().GetStage(stageLabel);
            stage.Time = timespan.Seconds.ToString() + "s";
        }

        public void BuildEvent(string stageLabel, StageResult result, bool isRunning)
        {
            Stage stage = StageControllerService.GetService().GetStage(stageLabel);
            if (stage != null)
            {
                stage.Result = result;
                stage.IsRunning = isRunning;
            }

            // Restart the interval for the pulse timer
            _pulseTimer.Stop();
            _pulseTimer.Start();
        }

        public Canvas StagesCanvas
        {
            get
            {
                return _stagesCanvas;
            }
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            List<Stage> stages = StageControllerService.GetService().Stages;
            var sceneDrawer = new StagesViewDrawer(stages);
            sceneDrawer.Draw(_stagesCanvas, _pulse);
            RaisePropertyChanged(nameof(StagesCanvas));
        }

        private void OnUpdatePulse(object sender, EventArgs e)
        {
            _pulse = !_pulse;
        }

    }
}