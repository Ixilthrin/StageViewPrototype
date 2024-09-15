using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFPlayground
{
    public enum StageResult
    {
        None,
        Error,
        Warning,
        AllGood
    }

    public class Stage
    {
        public string Label { get; set; } = string.Empty;
        public StageResult Result { get; set; }
        public bool IsRunning { get; set; }
        public string Time { get; set; } = "---";
    }

}
