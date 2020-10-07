using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;

namespace MusicAnalyser.App
{
    class LiveInputRecorder
    {
        private Form1 frm;
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;
        public bool Recording { get; set; }
        public List<byte> MyBuffer { get; set; }

        public LiveInputRecorder(Form1 form)
        {
            frm = form;
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
                MyBuffer.AddRange(e.Buffer);
                frm.OnRecordDataAvailable(MyBuffer.ToArray(), GetMaxLevel(e.Buffer));
            }
        }

        private float GetMaxLevel(byte[] buffer)
        {
            float max = 0;
            // interpret as 16 bit audio
            for (int index = 0; index < buffer.Length; index += 2)
            {
                short sample = (short)((buffer[index + 1] << 8) |
                                        buffer[index + 0]);
                // to floating point
                var sample32 = sample / 32768f;
                // absolute value 
                if (sample32 < 0) sample32 = -sample32;
                // is this the max value?
                if (sample32 > max) max = sample32;
            }
            return max;
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

        public bool StartRecording()
        {
            try
            {
                MyBuffer = new List<byte>();
                waveSource = new WaveIn();
                waveSource.WaveFormat = new WaveFormat(48000, 2);
                waveSource.DeviceNumber = Prefs.CAPTURE_DEVICE;
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
