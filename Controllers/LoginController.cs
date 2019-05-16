using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.Web.Mvc;
using CampIAFCJ.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace CampIAFCJ.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            Session["username"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login lc) 
        {
            string mainconn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = "select Name, UserPassword from [dbo].[UserReg] " +
                "where Name = @Name and UserPassword = @UserPassword";
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlcomm.Parameters.AddWithValue("@Name", lc.Name);
            sqlcomm.Parameters.AddWithValue("@UserPassword", lc.UserPassword);
            SqlDataReader sdr = sqlcomm.ExecuteReader();
            if (sdr.Read())
            {
                Session["username"] = lc.Name.ToString();
                return RedirectToAction("welcome");
            }
            else
            {
                ViewData["Message"] = "Usuario y contraseña inválidos.";
            }
            sqlconn.Close();
            return View();
        }

        public ActionResult welcome()
        {
            return View();
        }
    }
}