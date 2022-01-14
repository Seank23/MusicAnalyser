using MusicAnalyser.App.DSP;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using VarispeedDemo.SoundTouch;

namespace MusicAnalyser.App
{
    public class AudioSource : IDisposable
    {
        public string Filename { get; set; }
        private WaveStream audio;
        private WaveStream audioAnalysis;
        private WaveStream audioGraph;
        private WaveChannel32 audioStream;
        public VarispeedSampleProvider SpeedControl { set; get; }
        public FilterWaveProvider FilteredSource { get; set; }
        public SmbPitchShiftingSampleProvider PitchControl { get; set; }

        public WaveStream Audio
        {
            get { return audio; }
            set
            {
                audio = value;
                audioStream = new WaveChannel32(value);
                SpeedControl = new VarispeedSampleProvider(new WaveToSampleProvider(audioStream), 10, new SoundTouchProfile(true, false));
                PitchControl = new SmbPitchShiftingSampleProvider(SpeedControl);
                FilteredSource = new FilterWaveProvider(PitchControl);
            }
        }

        public WaveStream AudioGraph
        {
            get { return audioGraph; }
            set { audioGraph = value; }
        }

        public WaveChannel32 AudioStream
        {
            get { return audioStream; }
            set { audioStream = value; }
        }

        public WaveStream AudioAnalysis
        {
            get { return audioAnalysis; }
            set { audioAnalysis = value; }
        }

        public void Dispose()
        {
            FilteredSource.Dispose();
            FilteredSource = null;
            SpeedControl.Dispose();
            SpeedControl = null;
            audio.Dispose();
            audio = null;
            audioStream.Dispose();
            audioStream = null;
            audioGraph.Dispose();
            audioGraph = null;
            audioAnalysis.Dispose();
            audioAnalysis = null;
            GC.Collect();
        }
    }
}
