using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.DirectoryServices;
using System.Threading;

namespace CrearCuentaNueva
{
    /// <summary>
    /// Lógica de interacción para frmLogin.xaml
    /// </summary>
    public partial class frmLogin : Window
    {

        public SqlConnection conexion;
        private AdmisionConnection AdmConexion;
        private AdmisionConnectionSinBD AdmConexionSinBD;
        public int id_usuario;
        public int permiso_previo;
        public int permiso_aprobo;
        public int permiso_foto;
        public int permiso_documento;
        public int cconectar;
        public int pconectar;

        public frmLogin()
        {
            InitializeComponent();
           
        }

        private void FrmLogin1_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsuario.Focus();
            AdmConexionSinBD = new AdmisionConnectionSinBD();// Llama la Clase AdmisionConnection
            LlenarBaseDatos();
            
        }

        private bool autenticar(string dominio, string usuario, string password)
        {
            DirectoryEntry de = new DirectoryEntry(dominio, usuario, password, AuthenticationTypes.Secure);
            try
            {
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.FindOne();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool conectar(string usuario, string pass)
        {
            try
            {
                string path = @"LDAP://uprivada.edu";/* LDAP://172.21.10.175*//*//uprivada.edu*/
                string dominio = @"uprivada";
                usuario = txtUsuario.Text.Trim();
                pass = txtContraseña.Password.Trim();
                string domusuario = dominio + @"\" + usuario;

                bool permiso = autenticar(path, domusuario, pass);
                return permiso;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void LlenarBaseDatos()
        {
            try
            {
                SqlCommand queryBD = new SqlCommand("SELECT name AS NOMBRE_BD, database_id AS ID_BD FROM sys.databases WHERE name LIKE 'NNEC%' OR name LIKE 'DUX%' ORDER BY database_id ASC", AdmConexionSinBD.OpenConnectionSinBD());
                //queryBD.CommandTimeout = 15;
                SqlDataAdapter adaptadorBD = new SqlDataAdapter();
                adaptadorBD.SelectCommand = queryBD;
                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorBD.Fill(dt);

                cbxBasedeDatos.DisplayMemberPath = "NOMBRE_BD";
                cbxBasedeDatos.SelectedValuePath = "ID_BD";
                cbxBasedeDatos.ItemsSource = dt.DefaultView;
                cbxBasedeDatos.SelectedIndex = 0;

                Console.WriteLine("LlenarInformacionDocPendientes request tiempo busquedad" + Convert.ToString(queryBD.CommandTimeout));

            }
            catch (Exception ex)
            {
                this.Close();
                AdmConexionSinBD.CloseConnection1();
                MessageBox.Show("Error:BaseDatos" + ex.InnerException);
                //Console.WriteLine("Error:BaseDatos" + ex.InnerException);
            }

        }

        private void BtnAcceso_Click(object sender, RoutedEventArgs e)
        {
            if (cbxBasedeDatos.Text.Trim().Length == 0)
            {
                MessageBox.Show("Seleccione una Base de datos", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtUsuario.Text.Trim().Length == 0)
            {
                MessageBox.Show("Ingrese el usuario", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsuario.Text = "";
                txtUsuario.Focus();
                return;
            }

            if (txtContraseña.Password.Trim().Length == 0)
            {
                MessageBox.Show("Ingrese la contraseña.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtContraseña.Focus();
                return;
            }

            try
            {
                bool Permiso = conectar(txtUsuario.Text, txtContraseña.Password);

                if (Permiso == true)
                {
                    string mensaje = "OK"; //VALIDACION PERMISOS 

                    AdmConexion = new AdmisionConnection(Convert.ToString(cbxBasedeDatos.Text));// Llama la Clase AdmisionConnection

                    SqlCommand queryAlumno = new SqlCommand("EXEC SP_ADMINSION_CREAR_CUENTA_VALIDACION 1,@USUARIO", AdmConexion.OpenConnection());
                    queryAlumno.Parameters.AddWithValue("@USUARIO", txtUsuario.Text);
                    //queryAlumno.Parameters.AddWithValue("@BASE_DATO",cbxBasedeDatos.Text);

                    AdmConexion.OpenConnection();
                    SqlDataReader resultado = queryAlumno.ExecuteReader();
                    if (resultado.Read())
                    {
                        mensaje = resultado["RESP"].ToString();
                    }
                    //-------INICIO-------------------------------------------------------------------------------------------------------
                    if (mensaje == "OK")
                    {  
                        frmInformacion ventana = new frmInformacion();
                        ventana.baseDeDatos = cbxBasedeDatos.Text;
                        ventana.id_usuario = Int16.Parse(resultado["ID_USUARIO"].ToString());
                        ventana.permiso_previo = Convert.ToInt32(Convert.ToByte(resultado["PERMISO_PREVIO"]));
                        ventana.permiso_aprobo = Convert.ToInt32(Convert.ToByte(resultado["PERMISO_APROBO"]));
                        ventana.permiso_foto = Convert.ToInt32(Convert.ToByte(resultado["PERMISO_FOTO"]));
                        ventana.permiso_documento = Convert.ToInt32(Convert.ToByte(resultado["PERMISO_DOCUMENTO"]));

                        this.Close();
                        ventana.Show();
                        //TABCONTROL tABCONTROL = new TABCONTROL();
                        //this.Close();
                        //tABCONTROL.Show();

                        Console.WriteLine("PermisoRol USUARIO" + id_usuario);

                    }
                    else
                    {
                        AdmConexion.CloseConnection();
                        MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    //-------FIN-------------------------------------------------------------------------------------------------------
                }
                else
                {
                    MessageBox.Show("Usuario o Contraseña incorrecto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtContraseña.Password = "";
                    txtContraseña.Focus();

                }
            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error: LOGIN" + ex.InnerException + ex);
            }

        }

        private void TxtContraseña_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnAcceso_Click(btnAcceso, null);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
