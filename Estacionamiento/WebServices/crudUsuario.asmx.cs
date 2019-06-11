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
    /// Descripción breve de crudUsuario
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class crudUsuario : System.Web.Services.WebService
    {
        SqlConnection conn = new SqlConnection();
        public struct User
        {
            public int id_usuario;
            public string nombre_usuario;
            public string apellido_usuario;
            public string rut_usuario;
            public string contrasena_usuario;
            public int estado_usuario;
            public string correo_usuario;
            public string imagen_usuario;
            public int tipo_usuario;
        }

        //Lista de todos los usuarios existentes, para la gestión del Administrador.
        [WebMethod]
        public DataSet ListaUsuarios()
        {
            
            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            SqlDataAdapter da = new SqlDataAdapter("select id_usuario from usuario",conn);
            DataSet ds = new DataSet();
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.Fill(ds);
            return ds;
        }

        //Método que valida si existen los datos ingresados, permite ingresar a la aplicación.
        [WebMethod]
        public bool IniciarSesion(string correo_usuario,string contrasena_usuario)
        {
            bool t = false;

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();

            var sql = ("select * from usuario where correo_usuario = '"+correo_usuario+"' and contrasena_usuario='"+contrasena_usuario+"';");

            SqlDataAdapter da = new SqlDataAdapter(sql,conn);
            DataSet ds = new DataSet();


            int i = da.Fill(ds);

            if (i > 0)
                t = true;
            return t;
        }

        //Método que cambia el estado, para que así el administrador lo inhabilite.
        [WebMethod]
        public bool CambiaEstado(int id_usuario)
        {
            bool t = false;

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();

            var sql = ("update usuario set estado_usuario = 0 where id_usuario="+id_usuario);

            SqlCommand cmd = new SqlCommand(sql, conn);

            int i = cmd.ExecuteNonQuery();
            if (i > 0)
                t = true;
            return t;
        }

        //Método que registra a un usuario nuevo.
        [WebMethod]
        public bool CreaUsuario(string nombre_usuario, string apellido_usuario, string rut_usuario, string contrasena_usuario,int estado_usuario ,string correo_usuario)
        {
            bool t = false;
            
            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();
            var sql = "insert into usuario (nombre_usuario,apellido_usuario,rut_usuario,contrasena_usuario,estado_usuario,correo_usuario) values('"+ nombre_usuario + "','" + apellido_usuario + "','" + rut_usuario + "','" + contrasena_usuario + "'," + estado_usuario + ",'" + correo_usuario + "');";
            SqlCommand cmd = new SqlCommand(sql,conn);

            int i = cmd.ExecuteNonQuery();
            if (i > 0)
                t = true;
            return t;
        }

        //Método para asociar un tipo de usuario.
        [WebMethod]
        public bool InsertaUsuario_Tipo(int id_usuario,int id_tipo)
        {
            bool t = false;

            conn.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
            conn.Open();
            
            var sql = "insert into usuario_tipo values(@tipo,@usuario);";
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(new SqlParameter("@usuario", id_usuario));
            cmd.Parameters.Add(new SqlParameter("@tipo", id_tipo));

            int i = cmd.ExecuteNonQuery();
            if (i > 0)
                t = true;
            return t;
        }

        //Método que permite conseguir los datos de un usuario, para así manejar las SESSIONS.
        [WebMethod]
        public User GetUser(string emailAddress,string pass)
        {
            int id = 0;
            string nombre = string.Empty;
            string apellido = string.Empty;
            string rut = string.Empty;
            string contrasena = string.Empty;
            int estado = 0;
            string correo = string.Empty;
            string imagen = string.Empty;
            int tipo = 0;

            using (var con = new SqlConnection())
            {
                con.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
                con.Open();

                var sql = "select u.id_usuario,u.nombre_usuario,u.apellido_usuario,u.rut_usuario,u.contrasena_usuario,u.estado_usuario,u.correo_usuario,u.imagen_usuario,ut.id_tipo from usuario u inner join usuario_tipo ut on u.id_usuario = ut.id_usuario where correo_usuario = @email and contrasena_usuario = @pass";
                SqlCommand cmd = new SqlCommand(sql,con);

                cmd.Parameters.Add(new SqlParameter("@email", emailAddress));
                cmd.Parameters.Add(new SqlParameter("@pass",pass));
                var sqlReader = cmd.ExecuteReader();
                while (sqlReader.Read())
                {
                    id = int.Parse(sqlReader.GetValue(0).ToString());
                    nombre = sqlReader.GetValue(1).ToString();
                    apellido = sqlReader.GetValue(2).ToString();
                    rut = sqlReader.GetValue(3).ToString();
                    contrasena =sqlReader.GetValue(4).ToString();
                    estado = int.Parse(sqlReader.GetValue(5).ToString());
                    correo = sqlReader.GetValue(6).ToString();
                    imagen = sqlReader.GetValue(7).ToString();
                    tipo = int.Parse(sqlReader.GetValue(8).ToString());
                }
            }
            return new User { id_usuario = id,
                              nombre_usuario=nombre,
                              apellido_usuario=apellido,
                              rut_usuario=rut,
                              contrasena_usuario=contrasena,
                              estado_usuario=estado,
                              correo_usuario=correo,
                              imagen_usuario=imagen,
                              tipo_usuario=tipo};
        }

        //Lo mismo de arriba, pero sin id_tipo
        [WebMethod]
        public User GetUserWithoutTipo(string emailAddress, string pass)
        {
            int id = 0;
            string nombre = string.Empty;
            string apellido = string.Empty;
            string rut = string.Empty;
            string contrasena = string.Empty;
            int estado = 0;
            string correo = string.Empty;
            string imagen = string.Empty;

            using (var con = new SqlConnection())
            {
                con.ConnectionString = "data source=TERMICL-ROG\\SQLEXPRESS;initial catalog=appEstacionamiento; Integrated Security = True";
                con.Open();

                var sql = "select u.id_usuario,u.nombre_usuario,u.apellido_usuario,u.rut_usuario,u.contrasena_usuario,u.estado_usuario,u.correo_usuario,u.imagen_usuario from usuario u where u.correo_usuario = @email and u.contrasena_usuario = @pass";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.Add(new SqlParameter("@email", emailAddress));
                cmd.Parameters.Add(new SqlParameter("@pass", pass));
                var sqlReader = cmd.ExecuteReader();
                while (sqlReader.Read())
                {
                    id = int.Parse(sqlReader.GetValue(0).ToString());
                    nombre = sqlReader.GetValue(1).ToString();
                    apellido = sqlReader.GetValue(2).ToString();
                    rut = sqlReader.GetValue(3).ToString();
                    contrasena = sqlReader.GetValue(4).ToString();
                    estado = int.Parse(sqlReader.GetValue(5).ToString());
                    correo = sqlReader.GetValue(6).ToString();
                    imagen = sqlReader.GetValue(7).ToString();
                }
            }
            return new User
            {
                id_usuario = id,
                nombre_usuario = nombre,
                apellido_usuario = apellido,
                rut_usuario = rut,
                contrasena_usuario = contrasena,
                estado_usuario = estado,
                correo_usuario = correo,
                imagen_usuario = imagen
            };
        }

    }
}
