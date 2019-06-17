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
        public ActionResult Map(int id)
        {
            crudEstacionamientoSoapClient wsE = new crudEstacionamientoSoapClient();
            var lista = wsE.ListaEstacionamientoLibre(id);
            return View(lista);
        }

        public ActionResult DashUser()
        {
            return View();
        }

        public ActionResult Perfil()
        {
            return View();
        }

        public ActionResult Estacionamiento(int id)
        {

                 crudEstacionamientoSoapClient wsE = new crudEstacionamientoSoapClient();
                 var lista = wsE.ListaEstacionamiendoById(id);
                 return View("Estacionamiento", lista);
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

                var estado = datos.estado_usuario;

                Session["id_usuario"]=datos.id_usuario;
                Session["Nombre"] = datos.nombre_usuario + " " + datos.apellido_usuario;
                Session["Mensaje"] = string.Empty;

                if (estado==1)
                {
                    if (tipo == 3 || tipo==2)
                    {

                        var arrendador= ws.CompruebaArrendador_Duenno(int.Parse(Session["id_usuario"].ToString()),3);
                        var duenno = ws.CompruebaArrendador_Duenno(int.Parse(Session["id_usuario"].ToString()),2);

                        if (arrendador ==true)
                        {
                            Session["Arrendador"]=1;
                        }
                        if (duenno == true)
                        {
                            Session["Duenno"] = 1;
                        }

                        return View("DashUser");

                    }
                    else
                    {
                        return RedirectToAction("DashADM", "Admin");
                    }
                }
                else
                {
                    Session["Mensaje"] = "Cuenta inhabilitada.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                Session["Mensaje"] = "Datos incorrectos.";
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

            bool existe = ws.ValidaExistencia(correo);
            var cuentaArre = ws.CompruebaArrendador_DuennoWithCorreo(correo,3);
            var cuentaDue = ws.CompruebaArrendador_DuennoWithCorreo(correo, 2);

            if (existe==true)
            {
                if (cuentaArre == true && cuentaDue==true)
                {
                    Session["Mensaje"] = "Ya posee una cuenta de éste tipo";
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    Session["pass"] = pass;
                    Session["correo"] = correo;
                    return RedirectToAction("RegistroTipo", "Home");
                }

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

            var datos = ws.GetUserWithoutTipo(correo);

            var existeArren = ws.CompruebaArrendador_Duenno(datos.id_usuario,3);
            var existeDue = ws.CompruebaArrendador_Duenno(datos.id_usuario,2);

            if (existeArren==false)
            {
                ws.InsertaUsuario_Tipo(datos.id_usuario, tipo);

                Session.Remove("pass");
                Session.Remove("correo");

                Session["Mensaje"] = "Registro exitoso!!";
                return RedirectToAction("Index", "Home");
            }else if (existeDue == false)
            {
                ws.InsertaUsuario_Tipo(datos.id_usuario, tipo);

                Session.Remove("pass");
                Session.Remove("correo");

                Session["Mensaje"] = "Registro exitoso!!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session.Remove("pass");
                Session.Remove("correo");

                Session["Mensaje"] = "Ya posee los 2 tipos de cuenta.";
                return RedirectToAction("Index", "Home");
            }



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

            var lista = ws.ListaEstacionamiendoById(int.Parse(Session["id_usuario"].ToString()));

            return View("Estacionamiento",lista);
         
        }

<<<<<<< HEAD
        [HttpPost]
        public ActionResult OcupaEstacionamiento()
        {
            int idEsta = int.Parse(Request["txtIdEsta"]);
            int usuario = int.Parse(Session["id_usuario"].ToString());

            crudEstacionamientoSoapClient wsE = new crudEstacionamientoSoapClient();
=======
                //Método para cerrar la sesión de un usuario.
        [HttpPost]
        public ActionResult logout()
        {
            if (Session["id_usuario"] != null)
            {
                Session.Remove("id_usuario");
                Session.Remove("Nombre");
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

>>>>>>> master

            wsE.ActualizaEstacionamiento(usuario,idEsta);

            Session["MensajeEST"] = "Estacionamiento registrado correctamente! Recuerda terminar el uso de éste o tu cuenta se irá a las nubes!";
            return View("DashUser");

        }

        //Método para cerrar la sesión de un usuario. 
        [HttpPost]
        public ActionResult logout()
        {
            if (Session["id_usuario"] != null)
            {
                Session.Remove("id_usuario");
                Session.Remove("Nombre");
                Session.Remove("Arrendador");
                Session.Remove("Duenno");
                Session.RemoveAll();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /*
         Inicia y cierra SESSION
         --Session["id"]= id;
         --Session.Remove("id");
         */


    }
}