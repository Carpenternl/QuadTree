using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ParticleViewBox.xaml
    /// </summary>
    public partial class ParticleViewBox : Page
    {
        internal QuadTree Tree;
        internal List<Particle> Particles = new List<Particle>();

        public ParticleViewBox()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            int TotalParticles = 100;
            Tree = new QuadTree(new Rect(0, 0, ActualWidth, ActualHeight), 4);
            Random r = new Random();
            for (int i = 0; i < TotalParticles; i++)
            {
                Particle p = new Particle(r.Next((int)ActualWidth), r.Next((int)ActualHeight));
                Particles.Add(p);
            }
            InvalidateVisual();
            StartLogic();

        }
        int c = 0;

        private async void StartLogic()
        {
            while (true)
            {
                await Task.Run(() => Logic());
                await Task.Run(() => Move());
                ((Label)Content).Content = c.ToString();
                InvalidateVisual();
            }
        }

        private void Move()
        {
            List<Particle> P = Particles;
            Random r = new Random();
            for (int i = 0; i < P.Count; i++)
            {
                double x = Math.Min(ActualWidth, (Math.Max(P[i].Pos.X + r.NextDouble() - .5, 0)));
                double y = Math.Min(ActualHeight, (Math.Max(P[i].Pos.Y + r.NextDouble() - .5, 0)));
                Point point = new Point(x, y);
                P[i].Pos = point;
            }
        }
        private void Logic2()
        {
            int u = 0;
            for (int i = 0; i < Particles.Count; i++)
            {
                for (int j = 0; j < Particles.Count; j++)
                {
                    Particle PA = Particles[i];
                    Particle PB = Particles[j];
                    if (PA != PB)
                    {
                        double DeltaX = Math.Abs(PA.Pos.X - PB.Pos.X);
                        double DeltaY = Math.Abs(PA.Pos.Y - PB.Pos.Y);
                        double DeltaR = Math.Sqrt(Math.Pow(DeltaX, 2) + Math.Pow(DeltaY, 2));
                        Task.Delay(1);
                        u++;
                        if (DeltaR < PA.Rad + PB.Rad)
                        {
                            PA.Fill = Colors.Red;
                        }
                    }
                }
            }
            c = u;
           
        }

        private void Logic()
        {

            Tree = new QuadTree(new Rect(0, 0, ActualWidth, ActualHeight), 4);
            foreach (var item in Particles)
            {
                Tree.Insert(item);
            }
            List<Particle> p;
            Particle PA;
            int k = 0;
            for (int i = 0; i < Particles.Count; i++)
            {
                PA = Particles[i];
                PA.Fill = Colors.Green;
                if (Tree.GetParticles(PA, out p))
                {

                    Particle PB;
                    for (int j = 0; j < p.Count; j++)
                    {
                        PB = p[j];
                        if (PA != PB)
                        {
                            Task.Delay(1);
                            double DeltaX = Math.Abs(PA.Pos.X - PB.Pos.X);
                            double DeltaY = Math.Abs(PA.Pos.Y - PB.Pos.Y);
                            double DeltaR = Math.Sqrt(Math.Pow(DeltaX, 2) + Math.Pow(DeltaY, 2));
                            Task.Delay(1);
                            k++;
                            if (DeltaR < PA.Rad + PB.Rad)
                            {
                                PA.Fill = Colors.Red;
                            }
                        }
                    }
                }
                c = k;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (!(Tree is null))
            {
                Tree.Draw(drawingContext);
            }
            if (!(Particles is null))
            {
                foreach (var item in Particles)
                {
                    item.Draw(drawingContext);
                }
            }

        }


        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}
