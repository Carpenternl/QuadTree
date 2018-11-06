using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QT
{
    class Particle
    {
        private double speed;
        public Point _Point;
        public Vector _Vector;
        public double _Radius;

        public double _Speed
        {
            set
            {
                speed = Math.Min(10, Math.Max(5, value));
            }
            get
            {
                return speed;
            }
        }

        public Particle(Point location,Vector direction,double startspeed,double rad)
        {
            _Speed = startspeed;
            _Vector = direction;
            _Point = location;
            _Radius = rad;
        }
        public void update()
        {
            _Point.X += _Vector.X*speed;
            _Point.Y += _Vector.Y*speed;
        }

    }
   
}
