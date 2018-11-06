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

namespace QT
{
    /// <summary>
    /// Interaction logic for ParticlePage.xaml
    /// </summary>
    public partial class ParticlePage : Page
    {
        Particle[] Particles;
        int Margn;
        SolidColorBrush Fill;
        SolidColorBrush Bordr;
        Pen Bordrpen;
        Random R = new Random();
        Thread myclock;
        public ParticlePage()
        {
            Margn = 5;
            Fill = new SolidColorBrush(Colors.Green);
            Bordr = new SolidColorBrush(Colors.Black);
            Bordrpen = new Pen(Bordr, 1);
            int ParticleCount = 50;
            Particles = new Particle[ParticleCount];
            RandomizeParticles();
            InitializeComponent();
        }

        private void RandomizeParticles()
        {

            
            for (int i = 0; i < Particles.Length; i++)
            {
                Particles[i] = RandomParticle(R);
            }
        }
        public delegate void MyAction();

        private Particle RandomParticle(Random r)
        {
            double X = r.Next((int)(ActualWidth - Margn) + Margn);
            double Y = r.Next((int)(ActualWidth - Margn) + Margn);
            double vx = getvec(0,r);
            double vy = getvec(0, r);
            Particle Result = new Particle(new Point(X, Y), new Vector(vx, vy), 10,10);
            return Result;
        }

        private double getvec(double arg, Random r)
        {
            do
            {
                arg = r.NextDouble()-0.5;
            } while (arg == 0);
            return arg;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            for (int i = 0; i < Particles.Length; i++)
            {
                drawingContext.DrawEllipse(Fill, Bordrpen, Particles[i]._Point, 10, 10);
            }
        }

        public void LogicBoi()
        {

            UpdateParticles();
            this.InvalidateVisual();
        }

        private void UpdateParticles()
        {
            for (int i = 0; i < Particles.Length; i++)
            {
                Particle Prt = Particles[i];
                CheckBorder(Prt);
                Prt.update();
                Prt._Speed = Prt._Speed * 0.99;
                Particles[i] = Prt;

            }

        }

        private void CheckBorder(Particle prt)
        {
           
            if (prt._Point.X < 0+prt._Radius)
            {
                prt._Vector.X = randvac(prt._Vector.X);
                prt._Speed += 3;
            }
            if(prt._Point.X> this.ActualWidth-prt._Radius)
            {
                prt._Vector.X = randvac(prt._Vector.X) * -1;
                prt._Speed += 3;
            }
            if(prt._Point.Y > this.ActualHeight-prt._Radius)
            {

                prt._Vector.Y = randvac(prt._Vector.Y) * -1;
                prt._Speed += 3;
            }
            if (prt._Point.Y < 0+ prt._Radius)
            {
                prt._Vector.Y = randvac(prt._Vector.Y);
                prt._Speed += 3;
            }
        }

        private double randvac(double y)
        {
            double Y = Math.Abs(y * 100);
            double a = R.Next(0,(int)Y);
            double b = R.Next((int)Y,100);
            return (a + b) / 200;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RandomizeParticles();
            myclock = new Thread(tickTock);
            myclock.Start();

        }
        private static Action EmptyDelegate = delegate () { };
        private void tickTock()
        {
            while (true)
            {
                Thread.Sleep(16);
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, EmptyDelegate);
                Dispatcher.BeginInvoke(new MyAction(LogicBoi), null);
            }
        }

        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
            // Dispatcher.BeginInvoke(new MyAction(LogicBoi), null);

        }
    }
}
