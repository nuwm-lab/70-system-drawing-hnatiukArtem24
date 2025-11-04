using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LabWork
{
    public class GraphForm : Form
    {
        private const double Xmin = 2.3;
        private const double Xmax = 5.4;
        private const double Dx = 0.8;
        private const double Margin = 50;

        public GraphForm()
        {
            Text = "Графік y = (x + cos(2x)) / (x + 2)";
            BackColor = Color.White;
            ClientSize = new Size(900, 600);
            ResizeRedraw = true;

       
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawGraph(e.Graphics, ClientSize);
        }

        private void DrawGraph(Graphics g, Size size)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            double width = size.Width - 2 * Margin;
            double height = size.Height - 2 * Margin;

    
            double ymin = double.MaxValue, ymax = double.MinValue;
            int n = (int)Math.Ceiling((Xmax - Xmin) / Dx) + 1;
            for (int i = 0; i < n; i++)
            {
                double x = Xmin + i * Dx;
                double y = (x + Math.Cos(2 * x)) / (x + 2);
                ymin = Math.Min(ymin, y);
                ymax = Math.Max(ymax, y);
            }

            if (Math.Abs(ymax - ymin) < 1e-9)
            {
                ymin -= 1;
                ymax += 1;
            }

            double kx = width / (Xmax - Xmin);
            double ky = height / (ymax - ymin);

            using (Pen axisPen = new Pen(Color.Black, 1.5f))
            using (Pen graphPen = new Pen(Color.Blue, 2f))
            using (Font font = new Font("Arial", 10))
            using (Brush textBrush = new SolidBrush(Color.Black))
            {
        
                float xAxisY = (float)(size.Height - Margin);
                float yAxisX = (float)Margin;
                g.DrawLine(axisPen, yAxisX, Margin, yAxisX, xAxisY); // Y
                g.DrawLine(axisPen, yAxisX, xAxisY, (float)(size.Width - Margin), xAxisY); // X

      
                PointF? prev = null;
                int points = Math.Max(200, size.Width / 3); 
                for (int i = 0; i <= points; i++)
                {
                    double x = Xmin + i * (Xmax - Xmin) / points;
                    double y = (x + Math.Cos(2 * x)) / (x + 2);

                    float sx = (float)(Margin + (x - Xmin) * kx);
                    float sy = (float)(size.Height - Margin - (y - ymin) * ky);

                    if (prev != null)
                        g.DrawLine(graphPen, prev.Value, new PointF(sx, sy));
                    prev = new PointF(sx, sy);
                }

            
                using (Brush red = new SolidBrush(Color.Red))
                {
                    for (double x = Xmin; x <= Xmax + 1e-9; x += Dx)
                    {
                        double y = (x + Math.Cos(2 * x)) / (x + 2);
                        float sx = (float)(Margin + (x - Xmin) * kx);
                        float sy = (float)(size.Height - Margin - (y - ymin) * ky);
                        g.FillEllipse(red, sx - 4, sy - 4, 8, 8);
                    }
                }

         
                g.DrawString("X", font, textBrush, size.Width - 45, size.Height - 40);
                g.DrawString("Y", font, textBrush, 20, 20);
                g.DrawString("y = (x + cos(2x)) / (x + 2)", font, Brushes.DarkBlue, (float)Margin + 5, 5);
            }
        }
    }
}
