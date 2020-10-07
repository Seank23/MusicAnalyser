using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;

namespace MusicAnalyser.UnitTests
{
    public class AppControllerTests
    {
        Form1 frm;
        AppController app;

        [SetUp]
        public void Setup()
        {
            frm = new Form1();
            app = new AppController(frm);
        }

        [Test]
        public void Test_PerformFFT_100HzSine()
        {
            short[] inputSignal = new short[1024];
            double[] fft;
            for(int i = 0; i < inputSignal.Length; i++)
            {
                inputSignal[i] = (short)(Math.Sin(2 * Math.PI * 100 * i / 1000) * 32768);
            }
            double fftScale = app.PerformFFT(inputSignal, out fft, 1000);
            Assert.AreEqual(100, Math.Round(GetLargestIndex(fft) / fftScale));
        }

        [Test]
        public void Test_SmoothSignal()
        {
            app.dataFftPrev.Add(new double[] { 7, 3, 6, 7, 1 });
            app.dataFftPrev.Add(new double[] { 3, 8, 2, 9, 3 });
            app.dataFftPrev.Add(new double[] { 5, 2, 4, 7, 8 });
            app.dataFftPrev.Add(new double[] { 8, 2, 4, 1, 7 });
            double[] signal = new double[] { 2, 7, 4, 5, 9 };
            signal = app.SmoothSignal(signal, 5);
            Assert.AreEqual(new double[] { 5, 4.4, 4, 5.8, 5.6 }, signal);
        }

        private int GetLargestIndex(double[] array)
        {
            int largestI = 0;
            var largestVal = array[0];
            for(int i = 1; i < array.Length; i++)
            {
                if(array[i] > largestVal)
                {
                    largestI = i;
                    largestVal = array[i];
                }
            }
            return largestI;
        }
    }
}