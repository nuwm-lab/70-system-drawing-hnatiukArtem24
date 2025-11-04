using System;
using System.Drawing;
using System.Windows.Forms;

class GraphForm : Form
{
    private const double Xmin = 2.3;
    private const double Xmax = 5.4;
    private const double Dx = 0.8;

    public GraphForm()
    {
        this.Text = "Графік y = (x + cos(2x)) / (3x)";
        this.BackColor = Color.White;
        this.ClientSize = new Size(800, 600);
        this.ResizeRedraw = true; // Автоматичне перемальовування при зміні розміру
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        DrawGraph(e.Graphics);
    }

    private void DrawGraph(Graphics g)
    {
        double margin = 40;
        double width = ClientSize.Width - 2 * margin;
        double height = ClientSize.Height - 2 * margin;

        // Знайдемо межі Y
        double ymin = double.MaxValue, ymax = double.MinValue;
        for (double x = Xmin; x <= Xmax; x += Dx)
        {
            double y = (x + Math.Cos(2 * x)) / (3 * x);
            ymin = Math.Min(ymin, y);
            ymax = Math.Max(ymax, y);
        }

        // Масштаби
        double kx = width / (Xmax - Xmin);
        double ky = height / (ymax - ymin);

        // Малюємо осі координат
        Pen axisPen = new Pen(Color.Black, 2);
        g.DrawLine(axisPen, (float)margin, (float)(ClientSize.Height - margin),
                   (float)(ClientSize.Width - margin), (float)(ClientSize.Height - margin)); // X
        g.DrawLine(axisPen, (float)margin, (float)margin,
                   (float)margin, (float)(ClientSize.Height - margin)); // Y

        // Малюємо графік
        Pen graphPen = new Pen(Color.Blue, 2);
        PointF? prev = null;
        for (double x = Xmin; x <= Xmax; x += 0.01)
        {
            double y = (x + Math.Cos(2 * x)) / (3 * x);
            float sx = (float)(margin + (x - Xmin) * kx);
            float sy = (float)(ClientSize.Height - margin - (y - ymin) * ky);

            if (prev != null)
                g.DrawLine(graphPen, prev.Value, new PointF(sx, sy));

            prev = new PointF(sx, sy);
        }

        // Малюємо червоні точки кожні Δx = 0.8
        Brush red = Brushes.Red;
        for (double x = Xmin; x <= Xmax; x += Dx)
        {
            double y = (x + Math.Cos(2 * x)) / (3 * x);
            float sx = (float)(margin + (x - Xmin) * kx);
            float sy = (float)(ClientSize.Height - margin - (y - ymin) * ky);
            g.FillEllipse(red, sx - 4, sy - 4, 8, 8);
        }

        // Підпис формули
        Font font = new Font("Arial", 10);
        g.DrawString("y = (x + cos(2x)) / (3x)", font, Brushes.Black, (float)margin, 10);
    }
}

class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new GraphForm());
    }
}
