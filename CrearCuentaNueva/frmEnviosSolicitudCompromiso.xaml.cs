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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CrearCuentaNueva
{
    /// <summary>
    /// Lógica de interacción para frmEnviosSolicitudCompromiso.xaml
    /// </summary>
    public partial class frmEnviosSolicitudCompromiso : Window
    {
        public SqlConnection conexion;
        private AdmisionConnection AdmConexion;
        public string baseDeDatos;
        public int cuenta_alumno;
        public string filtro;
        public frmEnviosSolicitudCompromiso()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //conexion = new SqlConnection("server=172.21.10.230; user=SIGA_D; password=MayD4BwithU; database=" + baseDeDatos + "; integrated security=false");
            //conexion = new SqlConnection("server=172.21.10.18; user=jessica; password=jessica.test; database=" + baseDeDatos + "_PRO2; integrated security=false");

            AdmConexion = new AdmisionConnection(baseDeDatos);// Llama la Clase AdmisionConnection
            Console.WriteLine("frmEnviosSolicitudCompromiso " + baseDeDatos);

            LlenarInfEnvioSolicitudComprmiso();

        }


        private void LlenarInfEnvioSolicitudComprmiso()
        {
            try
            {


                SqlCommand queryDetalleInformacionESC = new SqlCommand("EXEC SP_LISTADO_ESTUDIANTES_DOCPENDIENTES 4,@CUENTA_ALUMNO", AdmConexion.OpenConnection());
                queryDetalleInformacionESC.Parameters.AddWithValue("@CUENTA_ALUMNO", cuenta_alumno);
               
                SqlDataAdapter adaptadorDetalleInformacionESC = new SqlDataAdapter();
                adaptadorDetalleInformacionESC.SelectCommand = queryDetalleInformacionESC;
                DataTable dt = new DataTable();
                adaptadorDetalleInformacionESC.Fill(dt);

                dgInformacionEnvioSolicitudCompromiso.AutoGenerateColumns = true;
                dgInformacionEnvioSolicitudCompromiso.ItemsSource = dt.DefaultView;


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:: " + ex);
            }

        }

        private void txtFiltroeESC_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgInformacionEnvioSolicitudCompromiso.SelectedItem = null;
            filtro = txtFiltroeESC.Text;

            //Hace filtro en la columna Documento
            BindingSource dgInformacionSC = new BindingSource();
            dgInformacionSC.DataSource = dgInformacionEnvioSolicitudCompromiso.ItemsSource;
            dgInformacionSC.Filter = dgInformacionEnvioSolicitudCompromiso.Columns[1].Header.ToString() + " LIKE '%" + filtro + "%'";
            dgInformacionEnvioSolicitudCompromiso.ItemsSource = dgInformacionSC;
        }


    }
}
