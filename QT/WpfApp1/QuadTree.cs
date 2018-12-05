using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    internal class QuadTree
    {
        Rect Bounds;
        QuadTree[] Trees;
        int Capacity { get; set; }
        public bool Divided { get; private set; }
        public QuadTree NW { get; private set; }
        public QuadTree NE { get; private set; }
        public QuadTree SW { get; private set; }
        public QuadTree SE { get; private set; }

        List<Particle> Particles;
        public QuadTree(Rect bounds, int capacity)
        {
            Bounds = bounds;
            Capacity = capacity;
            Particles = new List<Particle>();
        }

        internal void Insert(Particle p)
        {
            if (!Contains(p))
                return;
            if (Particles.Count < Capacity)
            {
                Particles.Add(p);
            }
            else
            {
                if (!Divided)
                {
                    SubDivide();
                }
                foreach (var item in Trees)
                {
                    item.Insert(p);
                }
            }
        }

        internal void Draw(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(new SolidColorBrush(Colors.Transparent), new Pen(new SolidColorBrush(Colors.Black), 1), Bounds);
            if (Divided)
            {
                foreach (var item in Trees)
                {
                    item.Draw(drawingContext);
                }
            }
            //foreach (var item in Particles)
            //{
            //    item.Draw(drawingContext);
            //}
        }

        private bool Contains(Particle p)
        {
            Point P = p.Pos;
            Rect R = Bounds;
            return (P.X >= R.X && P.X < R.X + R.Width && P.Y >= R.Y && P.Y < R.Y + R.Height);
        }

        //Returns the 
        internal bool GetParticles(Particle item, out List<Particle> p)
        {
            p = new List<Particle>();
            if (Particles.Count <= 0) return false;
            if (!(Contains(item))) return false;
            if (Divided)
            {
                foreach (var Titem in Trees)
                {
                    if(Titem.GetParticles(item,out p))
                    {
                        return true;
                    }
                }
                return false;
            }
            p = Particles;
            return true;
        }

        private void SubDivide()
        {
            double x0 = Bounds.X;
            double y0 = Bounds.Y;
            double HalfW = Bounds.Width / 2;
            double HalfH = Bounds.Height / 2;
            double x1 = Bounds.X + HalfW;
            double y1 = Bounds.Y + HalfH;
            Trees = new QuadTree[4];
            Trees[0] = NW = new QuadTree(new Rect(x0, y0, HalfW, HalfH), Capacity);
            Trees[1] = NE = new QuadTree(new Rect(x1, y0, HalfW, HalfH), Capacity);
            Trees[2] = SW = new QuadTree(new Rect(x0, y1, HalfW, HalfH), Capacity);
            Trees[3] = SE = new QuadTree(new Rect(x1, y1, HalfW, HalfH), Capacity);
            Capacity = 0;
            Divided = true;

        }
    } 
}
