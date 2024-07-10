using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using System.Timers;
using System.Windows.Threading;

namespace CrearCuentaNueva
{
    /// <summary>
    /// Lógica de interacción para TABCONTROL.xaml
    /// </summary>
    public partial class TABCONTROL : Window
    {

        public int VTimerOut;
        public System.Windows.Forms.Timer Timer { get; private set; }

        public TABCONTROL()
        {
            InitializeComponent();

            Timer = new System.Windows.Forms.Timer();
            Timer.Tick += new EventHandler(Timer_Out);
            Timer.Interval = 20;

        }
        //---------------------------------------------
        private void CLoading(int pvalor)
        {
            VTimerOut = pvalor;
            Timer.Start();

        }

        private void Timer_Out(object sender, EventArgs e)
        {
            if (VTimerOut == 0)
            {
                Timer.Stop();
                Loading.Visibility = Visibility.Collapsed;
                Console.WriteLine("Stop");
            }
            else if (VTimerOut > 0)
            {
                VTimerOut--;
                Console.WriteLine(VTimerOut.ToString());
                Loading.Visibility = Visibility.Visible;

            }

        }
        //---------------------------------------------

        private void TabItem1_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TabItem1.Visibility = Visibility.Visible;
            TabItem1.IsSelected = true;
            GTabItem1.Visibility = Visibility.Visible;

        }
           private void TabItem2_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TabItem2.Visibility = Visibility.Visible;
            TabItem2.IsSelected = false;
            GTabItem2.Visibility = Visibility.Visible;
        }

        private void TabItem3_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TabItem3.Visibility = Visibility.Collapsed;
            GTabItem3.Visibility = Visibility.Collapsed;
        }
        private void TabItem4_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TabItem4.Visibility = Visibility.Collapsed;
            GTabItem4.Visibility = Visibility.Collapsed;
        }
        private void TabItem5_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TabItem5.Visibility = Visibility.Visible;
            TabItem5.IsSelected = false;
            GTabItem5.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CLoading(125);
           
        }

    }
}
