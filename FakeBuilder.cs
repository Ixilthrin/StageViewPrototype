using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WPFPlayground
{
    public class FakeBuilder
    {
        public delegate void NotifyBuildStageStateChanged(string eventLabel, StageResult result, bool isRunning);
        public event NotifyBuildStageStateChanged BuildStateChanged;

        DispatcherTimer _updateTimer;

        int frameNumber = 0;
        Random _rnd = new Random();

        List<int> stageStartFrames = new List<int>();

        List<StageResult> stageResults = new List<StageResult>();

        StageControllerService service = StageControllerService.GetService();

        int lastFrame;

        public FakeBuilder()
        {
            int startFrame = _rnd.Next(5) + 2;
            foreach (var stage in service.Stages)
            {
                stageStartFrames.Add(startFrame);
                startFrame += _rnd.Next(20) + 5;

                stageResults.Add((StageResult)(_rnd.Next(3) + 1));
            }
            lastFrame = startFrame;
        }

        public void Run()
        {
            _updateTimer = new DispatcherTimer();
            _updateTimer.Tick += OnUpdate;
            _updateTimer.Interval = TimeSpan.FromSeconds(0.5);
            _updateTimer.Start();
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            ++frameNumber;

            if (frameNumber > lastFrame)
            {
                _updateTimer.Stop();
                return;
            }

            if (BuildStateChanged == null)
            {
                return;
            }

            if (frameNumber < stageStartFrames[0])
            {
                return;
            }

            TimeSpan totalTime = new TimeSpan();

            for (int i = 0; i < stageStartFrames.Count; ++i)
            {
                int lowerFrameNumber = stageStartFrames[i];
                int upperFrameNumber = i + 1 == stageStartFrames.Count ? lastFrame : stageStartFrames[i + 1];
                if (frameNumber >= lowerFrameNumber && frameNumber < upperFrameNumber)
                {
                    if (i > 0)
                    {
                        var controller = service.StageControllers[i - 1];
                        if (controller.IsRunning)
                        {
                            controller.Stop();
                            totalTime = controller.TotalTime;
                            BuildStateChanged(controller.StageLabel, stageResults[i - 1], false);
                        }
                    }

                    var nextController = service.StageControllers[i];
                    nextController.StartOrContinue();
                    totalTime = nextController.TotalTime;
                    BuildStateChanged(nextController.StageLabel, StageResult.None, true);
                    return;
                }
            }

            if (frameNumber >= lastFrame)
            {
                var lastController = service.StageControllers[service.Stages.Count - 1];
                lastController.Stop();
                totalTime = lastController.TotalTime;
                BuildStateChanged(lastController.StageLabel, stageResults[service.Stages.Count - 1], false);
                return;
            }
        }
    }
}
