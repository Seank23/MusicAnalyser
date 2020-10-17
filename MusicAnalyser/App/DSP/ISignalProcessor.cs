using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser.App.DSP
{
    interface ISignalProcessor
    {
        short[] InputBuffer { get; set; }
        int SampleRate { get; set; }
        double[] OutputBuffer { get; set; }
        double OutputScale { get; set; }

        void Process();
    }
}
