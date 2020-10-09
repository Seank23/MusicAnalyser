using MusicAnalyser.App.Analysis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser.App.DSP
{
    class DSPMain
    {
        public Analyser Analyser { get; set; }

        private AppController app;

        private double[] processedData;
        public List<double[]> prevProcessedData = new List<double[]>();
        private double scale;
        private double avgGain;
        public double maxGain;
        public Dictionary<double, double> fftPeaks;

        public DSPMain(AppController appController)
        {
            Analyser = new Analyser();
            app = appController;
        }

        /*
         * Master method for calculating realtime frequency domain data from audio playback
         */
        public bool FFTMain()
        {
            byte[] bytesBuffer;
            short[] audioBuffer;

            bytesBuffer = new byte[Prefs.BUFFERSIZE];
            double posScaleFactor = (double)app.AudioSource.Audio.WaveFormat.SampleRate / (double)app.AudioSource.AudioFFT.WaveFormat.SampleRate;
            app.AudioSource.AudioFFT.Position = (long)(app.AudioSource.AudioStream.Position / posScaleFactor / app.AudioSource.AudioStream.WaveFormat.Channels); // Syncs position of FFT WaveStream to current playback position
            app.AudioSource.AudioFFT.Read(bytesBuffer, 0, Prefs.BUFFERSIZE); // Reads PCM data at synced position to bytesBuffer
            audioBuffer = new short[Prefs.BUFFERSIZE];
            Buffer.BlockCopy(bytesBuffer, 0, audioBuffer, 0, bytesBuffer.Length); // Bytes to shorts

            scale = PerformFFT(audioBuffer, out processedData, app.AudioSource.AudioFFT.WaveFormat.SampleRate);

            if(!Double.IsInfinity(processedData[0]))
                processedData = SmoothSignal(processedData, Prefs.SMOOTH_FACTOR);
            avgGain = processedData.Average();
            maxGain = processedData.Max();
            app.DrawSpectrum(processedData, scale, avgGain, maxGain);

            return true;
        }

        public double PerformFFT(short[] audioBuffer, out double[] fftOutput, int sampleRate)
        {
            int fftPoints = 2;
            while (fftPoints * 2 <= audioBuffer.Length) // Sets fftPoints to largest multiple of 2 in BUFFERSIZE
                fftPoints *= 2;
            fftOutput = new double[fftPoints / 2];

            // FFT Process
            NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
            for (int i = 0; i < fftPoints; i++)
                fftFull[i].X = (float)(audioBuffer[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftPoints));
            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftPoints, 2.0), fftFull);

            for (int i = 0; i < fftPoints / 2; i++) // Since FFT output is mirrored above Nyquist limit (fftPoints / 2), these bins are summed with those in base band
            {
                double fft = Math.Abs(fftFull[i].X + fftFull[i].Y);
                double fftMirror = Math.Abs(fftFull[fftPoints - i - 1].X + fftFull[fftPoints - i - 1].Y);
                fftOutput[i] = 20 * Math.Log10(fft + fftMirror) - Prefs.PEAK_FFT_POWER; // Estimates gain of FFT bin
            }
            return (double)fftPoints / sampleRate;
        }

        /*
         * Performs smoothing on frequency domain data by averaging several frames
         */
        public double[] SmoothSignal(double[] signal, int smoothDepth)
        {
            double[] newSignal = new double[signal.Length];
            Array.Copy(signal, newSignal, signal.Length);
            prevProcessedData.Add(newSignal);

            if (prevProcessedData.Count > smoothDepth)
                prevProcessedData.RemoveAt(0);

            for (int i = 0; i < newSignal.Length; i++)
            {
                double smoothedValue = 0;
                for (int j = 0; j < prevProcessedData.Count; j++)
                {
                    smoothedValue += prevProcessedData[j][i];
                }
                smoothedValue /= prevProcessedData.Count;
                newSignal[i] = smoothedValue;
            }
            return newSignal;
        }

        /*
         * Returns the slope at each point of the signal passed in (By Slope Algorithm)
         */
        private double[] GetSlope(double[] source)
        {
            double[] derivative = new double[source.Length];
            derivative[0] = 0;
            for (int i = 1; i < source.Length - 1; i++)
            {
                double deltaX = ((i + 2) / scale) - (i / scale);
                derivative[i] = (source[i + 1] - source[i - 1]) / deltaX; // P[i] = y[i + 1] - y[i - 1] / x[i + 1] - x[i - 1]
            }
            derivative[source.Length - 1] = 0;
            return derivative;
        }

        /*
         * First step of analysis, identifies the most prominent frequency bins in the spectrum by using the slope of the signal (By Slope Algorithm)
         */
        public void GetPeaksBySlope()
        {
            double[] derivative = GetSlope(processedData);
            fftPeaks = new Dictionary<double, double>();
            double gainThreshold = avgGain + 25;

            for (int i = (int)(scale * Prefs.MIN_FREQ); i < Math.Min(processedData.Length, (int)(scale * Prefs.MAX_FREQ)); i++)
            {
                if (processedData[i] < gainThreshold)
                    continue;

                if (derivative[i] > 0 && derivative[i + 1] < 0)
                {
                    double freq = (i + 1) / scale;
                    double avgGainChange = (derivative[i] + derivative[i - 1] + derivative[i - 2]) / 3;
                    if (avgGainChange > 3)
                        fftPeaks.Add(freq, processedData[i]);
                }
            }
            RemoveKickNoise();

            fftPeaks = fftPeaks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low
        }

        /*
         * First step of analysis, identifies the most prominent frequency bins in the spectrum - these represent possible notes (By Magnitude Algorithm)
         */
        public void GetPeaksByMagnitude()
        {
            fftPeaks = new Dictionary<double, double>();
            double freq;
            double gainThreshold = avgGain + 25;

            // Iterates through frequency data, storing the frequency and gain of the largest frequency bins 
            for (int i = (int)(scale * Prefs.MIN_FREQ); i < Math.Min(processedData.Length, (int)(scale * Prefs.MAX_FREQ)); i++)
            {
                if (processedData[i] > gainThreshold)
                {
                    freq = (i + 1) / scale; // Frequency value of bin

                    fftPeaks.Add(freq, processedData[i]);
                    fftPeaks = fftPeaks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low

                    if (fftPeaks.Count > Prefs.PEAK_BUFFER) // When fftPeaks overflows, remove smallest frequency bin
                    {
                        double keyToRemove = GetDictKey(fftPeaks, fftPeaks.Count - 1);
                        fftPeaks.Remove(keyToRemove);
                    }
                }
            }

            fftPeaks = fftPeaks.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value); // Order: Frequency - low to high
            List<KeyValuePair<double, double>> cluster = null;
            KeyValuePair<double, double> largestGain = new KeyValuePair<double, double>();
            int peakIndex = 0;
            while (peakIndex < fftPeaks.Count) // Removes unwanted and redundant peaks
            {
                double myFreq = GetDictKey(fftPeaks, peakIndex);

                if (cluster == null)
                {
                    cluster = new List<KeyValuePair<double, double>>();
                    largestGain = new KeyValuePair<double, double>(myFreq, fftPeaks[myFreq]);
                    cluster.Add(largestGain);
                    peakIndex++;
                    continue;
                }
                else if ((myFreq - largestGain.Key) <= largestGain.Key / 100 * Prefs.MAX_FREQ_CHANGE) // Finds clusters of points that represent the same peak
                {
                    cluster.Add(new KeyValuePair<double, double>(myFreq, fftPeaks[myFreq]));

                    if (fftPeaks[myFreq] > largestGain.Value)
                        largestGain = new KeyValuePair<double, double>(myFreq, fftPeaks[myFreq]);

                    if (peakIndex < fftPeaks.Count - 1)
                    {
                        peakIndex++;
                        continue;
                    }
                }

                if (cluster.Count > 1) // Keeps only the largest value in the cluster
                {
                    cluster.Remove(largestGain);
                    for (int j = 0; j < cluster.Count; j++)
                    {
                        fftPeaks.Remove(cluster[j].Key);
                    }
                    peakIndex -= cluster.Count;
                }
                cluster = null;
            }

            fftPeaks = fftPeaks.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value); // Order: Frequency - low to high
            List<double> discardFreqs = new List<double>();

            for (int i = 0; i < fftPeaks.Count - 1; i++) // Removes any unwanted residual peaks after a large peak
            {
                double freqA = GetDictKey(fftPeaks, i);
                double freqB = GetDictKey(fftPeaks, i + 1);
                if (Math.Abs(fftPeaks[freqA] - fftPeaks[freqB]) >= Prefs.MAX_GAIN_CHANGE)
                {
                    if (fftPeaks[freqA] > fftPeaks[freqB]) // Discard lowest value
                        discardFreqs.Add(freqB);
                    else
                        discardFreqs.Add(freqA);
                }
            }

            foreach (double frequency in discardFreqs)
                fftPeaks.Remove(frequency);

            RemoveKickNoise();

            fftPeaks = fftPeaks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low
        }

        /*
         * Performs further refinement of peaks, removing noise mostly attributed to the kick drum
         */
        private void RemoveKickNoise()
        {
            List<double> discardFreqs = new List<double>();
            double prevFreq = 0;
            fftPeaks = fftPeaks.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value); // Order: Frequency - low to high

            foreach (double freq in fftPeaks.Keys)
            {
                if (freq > 200)
                    break;
                if (prevFreq == 0)
                {
                    prevFreq = freq;
                    continue;
                }
                if ((freq - prevFreq) <= freq / 100 * (2.5 * Prefs.MAX_FREQ_CHANGE)) // Checking for consecutive, closely packed peaks - noise
                {
                    if (Math.Abs(fftPeaks[freq] - fftPeaks[prevFreq]) <= Prefs.SIMILAR_GAIN_THRESHOLD)
                    {
                        if (!discardFreqs.Contains(prevFreq))
                            discardFreqs.Add(prevFreq);
                        discardFreqs.Add(freq);
                    }
                }
                prevFreq = freq;
            }

            foreach (double frequency in discardFreqs)
                fftPeaks.Remove(frequency);
        }

        /*
         * Returns the key value of 'dict' at 'index'
         */
        private double GetDictKey(Dictionary<double, double> dict, int index)
        {
            int i = 0;
            foreach (var key in dict.Keys)
            {
                if (i == index)
                    return key;
                i++;
            }
            return 0;
        }
    }
}
