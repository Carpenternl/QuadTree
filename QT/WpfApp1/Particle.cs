using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    class Particle
    {
        private int v1;
        private int v2;
        private Point point;

        public Particle(double x, double y)
        {
            Pos = new Point(x, y);
            init();
        }

        public Particle(Point point)
        {
            Pos = point;
            init();
        }

        private void init()
        {
            Rad = 10;
            Fill = Colors.Green;
        }

        public void Draw(DrawingContext drawingContext)
        {
            drawingContext.DrawEllipse(new SolidColorBrush(Fill), new Pen(new SolidColorBrush(Colors.Black), 2),Pos, Rad, Rad);
        }

        public Point Pos { get; set; }
        public double Rad { get; set; }
        public Color Fill { get; set; }
        public Vector Direction { get; set; }
    }
    
}
