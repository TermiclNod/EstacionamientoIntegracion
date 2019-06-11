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
    /// Descripción breve de crudInfo
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class crudInfo : System.Web.Services.WebService
    {
        SqlConnection conn = new SqlConnection();

        //Método que registra una reseña
        [WebMethod]
        public bool RegistraResena(int puntaje, string comentario, int id_usuario)
        {
            bool t = false;

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();
            var sql = "insert into resena values("+puntaje+",'"+comentario+"',"+id_usuario+")";
            SqlCommand cmd = new SqlCommand(sql, conn);

            int i = cmd.ExecuteNonQuery();
            if (i > 0)
                t = true;
            return t;
        }

        //Método que registra una Denuncia
        [WebMethod]
        public bool RegistraDenuncia(string descripcion, int id_usuario)
        {
            bool t = false;

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();
            var sql = "insert into denuncia values('" + descripcion + "','" + id_usuario + "')";
            SqlCommand cmd = new SqlCommand(sql, conn);

            int i = cmd.ExecuteNonQuery();
            if (i > 0)
                t = true;
            return t;
        }


        //Método que elimina una reseña
        [WebMethod]
        public bool EliminaResena(int id)
        {
            bool t = false;

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();
            var sql = "delete from resena where id_resena="+id;
            SqlCommand cmd = new SqlCommand(sql, conn);

            int i = cmd.ExecuteNonQuery();
            if (i > 0)
                t = true;
            return t;
        }

        //Método que elimina una Denuncia
        [WebMethod]
        public bool EliminaDenuncia(int id)
        {
            bool t = false;

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();
            var sql = "delete from denuncia where id_denuncia="+id;
            SqlCommand cmd = new SqlCommand(sql, conn);

            int i = cmd.ExecuteNonQuery();
            if (i > 0)
                t = true;
            return t;
        }

        //Lista todas las reseñas
        [WebMethod]
        public DataSet ListaResenas()
        {

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            SqlDataAdapter da = new SqlDataAdapter("select * from resena", conn);
            DataSet ds = new DataSet();
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.Fill(ds);
            return ds;
        }

        //Lista todas las denuncias
        [WebMethod]
        public DataSet ListaDenuncia()
        {

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            SqlDataAdapter da = new SqlDataAdapter("select * from denuncia", conn);
            DataSet ds = new DataSet();
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.Fill(ds);
            return ds;
        }

    }
}
