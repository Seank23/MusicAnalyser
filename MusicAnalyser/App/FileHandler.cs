using MusicAnalyser.App.DSP;
using MusicAnalyser.App.Spectrogram;
using NAudio.MediaFoundation;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace MusicAnalyser.App
{
    static class FileHandler
    {
        public static void OpenWav(string filename, out AudioSource source)
        {
            source = new AudioSource();
            source.Audio = new AudioFileReader(filename);
            source.AudioGraph = WaveFormatConversionStream.CreatePcmStream(new WaveFileReader(filename));
            source.AudioAnalysis = ResampleWav(new WaveFileReader(filename), 8000);
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
                using (var filter = new FilterWaveProvider(resampledReader, sampleRate / 4, 1, 80, 1))
                {
                    outFile = Path.Combine(Path.GetTempPath(), "resample_and_filtered.wav");
                    WaveFileWriter.CreateWaveFile16(outFile, filter);
                }
            }
            return new WaveFileReader(outFile);
        }

        public static void WriteFile(string fileName, string[] content)
        {
            using (StreamWriter file = new StreamWriter(fileName))
            {
                foreach (string line in content)
                {
                    file.WriteLine(line);
                }
            }
        }

        public static string[] ReadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                string[] lines;
                List<string> list = new List<string>();
                using (StreamReader file = new StreamReader(fileName))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                lines = list.ToArray();
                return lines;
            }
            else
                return null;
        }

        public static void AppendOrReplace(string fileName, string[] content, string itemName, string separator)
        {
            string[] file = ReadFile(fileName);
            if(file == null)
                WriteFile(fileName, content);
            else
            {
                if(Array.Exists(file, item => item.Contains(separator + itemName)))
                {
                    int start = Array.FindIndex(file, item => item.Contains(separator + itemName));
                    int end = file.Length;
                    for(int i = start + 1; i < file.Length; i++)
                    {
                        if(file[i].Contains(separator))
                        {
                            end = i;
                            break;
                        }
                    }
                    List<string> newContent = file.ToList();
                    newContent.RemoveRange(start, end - start);
                    newContent.AddRange(content);
                    WriteFile(fileName, newContent.ToArray());
                }
                else
                {
                    List<string> newContent = file.ToList();
                    newContent.AddRange(content);
                    WriteFile(fileName, newContent.ToArray());
                }
            }
        }

        public static bool WriteMp3(string filename, WaveFormat format)
        {
            var mediaType = MediaFoundationEncoder.SelectMediaType(AudioSubtypes.MFAudioFormat_MP3, format, 192000);

            if(mediaType != null)
            {
                using (var reader = new WaveFileReader(Path.Combine(Path.GetTempPath(), "recording.wav")))
                {
                    MediaFoundationEncoder.EncodeToMp3(reader, filename, 192000);
                    return true;
                }
            }
            return false;
        }

        public static void WriteSpectrogram(SpectrogramData spectrogram, string filename)
        {
            FileStream fileStream;
            BinaryFormatter bf = new BinaryFormatter();

            if (File.Exists(filename))
                File.Delete(filename);

            fileStream = File.Create(filename);
            bf.Serialize(fileStream, spectrogram);
            fileStream.Close();
        }

        public static SpectrogramData ReadSpectrogram(string filename)
        {
            SpectrogramData spectrogram = null;
            FileStream fileStream;
            BinaryFormatter bf = new BinaryFormatter();

            if(File.Exists(filename))
            {
                fileStream = File.OpenRead(filename);
                spectrogram = (SpectrogramData)bf.Deserialize(fileStream);
                fileStream.Close();
            }

            return spectrogram;
        }
    }
}
