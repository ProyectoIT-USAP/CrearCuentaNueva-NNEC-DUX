using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;


namespace CrearCuentaNueva
{
    /// <summary>
    /// Lógica de interacción para frmImgDoc.xaml
    /// </summary>
    public partial class frmImgDoc : Window
    {

       //-------------------
        public double angulo = 0;
        public TranslateTransform trasladar;
        public ScaleTransform escalar;
        public TransformGroup transformGroup;
        public Point Centro = new Point(0, 0);
        public Point Inicio;
        //-------------------
        public frmImgDoc()
        {
            InitializeComponent();

            trasladar = new TranslateTransform();
            escalar = new ScaleTransform();
            transformGroup = new TransformGroup();
            transformGroup.Children.Add(trasladar);
            transformGroup.Children.Add(escalar);
            GrpImgDoc.RenderTransform = transformGroup;
            GrpImgDoc.MouseDown += new MouseButtonEventHandler(ImgDoc_MouseDown);
            GrpImgDoc.MouseMove += new MouseEventHandler(ImgDoc_MouseMove);
            GrpImgDoc.MouseUp += new MouseButtonEventHandler(ImgDoc_MouseUp);
        }

        private void BGirarImagen_Click(object sender, RoutedEventArgs e)
        {
            angulo += 90;
         
            if(angulo==360)
            {
              angulo = 0;  //reinicio de la variable angulo              
            }

            if (angulo == 90 || angulo==270)
            {
                GrpImgDoc.Width = 550; //tamaño de la imagen
                GrpImgDoc.LayoutTransform = new RotateTransform(angulo);
            }
            else
            {
                GrpImgDoc.Width = 710; //tamaño de la imagen
                GrpImgDoc.LayoutTransform = new RotateTransform(angulo);
            }
            
            
            //Console.WriteLine(angulo);
        }

        //-------------------------------------------------
        void ImgDoc_MouseMove(object sender, MouseEventArgs e)

        {
            if (GrpImgDoc.IsMouseCaptured)

            {  Point clic = e.GetPosition(this);
               trasladar.X = clic.X - Centro.X + Inicio.X;
               trasladar.Y = clic.Y - Centro.Y + Inicio.Y;

            }

        }

        void ImgDoc_MouseDown(object sender, MouseButtonEventArgs e)

        {   GrpImgDoc.CaptureMouse();
            Centro = e.GetPosition(this);
            Inicio = new Point(trasladar.X, trasladar.Y);
            Cursor = Cursors.ScrollAll;
        }

        void ImgDoc_MouseUp(object sender, MouseButtonEventArgs e)

        {   GrpImgDoc.ReleaseMouseCapture();
            Cursor = Cursors.Arrow;
        }

        private void BZOOM_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (escalar != null && BZOOM.Value>=0.85)
            {
                escalar.ScaleX = e.NewValue;
                escalar.ScaleY = e.NewValue;
            }
        }

       
    }
}
