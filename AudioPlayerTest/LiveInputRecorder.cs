using NAudio.Wave;
using System;
using System.IO;

namespace MusicAnalyser
{
    class LiveInputRecorder
    {
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;
        public bool Recording { get; set; }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        public void StopRecording()
        {
            waveSource.StopRecording();
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
            Recording = false;
        }

        public bool StartRecording(int audioDeviceNumber = 0)
        {
            try
            {
                waveSource = new WaveIn();
                waveSource.WaveFormat = new WaveFormat(48000, 2);

                waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);

                waveFile = new WaveFileWriter(Path.Combine(Path.GetTempPath(), "recording.wav"), waveSource.WaveFormat);

                waveSource.StartRecording();
                Recording = true;
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e);
                return false;
            }
        }
    }
}
