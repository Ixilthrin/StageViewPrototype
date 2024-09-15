using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFPlayground
{
    public class StageControllerService
    {
        private static StageControllerService TheService = new StageControllerService();

        public List<Stage> _stages = new List<Stage>();
        public List<StageController> _controllers = new List<StageController>();
        private StageControllerService()
        {
        }

        public static StageControllerService GetService()
        {
            return TheService;
        }

        public void AddStage(string label)
        {
            Stage stage = new Stage();
            stage.Label = label;
            stage.Result = StageResult.None;
            _stages.Add(stage);
            _controllers.Add(new StageController(stage.Label));
        }

        public Stage GetStage(string label)
        {
            foreach (var stage in _stages)
            {
                if (stage.Label == label)
                {
                    return stage;
                }
            }
            return null;
        }

        public StageController GetStageController(string label)
        {
            foreach (var controller in _controllers)
            {
                if (controller.StageLabel == label)
                {
                    return controller;
                }
            }
            return null;
        }

        public List<Stage> Stages
        {
            get { return _stages; }
        }

        public List<StageController> StageControllers
        {
            get { return _controllers; }
        }
    }
}
