using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Estacionamiento.ServiceUsuario;
using Estacionamiento.ServiceEstacionamiento;
using Estacionamiento.ServiceInfo;

namespace Estacionamiento.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Map()
        {
            return View();
        }

        public ActionResult Perfil()
        {
            return View();
        }

        public ActionResult Estacionamiento()
        {
            return View();
        }

        public ActionResult RegistraEstacionamiento()
        {
            return View();
        }

        //Valida el inicio de sesión
        [HttpPost]
        public ActionResult Ingresar()
        {
            string correo = Request["txtCorreo"];
            string pass = Request["txtPass"];

            crudUsuarioSoapClient ws = new crudUsuarioSoapClient();
            bool stmt = ws.IniciarSesion(correo,pass);

            if (stmt==true)
            {
                var datos = ws.GetUser(correo,pass);
                int tipo=datos.tipo_usuario;

                Session["id_usuario"]=datos.id_usuario;
                Session["Nombre"] = datos.nombre_usuario + " " + datos.apellido_usuario;

                if (tipo==3)
                {
                    return View("Map");

                }else if(tipo==2){

                    return View("Estacionamiento");
                }
                else
                {
                    return RedirectToAction("DashADM", "Admin");
                }
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        //Registra a un usuario nuevo
        [HttpPost]
        public ActionResult RegistraUsuario()
        {
            string nombre = Request["txtNombre"];
            string apellido = Request["txtApellido"];
            string rut = Request["txtRut"];
            string pass = Request["txtPass"];
            string correo = Request["txtCorreo"];

            crudUsuarioSoapClient ws = new crudUsuarioSoapClient();

            bool existe = ws.IniciarSesion(correo,pass);

            if (existe==true)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ws.CreaUsuario(nombre,apellido,rut,pass,1,correo);
                
                Session["pass"] = pass;
                Session["correo"] = correo;

                return RedirectToAction("RegistroTipo", "Home");

            }
        }

        //Registra en la tabla usuario_tipo.
        [HttpPost]
        public ActionResult RegistraTipoUsuario()
        {
            string pass = Session["pass"].ToString();
            string correo = Session["correo"].ToString();
            int tipo = int.Parse(Request["slcTipo"].ToString());

            crudUsuarioSoapClient ws = new crudUsuarioSoapClient();

            var datos = ws.GetUserWithoutTipo(correo,pass);

            ws.InsertaUsuario_Tipo(datos.id_usuario,tipo);

            Session.Remove("pass");
            Session.Remove("correo");

            return RedirectToAction("Index", "Home");

         }

        //Ingresa un estacionamiento a la base de datos.
        [HttpPost]
        public ActionResult RegistraDatosEstacionamiento()
        {
            string comuna = Request["txtComuna"];
            string direccion = Request["txtDireccion"];
            string comentario = Request["txtComentario"];
            int valor = int.Parse(Request["txtValor"]);
            string vehiculo = Request["txtAuto"];
            int id = int.Parse(Session["id_usuario"].ToString());

            crudEstacionamientoSoapClient ws = new crudEstacionamientoSoapClient();
            crudUsuarioSoapClient wsU = new crudUsuarioSoapClient();

            ws.InsertEstacionamiento(comuna,direccion,comentario,id,valor,vehiculo);

            return RedirectToAction("Estacionamiento", "Usuario");
         
        }



        /*
         Inicia y cierra SESSION
         --Session["id"]= id;
         --Session.Remove("id");
         */


    }
}