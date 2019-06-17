using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estacionamiento.ServiceUsuario;
using Estacionamiento.ServiceEstacionamiento;
using Estacionamiento.ServiceInfo;

namespace Estacionamiento.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult DashADM()
        {
            crudUsuarioSoapClient ws = new crudUsuarioSoapClient();

            var lista = ws.ListaUsuarios();

            return View(lista);
        }

    }
}