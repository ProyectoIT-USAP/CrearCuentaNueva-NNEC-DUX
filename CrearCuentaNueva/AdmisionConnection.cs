using System;
using System.Data;
using System.Data.SqlClient;

using System.Windows;

namespace CrearCuentaNueva
{
    class AdmisionConnection
    {
        static string vConnectionBD;
        public SqlConnection Connection;

        public AdmisionConnection(string nConnectionBD)//BASEDATOS,SERVER,USUARIO,PASSOWORD
        {
            try //USAP - 1C8QSD2\\MSSQLSERVER2019
            {
                //vConnectionBD = "server=172.21.10.18; user=jessica; password=jessica.test; database=" + nConnectionBD + "; integrated security=false";//SERVER .18
                //vConnectionBD="server=172.21.10.152; user=sa; password=abc+123; database=" + nConnectionBD + "; integrated security=false";//SERVER .152
                //vConnectionBD = "server=172.21.12.197; user=SIGA_D; password=MayD4BwithU; database=" + nConnectionBD + "; integrated security=false";//AWS .197
                vConnectionBD = "server=172.21.20.25\\MSSQLSERVER2019; user=SIGA_D; password=MayD4BwithU; database=" + nConnectionBD + "; integrated security=false";// USAP-1C8QSD2 PRUEBA

                Connection = new SqlConnection("" + vConnectionBD + "");

                Console.WriteLine("CONEXION1 " + vConnectionBD);
                
            }
            catch(Exception ex)
            {
               MessageBox.Show("Error: AdmisionConnection" + ex);
            }
        }


        public SqlConnection OpenConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();

            }

            return Connection;
            
        }

        public SqlConnection CloseConnection()
        {
            if (Connection.State == ConnectionState.Open )
            {
                Connection.Close();
            }
    
             return Connection;
       
        }

    }

    class AdmisionConnectionSinBD //SERVER,USUARIO,PASSOWORD
    {
        static string vConnectionSinBD;
        public SqlConnection ConnectionSinBD;

        public AdmisionConnectionSinBD()
        {
            try
            {
                //vConnectionBD = "server=172.21.10.18; user=jessica; password=jessica.test; database=" + nConnectionBD + "; integrated security=false";//SERVER .18
                //vConnectionBD="server=172.21.10.152; user=sa; password=abc+123; database=" + nConnectionBD + "; integrated security=false";//SERVER .152
                //vConnectionSinBD = "server=172.21.12.197; user=SIGA_D; password=MayD4BwithU; integrated security=false";//AWS .197
                vConnectionSinBD = "server=172.21.20.25\\MSSQLSERVER2019; user=SIGA_D; password=MayD4BwithU; integrated security=false";// USAP-1C8QSD2 PRUEBA

                ConnectionSinBD = new SqlConnection("" + vConnectionSinBD+ "");

                Console.WriteLine("CONEXION2 " + vConnectionSinBD);
               
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: AdmisionConnection1" + ex);
            }
        }

        public SqlConnection OpenConnectionSinBD()
        {
           if (ConnectionSinBD.State == ConnectionState.Closed)
           {
               ConnectionSinBD.Open();
           }
            return ConnectionSinBD;
          
        }

        public SqlConnection CloseConnection1()
        {
            if (ConnectionSinBD.State == ConnectionState.Open)
            {
                ConnectionSinBD.Close();
            }

            return ConnectionSinBD;

        }

    }
}
