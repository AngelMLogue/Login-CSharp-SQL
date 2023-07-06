using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using URP.Filters;
using URP.Models;

namespace URP.Controllers
{
    [VerificarSession]
    public class HomeController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        //Operacion 1
        [AuthorizeUser(OID: 1)]
        public ActionResult Index()
        {
            using (URPDBEntities Model = new URPDBEntities())
            {
                string correo = TempData["UEmail"] as string;
                TempData.Keep("UEmail");

                var User = Model.Users.FirstOrDefault(u => u.UEmail == correo);
                if (User != null)
                {
                    var Grade = Model.Grades.FirstOrDefault(g => g.GID == User.GID);
                    if (Grade != null)
                    {
                        var Subject = Model.Subjects.Where(s => s.GID == User.GID).ToList();

                        ViewBag.Materia = Subject;
                        ViewBag.Grado = Grade.GName;

                        return View();
                    }
                }

                return View("Index");
            }
        }



        //Operacion 2
        [AuthorizeUser(OID: 2)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //Operacion 3
        [AuthorizeUser(OID: 3)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult LogOn()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            Session["User"] = null;
            return RedirectToAction("LogIn", "Home");
        }

        [HttpPost]
        public ActionResult LogOn(Users oUser)
        {
            bool statusLogOn;
            string message;

            if (oUser.UPassword == oUser.UConfirmPassword)
            {
                oUser.UPassword = SHA256Converter.ToSHA256(oUser.UPassword);
                oUser.UConfirmPassword = SHA256Converter.ToSHA256(oUser.UConfirmPassword);
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_LogOn", cn);
                cmd.Parameters.AddWithValue("UName", oUser.UName);
                cmd.Parameters.AddWithValue("URealName", oUser.URealName);
                cmd.Parameters.AddWithValue("ULastName", oUser.ULastName);
                cmd.Parameters.AddWithValue("UEmail", oUser.UEmail);
                cmd.Parameters.AddWithValue("UPassword", oUser.UPassword);
                cmd.Parameters.AddWithValue("UConfirmPassword", oUser.UConfirmPassword);
                cmd.Parameters.Add("StatusLogOn", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Message", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                statusLogOn = Convert.ToBoolean(cmd.Parameters["StatusLogOn"].Value);
                message = cmd.Parameters["Message"].Value.ToString();

            }

            ViewData["Mensaje"] = message;

            if (statusLogOn)
            {
                return RedirectToAction("LogIn", "Home");
            }
            else
            {
                return View();
            }

            return View();
        }

        [HttpPost]
        public ActionResult LogIn(Users oUser)
        {
            oUser.UPassword = SHA256Converter.ToSHA256(oUser.UPassword);

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_LogIn", cn);
                    cmd.Parameters.AddWithValue("UNameEmail", oUser.UEmail);
                    cmd.Parameters.AddWithValue("UPassword", oUser.UPassword);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        oUser.UID = Convert.ToInt32(reader["UID"]);
                        oUser.RID = Convert.ToInt32(reader["RID"]);
                    }
                    reader.Close();

                }

                if (oUser.UID != 0)
                {
                    Session["User"] = oUser;
                    TempData["UEmail"] = oUser.UEmail;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                ViewData["Mensaje"] = "Email o Contraseña incorrectos";
                return View();
            }
            return View();
            

        }

        public static class SHA256Converter
        {
            public static string ToSHA256(string text)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(text);
                    byte[] hash = sha256.ComputeHash(bytes);
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++)
                    {
                        stringBuilder.Append(hash[i].ToString("x2"));
                    }
                    return stringBuilder.ToString();
                }
            }
        }
    }
}