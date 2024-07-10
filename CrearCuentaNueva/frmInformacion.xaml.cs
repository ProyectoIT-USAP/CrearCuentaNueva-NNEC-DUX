using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CrearCuentaNueva
{
    /// <summary>
    /// Lógica de interacción para frmInformacion.xaml
    /// </summary>
    public partial class frmInformacion : Window
    {

        public string baseDeDatos;
        private AdmisionConnection AdmConexion;
        private AdmisionConnection AdmConexion1;
        private AdmisionConnection AdmConexion2;
        private AdmisionConnection AdmConexion3;

        //private DocumentosPendientes LlenarAlumnosDocPendientes;

        //----
        public int id_deptoActual;
        public int id_deptoNac;
        public int id_muniActual;
        public int id_muniNac;
        public int id_colActual;
        public int id_plan_seleccionado;
        public int id_admision_global;
        public int id_admision_globalPrevio;
        public int id_control_matricula;
        public int errorValidacion;
        public int errorValidacionRegistro;
        public int ValidacionGAR;
        public int ValidacionRPE;
        public int ValidacionSC;
        public string ValidacionesPrevio = "";
        public string ValidacionesFoto = "";
        public string ValidacionesDocPendientes = "";
        public int Aviso;
        public int id_usuario;
        public int permiso_previo;
        public int permiso_aprobo;
        public int permiso_foto;
        public int permiso_documento;
        public string img_doc = null;
        public int id_comentario;
        public int id_fecha_intentos;
        public string comentario_previo;
        public string intento_fecha_previo;
        public int id_intentos_call = 1;
        public string FiltroCuenta;
        public string Filtros;
        public string FiltrosRI;
        public int cuenta_alumno;
        public string Periodo_Activo;
        public int DropDownRefreshAnioPeriodo;
        List<ADM_DUPLICADA> ListAdmisionDuplicada= new List<ADM_DUPLICADA>();
        List<string> ListAdmisionPrevio = new List<string>();
        //---LOADING
        public int VTimerOut;
        public System.Windows.Forms.Timer Timer { get; private set; }
        //---
        public frmInformacion()
        {
            InitializeComponent();

            Timer = new System.Windows.Forms.Timer();
            Timer.Tick += new EventHandler(Timer_Out);
            Timer.Interval = 20;



        }

        class ADM_DUPLICADA
        {
            public string ID_ADMISION { get; set; }
            public string CUENTA_ALUMNO { get; set; }
            public string MENSAJE { get; set; }
            public string COLOR { get; set; }
            public string INTENTOS_DIAS { get; set; }
        }

        private void vtnInformacion_Loaded(object sender, RoutedEventArgs e)
        {
            //conexion = new SqlConnection("server=172.21.10.18; user=jessica; password=jessica.test; database=" + baseDeDatos + "_PRO2; integrated security=false");
            //conexion2 = new SqlConnection("server=172.21.10.18; user=jessica; password=jessica.test; database=" + baseDeDatos + "_PRO2; integrated security=false");
            //conexion3 = new SqlConnection("server=172.21.10.18; user=jessica; password=jessica.test; database=" + baseDeDatos + "_PRO2; integrated security=false");

            AdmConexion = new AdmisionConnection(baseDeDatos);// Llama la Clase AdmisionConnection
            AdmConexion1 = new AdmisionConnection(baseDeDatos);
            AdmConexion2 = new AdmisionConnection(baseDeDatos);
            AdmConexion3 = new AdmisionConnection(baseDeDatos);
            Console.WriteLine("frmInformacion " + baseDeDatos);


        }


        //public async Task LlenasListado()
        //{
            
        //    await Task.Run(() =>
        //    {
        //        Thread.Sleep(8000);
                
        //        Console.WriteLine("INICIO DE PRUEBA");
        //        for (int i = 0; i < 10; i++)
        //        {  
        //            Console.WriteLine("Llenar Listado" + i);
        //            Task.Delay(1000).Wait();
        //        }
        //        LlenarListadoSolicitantesPrevio("2024-2");
            
        //        Console.WriteLine("FIN DE PRUEBA");
        //    }
        //    ) ;
        //}



        //----------------------------------------------------
        //----LOADING
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
                DockPanel.Visibility = Visibility.Collapsed;
                Console.WriteLine("Stop");
            }
            else if (VTimerOut > 0)
            {
                VTimerOut--;
                Console.WriteLine(VTimerOut.ToString());
                DockPanel.Visibility = Visibility.Visible;

            }

        }
        
        //---------------------------------------------------

        //--------------------------------PERIODO_ANIO_ACTIVO
        //public void PeriodoActivo()
        //{
        //    try
        //    {
        //        SqlCommand queryPeriodoActivo = new SqlCommand("SP_LISTADO_SOLICITANTES_FOTO 5,NULL", AdmConexion.OpenConnection());
        //        SqlDataReader ResultPeriodoActivo = queryPeriodoActivo.ExecuteReader();

        //        AdmConexion.OpenConnection();
        //        if (ResultPeriodoActivo.Read())
        //        {

        //            Periodo_Activo = ResultPeriodoActivo["PERIODO_ANIO"].ToString();
        //            Console.WriteLine("SP_LISTADO_SOLICITANTES_FOTO INCISO 5 " + Periodo_Activo);
        //        }
        //        AdmConexion.CloseConnection();

        //    }
        //    catch (Exception ex)
        //    {
        //        AdmConexion.CloseConnection();
        //        MessageBox.Show("Error::ActivoPeriodo" + ex);
        //    }
        //}

        //---------------------------------------------------

        //----------------------LLENARLISTADOSOLICITANTESPREVIO
        private async void LlenarListadoSolicitantesPrevio(string periodo_anio)
        {

            if (cbxAnioPeriodo.SelectedValue != null)
            {
                try
                { 
                    await Task.Delay(3500);
                    SqlCommand queryDetalleSolicitantesPrevio = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_PREVIO 1,@ANIO_PERIODO,NULL", AdmConexion.OpenConnection());
                    queryDetalleSolicitantesPrevio.Parameters.AddWithValue("@ANIO_PERIODO", "2024-2");

                    SqlDataAdapter adaptadorDetalleSolicitantesPrevio = new SqlDataAdapter();
                    adaptadorDetalleSolicitantesPrevio.SelectCommand = queryDetalleSolicitantesPrevio;
                    System.Data.DataTable dt1 = new System.Data.DataTable();
                    adaptadorDetalleSolicitantesPrevio.Fill(dt1);

                    dgSolicitantesPrevio.AutoGenerateColumns = true;
                    dgSolicitantesPrevio.ItemsSource = dt1.DefaultView;

                    AdmConexion.CloseConnection();
                }
                catch (Exception ex)
                {
                    AdmConexion.CloseConnection();
                    MessageBox.Show("Error:: " + ex);
                }

                //---------------------------------------------------------------------------------------------------------------------------
                //---------ADMISION DUPLICADAS
                try
                {
                    SqlCommand queryAdmisionDuplicadaPrevio = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_PREVIO 3,NULL,NULL", AdmConexion.OpenConnection());
                    SqlDataAdapter adaptadorAdmisionDuplicadaPrevio = new SqlDataAdapter();
                    adaptadorAdmisionDuplicadaPrevio.SelectCommand = queryAdmisionDuplicadaPrevio;
                    System.Data.DataTable dt1 = new System.Data.DataTable();
                    adaptadorAdmisionDuplicadaPrevio.Fill(dt1);

                    Console.WriteLine("LISTADO_ADMISION_DUPLICADA_PREVIO" + " " + dt1.Rows.Count + " ");

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        Console.WriteLine(dt1.Rows[i]["ID_ADMINSION"].ToString() + " , " + dt1.Rows[i]["CUENTA_ALUMNO"].ToString());
                        ListAdmisionDuplicada.Add(new ADM_DUPLICADA()
                        {
                            ID_ADMISION = dt1.Rows[i]["ID_ADMINSION"].ToString()
                                                                       ,
                            CUENTA_ALUMNO = dt1.Rows[i]["CUENTA_ALUMNO"].ToString()
                                                                       ,
                            MENSAJE = dt1.Rows[i]["MENSAJE"].ToString()
                                                                       ,
                            COLOR = dt1.Rows[i]["COLOR"].ToString()
                                                                       ,
                            INTENTOS_DIAS = dt1.Rows[i]["INTENTOS_DIAS"].ToString()
                        });

                    }

                    Console.WriteLine("SE AGREGO EN LA LISTA ListAdmisionDuplicada " + ListAdmisionDuplicada.Count());

                    AdmConexion.CloseConnection();

                }
                catch (Exception ex)
                {
                    AdmConexion.CloseConnection();
                    MessageBox.Show("Error:: " + ex);
                }

                Console.WriteLine("FIN DE PRUEBA   " + "LlenarListadoSolicitantesPrevio" + cbxAnioPeriodo.SelectedValue);

            }

        }



        //-------------------------LLENARLISTADOSOLICITANTES
        private async void LlenarListadoSolicitantes()
        {
            try
            {
                await Task.Delay(3500);
                SqlCommand queryDetalleSolicitantes = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_REGISTRO 1 ,NULL,NULL", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorDetalleSolicitantes = new SqlDataAdapter();
                adaptadorDetalleSolicitantes.SelectCommand = queryDetalleSolicitantes;

                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorDetalleSolicitantes.Fill(dt);

                dgSolicitantes.AutoGenerateColumns = true;
                dgSolicitantes.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();
            }
            catch (SqlException ee)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error::TABREGISTRO " + ee);
                this.Close();
            }

        }

        //--------LLENAR LISTA DE ESTUDIANTES CON FOTOS PENDIENTE O NUEVO INTENTO
        private async void LlenarListadoAlumnosFoto()
        {
            try
            {
                await Task.Delay(3500);
                SqlCommand queryDetalleAlumnoFoto = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_FOTO 1,null", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorDetalleAlumnoFoto = new SqlDataAdapter();
                adaptadorDetalleAlumnoFoto.SelectCommand = queryDetalleAlumnoFoto;

                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorDetalleAlumnoFoto.Fill(dt);

                dgSolicitantesFoto.AutoGenerateColumns = true;
                dgSolicitantesFoto.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();
            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }

        }


        private void LlenarListadoAlumnosFotoRI( string cuenta)
        {
            try
            {
                
                SqlCommand queryAlumnosFotoRI = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_FOTO 4,@CUENTA_ALUMNO", AdmConexion.OpenConnection());
                queryAlumnosFotoRI.Parameters.AddWithValue("@CUENTA_ALUMNO", cuenta);
                SqlDataAdapter adaptadorAlumnosFotoRI = new SqlDataAdapter();
                adaptadorAlumnosFotoRI.SelectCommand = queryAlumnosFotoRI;

                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorAlumnosFotoRI.Fill(dt);

                dgSolicitantesFoto.AutoGenerateColumns = true;
                dgSolicitantesFoto.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();

            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: LlenarListadoAlumnosFotoRI" + ex);
            }

        }

        //--------LLENAR LISTA DE ESTUDIANTES CON DOCUMENTOS PENDIENTES
        private async void LlenarListadoAlumnosDocPendientes()
        {
            try
            {
                await Task.Delay(3500);

                SqlCommand queryDetalleAlumnoDocPendientes = new SqlCommand("EXEC SP_LISTADO_ESTUDIANTES_DOCPENDIENTES 1,null", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorDetalleAlumnoDocPendientes = new SqlDataAdapter();
                adaptadorDetalleAlumnoDocPendientes.SelectCommand = queryDetalleAlumnoDocPendientes;
                  
                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorDetalleAlumnoDocPendientes.Fill(dt);
                dgEstudiantesDocPendientes.AutoGenerateColumns = true;
                dgEstudiantesDocPendientes.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();

            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }

        }

        private void LlenarListadoEstudDocPendientes() // ESTUDIANTES QUE NO SE ENCUENTRA EN EL LISTADO DEL PERIODO ACTIVO O UN AÑO ANTERIOR
        {
            string filtrodoc = txtFiltroDocPendientes.Text;
            try
            {
                SqlCommand queryDetalleAlumnoDocPendientes = new SqlCommand("EXEC SP_LISTADO_ESTUDIANTES_DOCPENDIENTES 5,@CUENTA_ALUMNO", AdmConexion.OpenConnection());
                queryDetalleAlumnoDocPendientes.Parameters.AddWithValue("@CUENTA_ALUMNO", filtrodoc);
                SqlDataAdapter adaptadorDetalleAlumnoDocPendientes = new SqlDataAdapter();
                adaptadorDetalleAlumnoDocPendientes.SelectCommand = queryDetalleAlumnoDocPendientes;

                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorDetalleAlumnoDocPendientes.Fill(dt);
                dgEstudiantesDocPendientes.AutoGenerateColumns = true;
                dgEstudiantesDocPendientes.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();
            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }

        }



        //--------LLENAR LISTA DE ESTUDIANTES CON DOCUMENTOS PENDIENTES
        private void LlenarDocPendientes(int CUENTA_ALUMNO)
        {
            try
            {
                SqlCommand queryDocPendientes = new SqlCommand("EXEC SP_LISTADO_ESTUDIANTES_DOCPENDIENTES 3,@CUENTA_ALUMNO", AdmConexion.OpenConnection());
                queryDocPendientes.Parameters.AddWithValue("@CUENTA_ALUMNO", CUENTA_ALUMNO);
                SqlDataAdapter adaptadorDocPendientes = new SqlDataAdapter();
                adaptadorDocPendientes.SelectCommand = queryDocPendientes;

                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorDocPendientes.Fill(dt);

                dgDocumentosPendientesDetalle.AutoGenerateColumns = true;
                dgDocumentosPendientesDetalle.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();

            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }

        }

        public void LlenarComboboxAnioPeriodo()
        {
            try
            {
                SqlCommand queryAnioPeriodo = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_PREVIO 2,NULL,NULL", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorAnioPeriodo = new SqlDataAdapter();
                adaptadorAnioPeriodo.SelectCommand = queryAnioPeriodo;
                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorAnioPeriodo.Fill(dt);

                if (DropDownRefreshAnioPeriodo != 1)
                {
                    cbxAnioPeriodo.DisplayMemberPath = "ANIO_PERIODO";
                    cbxAnioPeriodo.SelectedValuePath = "ANIO_PERIODO";
                    cbxAnioPeriodo.ItemsSource = dt.DefaultView;
                    cbxAnioPeriodo.SelectedIndex = 0;
                }
                else if ((dt.Rows.Count.ToString()!=cbxAnioPeriodo.Items.Count.ToString()) && DropDownRefreshAnioPeriodo == 1)
                {
                    //Console.WriteLine("Diferencias" + dt.Rows.Count.ToString() + " " + cbxAnioPeriodo.Items.Count.ToString());
                    cbxAnioPeriodo.DisplayMemberPath = "ANIO_PERIODO";
                    cbxAnioPeriodo.SelectedValuePath = "ANIO_PERIODO";
                    cbxAnioPeriodo.ItemsSource = dt.DefaultView;
                    cbxAnioPeriodo.SelectedIndex = 0;
                }

                AdmConexion.CloseConnection();
            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: LlenarAnioPeriodo " + ex);
            }

            
        }


        //---------------------------INICIO INFORMACION GENERAL DE LOS SOLICITANTES DE ADMISION(REGISTRO)-------------------
        private void TxtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgSolicitantes.SelectedItem = null;
            Filtros = txtFiltro.Text;

            //Hace filtro por el nombre
            System.Windows.Forms.BindingSource dgSolicitRegistro = new System.Windows.Forms.BindingSource();
            dgSolicitRegistro.DataSource = dgSolicitantes.ItemsSource;
            dgSolicitRegistro.Filter = string.Format("NOMBRE LIKE '%{0}%' OR APELLIDO LIKE '%{1}%'", Filtros, Filtros);
            dgSolicitantes.ItemsSource = dgSolicitRegistro;

        }

        private void DgSolicitantes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSolicitantes.ItemsSource != null)
            {
                try
                {
                    if (dgSolicitantes.SelectedItem != null)
                    {
                        btnCrearCuenta.IsEnabled = true;
                        btnEliminarRegistro.IsEnabled = true;
                        System.Data.DataRowView row = (System.Data.DataRowView)dgSolicitantes.SelectedItems[0];
                        LlenarInformacion(Int32.Parse(row["ID"].ToString()));
                        id_admision_global = Int32.Parse(row["ID"].ToString());

                        Console.WriteLine(Int32.Parse(row["ID"].ToString()));

                    }
                    AdmConexion.CloseConnection();
                }
                catch (Exception ex)
                {
                    AdmConexion.CloseConnection();
                    MessageBox.Show("Error::DgSolicitantes_SelectionChanged " + ex);
                }
            }
        }


        public void LlenarInformacion(int id_admision)
        {
            try
            {
                string espacios_segundo_nombre = "";
                string espacios_primer_apellido = "";
                string espacios_segundo_apellido = "";

                SqlCommand queryAlumno = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_REGISTRO 2, @ID_ADMISION,NULL", AdmConexion.OpenConnection());
                queryAlumno.Parameters.AddWithValue("@ID_ADMISION", id_admision);

                if (Convert.ToInt32(AdmConexion.OpenConnection().ConnectionTimeout.ToString()) == 15 || Convert.ToInt32(queryAlumno.CommandTimeout) == 30)
                {
                    AdmConexion.OpenConnection();
                    SqlDataReader resultado = queryAlumno.ExecuteReader();
                    if (resultado.Read())
                    {
                        //---------------------------------------------------------------------------

                        espacios_segundo_nombre = resultado["SEGUNDO_NOMBRE"].ToString().ToUpper();
                        espacios_primer_apellido = resultado["PRIMER_APELLIDO"].ToString().ToUpper();
                        espacios_segundo_apellido = resultado["SEGUNDO_APELLIDO"].ToString().ToUpper();

                        //-----------------------------------------------------------------------------

                        txtprimerApellido.Text = resultado["PRIMER_APELLIDO"].ToString().ToUpper();
                        txtSegundoApellido.Text = resultado["SEGUNDO_APELLIDO"].ToString().ToUpper();
                        txtNombres.Text = resultado["PRIMER_NOMBRE"].ToString().ToUpper() +(espacios_segundo_nombre == ""? "":" " )+ resultado["SEGUNDO_NOMBRE"].ToString().ToUpper();
                        
                        txtNombreCompleto.Text = resultado["PRIMER_NOMBRE"].ToString().ToUpper() + (espacios_segundo_nombre == "" ? "" : " ") + 
                                                 resultado["SEGUNDO_NOMBRE"].ToString().ToUpper() + (espacios_primer_apellido == "" ? "" : " ") +
                                                 resultado["PRIMER_APELLIDO"].ToString().ToUpper() + (espacios_segundo_apellido == "" ? "" : " ") + 
                                                 resultado["SEGUNDO_APELLIDO"].ToString().ToUpper();

                        cbxPaisActual.SelectedValue =(resultado["ID_PAIS"].ToString() == null && resultado["ID_PAIS"].ToString() == "") ? 0 : Int16.Parse(resultado["ID_PAIS"].ToString());
                        cbxPais.SelectedValue = (resultado["ID_PAIS_NACIMIENTO"].ToString() == null && resultado["ID_PAIS_NACIMIENTO"].ToString() == "") ? 0 : Int16.Parse(resultado["ID_PAIS_NACIMIENTO"].ToString());

                        id_deptoActual = (resultado["ID_DEPARTAMENTO"].ToString()==null && resultado["ID_DEPARTAMENTO"].ToString()=="") ? 0 : Int16.Parse(resultado["ID_DEPARTAMENTO"].ToString());
                        id_deptoNac = (resultado["ID_DEPARTAMENTO_NACIMIENTO"].ToString() == null && resultado["ID_DEPARTAMENTO_NACIMIENTO"].ToString() == "") ? 0 : Int16.Parse(resultado["ID_DEPARTAMENTO_NACIMIENTO"].ToString());

                        id_muniNac = (resultado["ID_MUNICIPIO_NACIMIENTO"].ToString() == null && resultado["ID_MUNICIPIO_NACIMIENTO"].ToString() == "") ? 0 : Int16.Parse(resultado["ID_MUNICIPIO_NACIMIENTO"].ToString());
                        id_muniActual = (resultado["ID_MUNICIPIO"].ToString() == null && resultado["ID_MUNICIPIO"].ToString() == "") ? 0 : Int16.Parse(resultado["ID_MUNICIPIO"].ToString());

                        id_colActual = (resultado["ID_COLONIA"].ToString() == null && resultado["ID_COLONIA"].ToString() == "") ? 0 : Int16.Parse(resultado["ID_COLONIA"].ToString());

                        cbxCampusEstudio.SelectedValue = resultado["ID_SUCURSAL_AM"].ToString();
                        cbxSucursal.SelectedValue = resultado["ID_SUCURSAL_AM"].ToString();
                        txtTelefono1.Text = resultado["TELEFONO_CASA"].ToString();
                        txtTelefono2.Text = resultado["CELULAR"].ToString();

                        txtCorreo2.Text = resultado["EMAIL_PERSONAL"].ToString();
                        txtCompania.Text = resultado["NOMBRE_EMPRESA"].ToString().ToUpper();
                        txtTelefonoCompania.Text = resultado["TELEFONO_EMPRESA"].ToString();
                        txtPuesto.Text = resultado["PUESTO"].ToString().ToUpper();
                        txtObservacion.Text = resultado["OBSERVACION_APROBO"].ToString();

                        if (resultado["GENERO"].ToString() == "M ")
                        {
                            optMaculino.IsChecked = true;
                        }
                        else if (resultado["GENERO"].ToString() == "F ")
                        {
                            optFemenino.IsChecked = true;
                        }

                        cbxTipoIdentificacion.SelectedValue = resultado["ID_TIPO_IDENTIFICACION"].ToString();
                        cbxEstadoCivil.SelectedValue = resultado["ID_ESTADO"].ToString();
                        txtIdentidad.Text = resultado["IDENTIFICACION"].ToString().Replace("-", "").ToUpper();
                        txtDireccion.Text = resultado["DIRECCCION"].ToString().ToUpper();
                        txtFechaNacimiento.Text = resultado["FECHA_NACIMIENTO"].ToString();

                        LlenarPlanes(Int16.Parse(resultado["ID_PLAN"].ToString()));
                        id_plan_seleccionado = Int16.Parse(resultado["ID_PLAN"].ToString());
                        LlenarFamiliares(id_admision);
                        LlenarInstituto(id_admision);

                        txtCuenta.Text = "";
                        txtCorreo1.Text = "";

                        if (resultado["OBSERVACION_APROBO"].ToString() != "" && resultado["OBSERVACION_APROBO"].ToString() != null 
                           && Convert.ToInt32(resultado["EXISTE_OTROS"].ToString())==0)
                        {
                           MessageBox.Show("¡Hay información en la pestaña Observación del estudiante!", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (resultado["OBSERVACION_APROBO"].ToString() != "" && resultado["OBSERVACION_APROBO"].ToString() != null
                           && Convert.ToInt32(resultado["EXISTE_OTROS"].ToString()) >=1)
                        {
                            MessageBox.Show("--> ¡Hay información de otro instituto o título en el admisión del estudiante! \r\n\n" +
                                "--> ¡Hay información en la pestaña Observación del estudiante! ",
                                "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (Convert.ToInt32(resultado["OBSERVACION_APROBO"].ToString().Length)==0  && Convert.ToInt32(resultado["EXISTE_OTROS"].ToString()) >= 1)
                        {
                            MessageBox.Show("¡Hay información de otro instituto o título en el admisión del estudiante!", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }


                        Console.WriteLine("LLENAR INFORMACION LONGITUD OBSERVACION_APROBO" + resultado["OBSERVACION_APROBO"].ToString().Length);
                    }
                    //conexion.Close();
                    AdmConexion.CloseConnection();
                    CbxPaisActual_SelectionChanged(cbxPaisActual, null);
                    CbxPais_SelectionChanged(cbxPais, null);
                    cbxDepartamentoActual.SelectedValue = id_deptoActual;
                    cbxDepartamento.SelectedValue = id_deptoNac;
                    CbxDepartamentoActual_SelectionChanged(cbxDepartamentoActual, null);
                    CbxDepartamento_SelectionChanged(cbxDepartamento, null);
                    cbxMunicipioActual.SelectedValue = id_muniActual;                    
                    cbxMunicipio.SelectedValue = id_muniNac;
                    CbxMunicipioActual_SelectionChanged(cbxMunicipioActual, null);
                    cbxColoniaActual.SelectedValue = id_colActual;

                    CLoading(Convert.ToInt32(AdmConexion.OpenConnection().ConnectionTimeout.ToString()));

                }
                else
                {
                    MessageBox.Show("Error de Conexión!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Error);
                    AdmConexion.CloseConnection();
                }

                AdmConexion.CloseConnection();

            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: LlenarInformacion " + ex);
            }


        }

        //---------------------------FIN INFORMACION GENERAL DE LOS SOLICITANTES DE ADMISION(REGISTRO)-------------------

        //------------------------------------------------INFORMACION SELECCION PREVIO-----------------------------------
        private void TxtFiltroPrevio_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgSolicitantesPrevio.SelectedItem = null;
            Filtros = txtFiltroPrevio.Text;

            //Hace filtro por el nombre
            System.Windows.Forms.BindingSource dgSolicitPrevio = new System.Windows.Forms.BindingSource();
            dgSolicitPrevio.DataSource = dgSolicitantesPrevio.ItemsSource;
            dgSolicitPrevio.Filter = string.Format("NOMBRE LIKE '%{0}%' OR APELLIDO LIKE '%{1}%'", Filtros, Filtros);
            dgSolicitantesPrevio.ItemsSource = dgSolicitPrevio;
        }


        private void dgSolicitantesPrevio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSolicitantesPrevio.ItemsSource != null)
            {
                try
                {
                    if (dgSolicitantesPrevio.SelectedItem != null)
                    {
                        btnInformacionIntentoCPrevio.IsEnabled = true;
                        System.Data.DataRowView row = (System.Data.DataRowView)dgSolicitantesPrevio.SelectedItems[0];
                        LlenarInformacionPrevio(Int32.Parse(row["ID_ADMINSION"].ToString()));
                        id_admision_globalPrevio = Int32.Parse(row["ID_ADMINSION"].ToString());
                        Console.WriteLine(" ENTRO dgSolicitantesPrevio_SelectionChanged " + Int32.Parse(row["ID_ADMINSION"].ToString()));
                    }

                }
                catch (Exception ex)
                {
                    AdmConexion.CloseConnection();
                    MessageBox.Show("Error:: dgSolicitantesPrevio_SelectionChanged  " + ex);
                }
            }
        }

        public void LlenarInformacionPrevio(int id_admisionPrevio)
        {
            try
            {   SqlCommand queryAlumnoPrevio = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_PREVIO 4,NULL,@ID_ADMISION", AdmConexion.OpenConnection());
                queryAlumnoPrevio.Parameters.AddWithValue("@ID_ADMISION", id_admisionPrevio);

                CLoading(Convert.ToInt32(AdmConexion.OpenConnection().ConnectionTimeout.ToString()));
                if (Convert.ToInt32(AdmConexion.OpenConnection().ConnectionTimeout.ToString()) == 15 || Convert.ToInt32(queryAlumnoPrevio.CommandTimeout) == 30)
                {
                    AdmConexion.OpenConnection();
                    SqlDataReader resultadoPrevio = queryAlumnoPrevio.ExecuteReader();
                    if (resultadoPrevio.Read())
                    {
                        txtTelefono1Previo.Text = resultadoPrevio["TELEFONO_CASA"].ToString();
                        txtTelefono2Previo.Text = resultadoPrevio["CELULAR"].ToString();
                        txtCorreo2Previo.Text = resultadoPrevio["EMAIL_PERSONAL"].ToString();
                        cbxTipoIdentificacionPrevio.SelectedValue = resultadoPrevio["ID_TIPO_IDENTIFICACION"].ToString();
                        txtIdentidadPrevio.Text = resultadoPrevio["IDENTIFICACION"].ToString().Replace("-", "").ToUpper();
                        txtAsignada.Text = resultadoPrevio["ASIGNADA"].ToString();
                        cbxEstadollamadaPrevio.SelectedValue = resultadoPrevio["ESTADO_LLAMADA"].ToString();
                        txtComentarioPrevio.Text = resultadoPrevio["VCOMENTARIO"].ToString();
                        id_comentario = Convert.ToInt32(resultadoPrevio["VIDCOMENTARIO"].ToString());
                        id_fecha_intentos = Convert.ToInt32(resultadoPrevio["VIDINTENTOS_FECHA"].ToString());

                        //----------------------------------
                        comentario_previo = resultadoPrevio["VCOMENTARIO"].ToString();
                        intento_fecha_previo = resultadoPrevio["VINTENTOS_FECHA"].ToString();

                        //----------------------------------

                        txtObservacionPrevioPruebas.Text = resultadoPrevio["COMENTARIO"].ToString();// QUITAR SOLAMENTE ES PRUEBA

                        txtObservacionPrevio.Text = resultadoPrevio["OBSERVACION_APROBO"].ToString();
                        cbxIntentosLlamadas.Items.Clear();
                        id_intentos_call = Int32.Parse(resultadoPrevio["INTENTOS_CALL"].ToString());
                        LlenarIntentosCall(id_intentos_call);
                        cbxIntentosLlamadas.SelectedValue = Int32.Parse(resultadoPrevio["INTENTOS_CALL"].ToString());

                        if (resultadoPrevio["VINTENTOS_FECHA"].ToString() != "" && resultadoPrevio["VINTENTOS_FECHA"].ToString() != null && Convert.ToInt32(cbxIntentosLlamadas.SelectedValue.ToString()) == id_intentos_call)
                        {
                            txtIntentosFechaPrevio.Text = resultadoPrevio["VINTENTOS_FECHA"].ToString();
                        }
                        else if (Convert.ToInt32(cbxIntentosLlamadas.SelectedValue.ToString()) != id_intentos_call)
                        {
                            txtIntentosFechaPrevio.Text = resultadoPrevio["INTENTOS_FECHA"].ToString(); ;
                        }
                        else
                        {
                            txtIntentosFechaPrevio.Text = DateTime.Now.ToString("d");
                        }
                     //-----------------------------------------------------------------------------------------------------------------
                        string MensajePrevio = "";
                        string ColorPrevio = "Black";
                        int Estado_SolicitudP=0;

                        foreach (ADM_DUPLICADA ADMISIONDUPLICADA in ListAdmisionDuplicada)
                        {
                            if (ADMISIONDUPLICADA.ID_ADMISION == Convert.ToString(id_admisionPrevio) && ADMISIONDUPLICADA.CUENTA_ALUMNO != "" && Convert.ToInt32(ADMISIONDUPLICADA.INTENTOS_DIAS)<=0)
                            {//ADMISIONES DUPLICADAS QUE TIENE CUENTA CREADA
                                MensajePrevio = ADMISIONDUPLICADA.MENSAJE;
                                ColorPrevio = ADMISIONDUPLICADA.COLOR;
                                Estado_SolicitudP = 1;
                            }
                            else if(ADMISIONDUPLICADA.ID_ADMISION == Convert.ToString(id_admisionPrevio) && ADMISIONDUPLICADA.CUENTA_ALUMNO=="" && Convert.ToInt32(ADMISIONDUPLICADA.INTENTOS_DIAS) <= 0)
                            {//ADMISIONES DUPLICADAS QUE TIENE CUENTA CREADA
                                MensajePrevio = ADMISIONDUPLICADA.MENSAJE;
                                ColorPrevio = ADMISIONDUPLICADA.COLOR;
                                Estado_SolicitudP = 2;
                            }

                            else if(ADMISIONDUPLICADA.ID_ADMISION == Convert.ToString(id_admisionPrevio) && ADMISIONDUPLICADA.CUENTA_ALUMNO == "" && Convert.ToInt32(ADMISIONDUPLICADA.INTENTOS_DIAS) >= 0)
                            {
                                MensajePrevio = ADMISIONDUPLICADA.MENSAJE;
                                ColorPrevio = ADMISIONDUPLICADA.COLOR;
                                Estado_SolicitudP = 3;
                            }
                        }

                        if (Estado_SolicitudP == 1)
                        {//ADMISIONES DUPLICADAS QUE TIENE CUENTA CREADA
                            Estado_Solicitud_Previo.Text = MensajePrevio;
                            Estado_Solicitud_Previo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorPrevio.ToString()));
                            btnAprobadaPrevio.IsEnabled = false;
                            btnGuardarPrevio.IsEnabled = false;
                            btnRechazadaPrevio.IsEnabled = true;

                            Console.WriteLine("LlenarInformacionPrevio" + id_admisionPrevio);
                            Console.WriteLine("LlenarInformacionPrevio" + MensajePrevio);
                        }
                        else if (Estado_SolicitudP == 2)
                        {//ADMISIONES DUPLICADAS QUE NO TIENE CUENTA CREADA
                            Estado_Solicitud_Previo.Text = MensajePrevio;
                            Estado_Solicitud_Previo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorPrevio.ToString()));
                            btnAprobadaPrevio.IsEnabled = false;
                            btnGuardarPrevio.IsEnabled = false;
                            btnRechazadaPrevio.IsEnabled = true;

                            Console.WriteLine("LlenarInformacionPrevio" + id_admisionPrevio);
                            Console.WriteLine("LlenarInformacionPrevio" + MensajePrevio);
                        }
                        else if (Estado_SolicitudP == 3)
                        {//ADMISIONES CON VARIOS INTENTOS DE LLAMADAS
                            Estado_Solicitud_Previo.Text = MensajePrevio;
                            Estado_Solicitud_Previo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorPrevio.ToString()));
                            btnAprobadaPrevio.IsEnabled = true;
                            btnGuardarPrevio.IsEnabled = true;
                            btnRechazadaPrevio.IsEnabled = true;

                            Console.WriteLine("LlenarInformacionPrevio" + id_admisionPrevio);
                            Console.WriteLine("LlenarInformacionPrevio" + MensajePrevio);
                        }
                        else if (Estado_SolicitudP == 0)
                        {//ADMISIONES QUE NO TIENE DUPLICIDAD
                            Estado_Solicitud_Previo.Text = "";
                            btnAprobadaPrevio.IsEnabled = true;
                            btnGuardarPrevio.IsEnabled = true;
                            btnRechazadaPrevio.IsEnabled = true;

                            Console.WriteLine("LlenarInformacionPrevio" + id_admisionPrevio);
                            Console.WriteLine("LlenarInformacionPrevio no tiene mensaje");
                        }
                      //-----------------------------------------------------------------------------------------------------------------
                    }
                    else
                    {
                        MessageBox.Show("Error de Conexión!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Error);
                        AdmConexion.CloseConnection();
                    }

                }
                //conexion.Close();
                AdmConexion.CloseConnection();

            }
            catch (Exception ex)
            {
                //conexion.Close();
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: LlenarInformacionPrevio" + ex);
            }
        }

        //-----------------------------------------FIN INFORMACION SELECCION PREVIO------------------------------------------------

        //-----------------------------------------INICIO INFORMACION ALUMNO FOTO---------------------------------------------

        private void txtFiltroFoto_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgSolicitantesFoto.SelectedItem = null;
            FiltroCuenta = txtFiltroFoto.Text;

            if (FiltroCuenta.Length >= 3)
            {
                //Hace filtro en la columna cuenta alumno en el datagridview
                System.Windows.Forms.BindingSource dgSolicitantes_Foto = new System.Windows.Forms.BindingSource();
                dgSolicitantes_Foto.DataSource = dgSolicitantesFoto.ItemsSource;
                dgSolicitantes_Foto.Filter = string.Format("CUENTA LIKE '%{0}%'", FiltroCuenta);
                 dgSolicitantesFoto.ItemsSource = dgSolicitantes_Foto;

                if (dgSolicitantesFoto.Items.Count == 0 && FiltroCuenta.Length == 7) // PARA BUSCAR LOS ESTUDIANTES DE REINGRESOS
                {
                    Console.WriteLine("REINGRESO FOTOS");
                    LlenarListadoAlumnosFotoRI(FiltroCuenta);
                }

            }
            else
            {
                LlenarListadoAlumnosFoto();
            }

        }

        private void dgSolicitantesFoto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSolicitantesFoto.ItemsSource != null)
            {
                try
                {
                    if (dgSolicitantesFoto.SelectedItem != null)
                    {
                        System.Data.DataRowView row = (System.Data.DataRowView)dgSolicitantesFoto.SelectedItems[0];
                        LlenarInformacionAlumnoFoto(Int32.Parse(row["CUENTA"].ToString()));
                        cuenta_alumno = Int32.Parse(row["CUENTA"].ToString());

                        Console.WriteLine(Int32.Parse(row["CUENTA"].ToString()));

                        if (Int32.Parse(row["IDESTADO"].ToString()) == 1 || Int32.Parse(row["IDESTADO"].ToString()) == 3)
                        {
                            btnAprobadaFoto.IsEnabled = false;
                            btnNuevoIntentoFoto.IsEnabled = false;
                            btnRechazadaFoto.IsEnabled = false;
                        }
                        else if (Int32.Parse(row["IDESTADO"].ToString()) == 0)
                        {
                            btnAprobadaFoto.IsEnabled = true;
                            btnNuevoIntentoFoto.IsEnabled = true;
                            btnRechazadaFoto.IsEnabled = true;
                        }
                        else if (Int32.Parse(row["IDESTADO"].ToString()) == 2)
                        {
                            btnAprobadaFoto.IsEnabled = false;
                            btnNuevoIntentoFoto.IsEnabled = true;
                            btnRechazadaFoto.IsEnabled = true;
                        }
                        else
                        {
                            btnAprobadaFoto.IsEnabled = false;
                            btnNuevoIntentoFoto.IsEnabled = false;
                            btnRechazadaFoto.IsEnabled = false;
                        }
                    }
                    AdmConexion.CloseConnection();
                }
                catch (Exception ex)
                {
                    AdmConexion.CloseConnection();
                    MessageBox.Show("Error:: " + ex);
                }
            }
        }


        public void LlenarInformacionAlumnoFoto(int CUENTA_ALUMNO)
        {
            try
            {
                SqlCommand queryAlumnoFoto = new SqlCommand("SP_LISTADO_SOLICITANTES_FOTO 2,@CUENTA_ALUMNO", AdmConexion.OpenConnection());
                queryAlumnoFoto.Parameters.AddWithValue("@CUENTA_ALUMNO", CUENTA_ALUMNO);

                CLoading(Convert.ToInt32(AdmConexion.OpenConnection().ConnectionTimeout.ToString()));
                if (Convert.ToInt32(AdmConexion.OpenConnection().ConnectionTimeout.ToString()) == 15 || Convert.ToInt32(queryAlumnoFoto.CommandTimeout) == 30)
                {
                    AdmConexion.OpenConnection();
                    SqlDataReader resultado = queryAlumnoFoto.ExecuteReader();
                    if (resultado.Read())
                    {
                        txtNombre_Completo_Foto.Text = resultado["NOMBRE_COMPLETO"].ToString().ToUpper();
                        txtTelefono1_Foto.Text = resultado["TELEFONO1"].ToString();
                        txtTelefono2_Foto.Text = resultado["TELEFONO2"].ToString();
                        txtCorreo2_Foto.Text = resultado["EMAIL_PERSONAL"].ToString();
                        txtComentario_Foto.Text = resultado["COMENTARIO"].ToString();

                        if (resultado["GENERO"].ToString() == "M")
                        {
                            optMaculino_Foto.IsChecked = true;
                        }
                        else if (resultado["GENERO"].ToString() == "F")
                        {
                            optFemenino_Foto.IsChecked = true;
                        }

                        MostrarFotoAlumno(Int32.Parse(resultado["CUENTA_ALUMNO"].ToString()));

                    }
                }
                else
                {
                    MessageBox.Show("Error de Conexión!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Error);
                    AdmConexion.CloseConnection();
                }
                AdmConexion.CloseConnection();

            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }
        }

        //-----------------------------------------FIN INFORMACION ALUMNO FOTO------------------------------------------------

        //-----------------------------------------INICIO INFORMACION DE LOS ESTUDIANTES CON DOCUMENTOS PENDIENTES-------------

        private void txtFiltroDocPendientes_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgEstudiantesDocPendientes.SelectedItem = null;
            FiltroCuenta = txtFiltroDocPendientes.Text;

            if (FiltroCuenta.Length >= 3)
            {
                //Hace filtro en la columna cuenta alumno
                System.Windows.Forms.BindingSource dgEstDocPendientes = new System.Windows.Forms.BindingSource();
                dgEstDocPendientes.DataSource = dgEstudiantesDocPendientes.ItemsSource;
                dgEstDocPendientes.Filter = string.Format("CUENTA LIKE '{0}%'", FiltroCuenta);
                //dgEstDocPendientes.Filter = string.Format("CUENTA LIKE '%{0}%' OR [NOMBRE COMPLETO] LIKE '%{1}%'", FiltroCuenta, FiltroCuenta);
                //dgEstudiantesDocPendientes.Columns[1].Header.ToString() + " LIKE '%" + FiltroCuenta + "%'";
                dgEstudiantesDocPendientes.ItemsSource = dgEstDocPendientes;

                if (dgEstudiantesDocPendientes.Items.Count == 0 && FiltroCuenta.Length == 7) // PARA BUSCAR LOS ESTUDIANTES DE REINGRESOS
                {
                    Console.WriteLine("DATADOCPENDIENTESENTRO-----" + dgEstDocPendientes.Count.ToString());
                    LlenarListadoEstudDocPendientes();
                }
            }
            else if(FiltroCuenta.Length == 0)
            {
                //LlenarListadoAlumnosDocPendientes();
                RefreshDocPendientes();
            }
        }

        //---------------------------------

        private void dgEstudiantesDocPendientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEstudiantesDocPendientes.ItemsSource != null)
            {
                try
                {
                    if (dgEstudiantesDocPendientes.SelectedItem != null)
                    {
                        btnInformacionDoc.IsEnabled = true;
                        btnEnviarDocPendientes.IsEnabled = true;
                        System.Data.DataRowView row = (System.Data.DataRowView)dgEstudiantesDocPendientes.SelectedItems[0];
                        LlenarInformacionDocPendientes(Int32.Parse(row["CUENTA"].ToString()));
                        LlenarDocPendientes(Int32.Parse(row["CUENTA"].ToString()));
                        cuenta_alumno = Int32.Parse(row["CUENTA"].ToString());

                        if (Int32.Parse(row["IDESTADO"].ToString()) == 0)
                        {
                            btnRechazadaDocPendientes.IsEnabled = true;
                        }
                        else
                        {
                            btnRechazadaDocPendientes.IsEnabled = false;
                        }
                        AdmConexion.CloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    AdmConexion.CloseConnection();
                    MessageBox.Show("Error:: " + ex);
                }
            }
        }

        public void LlenarInformacionDocPendientes(int CUENTA_ALUMNO)
        {
            //AdmConexion = new AdmisionConnection(baseDeDatos);
            try
            {
                SqlCommand queryDocPendientes = new SqlCommand("SP_LISTADO_ESTUDIANTES_DOCPENDIENTES 2,@CUENTA_ALUMNO", AdmConexion.OpenConnection());
                queryDocPendientes.Parameters.AddWithValue("@CUENTA_ALUMNO", CUENTA_ALUMNO);

                CLoading(Convert.ToInt32(AdmConexion.OpenConnection().ConnectionTimeout.ToString()));
                if (Convert.ToInt32(AdmConexion.OpenConnection().ConnectionTimeout.ToString()) == 15 || Convert.ToInt32(queryDocPendientes.CommandTimeout) == 30)
                {
                    AdmConexion.OpenConnection();
                    SqlDataReader resultado = queryDocPendientes.ExecuteReader();
                    if (resultado.Read())
                    {
                        txtNombre_CompletoDocPendientes.Text = resultado["NOMBRE_COMPLETO"].ToString().ToUpper();
                        txtTelefono1DocPendientes.Text = resultado["TELEFONO1"].ToString();
                        txtTelefono2DocPendientes.Text = resultado["TELEFONO2"].ToString();
                        txtCorreo2DocPendientes.Text = resultado["EMAIL2"].ToString();
                        txtCorreo1DocPendientes.Text = resultado["EMAIL1"].ToString();
                        txtIdentidadDocPendientes.Text = resultado["IDENTIDAD"].ToString().Replace("-", "").ToUpper();
                        cbxTipoIdentificacionDocPendientes.SelectedValue = resultado["TIPO_IDENTIFICACION"].ToString();
                        txtPlanDocPendientes.Text = resultado["PLAN"].ToString();
                        txtCarreraDocPendientes.Text = resultado["CARRERA"].ToString();
                        txtFechaDocPendientes.Text = resultado["FECHA_PLAZO"].ToString();

                        if (Int32.Parse(resultado["ID_ESTADO_SOLICITUD"].ToString()) == 0)//SE ENVIO CORREO(PENDIENTE EN ACEPTAR EL CORREO EL ESTUDIANTE)
                        {

                            Estado_Solicitud.Content = resultado["ESTADO_SOLICITUD"].ToString();
                            Estado_Solicitud.Foreground = new SolidColorBrush(Colors.OrangeRed);
                        }
                        else if (Int32.Parse(resultado["ID_ESTADO_SOLICITUD"].ToString()) == 1)//ACEPTÓ LA SOLICITUD DE COMPROMISO
                        {

                            Estado_Solicitud.Content = resultado["ESTADO_SOLICITUD"].ToString();
                            Estado_Solicitud.Foreground = new SolidColorBrush(Colors.Green);
                        }
                        else
                        {

                            Estado_Solicitud.Content = resultado["ESTADO_SOLICITUD"].ToString();
                            Estado_Solicitud.Foreground = new SolidColorBrush(Colors.Black);
                        }



                        if (resultado["SEXO"].ToString() == "M")
                        {
                            optMaculinoDocPendientes.IsChecked = true;
                        }
                        else if (resultado["SEXO"].ToString() == "F")
                        {
                            optFemeninoDocPendientes.IsChecked = true;
                        }

                        Console.WriteLine("LlenarInformacionDocPendientes conexion a la base de datos " + Convert.ToString(AdmConexion.OpenConnection().ConnectionTimeout));
                        Console.WriteLine("LlenarInformacionDocPendientes request tiempo busquedad" + Convert.ToString(queryDocPendientes.CommandTimeout));
                    }
                    else
                    {
                        MessageBox.Show("Error de Conexión!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Error);
                        AdmConexion.CloseConnection();
                    }

                    AdmConexion.CloseConnection();
                }

            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }
        }


        private void dgEstudiantesDocPendientes_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //Ocultar la Primera Columna
            if (e.Column.Header.ToString() == "IDESTADO")
            {
                //Console.WriteLine(e.Column.Header.ToString());
                e.Column.CanUserSort = false;
                e.Column.Visibility = Visibility.Collapsed;

            }
        }

        private void dgSolicitantesFoto_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //Console.WriteLine(e.Column.Header.ToString());
            //Ocultar la Primera Columna
            if (e.Column.Header.ToString() == "IDESTADO")
            {
                e.Column.CanUserSort = false;
                e.Column.Visibility = Visibility.Collapsed;

            }
        }

        private void dgSolicitantes_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //Console.WriteLine(e.Column.Header.ToString());
            //Ocultar la Primera Columna
            if (e.Column.Header.ToString() == "ID")
            {
                e.Column.CanUserSort = false;
                e.Column.Visibility = Visibility.Collapsed;

            }

        }

        private void dgSolicitantesPrevio_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "ID_ADMINSION")
            {
                e.Column.CanUserSort = false;
                e.Column.Visibility = Visibility.Collapsed;

            }
     
        }

     //------------------------------------------FIN INFORMACION DE LOS ESTUDIANTES CON DOCUMENTOS PENDIENTES-----------------

        private void btnInformacionDoc_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(cuenta_alumno);
            frmEnviosSolicitudCompromiso ventanaInfEnvioSolicitudCompromiso = new frmEnviosSolicitudCompromiso();
            ventanaInfEnvioSolicitudCompromiso.baseDeDatos = baseDeDatos;
            ventanaInfEnvioSolicitudCompromiso.cuenta_alumno = cuenta_alumno;
            ventanaInfEnvioSolicitudCompromiso.ShowDialog();

        }

        public void LlenarComboboxsPais()
        {
            try
            {
                SqlCommand queryPais = new SqlCommand("SELECT ID_PAIS,NOMBRE_PAIS FROM PAISES ORDER BY NOMBRE_PAIS ASC", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorPaises = new SqlDataAdapter();
                adaptadorPaises.SelectCommand = queryPais;
                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorPaises.Fill(dt);

                cbxPais.DisplayMemberPath = "NOMBRE_PAIS";
                cbxPais.SelectedValuePath = "ID_PAIS";
                cbxPais.ItemsSource = dt.DefaultView;

                cbxPaisActual.DisplayMemberPath = "NOMBRE_PAIS";
                cbxPaisActual.SelectedValuePath = "ID_PAIS";
                cbxPaisActual.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();
            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }
        }
        private void CbxPais_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbxDepartamento.ItemsSource = null;
            if (cbxPais.SelectedValue != null)
            {
                int id_pais = Int16.Parse(cbxPais.SelectedValue.ToString());
                try
                {
                    SqlCommand queryDepto = new SqlCommand("SELECT ID_DEPARTAMENTO,NOMBRE_DEPARTAMENTO FROM DEPARTAMENTO WHERE ID_PAIS=@ID_PAIS ORDER BY NOMBRE_DEPARTAMENTO ASC", AdmConexion3.OpenConnection());
                    queryDepto.Parameters.AddWithValue("@ID_PAIS", id_pais);
                    SqlDataAdapter adaptadorDepto = new SqlDataAdapter();
                    adaptadorDepto.SelectCommand = queryDepto;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adaptadorDepto.Fill(dt);

                    cbxDepartamento.DisplayMemberPath = "NOMBRE_DEPARTAMENTO";
                    cbxDepartamento.SelectedValuePath = "ID_DEPARTAMENTO";
                    cbxDepartamento.ItemsSource = dt.DefaultView;

                    AdmConexion3.CloseConnection();
                }
                catch (Exception ex)
                {
                    AdmConexion3.CloseConnection();
                    MessageBox.Show("Error:: " + ex);
                   
                }
            }
        }
        private void CbxDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbxMunicipio.ItemsSource = null;
            if (cbxDepartamento.SelectedValue != null)
            {
                int id_departamento = Int16.Parse(cbxDepartamento.SelectedValue.ToString());
                try
                {
                    SqlCommand queryMuni = new SqlCommand("SELECT ID_MUNICIPIO,NOMBRE_MUNICIPIO FROM MUNICIPIOS WHERE ID_DEPARTAMENTO=@ID_DEPARTAMENTO ORDER BY NOMBRE_MUNICIPIO ASC", AdmConexion.OpenConnection());
                    queryMuni.Parameters.AddWithValue("@ID_DEPARTAMENTO", id_departamento);
                    SqlDataAdapter adaptadorMuni = new SqlDataAdapter();
                    adaptadorMuni.SelectCommand = queryMuni;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adaptadorMuni.Fill(dt);

                    cbxMunicipio.DisplayMemberPath = "NOMBRE_MUNICIPIO";
                    cbxMunicipio.SelectedValuePath = "ID_MUNICIPIO";
                    cbxMunicipio.ItemsSource = dt.DefaultView;

                    AdmConexion.CloseConnection();
                }
                catch (Exception ex)
                {
                    AdmConexion.CloseConnection();
                    MessageBox.Show("Error:: " + ex);
                }
            }
        }
        private void CbxPaisActual_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbxDepartamentoActual.ItemsSource = null;
            if (cbxPaisActual.SelectedValue != null)
            {
                int id_paisActual = Int16.Parse(cbxPaisActual.SelectedValue.ToString());
                try
                {
                    SqlCommand queryDepto = new SqlCommand("SELECT ID_DEPARTAMENTO,NOMBRE_DEPARTAMENTO FROM DEPARTAMENTO WHERE ID_PAIS=@ID_PAIS ORDER BY NOMBRE_DEPARTAMENTO ASC", AdmConexion1.OpenConnection());
                    queryDepto.Parameters.AddWithValue("@ID_PAIS", id_paisActual);
                    SqlDataAdapter adaptadorDepto = new SqlDataAdapter();
                    adaptadorDepto.SelectCommand = queryDepto;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adaptadorDepto.Fill(dt);

                    cbxDepartamentoActual.DisplayMemberPath = "NOMBRE_DEPARTAMENTO";
                    cbxDepartamentoActual.SelectedValuePath = "ID_DEPARTAMENTO";
                    cbxDepartamentoActual.ItemsSource = dt.DefaultView;

                    AdmConexion1.CloseConnection();
                }
                catch (Exception ex)
                {
                    AdmConexion1.CloseConnection();
                    MessageBox.Show("Error:: " + ex);
                }
            }
        }

        private void CbxDepartamentoActual_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbxMunicipioActual.ItemsSource = null;
            if (cbxDepartamentoActual.SelectedValue != null)
            {
                int id_departamentoActual = Int16.Parse(cbxDepartamentoActual.SelectedValue.ToString());
                try
                {
                    SqlCommand queryMuni = new SqlCommand("SELECT ID_MUNICIPIO,NOMBRE_MUNICIPIO FROM MUNICIPIOS WHERE ID_DEPARTAMENTO=@ID_DEPARTAMENTO ORDER BY NOMBRE_MUNICIPIO ASC", AdmConexion.OpenConnection());
                    queryMuni.Parameters.AddWithValue("@ID_DEPARTAMENTO", id_departamentoActual);
                    SqlDataAdapter adaptadorMuni = new SqlDataAdapter();
                    adaptadorMuni.SelectCommand = queryMuni;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adaptadorMuni.Fill(dt);

                    cbxMunicipioActual.DisplayMemberPath = "NOMBRE_MUNICIPIO";
                    cbxMunicipioActual.SelectedValuePath = "ID_MUNICIPIO";
                    cbxMunicipioActual.ItemsSource = dt.DefaultView;

                    AdmConexion.CloseConnection();
                }
                catch (Exception ex)
                {
                    AdmConexion.CloseConnection();
                    MessageBox.Show("Error:: " + ex);
                }
            }
        }

        private void CbxMunicipioActual_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            cbxColoniaActual.ItemsSource = null;
            if (cbxMunicipioActual.SelectedValue != null)
            {
                int id_municipioActual = Int16.Parse(cbxMunicipioActual.SelectedValue.ToString());
                Console.WriteLine("ENTRO CbxMunicipioActual_SelectionChanged " + id_municipioActual);
                try
                {
                    SqlCommand queryColoR = new SqlCommand("SELECT ID_COLONIA,DESCRIPCION_COLONIA FROM COLONIA WHERE ID_MUNICIPIO=@ID_MUNICIPIO AND DESCRIPCION_COLONIA IS NOT NULL ORDER BY DESCRIPCION_COLONIA ASC", AdmConexion.OpenConnection());
                    queryColoR.Parameters.AddWithValue("@ID_MUNICIPIO", id_municipioActual);
                    SqlDataAdapter adaptadorColo = new SqlDataAdapter();
                    adaptadorColo.SelectCommand = queryColoR;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adaptadorColo.Fill(dt);

                    cbxColoniaActual.DisplayMemberPath = "DESCRIPCION_COLONIA";
                    cbxColoniaActual.SelectedValuePath = "ID_COLONIA";
                    cbxColoniaActual.ItemsSource = dt.DefaultView;

                    AdmConexion.CloseConnection();
                }
                catch (Exception ex)
                {
                    AdmConexion.CloseConnection();
                    MessageBox.Show("Error:: " + ex);
                }
            }
        }

        public void LlenarIntentosCall(int intentos)
        {
            //----------------------------------INTENTOS LLAMADAS Y FECHA---------------------------------------
            for (int i = intentos; i <= 50; i++)
            {
                //Console.WriteLine(i);
                cbxIntentosLlamadas.Items.Add(i);
            }
        }

        public void LlenarCombosOtros()
        {
            try 
            {
                //------------------------------------IDENTIFICACION REGISTRO--------------------------------------
                SqlCommand queryIndeti = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_REGISTRO 5,NULL,NULL", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorIdenti = new SqlDataAdapter();
                adaptadorIdenti.SelectCommand = queryIndeti;
                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorIdenti.Fill(dt);

                cbxTipoIdentificacion.DisplayMemberPath = "TIPO_IDENTIFICACION";
                cbxTipoIdentificacion.SelectedValuePath = "ID_TIPO_IDENTIFICACION";
                cbxTipoIdentificacion.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();

                //------------------------------------IDENTIFICACION PREVIO--------------------------------------
                SqlCommand queryIndetiPrevio = new SqlCommand("SELECT ID_TIPO_IDENTIFICACION,TIPO_IDENTIFICACION FROM TIPO_IDENTIFICACION", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorIdentiPrevio = new SqlDataAdapter();
                adaptadorIdentiPrevio.SelectCommand = queryIndeti;
                System.Data.DataTable dtp = new System.Data.DataTable();
                adaptadorIdentiPrevio.Fill(dtp);

                cbxTipoIdentificacionPrevio.DisplayMemberPath = "TIPO_IDENTIFICACION";
                cbxTipoIdentificacionPrevio.SelectedValuePath = "ID_TIPO_IDENTIFICACION";
                cbxTipoIdentificacionPrevio.ItemsSource = dtp.DefaultView;
                
                AdmConexion.CloseConnection();

                //------------------------------------IDENTIFICACION DOCUMENTOS PENDIENTES--------------------------------------
                SqlCommand queryIndetiDocPendientes = new SqlCommand("SELECT ID_TIPO_IDENTIFICACION,TIPO_IDENTIFICACION FROM TIPO_IDENTIFICACION", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorIdentiDocPendientes = new SqlDataAdapter();
                adaptadorIdentiDocPendientes.SelectCommand = queryIndeti;
                System.Data.DataTable dtcp = new System.Data.DataTable();
                adaptadorIdentiDocPendientes.Fill(dtcp);

                cbxTipoIdentificacionDocPendientes.DisplayMemberPath = "TIPO_IDENTIFICACION";
                cbxTipoIdentificacionDocPendientes.SelectedValuePath = "ID_TIPO_IDENTIFICACION";
                cbxTipoIdentificacionDocPendientes.ItemsSource = dtp.DefaultView;
               
                AdmConexion.CloseConnection();

                //------------------------------------ESTADO CIVIL--------------------------------------
                SqlCommand queryEstadoc = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_REGISTRO 6,NULL,NULL", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorEstadoc = new SqlDataAdapter();
                adaptadorEstadoc.SelectCommand = queryEstadoc;
                dt = null;
                dt = new System.Data.DataTable();
                adaptadorEstadoc.Fill(dt);

                cbxEstadoCivil.DisplayMemberPath = "DESCRIPCION";
                cbxEstadoCivil.SelectedValuePath = "ID_ESTADO";
                cbxEstadoCivil.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();

                //------------------------------------SUCURSAL--------------------------------------
                SqlCommand querySucursal = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_REGISTRO 7,NULL,NULL", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorSucursal = new SqlDataAdapter();
                adaptadorSucursal.SelectCommand = querySucursal;
                dt = null;
                dt = new System.Data.DataTable();
                adaptadorSucursal.Fill(dt);

                cbxSucursal.DisplayMemberPath = "DESCRIPCION_SUCURSAL";
                cbxSucursal.SelectedValuePath = "ID_SUCURSAL";
                cbxSucursal.ItemsSource = dt.DefaultView;
                //cbxSucursal.SelectedValue = 1;

                AdmConexion.CloseConnection();

                //------------------------------------ESTATUS--------------------------------------
                SqlCommand queryEstatus = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_REGISTRO 8,NULL,NULL", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorEstatus = new SqlDataAdapter();
                adaptadorEstatus.SelectCommand = queryEstatus;
                dt = null;
                dt = new System.Data.DataTable();
                adaptadorEstatus.Fill(dt);

                cbxEstatus.DisplayMemberPath = "DESCRIPCION";
                cbxEstatus.SelectedValuePath = "ID_ESTATUS";
                cbxEstatus.ItemsSource = dt.DefaultView;
                cbxEstatus.SelectedValue = 1;

                AdmConexion.CloseConnection();

                //------------------------------------DESCUENTO--------------------------------------
                SqlCommand queryDescuento = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_REGISTRO 9,NULL,NULL", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorDescuento = new SqlDataAdapter();
                adaptadorDescuento.SelectCommand = queryDescuento;
                dt = null;
                dt = new System.Data.DataTable();
                adaptadorDescuento.Fill(dt);

                cbxDescuento.DisplayMemberPath = "DESCRIPCION";
                cbxDescuento.SelectedValuePath = "ID_BECA_DESC";
                cbxDescuento.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();

                //------------------------------------PROMOCION--------------------------------------
                SqlCommand queryPromocion = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_REGISTRO 10,NULL,NULL", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorPromocion = new SqlDataAdapter();
                adaptadorPromocion.SelectCommand = queryPromocion;
                dt = null;
                dt = new System.Data.DataTable();
                adaptadorPromocion.Fill(dt);

                cbxPromocion.DisplayMemberPath = "DESCRIPCION";
                cbxPromocion.SelectedValuePath = "ID_PROMOCION";
                cbxPromocion.ItemsSource = dt.DefaultView;

                AdmConexion.CloseConnection();

                //--------------------------------CENTRO ESTUDIO-------------------------------------
                SqlCommand queryCentroEstudio = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_REGISTRO 11,NULL,NULL", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorCentroEstudio = new SqlDataAdapter();
                adaptadorCentroEstudio.SelectCommand = queryCentroEstudio;
                dt = null;
                dt = new System.Data.DataTable();
                adaptadorCentroEstudio.Fill(dt);

                cbxCampusEstudio.DisplayMemberPath = "DESCRIPCION_SUCURSAL";
                cbxCampusEstudio.SelectedValuePath = "ID_SUCURSAL_AM";
                cbxCampusEstudio.ItemsSource = dt.DefaultView;
                cbxCampusEstudio.SelectedValue = 1;

                AdmConexion.CloseConnection();

                //------------------------------------ESTADO LLAMADA--------------------------------------
                SqlCommand queryEstadoLlamada = new SqlCommand("EXEC ESTADO_LLAMADA", AdmConexion.OpenConnection());
                SqlDataAdapter adaptadorEstadoLlamada = new SqlDataAdapter();
                adaptadorEstadoLlamada.SelectCommand = queryEstadoLlamada;
                dt = null;
                dt = new System.Data.DataTable();
                adaptadorEstadoLlamada.Fill(dt);

                cbxEstadollamadaPrevio.DisplayMemberPath = "ESTADO";
                cbxEstadollamadaPrevio.SelectedValuePath = "ESTADO";
                cbxEstadollamadaPrevio.ItemsSource = dt.DefaultView;
                //cbxEstadollamadaPrevio.SelectedValue = 1;
                AdmConexion.CloseConnection();

            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }
        }

        private void TxtprimerApellido_TextChanged(object sender, TextChangedEventArgs e)
        {
            NombreCompelto();
        }

        private void TxtSegundoApellido_TextChanged(object sender, TextChangedEventArgs e)
        {
            NombreCompelto();

        }

        private void TxtNombres_TextChanged(object sender, TextChangedEventArgs e)
        {
            NombreCompelto();

        }

        public void NombreCompelto()
        {
            txtNombreCompleto.Text = txtNombres.Text + " " + txtprimerApellido.Text + " " + txtSegundoApellido.Text;
            
        }

        public void LlenarPlanes(int id_plan)
        {

            try
            {
                SqlCommand queryPlan = new SqlCommand("SP_LISTADO_SOLICITANTES_REGISTRO 3,NULL,@ID_PLAN", AdmConexion1.OpenConnection());
                queryPlan.Parameters.AddWithValue("@ID_PLAN", id_plan);
                SqlDataAdapter adaptadorPlan = new SqlDataAdapter();
                adaptadorPlan.SelectCommand = queryPlan;
                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorPlan.Fill(dt);

                dgPlanes.AutoGenerateColumns = true;
                dgPlanes.ItemsSource = dt.DefaultView;

                AdmConexion1.CloseConnection();
            }
            catch (Exception ex)
            {
                AdmConexion1.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }
        }

        public void LlenarFamiliares(int id_admision)
        {
            try
            {
                SqlCommand queryFamili = new SqlCommand("SP_LISTADO_SOLICITANTES_REGISTRO 4,@ID_ADMISION,NULL", AdmConexion1.OpenConnection());
                queryFamili.Parameters.AddWithValue("@ID_ADMISION", id_admision);
                SqlDataAdapter adaptadorFamili = new SqlDataAdapter();
                adaptadorFamili.SelectCommand = queryFamili;
                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorFamili.Fill(dt);

                dgFamiliares.AutoGenerateColumns = true;
                dgFamiliares.ItemsSource = dt.DefaultView;

                AdmConexion1.CloseConnection();
            }
            catch (Exception ex)
            {
                AdmConexion1.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }
        }

        private void BtnCrearCuenta_Click(object sender, RoutedEventArgs e)
        {
            //AdmConexion = new AdmisionConnection(baseDeDatos);
            MessageBoxResult oDlgRes;
            oDlgRes = MessageBox.Show("¿Está seguro que desea crear la cuenta del estudiante?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (oDlgRes == MessageBoxResult.Yes)
            {
                Validaciones();
                if (errorValidacion == 0)
                {
                    try
                    {
                        int anio = 0;
                        int periodo = 0;
                        string cuenta = "";
                        SqlCommand queryPeriodo = new SqlCommand("SELECT ANIO,PERIODO,ID_CONTROL_MATRICULA FROM CONTROL_MATRICULA WHERE ESTATUS=1", AdmConexion.OpenConnection());
                        AdmConexion.OpenConnection();
                        SqlDataReader resultado = queryPeriodo.ExecuteReader();
                        if (resultado.Read())
                        {
                            anio = Int16.Parse(resultado["ANIO"].ToString());
                            periodo = Int16.Parse(resultado["PERIODO"].ToString());
                            id_control_matricula = Int16.Parse(resultado["ID_CONTROL_MATRICULA"].ToString());

                        }
                        AdmConexion.CloseConnection();

                        SqlCommand queryGenerarCuenta = new SqlCommand("EXEC SP_GENERAR_CUENTA @PERIODO,@ANIO", AdmConexion.OpenConnection());
                        queryGenerarCuenta.Parameters.AddWithValue("@PERIODO", periodo);
                        queryGenerarCuenta.Parameters.AddWithValue("@ANIO", anio);

                        AdmConexion.OpenConnection();
                        resultado = queryGenerarCuenta.ExecuteReader();
                        if (resultado.Read())
                        {
                            cuenta = resultado[0].ToString();
                        }
                        AdmConexion.CloseConnection();

                        txtCuenta.Text = cuenta;
                        txtCorreo1.Text = cuenta + "@usap.edu";

                        GuaradarDatos();
                        if (errorValidacionRegistro == 0) //ENTRA AQUI CUANDO NO HAY UNA VALIDACION O ERROR
                        {
                            ////VALIDACION DE OTROS DE INSTITUTO O TITULO
                            //SqlCommand queryOtros = new SqlCommand(" EXEC SP_LISTADO_SOLICITANTES_REGISTRO 3, @ID_ADMISION,NULL", AdmConexion.OpenConnection());
                            //queryOtros.Parameters.AddWithValue("@ID_ADMISION", id_admision_global);
                            //AdmConexion.OpenConnection();
                            //SqlDataReader resultadoOtros = queryOtros.ExecuteReader();
                            //if (resultadoOtros.Read())
                            //{
                            //    if (resultadoOtros["EXISTE_OTROS"].ToString() != "0")
                            //    {
                            //        MessageBox.Show("Hay información de otro instituto o título en el admisión del estudiante.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                            //    }
                            //}

                            //AdmConexion.CloseConnection();

                            RefreshAprobado();


                        }

                        AdmConexion.CloseConnection();


                    }
                    catch (Exception ex)
                    {
                        AdmConexion.CloseConnection();
                        MessageBox.Show("Error BtnCrearCuenta_Click :: " + ex);
                        RefreshAprobado();
                    }
                }
            }
        }

        public void GuaradarDatos()
        {    
            //AdmConexion = new AdmisionConnection(baseDeDatos);
            //INSERTA EN LA TABLA CLIENTE
            try
            {
                //INSERTAR
                AdmConexion.OpenConnection();
                string sql = "EXEC SP_INSERTA_ADM_CLIENTE " +
                    "@CUENTA_ALUMNO, @APELLIDO1, @APELLIDO2, @NOMBRES, @NOMBRE_COMPLETO, @FECHA_NACIMIENTO,@SEXO,@TIPO_IDENTIFICACION,@IDENTIDAD, @ID_ESTADO, @DIRECCION,@TELEFONO1, @TELEFONO2" +
                    ",@EMAIL1, @EMAIL2, @ID_SUCURSAL, @ID_ESTATUS_ALUMNO,@ID_PROMOCION, @OBSERVACIONES, @COMPANIA, @TELEFONO_COMPANIA, @PUESTO, @COMENTARIOS,@ID_USUARIO,@ID_COLONIA,@ID_PLAN" +
                    ",@ID_PAIS_NAC,@ID_PAIS,@ID_DEPARTAMENTO_NAC,@ID_DEPARTAMENTO,@ID_MUNICIPIO_NAC,@ID_MUNICIPIO,@ID_ADMISION,@ID_BECA_DESC,@FECHA_VENCIMIENTO,@ID_CONTROL_MATRICULA";
                using (SqlCommand cmd = new SqlCommand(sql, AdmConexion.OpenConnection()))
                {
                    cmd.Parameters.Add("@CUENTA_ALUMNO", SqlDbType.VarChar, 20).Value = txtCuenta.Text;
                    cmd.Parameters.Add("@APELLIDO1", SqlDbType.VarChar).Value = txtprimerApellido.Text;
                    cmd.Parameters.Add("@APELLIDO2", SqlDbType.VarChar).Value = (txtSegundoApellido.Text != "" && txtSegundoApellido.Text != null) ? (txtSegundoApellido.Text ?? null): " ";

                    cmd.Parameters.Add("@NOMBRES", SqlDbType.VarChar).Value = txtNombres.Text;
                    cmd.Parameters.Add("@NOMBRE_COMPLETO", SqlDbType.VarChar).Value = txtNombreCompleto.Text;
                    cmd.Parameters.Add("@FECHA_NACIMIENTO", SqlDbType.DateTime).Value = DateTime.Parse(txtFechaNacimiento.Text);
                    if (optMaculino.IsChecked == true)
                    {
                        cmd.Parameters.Add("@SEXO", SqlDbType.VarChar, 1).Value = "M";
                    }
                    else if (optFemenino.IsChecked == true)
                    {
                        cmd.Parameters.Add("@SEXO", SqlDbType.VarChar, 1).Value = "F";
                    }
                    cmd.Parameters.Add("@TIPO_IDENTIFICACION", SqlDbType.Int).Value = cbxTipoIdentificacion.SelectedValue ?? DBNull.Value;
                    cmd.Parameters.Add("@IDENTIDAD", SqlDbType.VarChar, 20).Value = txtIdentidad.Text ?? null;
                    cmd.Parameters.Add("@ID_ESTADO", SqlDbType.Int).Value = cbxEstadoCivil.SelectedValue ?? DBNull.Value;
                    cmd.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = txtDireccion.Text ?? null;

                    if (txtTelefono1.Text != "" && txtTelefono1.Text != null)
                    {
                        cmd.Parameters.Add("@TELEFONO1", SqlDbType.Int).Value = txtTelefono1.Text ?? null;
                    }
                    else
                    {
                        cmd.Parameters.Add("@TELEFONO1", SqlDbType.Int).Value = "0";
                    }

                    if (txtTelefono2.Text != "" && txtTelefono2.Text != null)
                    {
                        cmd.Parameters.Add("@TELEFONO2", SqlDbType.Int).Value = txtTelefono2.Text ?? null;
                    }
                    else
                    {
                        cmd.Parameters.Add("@TELEFONO2", SqlDbType.Int).Value = "0";
                    }
                    cmd.Parameters.Add("@EMAIL1", SqlDbType.VarChar).Value = txtCorreo1.Text;
                    cmd.Parameters.Add("@EMAIL2", SqlDbType.VarChar).Value = txtCorreo2.Text ?? null;
                    cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.Int).Value = cbxSucursal.SelectedValue ?? DBNull.Value;
                    cmd.Parameters.Add("@ID_ESTATUS_ALUMNO", SqlDbType.Int).Value = cbxEstatus.SelectedValue ?? DBNull.Value;
                    cmd.Parameters.Add("@ID_PROMOCION", SqlDbType.Int).Value = cbxPromocion.SelectedValue ?? DBNull.Value;
                    cmd.Parameters.Add("@OBSERVACIONES", SqlDbType.VarChar).Value = txtObservaciones.Text ?? null;
                    cmd.Parameters.Add("@COMPANIA", SqlDbType.VarChar).Value = txtCompania.Text ?? null;
                    
                    
                    if (txtTelefonoCompania.Text != "" && txtTelefonoCompania.Text != null)
                    {
                        cmd.Parameters.Add("@TELEFONO_COMPANIA", SqlDbType.Int).Value = txtTelefonoCompania.Text ?? null;
                    }
                    else
                    {
                        cmd.Parameters.Add("@TELEFONO_COMPANIA", SqlDbType.Int).Value = "0";
                    }
                    //Console.WriteLine(txtTelefonoCompania.Text);
                    cmd.Parameters.Add("@PUESTO", SqlDbType.VarChar).Value = txtPuesto.Text ?? null;
                    cmd.Parameters.Add("@COMENTARIOS", SqlDbType.VarChar).Value = txtComentarios.Text ?? null;
                    cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar).Value = id_usuario;

                    cmd.Parameters.Add("@ID_COLONIA", SqlDbType.VarChar).Value = cbxColoniaActual.SelectedValue ?? DBNull.Value;

                    cmd.Parameters.Add("@ID_PAIS", SqlDbType.VarChar).Value =cbxPaisActual.SelectedValue ?? DBNull.Value;
                    cmd.Parameters.Add("@ID_PAIS_NAC", SqlDbType.VarChar).Value =cbxPais.SelectedValue ?? DBNull.Value;

                    cmd.Parameters.Add("@ID_DEPARTAMENTO", SqlDbType.VarChar).Value = cbxDepartamentoActual.SelectedValue?? DBNull.Value;
                    cmd.Parameters.Add("@ID_DEPARTAMENTO_NAC", SqlDbType.VarChar).Value = cbxDepartamento.SelectedValue ?? DBNull.Value;

                    cmd.Parameters.Add("@ID_MUNICIPIO", SqlDbType.Int).Value = cbxMunicipioActual.SelectedValue ?? DBNull.Value;
                    cmd.Parameters.Add("@ID_MUNICIPIO_NAC", SqlDbType.Int).Value = cbxMunicipio.SelectedValue ?? DBNull.Value;

                    //INSERTA EN LA TABLA PLAN ESTUDIO
                    cmd.Parameters.Add("@ID_PLAN", SqlDbType.Int).Value = id_plan_seleccionado;

                    //INSERTA EN LA TABLA FAMILIA
                    cmd.Parameters.Add("@ID_ADMISION", SqlDbType.Int).Value = id_admision_global;

                    //INSERTA LA BECA
                    cmd.Parameters.Add("@ID_BECA_DESC", SqlDbType.Int).Value = cbxDescuento.SelectedValue ?? DBNull.Value;

                    if (txtFechaVencimiento.Text != null && txtFechaVencimiento.Text != "")
                    {
                        cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.DateTime).Value = DateTime.Parse(txtFechaVencimiento.Text);
                    }
                    else
                    {
                        cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    cmd.Parameters.Add("@ID_CONTROL_MATRICULA", SqlDbType.Int).Value = id_control_matricula;

                    //using (SqlDataReader resultado = cmd.ExecuteReader())
                    //{
                    //    while (resultado.Read())
                    //    {
                    //        if (resultado[0].ToString() != "OK")
                    //        {
                    //            MessageBox.Show(resultado[0].ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //            errorValidacionRegistro = 1;
                    //        }
                    //        else
                    //        {
                    //            errorValidacionRegistro = 0;
                    //        }
                    //    }
                    //}

                    //cmd.CommandType = CommandType.Text;
                    //cmd.ExecuteNonQuery();

                    string getValueCCuenta = cmd.ExecuteScalar().ToString();
                    if (getValueCCuenta != null)
                    {
                        string resultado = getValueCCuenta.ToString();

                        if (resultado == "OK")
                        {
                            Console.WriteLine("ENTRO EN LA OPCION CREAR CUENTA " + resultado);
                            errorValidacionRegistro = 0;
                        }
                        else
                        {
                            errorValidacionRegistro = 1;
                            MessageBox.Show(resultado.ToString(), "Error Crear Cuenta", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("ERROR", "Error Crear Cuenta", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }

                if (errorValidacionRegistro == 0)
                {
                    //UPDATE DEL ESTADO DE LA SOLICITUD
                    LlenarListadoSolicitantes();
                    btnCrearCuenta.IsEnabled = false;
                    btnEliminarRegistro.IsEnabled = false;

                    MessageBox.Show("El registro se ha creado la cuenta es: " + txtCuenta.Text, "Solicitud Aceptada", MessageBoxButton.OK, MessageBoxImage.Information);
                    AdmConexion.CloseConnection();
                }
                AdmConexion.CloseConnection();
            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: GuardarDatos " + ex);
       
                RefreshAprobado();
            }
        }

        public void Validaciones()
        {
            AdmConexion = new AdmisionConnection(baseDeDatos);
            errorValidacion = 0;
            if (txtprimerApellido.Text == "" || txtNombres.Text == "")
            {
                MessageBox.Show("Ingrese los nombres y los apellidos para continuar", "Nombre y/o Apellido", MessageBoxButton.OK, MessageBoxImage.Error);
                errorValidacion = 1;
            }

            SqlCommand queryAlumno = new SqlCommand("SELECT COUNT(CUENTA_ALUMNO) EXISTE_IDENTIDAD FROM CLIENTE WHERE IDENTIDAD=@IDENTIDAD AND CUENTA_ALUMNO !=@CUENTA_ALUMNO", AdmConexion.OpenConnection());
            queryAlumno.Parameters.AddWithValue("@IDENTIDAD", txtIdentidad.Text);
            queryAlumno.Parameters.AddWithValue("@CUENTA_ALUMNO", txtCuenta.Text);
            AdmConexion.OpenConnection();
            SqlDataReader resultado = queryAlumno.ExecuteReader();
            if (resultado.Read())
            {
                if (resultado["EXISTE_IDENTIDAD"].ToString() != "0")
                {
                    MessageBox.Show("El numero de Identidad ingresado ya se encuentra registrado, por favor verifique.", "Error de Identidad", MessageBoxButton.OK, MessageBoxImage.Error);
                    errorValidacion = 1;
                }
            }
            AdmConexion.CloseConnection();

            if (cbxTipoIdentificacion.SelectedValue.ToString() == "1")
            {
                string stringToVerify = txtIdentidad.Text;
                int tieneChar = 0;
                if (System.Text.RegularExpressions.Regex.IsMatch(stringToVerify, "[^0-9]+"))
                    tieneChar = 1;

                if (tieneChar == 1)
                {
                    MessageBox.Show("La identidad solo debe ser numerica.", "Error de Identidad", MessageBoxButton.OK, MessageBoxImage.Error);
                    errorValidacion = 1;
                }
                else if(txtIdentidad.Text.Length!=13)
                {
                    MessageBox.Show("La identidad tiene que contener 13 caracteres.", "Error de Identidad", MessageBoxButton.OK, MessageBoxImage.Error);
                    errorValidacion = 1;
                }

            }

            //VALIDACION DEL NOMBRE COMPLETO
            SqlCommand queryNombre = new SqlCommand("SELECT COUNT(NOMBRE_COMPLETO)'EXISTE_NOMBRE' FROM CLIENTE WHERE NOMBRE_COMPLETO=@NOMBRE_COMPLETO", AdmConexion.OpenConnection());
            queryNombre.Parameters.AddWithValue("@NOMBRE_COMPLETO", txtNombreCompleto.Text);
            AdmConexion.OpenConnection();
            SqlDataReader resultadoNom = queryNombre.ExecuteReader();
            if (resultadoNom.Read())
            {
                if (resultadoNom["EXISTE_NOMBRE"].ToString() != "0")
                {
                    MessageBox.Show("El nombre completo del estudiante existe.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Error);
                    errorValidacion = 1;
                }
            }
            AdmConexion.CloseConnection();

            //VALIDACION TELEFONO2,IDENTIFICACION Y CORREO PERSONAL
            if (txtIdentidad.Text == "" || txtIdentidad.Text == null || txtTelefono2.Text == "" || txtCorreo2.Text == "")
            {
                MessageBox.Show("Ingrese el numero de Identidad,Teléfono o Correo Personal.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                errorValidacion = 1;
            }

        }

        public void ActualizarAdmision(int estado_registro)
        {
            AdmConexion.OpenConnection();
            string sql = "UPDATE TB_ADM_CLIE SET ESTADO_REGISTRO=@ESTADO_REGISTRO WHERE ID_ADMINSION=@ID_ADMISION";
            using (SqlCommand cmd = new SqlCommand(sql, AdmConexion.OpenConnection()))
            {
                cmd.Parameters.Add("@ESTADO_REGISTRO", SqlDbType.Int).Value = estado_registro;
                cmd.Parameters.Add("@ID_ADMISION", SqlDbType.Int).Value = id_admision_global;

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            AdmConexion.CloseConnection();
        }

        private Boolean TxtSoloNumero(TextBox txtValue)
        {
            var textBox = txtValue.Text;
            var bol = false;
            bol = System.Text.RegularExpressions.Regex.IsMatch(textBox, "[^0-9]+");
            return bol;
        }

        private void TxtTelefono1_KeyUp(object sender, KeyEventArgs e)
        {
            var txtCampo = txtTelefono1;
            if (e.Key.ToString() != "OemOpenBrackets")
            {
                if (TxtSoloNumero(txtCampo) == false)
                {
                    if (txtCampo.Text == "" || txtCampo.Text == null)
                        txtCampo.Text = "";
                }
                else
                {
                    txtCampo.Text = "0";
                }
            }
            else
            {
                txtCampo.Text = "";
            }
            txtCampo.SelectionStart = txtCampo.Text.Length;
        }

        private void TxtTelefono2_KeyUp(object sender, KeyEventArgs e)
        {
            var txtCampo = txtTelefono2;
            if (e.Key.ToString() != "OemOpenBrackets")
            {
                if (TxtSoloNumero(txtCampo) == false)
                {
                    if (txtCampo.Text == "" || txtCampo.Text == null)
                        txtCampo.Text = "";
                }
                else
                {
                    txtCampo.Text = "0";
                }
            }
            else
            {
                txtCampo.Text = "";
            }
            txtCampo.SelectionStart = txtCampo.Text.Length;
        }

        private void TxtTelefonoCompania_KeyUp(object sender, KeyEventArgs e)
        {
            var txtCampo = txtTelefonoCompania;
            if (e.Key.ToString() != "OemOpenBrackets")
            {
                if (TxtSoloNumero(txtCampo) == false)
                {
                    if (txtCampo.Text == "" || txtCampo.Text == null)
                        txtCampo.Text = "";
                }
                else
                {
                    txtCampo.Text = "0";
                }
            }
            else
            {
                txtCampo.Text = "";
            }
            txtCampo.SelectionStart = txtCampo.Text.Length;
        }

        private void BtnEliminarRegistro_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult oDlgRes;
            oDlgRes = MessageBox.Show("¿Está seguro que desea inactivar la solicitud?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (oDlgRes == MessageBoxResult.Yes)
            {
                //btnGuardar.IsEnabled = false;
                btnCrearCuenta.IsEnabled = false;
                btnEliminarRegistro.IsEnabled = false;
                ActualizarAdmision(2);
                LlenarListadoSolicitantes();
            }
        }
 
        private void LlenarInstituto(int id_admision)
        {
            //AdmConexion1 = new AdmisionConnection(baseDeDatos);
            try
            { //T0.ID_ADMINSION se quito en el select
                SqlCommand queryInstitu = new SqlCommand("SELECT T1.DESCRIPCION 'INSTITUTO',T0.OTRO_INSTITUCION 'OTRO INSTITUCIÓN',T4.NOMBRE_PAIS 'PAIS',T3.NOMBRE_DEPARTAMENTO 'DEPARTAMENTO',T2.NOMBRE_MUNICIPIO 'MUNICIPIO'," +
                    "RTRIM(LTRIM(T5.TITULO)) AS 'TITULO',T0.OTRO_TITULO 'OTRO TITULO',T0.ANIO_GRADUACION 'AÑO' FROM TB_ADM_CLIE T0 INNER JOIN INSTITUCION T1 ON T1.ID_INSTITUCION=T0.NOMBRE_INSTITUCION" +
                    " LEFT JOIN MUNICIPIOS T2 ON T2.ID_MUNICIPIO=T0.ID_MUNICIPIO_INSTITUTO LEFT JOIN TB_ADM_TITU T5 ON T5.ID_TITULO=T0.ID_TITULO " +
                    " LEFT JOIN DEPARTAMENTO T3 ON T3.ID_DEPARTAMENTO=T0.ID_DEPARTAMENTO_INSTITUTO INNER JOIN PAISES T4 ON T4.ID_PAIS=T0.ID_PAIS_INSTITUTO" +
                    " WHERE T0.ID_ADMINSION=@ID_ADMISION", AdmConexion1.OpenConnection());
                queryInstitu.Parameters.AddWithValue("@ID_ADMISION", id_admision);
                SqlDataAdapter adaptadorInstitu = new SqlDataAdapter();
                adaptadorInstitu.SelectCommand = queryInstitu;
                System.Data.DataTable dt = new System.Data.DataTable();
                adaptadorInstitu.Fill(dt);

                dgInstitucion.AutoGenerateColumns = true;
                dgInstitucion.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {   
                AdmConexion1.CloseConnection();
                MessageBox.Show("Error:: " + ex);
                
            }
        }

        //IMAGEN DE FOTO
        private void BtnImgDoc_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine(id_admision_global);

            if (id_admision_global != 0)
            {
                VtnFrmImgDoc(id_admision_global);
            }
            else
            {
                MessageBox.Show("No tiene seleccionado la admisión del estudiante.", "Admisión", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        //private void BtnImgDoc_ClickPrevio(object sender, RoutedEventArgs e)
        //{
        //    Console.WriteLine(id_admision_globalPrevio);

        //    if (id_admision_globalPrevio != 0)
        //    {
        //        VtnFrmImgDoc(id_admision_globalPrevio);
        //    }
        //    else
        //    {
        //        MessageBox.Show("No tiene seleccionado la admisión del estudiante.", "Admisión", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //}

        //------------------------------------------------- IMAGEN---------------------------------------
        public void VtnFrmImgDoc(int id_admision)
        {
            frmImgDoc ventanaImgDoc = new frmImgDoc();
            frmImgDocError ventanaImgDocError = new frmImgDocError();
            //ventanaImgDoc.id_admision_global = id_admision_global;
            //ventanaImgDoc.txtimagen.Text = id_admision_global.ToString();
            try
            {
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                Byte[] blob = new byte[0];
                 //SqlCommand queryFoto = new SqlCommand("SELECT (SELECT  TOP 1 (FOTO) FROM   CARNET WHERE CUENTA = @CUENTA_ALUMNO GROUP BY FOTO FOR XML PATH(''), BINARY BASE64) FOTO FROM CARNET WHERE CUENTA = @CUENTA_ALUMNO", conexion2);
                //queryFoto.Parameters.AddWithValue("@CUENTA_ALUMNO", "2090555");
                SqlCommand queryFoto = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_REGISTRO 12,@ID_ADMISION,NULL", AdmConexion.OpenConnection());
                queryFoto.Parameters.AddWithValue("@ID_ADMISION", id_admision);


                AdmConexion.OpenConnection();
                SqlDataReader sdr = queryFoto.ExecuteReader();

                if (sdr.Read())
                {
                    if (sdr["IMG_DOC"].ToString() != "")
                    {
                        //Console.WriteLine(sdr["IMG_DOC"].ToString());
                        blob = new Byte[(sdr.GetBytes(0, 0, null, 0, int.MaxValue))];
                        byte[] imgBytes = Convert.FromBase64String(sdr["IMG_DOC"].ToString());
                        sdr.Close();
                        AdmConexion.CloseConnection();
                        BitmapImage bitmapImage = new BitmapImage();
                        MemoryStream ms = new MemoryStream(imgBytes);
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = ms;
                        bitmapImage.EndInit();
                        ventanaImgDoc.GrpImgDoc.Source = bitmapImage;
                        ventanaImgDoc.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No tiene foto de identificación el estudiante.", "Identificación", MessageBoxButton.OK, MessageBoxImage.Warning);
                        sdr.Close();
                        AdmConexion.CloseConnection();
                    }
                }
                else
                {
                    sdr.Close();
                    AdmConexion.CloseConnection();
                }
          
            }
            catch (Exception)
            {
                AdmConexion.CloseConnection();
                //MessageBox.Show(ex.ToString(), "ERROR");
                MessageBox.Show("Hay incoveniente con la imagen cargada.", "Documento", MessageBoxButton.OK, MessageBoxImage.Warning);

                ventanaImgDocError.ShowDialog();
            }

        }

        //--------------MOSTRAR FOTO DEL ESTUDIANTE EN LA VENTANA DE FOTO
        public void MostrarFotoAlumno(int CUENTA_ALUMNO)
        {
            //AdmConexion2 = new AdmisionConnection(baseDeDatos);
            try
            {
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                Byte[] blob = new byte[0];
                SqlCommand queryAlumnoFoto = new SqlCommand("EXEC SP_LISTADO_SOLICITANTES_FOTO 3,@CUENTA_ALUMNO", AdmConexion2.OpenConnection());
                queryAlumnoFoto.Parameters.AddWithValue("@CUENTA_ALUMNO", CUENTA_ALUMNO);


                AdmConexion2.OpenConnection();
                SqlDataReader sdr = queryAlumnoFoto.ExecuteReader();

                if (sdr.Read())
                {
                    if (sdr["FOTO"].ToString() != "")
                    {
                        blob = new Byte[(sdr.GetBytes(0, 0, null, 0, int.MaxValue))];
                        byte[] imgBytes = Convert.FromBase64String(sdr["FOTO"].ToString());
                        sdr.Close();
                        AdmConexion2.CloseConnection();
                        BitmapImage bitmapImage = new BitmapImage();
                        MemoryStream ms = new MemoryStream(imgBytes);
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = ms;
                        bitmapImage.EndInit();
                        VMFOTO.Source = bitmapImage;
                        VMFOTO.Visibility = Visibility.Visible;
                        VMSINFOTO.Visibility = Visibility.Collapsed;
                        LbInfoMostrarFoto.Content = "";
                    }
                    else
                    {
                        //MessageBox.Show("No hay foto del estudiante.", "Foto", MessageBoxButton.OK, MessageBoxImage.Warning);
                        LbInfoMostrarFoto.Content = "No hay foto del estudiante.";
                        VMSINFOTO.Visibility = Visibility.Visible;
                        VMFOTO.Visibility = Visibility.Collapsed;
                        sdr.Close();
                        AdmConexion2.CloseConnection();
                    }

                }
                else
                {
                    sdr.Close();
                    AdmConexion2.CloseConnection();
                }


            }
            catch (Exception ex)
            {
                AdmConexion2.CloseConnection();
                //MessageBox.Show(ex.ToString(), "ERROR");
                MessageBox.Show("Hay incoveniente con la imagen cargada.", "Documento", MessageBoxButton.OK, MessageBoxImage.Warning);

            }

        }


        //------------------------------------------------- IMAGEN---------------------------------------
        private void txtIdentidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtIdentidad.Text != null && txtIdentidad.Text != "")
            {
                if (cbxTipoIdentificacion.SelectedValue.ToString() == "1")
                {
                    if (txtIdentidad.Text.Length > 13)
                    {
                        MessageBox.Show("Supera el límite del numero de identidad.", "Numero Identidad", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        //---------------------------------REFRESH------------

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgSolicitantes.SelectedItem = null;
            RefreshAprobado();
        }

        private void BtnRefreshPrevio_Click(object sender, RoutedEventArgs e)
        {
            dgSolicitantesPrevio.SelectedItem = null;
            DropDownRefreshAnioPeriodo = 1;
            LlenarComboboxAnioPeriodo();
            RefreshPrevio();
        }

        private void BtnRefreshFoto_Click(object sender, RoutedEventArgs e)
        {
            dgSolicitantesFoto.SelectedItem = null;
            RefreshFoto();
        
        }

        private void BtnRefreshDocPendientes_Click(object sender, RoutedEventArgs e)
        {
            dgEstudiantesDocPendientes.SelectedItem = null;
            RefreshDocPendientes();
        }

        public void RefreshPrevio()
        {
            btnGuardarPrevio.IsEnabled = false;
            btnAprobadaPrevio.IsEnabled = false;
            btnRechazadaPrevio.IsEnabled = false;
            btnInformacionIntentoCPrevio.IsEnabled = false;

            Estado_Solicitud_Previo.Text = "";
            txtFiltroPrevio.Text = "";
            txtAsignada.Text = "";
            txtIdentidadPrevio.Text = "";
            cbxTipoIdentificacionPrevio.SelectedValue =0;
            cbxEstadollamadaPrevio.SelectedValue = 0;
            cbxIntentosLlamadas.SelectedValue = 0;
            txtTelefono1Previo.Text = "";
            txtTelefono2Previo.Text = "";
            txtCorreo2Previo.Text = "";
            txtComentarioPrevio.Text = "";
            id_admision_globalPrevio = 0;
            txtObservacionPrevio.Text = "";
            txtIntentosFechaPrevio.Text = "";
            chkEnvioCorreo.IsChecked = false;
            LlenarListadoSolicitantesPrevio(Periodo_Activo);
            //ListadoAdmisionDuplicadaPrevio();
            cbxAnioPeriodo.SelectedItem = 0;

            if (dgSolicitantesPrevio.Items.Count > 0)
            {
                cbxAnioPeriodo.SelectedItem = 0;
                dgSolicitantesPrevio.ScrollIntoView(dgSolicitantesPrevio.Items[0], dgSolicitantesPrevio.Columns[1]);//Fila--Columna
            }

        }


        public void RefreshFoto()
        {
            txtFiltroFoto.Text = "";
            txtNombre_Completo_Foto.Text = "";
            txtTelefono1_Foto.Text = "";
            txtTelefono2_Foto.Text = "";
            txtCorreo2_Foto.Text = "";
            txtComentario_Foto.Text = "";
            optFemenino_Foto.IsChecked = false;
            optMaculino_Foto.IsChecked = false;
            VMFOTO.Visibility = Visibility.Collapsed;
            VMSINFOTO.Visibility = Visibility.Visible;
            LbInfoMostrarFoto.Content = "";
            LlenarListadoAlumnosFoto();
            if (dgSolicitantesFoto.Items.Count > 0)
            {
                dgSolicitantesFoto.ScrollIntoView(dgSolicitantesFoto.Items[0], dgSolicitantesFoto.Columns[1]);//Fila--Columna
            }

        }

        public void RefreshDocPendientes()
        {
            txtFiltroDocPendientes.Text = "";
            txtNombre_CompletoDocPendientes.Text = "";
            txtCorreo1DocPendientes.Text = "";
            txtCorreo2DocPendientes.Text = "";
            txtTelefono1DocPendientes.Text = "";
            txtTelefono2DocPendientes.Text = "";
            txtIdentidadDocPendientes.Text = "";
            cbxTipoIdentificacionDocPendientes.SelectedValue = 0;
            optFemeninoDocPendientes.IsChecked = false;
            optMaculinoDocPendientes.IsChecked = false;
            txtPlanDocPendientes.Text = "";
            txtCarreraDocPendientes.Text = "";
            btnEnviarDocPendientes.IsEnabled = false;
            btnRechazadaDocPendientes.IsEnabled = false;
            btnInformacionDoc.IsEnabled = false;
            dgDocumentosPendientesDetalle.AutoGenerateColumns = false;
            txtFechaDocPendientes.Text = "";
            Estado_Solicitud.Content = "";
            LlenarListadoAlumnosDocPendientes();
            if (dgEstudiantesDocPendientes.Items.Count>0)
            {
                dgEstudiantesDocPendientes.ScrollIntoView(dgEstudiantesDocPendientes.Items[0], dgEstudiantesDocPendientes.Columns[0]);//Fila--Columna
            }

        }

        public void RefreshAprobado()//Registro
        {
            txtFiltro.Text = "";
            txtprimerApellido.Text = "";
            txtSegundoApellido.Text = "";
            txtNombres.Text = "";
            cbxCampusEstudio.SelectedValue = 0;

            cbxPais.SelectedValue = 0;
            cbxDepartamento.SelectedValue = 0;
            cbxMunicipio.SelectedValue = 0;

            txtFechaNacimiento.Text = "";
            optFemenino.IsChecked = false;
            optMaculino.IsChecked = false;
            cbxTipoIdentificacion.SelectedValue = 0;
            txtIdentidad.Text = "";
            cbxEstadoCivil.SelectedValue = 0;
            txtDireccion.Text = "";

            cbxPaisActual.SelectedValue = 0;
            cbxDepartamentoActual.SelectedValue = 0;
            cbxMunicipioActual.SelectedValue = 0;
            cbxColoniaActual.SelectedValue = 0;

            txtTelefono1.Text = "";
            txtTelefono2.Text = "";
            txtCorreo1.Text = ""; 
            txtCorreo2.Text = "";
            cbxEstadoCivil.SelectedValue = 0;

            cbxPromocion.SelectedValue = null;
            cbxDescuento.SelectedValue = null;
            cbxSucursal.SelectedValue = 1;
            cbxEstatus.SelectedValue = 1;

            dgPlanes.AutoGenerateColumns = false;
            dgFamiliares.AutoGenerateColumns = false;
            dgInstitucion.AutoGenerateColumns = false;

            txtCompania.Text = "";
            txtTelefonoCompania.Text = "";
            txtPuesto.Text = "";
            txtNivelAcademico.Text = "";
            txtComentarios.Text = "";
            txtDireccionCompania.Text = "";
            txtCorreoCompania.Text = "";
            txtDepartamentoCompania.Text = "";
            txtFechaIngresoCompania.SelectedDate = null;

            txtObservacion.Text = "";

            LlenarListadoSolicitantes();
            if(dgSolicitantes.Items.Count>0)
            {
                dgSolicitantes.ScrollIntoView(dgSolicitantes.Items[0], dgSolicitantes.Columns[1]);//Fila--Columna
            }

        }

        //--------------------------------- FIN REFRESH------------


        //VENTANA PREVIO REGISTRO
        private void btnGuardarPrevio_Click(object sender, RoutedEventArgs e)
        {
            int VTipo = 1;
            ValidacionPrevio(VTipo);
            if (ValidacionesPrevio == "OK")
            {
                Gestion_Llamada(VTipo);

            }
            else
            {
                MessageBox.Show("" + ValidacionesPrevio + "", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnAprobadaPrevio_Click(object sender, RoutedEventArgs e)
        {
            int VTipo = 2;
            ValidacionPrevio(VTipo);
            if (ValidacionesPrevio == "OK")
            {
                Gestion_Llamada(VTipo);
            }
            else
            {
                MessageBox.Show("" + ValidacionesPrevio + "", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnRechazadaPrevio_Click(object sender, RoutedEventArgs e)
        {
            int VTipo = 3;
            ValidacionPrevio(VTipo);
            if (ValidacionesPrevio == "OK")
            {
                Gestion_Llamada(VTipo);

            }
            else
            {
                MessageBox.Show("" + ValidacionesPrevio + "", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        //VENTANA FOTO

        private void btnAprobadaFoto_Click(object sender, RoutedEventArgs e)
        {

            int VTipo = 1;
            ValidacionFoto();
            if (ValidacionesFoto == "OK")
            {
                Gestion_Foto(VTipo);
                //Console.WriteLine("entreo al boton aprobar" + ValidacionesFoto);
            }
            else
            {
                MessageBox.Show("" + ValidacionesFoto + "", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }


        private void btnNuevoIntentoFoto_Click(object sender, RoutedEventArgs e)
        {
            int VTipo = 2;
            ValidacionFoto();
            if (ValidacionesFoto == "OK")
            {
                Gestion_Foto(VTipo);
                //Console.WriteLine("entro al boton intento foto" + ValidacionesFoto);

            }
            else
            {
                MessageBox.Show("" + ValidacionesFoto + "", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void btnRechazadaFoto_Click(object sender, RoutedEventArgs e)
        {
            int VTipo = 3;
            ValidacionFoto();
            if (ValidacionesFoto == "OK")
            {
                Gestion_Foto(VTipo);
                //Console.WriteLine("entreo al rechazar" + ValidacionesFoto);

            }
            else
            {
                MessageBox.Show("" + ValidacionesFoto + "", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //VENTANA SOLICITUD COMPROMISO
        private void btnEnviarDocPendientes_Click(object sender, RoutedEventArgs e)
        {
            int VTipo = 0;
            Gestion_DocPendientes(VTipo);

        }

        private void btnRechazadaDocPendientes_Click(object sender, RoutedEventArgs e)
        {
            int VTipo = 2;
            Gestion_DocPendientes(VTipo);

        }


        //------------------------------------------VALIDACIONES
        public void ValidacionPrevio(int VTipo)
        {
            //AdmConexion = new AdmisionConnection(baseDeDatos);
            SqlCommand queryValidacionPrevio = new SqlCommand("EXEC SP_GESTION_ADMISION_LLAMADA @ID_ADMISION,@ASIGNADA,@ESTADO_LLAMADA,@COMENTARIO,null,@INTENTOS_CALL,@INTENTOS_FECHA,null," + VTipo + ",@TIDCOMENTARIO,@TIDFECHA,1,0", AdmConexion.OpenConnection());
            queryValidacionPrevio.Parameters.AddWithValue("@ID_ADMISION", id_admision_globalPrevio);
            queryValidacionPrevio.Parameters.AddWithValue("@ASIGNADA", txtAsignada.Text ?? null);
            queryValidacionPrevio.Parameters.AddWithValue("@ESTADO_LLAMADA", cbxEstadollamadaPrevio.SelectedValue ?? DBNull.Value);
            queryValidacionPrevio.Parameters.AddWithValue("@COMENTARIO", txtComentarioPrevio.Text ?? null);
            queryValidacionPrevio.Parameters.AddWithValue("@TIDCOMENTARIO", id_comentario);
            queryValidacionPrevio.Parameters.AddWithValue("@INTENTOS_CALL", cbxIntentosLlamadas.SelectedItem ?? DBNull.Value);
            queryValidacionPrevio.Parameters.AddWithValue("@INTENTOS_FECHA", txtIntentosFechaPrevio.ToString());
            queryValidacionPrevio.Parameters.AddWithValue("@TIDFECHA", id_fecha_intentos);

            //Console.WriteLine(txtIntentosFechaPrevio.Text);

            AdmConexion.OpenConnection();
            SqlDataReader resultadoPrevio = queryValidacionPrevio.ExecuteReader();
            if (resultadoPrevio.Read())
            {
                ValidacionesPrevio = resultadoPrevio[0].ToString();
                //Console.WriteLine(ValidacionesPrevio);
            }
            AdmConexion.CloseConnection();
        }

        public void ValidacionFoto()
        {
            AdmConexion = new AdmisionConnection(baseDeDatos);
            SqlCommand queryValidacionFoto = new SqlCommand("EXEC SP_GESTION_FOTO null,null,null,null", AdmConexion.OpenConnection());
            queryValidacionFoto.Parameters.AddWithValue("@CUENTA_ALUMNO", cuenta_alumno);


            AdmConexion.OpenConnection();
            SqlDataReader resultadoFoto = queryValidacionFoto.ExecuteReader();
            if (resultadoFoto.Read())
            {
                ValidacionesFoto = resultadoFoto[0].ToString();
                //Console.WriteLine(ValidacionesFoto);
            }
            AdmConexion.CloseConnection();
        }

        //-------------------------------------FIN VALIDACION
        //---------------------------GESTION PARA GUARDAR,APROBAR O RECHAZAR LA SOLICITUD DE ADMSION-------------------
        public void Gestion_Llamada(int tipo)
        {
            int VEnvioCorreo = 0;
            try
            {
                if (tipo == 1)
                {
                    MessageBox.Show("Se han guardado la información.", "Información Guardadas", MessageBoxButton.OK, MessageBoxImage.Information);
                    ValidacionGAR = 1;

                }
                else if (tipo == 2)
                {// MessageBox.Show("¿Está seguro que desea aprobar la solicitud?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                    if (MessageBox.Show("¿Está seguro que desea aprobar la solicitud?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                     
                        ValidacionGAR = 1;

                    }
                    else
                    {
                        RefreshPrevio();
                        ValidacionGAR = 0;
                    }
                }
                else if (tipo == 3)
                {//MessageBox.Show("¿Está seguro que desea rechazar la solicitud?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (MessageBox.Show("¿Está seguro que desea rechazar la solicitud?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        ValidacionGAR = 1;
                        if (chkEnvioCorreo.IsChecked == true)
                        {
                            VEnvioCorreo = 1;
                        }

                    }
                    else
                    {
                        RefreshPrevio();
                        ValidacionGAR = 0;
                    }
                }

                if (ValidacionGAR == 1)
                {
                    AdmConexion.OpenConnection();

                    string sql = "EXEC SP_GESTION_ADMISION_LLAMADA @ID_ADMISION,@ASIGNADA,@ESTADO_LLAMADA,@COMENTARIO,@ID_USUARIO_APROBO_CALL,@INTENTOS_CALL,@INTENTOS_FECHA,@OBSERVACION_APROBO," + tipo + ",@TIDCOMENTARIO,@TIDFECHA,2,@ENVIAR_CORREO";
                    using (SqlCommand cmd = new SqlCommand(sql, AdmConexion.OpenConnection()))
                    {
                        cmd.Parameters.Add("@ID_ADMISION", SqlDbType.Int).Value = id_admision_globalPrevio;
                        cmd.Parameters.Add("@ASIGNADA", SqlDbType.VarChar).Value = txtAsignada.Text ?? null;
                        cmd.Parameters.Add("@ESTADO_LLAMADA", SqlDbType.VarChar).Value = cbxEstadollamadaPrevio.SelectedValue ?? DBNull.Value;
                        cmd.Parameters.Add("@COMENTARIO", SqlDbType.VarChar).Value = txtComentarioPrevio.Text ?? null;
                        cmd.Parameters.Add("@ID_USUARIO_APROBO_CALL", SqlDbType.Int).Value = id_usuario;
                        cmd.Parameters.Add("@INTENTOS_CALL", SqlDbType.Int).Value = cbxIntentosLlamadas.SelectedValue ?? DBNull.Value;
                        cmd.Parameters.Add("@OBSERVACION_APROBO", SqlDbType.VarChar).Value = txtObservacionPrevio.Text ?? null;
                        cmd.Parameters.Add("@INTENTOS_FECHA", SqlDbType.VarChar).Value = txtIntentosFechaPrevio.Text ?? null;
                        cmd.Parameters.Add("@TIDCOMENTARIO", SqlDbType.Int).Value = id_comentario;
                        cmd.Parameters.Add("@TIDFECHA", SqlDbType.Int).Value = id_fecha_intentos;
                        cmd.Parameters.Add("@ENVIAR_CORREO", SqlDbType.Int).Value = VEnvioCorreo;
                        //Console.WriteLine(id_comentario);
                        //Console.WriteLine(VEnvioCorreo);

                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                    AdmConexion.CloseConnection();
                    RefreshPrevio();
                }
            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }

        }


        //---------------------------GESTION PENDIENTE O NUEVO INTENTO SOBRE EL PROCESO DE APROBACION DE LA FOTO-------------------
        public void Gestion_Foto(int tipo)
        {
            try
            {
                if (tipo == 1)
                {
                    if (MessageBox.Show("¿Está seguro que desea aprobar la foto?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        ValidacionRPE = 1;

                    }
                    else
                    {
                        ValidacionRPE = 0;
                    }
                }
                else if (tipo == 2)
                {
                    if (MessageBox.Show("¿Está seguro del nuevo intento de la foto?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        ValidacionRPE = 1;

                    }
                    else
                    {
                        ValidacionRPE = 0;
                    }
                }
                else if (tipo == 3)
                {
                    if (MessageBox.Show("¿Está seguro de rechazar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        ValidacionRPE = 1;

                    }
                    else
                    {
                        ValidacionRPE = 0;
                    }
                }

                if (ValidacionRPE == 1)
                {
                    //AdmConexion = new AdmisionConnection(baseDeDatos);
                    Console.WriteLine("ENTRO GESTION FOTO" + tipo);
                    AdmConexion.OpenConnection();
                    string sql = "EXEC SP_GESTION_FOTO @CUENTA_ALUMNO,@COMENTARIO,@ID_USUARIO," + tipo + "";
                    using (SqlCommand cmd = new SqlCommand(sql, AdmConexion.OpenConnection()))
                    {
                        cmd.Parameters.Add("@CUENTA_ALUMNO", SqlDbType.Int).Value = cuenta_alumno;
                        cmd.Parameters.Add("@COMENTARIO", SqlDbType.VarChar).Value = txtComentario_Foto.Text ?? null;
                        cmd.Parameters.Add("@ID_USUARIO", SqlDbType.Int).Value = id_usuario;

                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                    AdmConexion.CloseConnection();
                    RefreshFoto();
                }
            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }

        }

        //---------------------------GESTION PENDIENTE O RECHAZADA SOLICITUD COMPROMISO DE DOCUMENTOS PENDIENTES-------------------
        public void Gestion_DocPendientes(int tipo)
        {
            try
            {

                if (tipo == 0)
                {
                    if (MessageBox.Show("¿Está seguro de enviar el solicitud de compromiso?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        ValidacionSC = 1;

                    }
                    else
                    {
                        ValidacionSC = 0;
                    }
                }
                else if (tipo == 2)
                {
                    if (MessageBox.Show("¿Está seguro de rechazar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        ValidacionSC = 1;

                    }
                    else
                    {
                        ValidacionSC = 0;
                    }
                }

                if (ValidacionSC == 1)
                {
                    //AdmConexion = new AdmisionConnection(baseDeDatos);
                    Console.WriteLine("ENTRO SOLICITUD COMPROMISO" + tipo);
                    SqlCommand queryValidacionDocPendiente = new SqlCommand("EXEC SP_GESTION_SOLICITUD_COMPROMISO @CUENTA_ALUMNO,@FECHA_PLAZO,@ID_USUARIO," + tipo + "", AdmConexion.OpenConnection());
                    queryValidacionDocPendiente.Parameters.AddWithValue("@CUENTA_ALUMNO", cuenta_alumno);
                    queryValidacionDocPendiente.Parameters.AddWithValue("@ID_USUARIO", id_usuario);


                    if (Convert.ToInt32(AdmConexion.OpenConnection().ConnectionTimeout.ToString()) == 15 || Convert.ToInt32(queryValidacionDocPendiente.CommandTimeout) == 30)
                    {
                        if (txtFechaDocPendientes.ToString() != null && txtFechaDocPendientes.ToString() != "")
                        {
                            queryValidacionDocPendiente.Parameters.AddWithValue("@FECHA_PLAZO", DateTime.Parse(txtFechaDocPendientes.Text));
                        }
                        else
                        {
                            queryValidacionDocPendiente.Parameters.AddWithValue("@FECHA_PLAZO", DBNull.Value);
                        }
                        AdmConexion.OpenConnection();
                        SqlDataReader resultadoDocPendiente = queryValidacionDocPendiente.ExecuteReader();
                        if (resultadoDocPendiente.Read())
                        {
                            ValidacionesDocPendientes = resultadoDocPendiente[0].ToString();
                            if (ValidacionesDocPendientes != "OK")
                            {
                                MessageBox.Show("" + ValidacionesDocPendientes + "", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

                            }
                            else
                            {
                                AdmConexion.CloseConnection();
                                RefreshDocPendientes();
                                CLoading(Convert.ToInt32(AdmConexion.OpenConnection().ConnectionTimeout.ToString()));
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Error de Conexión!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //Console.WriteLine("Gestion_DocPendientes conexion a la base de datos " + Convert.ToString(AdmConexion.OpenConnection().ConnectionTimeout));
                    //Console.WriteLine("Gestion_DocPendientes request tiempo busquedad" + Convert.ToString(queryValidacionDocPendiente.CommandTimeout));
                    AdmConexion.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                AdmConexion.CloseConnection();
                MessageBox.Show("Error:: " + ex);
            }

        }

        private void cbxIntentosLlamadas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbxIntentosLlamadas.ItemsSource = null;
            if (cbxIntentosLlamadas.SelectedValue != null && Convert.ToInt32(cbxIntentosLlamadas.SelectedValue) != 0)
            {
                DateTime FechaIntentos = Convert.ToDateTime(DateTime.Now.ToString());
                FechaIntentos = FechaIntentos.AddDays(0);
                txtIntentosFechaPrevio.Text = FechaIntentos.ToString("d");
                if (Convert.ToInt32(cbxIntentosLlamadas.SelectedValue.ToString()) == id_intentos_call)
                {
                    txtComentarioPrevio.Text = comentario_previo;
                    txtIntentosFechaPrevio.Text = intento_fecha_previo;
                }
                else
                {
                    txtComentarioPrevio.Text = "";

                }
            }
        }


        private void dgSolicitantes_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (dgSolicitantes.Columns != null)
            {
                //Ocultar la Primera Columna
                dgSolicitantes.Columns[0].Visibility = Visibility.Collapsed;

            }
        }


        private void dgSolicitantesPrevio_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            

            if (dgSolicitantesPrevio.Columns != null || ((DataGrid)sender).CurrentItem == null)
            {
                //Ocultar la Primera Columna
                dgSolicitantesPrevio.Columns[0].Visibility = Visibility.Collapsed;


                //ASIGNACION DE COLORES A LAS FILA AL DATAGRID
                DataGridRow datarow = e.Row;
                DataRowView rowItem = datarow.Item as DataRowView;

                ListAdmisionPrevio.Add(rowItem["ID_ADMINSION"].ToString());

                //HACE UNA COMPARACION CON LA ID_ADMISION(LISTA ADMISION DUPLICADA) Y ID_ADMISION(LISTA ADMISION QUE SE INSERTA EN EL DATAGRID)
                List<ADM_DUPLICADA> ADMISION_DUPLICADAS = ListAdmisionDuplicada.Where(x => ListAdmisionPrevio.Any(y => y == x.ID_ADMISION)).ToList(); 

                foreach (ADM_DUPLICADA ADMISIONDUPLICADA in ADMISION_DUPLICADAS)
                {
                    Console.WriteLine("DUPLICADA LoadingRow 1 " + ADMISIONDUPLICADA.ID_ADMISION + " " + ADMISIONDUPLICADA.CUENTA_ALUMNO +" "+ADMISIONDUPLICADA.COLOR+" "+ ADMISIONDUPLICADA.INTENTOS_DIAS);

                    if (rowItem["ID_ADMINSION"].ToString() == ADMISIONDUPLICADA.ID_ADMISION && ADMISIONDUPLICADA.CUENTA_ALUMNO == "") // QUE NO TIENE UNA CUENTA
                    {
                        //datagrid.Background = new SolidColorBrush(Colors.DarkGreen);
                        datarow.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ADMISIONDUPLICADA.COLOR));
                        datarow.FontWeight = FontWeights.SemiBold;
                        return;
                    }
                    else if (rowItem["ID_ADMINSION"].ToString() == ADMISIONDUPLICADA.ID_ADMISION && ADMISIONDUPLICADA.CUENTA_ALUMNO != "")// SI TIENE UNA CUENTA
                    {
                        datarow.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ADMISIONDUPLICADA.COLOR)); //Colors.Blue
                        datarow.FontWeight = FontWeights.SemiBold;
                        return;
                    }
                    else if (rowItem["ID_ADMINSION"].ToString() != ADMISIONDUPLICADA.ID_ADMISION )
                    {
                        datarow.Foreground = new SolidColorBrush(Colors.Black);
                        datarow.FontWeight = FontWeights.Normal;

                    }


                }
            }

        }

        private void dgSolicitantesFoto_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (dgSolicitantesFoto.Columns != null)
            {
                DataGridRow datarow = e.Row;
                DataRowView rowItem = datarow.Item as DataRowView;
                if (rowItem != null)
                {
                    //Obtienes la fila
                    DataRow row = rowItem.Row;
                    //El campo para validar el valor 
                    if (Int32.Parse(row["IDESTADO"].ToString()) == 0)
                    {
                        //datagrid.Background = new SolidColorBrush(Colors.DarkGreen);
                        datarow.Foreground = new SolidColorBrush(Colors.DarkGreen);
                        
                    }
                    else if (Int32.Parse(row["IDESTADO"].ToString()) == 2)
                    {
                        datarow.Foreground = new SolidColorBrush(Colors.DarkBlue);
                        
                    }

                }
            }

        }

        private void dgEstudiantesDocPendientes_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (dgEstudiantesDocPendientes.Columns != null)
            {

                DataGridRow datarow = e.Row;
                DataRowView rowItem = datarow.Item as DataRowView;
                if (rowItem != null)
                {

                    //Obtienes la fila
                    DataRow row = rowItem.Row;
                    //El campo para validar el valor 
                    if (Int32.Parse(row["IDESTADO"].ToString()) == 0)//SE ENVIO CORREO(PENDIENTE EN ACEPTAR EL CORREO EL ESTUDIANTE)
                    {
                        //datagrid.Background = new SolidColorBrush(Colors.DarkGreen);
                        datarow.Foreground = new SolidColorBrush(Colors.OrangeRed);
                    }
                    else if (Int32.Parse(row["IDESTADO"].ToString()) == 1)//ACEPTÓ LA SOLICITUD DE COMPROMISO
                    {
                        datarow.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        datarow.Foreground = new SolidColorBrush(Colors.Black);//GENERAL
                    }

                }
            }


        }

        private void btnInformacionIntentoCPrevio_Click(object sender, RoutedEventArgs e)
        {
            frmInformacionIntentosCP ventanaInformacionIntentosCP = new frmInformacionIntentosCP();
            ventanaInformacionIntentosCP.baseDeDatos = baseDeDatos;
            ventanaInformacionIntentosCP.id_admision = id_admision_globalPrevio;
            ventanaInformacionIntentosCP.ShowDialog();

            //Console.WriteLine("Informacion" + Convert.ToInt32(id_admision_globalPrevio));
        }

        private void txtComentarioPrevio_TextChanged(object sender, TextChangedEventArgs e)
        {
            string caracter = "*";
            bool caracterespecial;

            if (caracterespecial = caracter.Intersect(txtComentarioPrevio.Text).Count() > 0)
            {
                txtComentarioPrevio.Text = "";
            }

        }

        //----------------------------FIN GESTION

        private void tabPrevio_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (permiso_previo == 0)
            {
                tabPrevio.IsEnabled = false;
                gtabPrevio.IsEnabled = false;
                tabPrevio.IsSelected = false;
            }

        }

        private void tabAprobado_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (permiso_aprobo == 0)
            {
                tabAprobado.IsEnabled = false;
                gtabAprobado.IsEnabled = false;

            }

        }

        private void tabFoto_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (permiso_foto == 0)
            {
                tabFoto.IsEnabled = false;
                gtabFoto.IsEnabled = false;

            }

        }

        private void tabCompromisoDocumento_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (permiso_documento == 0)
            {
                tabCompromisoDocumento.IsEnabled = false;
                gtabCompromisoDocumento.IsEnabled = false;

            }

        }

        private void TabControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (permiso_previo != 0)
            {
                tabPrevio.IsSelected = true;
                
            }
            else if (permiso_aprobo != 0)
            {
                tabAprobado.IsSelected = true;
            }
            else if (permiso_foto != 0)
            {
                tabFoto.IsSelected = true;
            }
            else if (permiso_documento != 0)
            {
                tabCompromisoDocumento.IsSelected = true;
            }
        }

        private void dgEstudiantesDocPendientes_Loaded(object sender, RoutedEventArgs e)
        {
            if (tabCompromisoDocumento.IsSelected==true)
            {
                CLoading(35);
                LlenarListadoAlumnosDocPendientes();
                if (dgEstudiantesDocPendientes.Items.Count > 0)
                {
                    Console.WriteLine("DOCUMENTOS" + dgEstudiantesDocPendientes.Items.Count.ToString());
                    dgEstudiantesDocPendientes.SelectedItem = null;
                    RefreshDocPendientes();
                }

            }
        }

        private void dgSolicitantes_Loaded(object sender, RoutedEventArgs e)
        {
            if (tabAprobado.IsSelected==true)
            {   
                CLoading(15);
                LlenarListadoSolicitantes();
                LlenarComboboxsPais();
                LlenarCombosOtros();
                
                if (dgSolicitantes.Items.Count > 0)
               {
                    Console.WriteLine("REGISTRO");
                    dgSolicitantes.SelectedItem = null;
                    RefreshAprobado();
               }

            }
        }

        private void dgSolicitantesFoto_Loaded(object sender, RoutedEventArgs e)
        {
            if (tabFoto.IsSelected==true)
            {
                CLoading(15);
                LlenarListadoAlumnosFoto();
                Console.WriteLine("FOTOS");
                if (dgSolicitantesFoto.Items.Count > 0)
                {
                    Console.WriteLine("FOTOS" + dgSolicitantesFoto.Items.Count.ToString());
                    dgSolicitantesFoto.SelectedItem = null;
                    RefreshFoto();

                }

            }
        }

        private void dgSolicitantesPrevio_Loaded(object sender, RoutedEventArgs e)
        {
            if (tabPrevio.IsSelected==true)
            { 
                CLoading(35);
               // LlenarComboboxAnioPeriodo();
                Console.WriteLine("dgSolicitantesPrevio_Loaded" + cbxAnioPeriodo.SelectedValue);
                Periodo_Activo = (string)cbxAnioPeriodo.SelectedValue;
               //LlenarListadoSolicitantesPrevio(Periodo_Activo);
                
                LlenarCombosOtros();
                LlenarIntentosCall(1);
                //PeriodoActivo();

                Console.WriteLine("PREVIO ADMISION dgSolicitantesPrevio_Loaded ANTES" + dgSolicitantesPrevio.Items.Count.ToString());

                //if (dgSolicitantesPrevio.Items.Count > 0)
                //{
                //    Console.WriteLine("PREVIO ADMISION dgSolicitantesPrevio_Loaded DESPUES" + dgSolicitantesPrevio.Items.Count.ToString());
                //    dgSolicitantesPrevio.SelectedItem = null;
                //    RefreshPrevio();


                //}

            }

        }

   
        private void cbxAnioPeriodo_Loaded(object sender, RoutedEventArgs e)
        {
            if (permiso_previo != 0)
            {
               LlenarComboboxAnioPeriodo();
                Console.WriteLine("cbxAnioPeriodo_Loaded" + cbxAnioPeriodo.SelectedValue);
            }
        }

        private void cbxAnioPeriodo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LlenarListadoSolicitantesPrevio(Periodo_Activo);
            //if (dgSolicitantesPrevio.Items.Count > 0)
            //{
            //    CLoading(35);
            //    RefreshPrevio();

            //    Console.WriteLine("cbxAnioPeriodo_SelectionChanged" + cbxAnioPeriodo.SelectedValue);
            //}



        }

        private void cbxAnioPeriodo_DropDownOpened(object sender, EventArgs e)
        {
            DropDownRefreshAnioPeriodo = 1;
            LlenarComboboxAnioPeriodo();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //LlenarListadoSolicitantesPrevio();
        }

    
    }

}
