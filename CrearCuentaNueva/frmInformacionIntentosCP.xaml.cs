using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;



namespace CrearCuentaNueva
{
    /// <summary>
    /// Lógica de interacción para frmInformacionIntentosCP.xaml
    /// </summary>
    public partial class frmInformacionIntentosCP : Window
    {

        public SqlConnection conexion;

        public string baseDeDatos;
        public int id_admision;
        public string filtro;
        private AdmisionConnection AdmConexion;

        //-------------------

        public frmInformacionIntentosCP()
        {
            InitializeComponent();

        }

        private void VtnIntentosCall_Loaded(object sender, RoutedEventArgs e)
        {
            //conexion = new SqlConnection("server=172.21.10.230; user=SIGA_D; password=MayD4BwithU; database=" + baseDeDatos + "; integrated security=false");
            //conexion = new SqlConnection("server=172.21.10.18; user=jessica; password=jessica.test; database=" + baseDeDatos + "_PRO2; integrated security=false");
   
            AdmConexion = new AdmisionConnection(baseDeDatos);// Llama la Clase AdmisionConnection
            Console.WriteLine("FrmInformacionIntentosCP " + baseDeDatos);

            LlenarListadoDetalleInformacionICP();
        }

        private void TxtFiltroICP_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgInformacionIntentosCPrevio.SelectedItem = null;
            filtro = txtFiltroICP.Text;

            //Hace filtro en la columna Comentario
            BindingSource dgInformacionICP = new BindingSource();
            dgInformacionICP.DataSource = dgInformacionIntentosCPrevio.ItemsSource;
            dgInformacionICP.Filter = dgInformacionIntentosCPrevio.Columns[1].Header.ToString() + " LIKE '%" + filtro + "%'";
            dgInformacionIntentosCPrevio.ItemsSource = dgInformacionICP;

        }

        
        private void LlenarListadoDetalleInformacionICP()
        {
            try
            {
                SqlCommand queryDetalleInformacionICP = new SqlCommand("EXEC SP_ADMISION_INFORMACION_INTENTOS_CP @ID_ADMISION", AdmConexion.OpenConnection());
                queryDetalleInformacionICP.Parameters.AddWithValue("@ID_ADMISION", id_admision);

                SqlDataAdapter adaptadorDetalleInformacionICP = new SqlDataAdapter();
                adaptadorDetalleInformacionICP.SelectCommand = queryDetalleInformacionICP;
                DataTable dt = new DataTable();
                adaptadorDetalleInformacionICP.Fill(dt);

                dgInformacionIntentosCPrevio.AutoGenerateColumns = true;
                dgInformacionIntentosCPrevio.ItemsSource = dt.DefaultView;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:: " + ex);
            }

        }


    }

}

