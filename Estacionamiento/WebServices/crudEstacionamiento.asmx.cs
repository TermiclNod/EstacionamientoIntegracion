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

        //Método que inserta valores en la tabla de estacionamiento
        [WebMethod]
        public bool InsertEstacionamiento(string comuna,string direccion, string comentario, int id, int valor, string auto)
        {
            bool t = false;

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();
            var sql = "insert into estacionamiento values ('" + comuna + "','" + direccion + "','" + comentario + "'," + id + ",0," + valor + ",'" + auto + "',0,0);";
            SqlCommand cmd = new SqlCommand(sql,conn);

            int i = cmd.ExecuteNonQuery();
            if (i>0)
                t = true;
            return t;

        }

        //Lista los estacionamientos de un usuario
        [WebMethod]
        public DataSet ListaEstacionamiendoById(int id_usuario)
        {

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            SqlDataAdapter da = new SqlDataAdapter("select id_estacionamiento,comuna_estacionamiento,direccion_estacionamiento,comentario_estacionamiento,arrendatario,valor_estacionamiento,tipovehiculo_estacionamiento,imagen_estacionamiento,estado_estacionamiento from estacionamiento where id_usuario="+id_usuario, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        //Método que elimina un estacionamiento de un usuario
        [WebMethod]
        public bool EliminaEstacionamiento(int id_estacionamiento, int id_usuario)
        {
            bool t = false;

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();
            var sql = "delete from estacionamiento where id_estacionamiento="+id_estacionamiento+" and id_usuario="+id_usuario+"";
            SqlCommand cmd = new SqlCommand(sql, conn);

            int i = cmd.ExecuteNonQuery();
            if (i > 0)
                t = true;
            return t;

        }
    }
}
