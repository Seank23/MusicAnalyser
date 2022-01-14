/*
 * Music Analyser - Primary Processor Script - BasicFFT
 * Author: Sean King
 * Simple Fast Fourier Transform (FFT) implementation using NAudio.
 * Based on the FFT implementation by Scott Harden:
 * https://github.com/swharden/Csharp-Data-Visualization/tree/master/projects/17-07-16_microphone
 * Properties:
 * InputBuffer: type short[]
 * OutputBuffer: type double[]
 * InputArgs: SAMPLE_RATE - sample rate (Hz) of the input signal - type int
 * OutputArgs: SCALE - ratio between FFT resolution and sample rate - type double
 * Settings:
 * - WINDOW: Specifies the window function used - type enum (values: Rectangle, Hamming, Hann, BlackmannHarris)
 * - OUTPUT_MODE: Specifies how the output magnitude should be scaled - type enum (values: Magnitude, dB)
 * - SQUARE: Specifies whether output magnitudes should be squared - type enum (values: Yes, No)
 * - MAG_LIMIT: Sets the maximum output magnitude value - type int (0 - 10000)
 */
using System;
using System.Collections.Generic;
using MusicAnalyser.App.DSP;

class BasicFFTProcessor : ISignalProcessor
{
    public bool IsPrimary { get { return true; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputBuffer { get; set; }
    public Dictionary<string, object> InputArgs { get; set; }
    public object OutputBuffer { get; set; }
    public Dictionary<string, object> OutputArgs { get; set; }

    public BasicFFTProcessor()
    {
        Settings = new Dictionary<string, string[]>()
        {
            { "WINDOW", new string[] { "Hamming", "enum", "Window Function", "Rectangle|Hamming|Hann|BlackmannHarris", "" } },
            { "OUTPUT_MODE", new string[] { "dB", "enum", "Output Mode", "Magnitude|dB", "" } },
            { "SQUARE", new string[] { "No", "enum", "Square Output", "Yes|No", "" } },
            { "MAG_LIMIT", new string[] { "10000", "int", "Magnitude Limit", "0", "10000" } },
        };
    }

    public void OnSettingsChange() { }

    public void Process()
    {
        short[] input = null;
        if (InputBuffer.GetType().Name == "Int16[]")
            input = (short[])InputBuffer;
        if (input == null)
            return;

        int sampleRate = 1;
        if (InputArgs.ContainsKey("SAMPLE_RATE"))
        {
            if(InputArgs["SAMPLE_RATE"].GetType().Name == "Int32")
                sampleRate = (int)InputArgs["SAMPLE_RATE"];
        }

        int fftPoints = 2;
        while (fftPoints * 2 <= input.Length) // Sets fftPoints to largest multiple of 2 in BUFFERSIZE
            fftPoints *= 2;
        double[] output = new double[fftPoints / 2];

        // FFT Process
        NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
        for (int i = 0; i < fftPoints; i++)
        {
            if (Settings["WINDOW"][0] == "Hamming")
                fftFull[i].X = (float)(input[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftPoints));
            else if (Settings["WINDOW"][0] == "Hann")
                fftFull[i].X = (float)(input[i] * NAudio.Dsp.FastFourierTransform.HannWindow(i, fftPoints));
            else if (Settings["WINDOW"][0] == "BlackmannHarris")
                fftFull[i].X = (float)(input[i] * NAudio.Dsp.FastFourierTransform.BlackmannHarrisWindow(i, fftPoints));
            else
                fftFull[i].X = input[i];
        }    
        NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftPoints, 2.0), fftFull);

        for (int i = 0; i < fftPoints / 2; i++) // Since FFT output is mirrored above Nyquist limit (fftPoints / 2), these bins are summed with those in base band
        {
            double fft = Math.Abs(fftFull[i].X + fftFull[i].Y);
            double fftMirror = Math.Abs(fftFull[fftPoints - i - 1].X + fftFull[fftPoints - i - 1].Y);
            if(Settings["OUTPUT_MODE"][0] == "dB")
                output[i] = 20 * Math.Log10(fft + fftMirror) - 20 * Math.Log10(input.Length); // Estimates gain of FFT bin
            else
            {
                if (fft + fftMirror <= int.Parse(Settings["MAG_LIMIT"][0]))
                    output[i] = (fft + fftMirror) * (0.5 + (i / (fftPoints * 2)));
                else
                    output[i] = int.Parse(Settings["MAG_LIMIT"][0]);
            }
            if (Settings["SQUARE"][0] == "Yes")
                output[i] = Math.Pow(output[i], 2) / 100; 
        }
        OutputBuffer = output;
        double scale = (double)fftPoints / sampleRate;
        OutputArgs.Add("SCALE", scale);
    }
}

