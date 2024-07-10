using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Timers;

namespace CrearCuentaNueva
{
    /// <summary>
    /// Lógica de interacción para loading.xaml
    /// </summary>
    public partial class loading : Window
    {
        
        public int VTimerOut;
        public System.Windows.Forms.Timer Timer { get; private set; }

        public loading()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Timer = new System.Windows.Forms.Timer();
            Timer.Tick += new EventHandler(Timer_Out);
            Timer.Interval =20;
            Timer.Start();
            
        }

        private void Timer_Out(object sender, EventArgs e)
        {
            if (VTimerOut == 0)
            {
                Timer.Stop();
                Console.WriteLine("Stop");
                this.Hide();
                this.Close();
            }
            else if (VTimerOut > 0)
            {
                VTimerOut--;
                //Console.WriteLine(VTimerOut.ToString());
            }
        }


    }
}
