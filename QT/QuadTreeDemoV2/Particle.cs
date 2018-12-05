using System;
using System.Windows;
using System.Windows.Media;

namespace QuadTreeDemoV2
{
    internal class Particle
    {
        private Color fillColor;

        public Color FillColor
        {
            get { return fillColor; }
            set
            {
                fillColor = value;
               // Fill = new SolidColorBrush(FillColor);
            }
        }

        public Brush Fill { get { return new SolidColorBrush(fillColor); } }
        public Pen Pen { get { return new Pen(new SolidColorBrush(Colors.Black), 2); } }
        public Point Center { get; set; }
        public double Radius { get; internal set; }
        public Vector Dir { get; set; }

        internal static void UpDate(ref Particle partA)
        {
            double NewX = partA.Center.X + partA.Dir.X;
            double NewY = partA.Center.Y + partA.Dir.Y;
            partA.Center = new Point(NewX, NewY);
        }
    }
}