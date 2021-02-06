using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MusicAnalyser.UI
{
    class RoundButton : Button
    {
        public RoundButton()
        {
            TabStop = false;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicsPath graphics = new GraphicsPath();
            graphics.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            Region = new Region(graphics);
            base.OnPaint(e);
        }
    }
}
