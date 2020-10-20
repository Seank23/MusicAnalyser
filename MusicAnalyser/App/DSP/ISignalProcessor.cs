namespace MusicAnalyser.App.DSP
{
    interface ISignalProcessor
    {
        object InputBuffer { get; set; }
        int SampleRate { get; set; }
        object OutputBuffer { get; set; }
        double OutputScale { get; set; }

        void Process();
    }
}
