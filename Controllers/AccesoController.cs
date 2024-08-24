using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.Web.UI.WebControls;

using AplicacionWebNomina.Models;

namespace AplicacionWebNomina.Controllers
{
    public class AccesoController : Controller
    {
        //Entidad para conectarme a la BDD y a los objetos de la misma
        
        //GET: Acceso
        public ActionResult Autenticar() 
        {
            return View();
        }

        public ActionResult Empleado()
        {
            return View();
        }

        //POST

        [HttpPost]

        public ActionResult Autenticar(Empleado oEmpleado)
        {

            string mensaje = "";
            try
            {
                using (SqlConnection cn=new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn"].ConnectionString))
                { 
                    SqlCommand cmd = new SqlCommand ("sp_ValidarUsuarios", cn);
                    cmd.Parameters.AddWithValue("user", oEmpleado.correo);
                    cmd.Parameters.AddWithValue("pass", oEmpleado.clave);
                    cmd.Parameters.Add("id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    oEmpleado.id = Convert.ToInt32(cmd.Parameters["id"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();

                    cn.Close();
                }

                if (oEmpleado.id == 0)
                {
                    return RedirectToAction("Error","Home");
                }
                if (oEmpleado.id == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }


            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }
    }
}