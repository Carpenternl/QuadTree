using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuadTreeDemoV2
{
    /// <summary>
    /// Interaction logic for ParticleWindow.xaml
    /// </summary>
    public partial class ParticleWindow : UserControl
    {
        List<Particle> Particles = new List<Particle>();
        QuadTree VisTrunk;
        public ParticleWindow()
        {
            Loaded += ParticleWindow_Loaded;
            InitializeComponent();
        }
        int checkcounter = 0;

        private void ParticleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int ParticleAmount = 100;
            Random R = new Random();
            for (int i = 0; i < ParticleAmount; i++)
            {
                Particles.Add(new Particle()
                {
                    Center = new Point(R.Next((int)ActualWidth), R.Next((int)ActualHeight)),
                    FillColor = Colors.Green,
                    Radius = Math.Max(.5, R.NextDouble()) * 10,
                    Dir = new Vector(R.NextDouble() - .5, R.NextDouble() - .5),
                });
            }
            StartRunning();
        }

        private async void StartRunning()
        {
            while (true)
            {
                Thread.Sleep(1);
                await Task.Run(() => MoveParticles());
                await Task.Run(() => BorderCheck());
                await Task.Run(() => QuadTreeDetection());
                this.Content = $"{checkcounter} calls";
                checkcounter = 0;
                InvalidateVisual();
            }
        }
        private void QuadTreeDetection()
        {
            QuadTree Trunk = new QuadTree(new Rect(0, 0, ActualWidth, ActualHeight), 4);
            List<Particle> Local = new List<Particle>(Particles);
            Particle PartA;
            for (int i = 0; i < Local.Count; i++)
            {
                PartA = Local[i];
                Trunk.Add(PartA);
            }
            for (int i = 0; i < Local.Count; i++)
            {
                PartA = Local[i];
                PartA.FillColor = Colors.Green;
                Particle PartB;
                List<Particle> testables = Trunk.GetBranch(PartA);
                for (int j = 0; j < testables.Count; j++)
                {
                    PartB = testables[j];
                    if (PartA != PartB)
                    {
                        double DeltaX = Math.Abs(PartA.Center.X - PartB.Center.X);
                        double DeltaY = Math.Abs(PartA.Center.Y - PartB.Center.Y);
                        double DeltaR = Math.Sqrt(Math.Pow(DeltaX, 2) + Math.Pow(DeltaY, 2));
                        checkcounter++;
                        if (DeltaR < PartA.Radius + PartB.Radius)
                        {

                            PartA.FillColor = Colors.Red;
                            PartB.FillColor = Colors.Red;
                        }
                    }
                }
            }
            Particles = new List<Particle>(Local);
            VisTrunk = Trunk;
        }

        private void BasicHitDetection()
        {
            List<Particle> LocaL = new List<Particle>(Particles);
            Particle PartA;
            Particle PartB;
            for (int i = 0; i < LocaL.Count; i++)
            {
                PartA = LocaL[i];
                PartA.FillColor = Colors.Green;
                for (int j = 0; j < LocaL.Count; j++)
                {
                    PartB = LocaL[j];
                    if (PartA != PartB)
                    {
                        double DeltaX = Math.Abs(PartA.Center.X - PartB.Center.X);
                        double DeltaY = Math.Abs(PartA.Center.Y - PartB.Center.Y);
                        double DeltaR = Math.Sqrt(Math.Pow(DeltaX, 2) + Math.Pow(DeltaY, 2));
                        checkcounter++;
                        if (DeltaR < PartA.Radius + PartB.Radius)
                        {
                            
                            PartA.FillColor = Colors.Red;
                            PartB.FillColor = Colors.Red;
                        }
                    }
                }
            }
            Particles = new List<Particle>(LocaL);
        }

        private void BorderCheck()
        {
            List<Particle> Local = new List<Particle>(Particles);
            Particle PartA;
            Random R = new Random();
            for (int i = 0; i < Local.Count; i++)
            {
                PartA = Local[i];
                double Rad = PartA.Radius;
                Vector V = PartA.Dir;
                Point P = PartA.Center;

                if (P.X-Rad < 0)
                    V.X = (R.NextDouble() - 0) / 2;
                if (P.X+Rad > this.ActualWidth)
                    V.X = (R.NextDouble() - 1) / 2;
                if (P.Y-Rad < 0)
                    V.Y = (R.NextDouble() - 0) / 2;
                if (P.Y + Rad > ActualHeight)
                    V.Y = (R.NextDouble() - 1) / 2;
                PartA.Dir = V;
                Local[i] = PartA;
            }
            Particles = new List<Particle>(Local);
        }

        private void MoveParticles()
        {
            Particle PartA;
            for (int i = 0; i < Particles.Count; i++)
            {
                PartA = Particles[i];
                Particle.UpDate(ref PartA);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            
            DrawingContext Artist = drawingContext;
            foreach (Particle P in Particles)
            {
                Artist.DrawEllipse(P.Fill, P.Pen, P.Center, P.Radius, P.Radius);
            }
            if(!(VisTrunk is null))
            {
                VisTrunk.draw(drawingContext);
            }
        }
    }
}
