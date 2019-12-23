using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VarispeedDemo.SoundTouch;

namespace MusicAnalyser
{
    public class AudioSource : IDisposable
    {
        private WaveStream audio;
        private WaveStream audioFFT;
        private WaveStream audioGraph;
        private WaveChannel32 audioStream;
        public VarispeedSampleProvider SpeedControl { set; get; }

        public WaveStream Audio
        {
            get { return audio; }
            set
            {
                audio = value;
                audioStream = new WaveChannel32(value);
                SpeedControl = new VarispeedSampleProvider(new WaveToSampleProvider(audioStream), 10, new SoundTouchProfile(true, false));
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

        public WaveStream AudioFFT
        {
            get { return audioFFT; }
            set { audioFFT = value; }
        }

        public void Dispose()
        {
            SpeedControl.Dispose();
            SpeedControl = null;
            audio.Dispose();
            audio = null;
            audioStream.Dispose();
            audioStream = null;
            audioGraph.Dispose();
            audioGraph = null;
            audioFFT.Dispose();
            audioFFT = null;
            GC.Collect();
        }
    }
}
