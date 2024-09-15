using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFPlayground
{
    public class StagesViewDrawer
    {
        private List<Stage> _stages = new List<Stage>();

        public StagesViewDrawer(List<Stage> stages)
        {
            _stages = stages;
        }

        public void Draw(Canvas canvas, bool pulse)
        {
            int x = 10;
            int y = 10;

            canvas.Children.Clear();
            foreach (var stage in _stages)
            {
                var drawer = new StageDrawer(stage);
                drawer.Width = 100;
                drawer.Height = 80;
                drawer.Draw(canvas, x, y, pulse);
                x += drawer.Width + 5;
            }
        }
    }
}
