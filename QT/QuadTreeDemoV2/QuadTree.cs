using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace QuadTreeDemoV2
{
    internal class QuadTree
    {
        private Rect Range;
        private int Cap;
        private List<Particle> particles;
        private QuadTree[] Trees;

        public bool Divided { get; private set; }

        public QuadTree(Rect range, int cap)
        {
            this.Range = range;
            this.Cap = cap;
            particles = new List<Particle>(cap);
        }
        public bool Add(Particle p)
        {
            //Ignore of particle is outside range
            if (!InsideBounds(p))
                return false;
            //Add to list of particles
            if (Divided)
            {
                foreach (var T in Trees)
                {
                    if (T.Add(p))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                particles.Add(p);
                if (particles.Count > Cap)
                {
                    Subdivide();
                }
                return true;
            }
        }

        private bool NextAdd(Particle p)
        {
            foreach (var T in Trees)
            {
                if (T.Add(p))
                {
                    return true;
                }
            }
            return false;
        }

        private void Subdivide()
        {
            Trees = new QuadTree[4];
            for (int Y = 0; Y < 2; Y++)
            {
                for (int X = 0; X < 2; X++)
                {
                    Trees[X + Y * 2] = new QuadTree(new Rect(Range.X + X * Range.Width / 2, Range.Y + Y * Range.Height / 2, Range.Width / 2, Range.Height / 2), Cap); 
                }
            }
            Divided = true;
        }

        private bool InsideBounds(Particle p)
        {
            double Px0 = p.Center.X - p.Radius;
            double Py0 = p.Center.Y - p.Radius;
            double Px1 = p.Center.X + p.Radius;
            double Py1 = p.Center.Y + p.Radius;
            double Bx0 = Range.X;
            double By0 = Range.Y;
            double Bx1 = Range.X + Range.Width;
            double By1 = Range.Y + Range.Height;
            return (Px0 >= Bx0 && Px1 <= Bx1 && Py0 >= By0 && Py1 <= By1);

        }

        internal List<Particle> GetBranch(Particle partA)
        {
            List<Particle> Result = new List<Particle>();
            if (!InsideBounds(partA))
                return new List<Particle>();
            if (Divided)
            {
                List<Particle> returnval = new List<Particle>();
                foreach (var T in Trees)
                {
                    returnval = T.GetBranch(partA);
                    Result.AddRange(returnval);
                }
               
            }
            Result.AddRange(particles);
            return Result;
        }

        internal void draw(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(new SolidColorBrush(Colors.Transparent), new Pen(new SolidColorBrush(Colors.Blue), 2), Range);
            if (Divided)
            {
                foreach (var T in Trees)
                {
                    T.draw(drawingContext);
                }
            }
        }
    }
}