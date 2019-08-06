using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser
{
    static class FileHandler
    {
        public static void OpenWav(string filename, out AudioSource source)
        {
            source = new AudioSource();
            source.Audio = new AudioFileReader(filename);
            source.AudioGraph = WaveFormatConversionStream.CreatePcmStream(new WaveFileReader(filename));
            source.AudioFFT = ResampleWav(new WaveFileReader(filename), 8000);
        }

        public static void OpenMP3(string filename, out AudioSource source)
        {
            string outFile;
            using (var reader = new Mp3FileReader(filename))
            {
                outFile = Path.Combine(Path.GetTempPath(), "conversion.wav");
                WaveFileWriter.CreateWaveFile(outFile, reader);
            }
            OpenWav(outFile, out source);
        }

        public static WaveStream ResampleWav(WaveFileReader reader, int sampleRate)
        {
            string outFile;
            using (reader)
            {
                var outFormat = new WaveFormat(sampleRate, reader.WaveFormat.Channels);

                using (var resampler = new MediaFoundationResampler(reader, outFormat))
                {
                    resampler.ResamplerQuality = 60;
                    outFile = Path.Combine(Path.GetTempPath(), "resampled.wav");
                    WaveFileWriter.CreateWaveFile(outFile, resampler);
                }
            }

            using (var resampledReader = new AudioFileReader(outFile))
            {
                var lowPass = BiQuadFilter.LowPassFilter(sampleRate, sampleRate / 4, 1);
                var highPass = BiQuadFilter.HighPassFilter(sampleRate, 80, 1);

                using (var filter = new FilterWaveProvider(resampledReader, lowPass, highPass))
                {
                    outFile = Path.Combine(Path.GetTempPath(), "resample_and_filtered.wav");
                    WaveFileWriter.CreateWaveFile16(outFile, filter);
                }
            }
            return new WaveFileReader(outFile);
        }
    }
}
