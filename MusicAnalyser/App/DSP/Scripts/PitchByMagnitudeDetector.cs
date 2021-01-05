/*
 * Music Analyser - Primary Detector Script - PitchByMagnitude
 * Author: Sean King
 * Peak detection algorithm optimised for detecting musical pitches (12 tone equal temperment) from an FFT spectrum.
 * Searches through chunks of the input signal at intervals corresponding to musical pitches, resolves clusters of points to a single value (largest in cluster), returns largest n values.
 * Properties:
 * InputData: type double[]
 * Output: type Dictionary<double, double>
 * InputArgs: SCALE - Ratio between number of input values and sample rate - type double
 *            TUNING - Percentage difference from standard tuning - type double
 * OutputArgs: None
 * Settings:
 * - MIN_FREQ: Starting frequency (Hz), for analysis in standard tuning use appropriate note frequency (eg. C1 = 32.7 Hz) - type double (1 - 1000)
 * - OCTAVES: Number of octaves to analyse  - type int (1 - 10)
 * - MAX_VALS: Maximum number of peaks to return - tyoe int (1 - 100)
 * - MAG_THRESHOLD: Threshold above which point is considered a peak, relative to max magnitude-average magnitude - type double (0 - 1)
 * - FREQ_TOLERANCE: Percentage tolerance from musical pitch, if within this a frequency bin is considered for analysis - type double (0 - 50)
 */
using MusicAnalyser.App.DSP;
using System;
using System.Collections.Generic;
using System.Linq;

class PitchByMagnitudeDetector : ISignalDetector
{
    public bool IsPrimary { get { return true; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputData { get; set; }
    public Dictionary<string, object> InputArgs { get; set ; }
    public object Output { get; set; }
    public Dictionary<string, object> OutputArgs { get; set; }

    public PitchByMagnitudeDetector()
    {
        Settings = new Dictionary<string, string[]>
        {
            { "MIN_FREQ", new string[] { "32.7", "double", "Minimum Frequency (Hz)", "1", "1000" } },
            { "OCTAVES", new string[] { "6", "int", "Octaves", "1", "10" } },
            { "MAX_VALS", new string[] { "10", "int", "Maximum Frequency Points", "1", "100" } },
            { "MAG_THRESHOLD", new string[] { "0.1", "double", "Magnitude Threshold", "0", "1" } },
            { "FREQ_TOLERANCE", new string[] { "2.8", "double", "Frequency Tolerance (%)", "0", "50" } },
        };
    }

    public void Detect()
    {
        double[] input = null;
        double scale = 0;
        double tuning = 0;
        if (InputData.GetType().Name == "Double[]")
            input = (double[])InputData;
        if (InputArgs.ContainsKey("SCALE"))
        {
            if (InputArgs["SCALE"].GetType().Name == "Double")
                scale = (double)InputArgs["SCALE"];
        }
        if(InputArgs.ContainsKey("TUNING"))
        {
            if (InputArgs["TUNING"].GetType().Name == "Double")
                tuning = (double)InputArgs["TUNING"];
        }
        if (input == null || scale == 0)
            return;

        Dictionary<double, double> output = new Dictionary<double, double>();
        double spectAvg = input.Average();
        double spectMax = input.Max();
        int notesPerOctave = 12;
        double minFreq = double.Parse(Settings["MIN_FREQ"][0]) + (tuning * double.Parse(Settings["MIN_FREQ"][0]) / 100);
        double[] noteFreqs = new double[notesPerOctave * int.Parse(Settings["OCTAVES"][0])];
        for (int i = 0; i < noteFreqs.Length; i++)
            noteFreqs[i] = minFreq * Math.Pow(2, (double)i / notesPerOctave);

        double prevMag = 0;
        double prevFreq = 0;
        for(int i = 0; i < noteFreqs.Length; i++)
        {
            int inputIndex = (int)Math.Round(noteFreqs[i] * scale) - 1;
            int clusterSize = 0;
            while (clusterSize / scale <= noteFreqs[i] / 100 * double.Parse(Settings["FREQ_TOLERANCE"][0]))
                clusterSize++;

            double avgMag = 0;
            double maxVal = 0, maxIndex = 0;
            for (int j = -clusterSize; j < clusterSize; j++)
            {
                avgMag += input[inputIndex + j];
                if(input[inputIndex + j] > maxVal)
                {
                    maxVal = input[inputIndex + j];
                    maxIndex = inputIndex + j;
                }
            }
            avgMag /= 2 * clusterSize + 1;
            double maxFreq = maxIndex / scale;

            if(Math.Abs(prevMag - maxVal) > Math.Max(prevMag, maxVal) / 4 && noteFreqs[i] < 200)
            {
                if (prevMag - maxVal < 0)
                    output.Remove(prevFreq);
                else
                {
                    prevMag = maxVal;
                    prevFreq = maxFreq;
                    continue;
                }
            }

            if (avgMag > spectAvg + ((spectMax - spectAvg) * double.Parse(Settings["MAG_THRESHOLD"][0])))
                output[maxFreq] = maxVal;
            prevMag = maxVal;
            prevFreq = maxFreq;
            //Console.WriteLine("Note Freq: " + noteFreqs[i] + " Cluster Size: " + clusterSize + " Max Freq: " + (maxIndex / scale) + " Max Mag: " + maxVal);
        }
        output = output.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        Output = output.Take(int.Parse(Settings["MAX_VALS"][0])).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void OnSettingsChange() { }
}

