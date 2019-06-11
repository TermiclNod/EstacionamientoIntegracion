using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;

namespace WebServices
{
    /// <summary>
    /// Descripción breve de crudEstacionamiento
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class crudEstacionamiento : System.Web.Services.WebService
    {
        SqlConnection conn = new SqlConnection();

        public struct Estacionamiento
        {
            public int id_estacionamiento;
            public string comuna_estacionamiento;
            public string direccion_estacionamiento;
            public string comentario_estacionamiento;

            public int id_usuario;

            public int arrendatario;

            public int valor_estacionamiento;
            public string tipovehiculo_estacionamiento;
            public string imagen_estacionamiento;
            public int estado_estacionamiento;
        }


        [WebMethod]
        public bool InsertEstacionamiento(string comuna,string direccion, string comentario, int id, int valor, string auto)
        {
            bool t = false;

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();
            var sql = "insert into estacionamiento (comuna_estacionamiento,direccion_estacionamiento,comentario_estacionamiento,id_usuario,valor_estacionamiento,tipovehiculo_estacionamiento,estado_estacionamiento) values ('"+comuna+"','"+direccion+"','"+comentario+"',"+id+","+valor+",'"+auto+"',0);";
            SqlCommand cmd = new SqlCommand(sql,conn);

            int i = cmd.ExecuteNonQuery();
            if (i>0)
                t = true;
            return t;

        }
    }
}
