using MusicAnalyser.App.DSP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicAnalyser.UI
{
    public partial class SpectrogramViewer : UserControl
    {
        public List<SpectrogramFrame> MySpectrogramFrames { get; set; }

        public SpectrogramViewer()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (MySpectrogramFrames == null)
                return;

            double binsPerPixel = (double)MySpectrogramFrames[0].SpectrumData.Length / this.Height;
            double framesPerPixel = (double)MySpectrogramFrames.Count / this.Width;
            Brush specPainter;
            for (int i = 0; i < MySpectrogramFrames.Count; i++)
            {
                SpectrogramFrame frame = MySpectrogramFrames[i];
                int binIndex = 0;
                double overlap = 0;
                for (int j = this.Height; j > 0; j--)
                {
                    double binsInPixel = binsPerPixel;
                    double binAvg = overlap;
                    for(int k = binIndex; k < frame.SpectrumData.Length; k++)
                    {
                        if (binsInPixel >= 1)
                        {
                            binAvg += frame.SpectrumData[k];
                            binsInPixel--;
                        }
                        else
                        {
                            binAvg += binsInPixel * frame.SpectrumData[k];
                            overlap = (1 - binsInPixel) * frame.SpectrumData[k];
                            binIndex = k + 1;
                            break;
                        }
                    }
                    binAvg /= binsPerPixel;
                    byte pixelVal = (byte)Math.Min((int)Math.Round(binAvg), 255);
                    specPainter = new SolidBrush(Color.FromArgb(pixelVal, pixelVal, pixelVal));
                    e.Graphics.FillRectangle(specPainter, new RectangleF(i, j, 1, 1));
                }
            }
        }
    }
}