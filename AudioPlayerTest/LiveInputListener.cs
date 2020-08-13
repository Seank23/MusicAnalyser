using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicAnalyser
{
    class LiveInputListener
    {
        private int sampleRate;
        private int bufferSize;
        public bool Listening { get; set; }

        public RollingBufferWaveProvider bufferedProvider;
        private WaveIn wi;

        public LiveInputListener(int sampleRate = 8000, int bufferSize = 2048)
        {
            this.sampleRate = sampleRate;
            this.bufferSize = bufferSize;
            Listening = false;
        }

        public int getBufferSize() { return bufferSize; }
        public int getSampleRate() { return sampleRate; }

        void AudioDataAvailable(object sender, WaveInEventArgs e)
        {
            bufferedProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        public void StopListening()
        {
            wi.StopRecording();
            Listening = false;
        }

        public bool StartListening(int audioDeviceNumber = 0)
        {
            wi = new WaveIn();
            wi.DeviceNumber = audioDeviceNumber;
            wi.WaveFormat = new WaveFormat(sampleRate, 1);
            wi.BufferMilliseconds = (int)((double)128 / (double)sampleRate * 1000.0);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(AudioDataAvailable);
            bufferedProvider = new RollingBufferWaveProvider(wi.WaveFormat);
            bufferedProvider.BufferLength = bufferSize;
            bufferedProvider.DiscardOnBufferOverflow = true;
            try
            {
                wi.StartRecording();
                Listening = true;
                return true;
            }
            catch
            {
                string msg = "Could not record from audio device!\n\n";
                MessageBox.Show(msg, "ERROR");
                Listening = false;
                return false;
            }
        }
    }
}
