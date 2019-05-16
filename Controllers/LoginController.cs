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
//librerías para PDF
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.IO;

namespace CampIAFCJ.Controllers
{
    public class LoginController : Controller
    {
        private CampamentoContext cc = new CampamentoContext();

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
            string sqlquery = "select Nombre, Contrasena from [dbo].[Admin] " +
                "where Nombre = @Nombre and Contrasena = @Contrasena";
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlcomm.Parameters.AddWithValue("@Nombre", lc.Name);
            sqlcomm.Parameters.AddWithValue("@Contrasena", lc.UserPassword);
            SqlDataReader sdr = sqlcomm.ExecuteReader();
            if (sdr.Read())
            {
                Session["username"] = lc.Name.ToString();
                return RedirectToAction("administrador");
            }
            else
            {
                ViewData["Message"] = "Usuario y contraseña inválidos.";
            }
            sqlconn.Close();
            return View();
        }

        public ActionResult administrador()
        {
            List<Usuarios> usuarios = cc.Usuario.ToList();
            List<Campamentos> campamentos = cc.Campamento.ToList();
            var multipletable = from u in usuarios
                                join c in campamentos on u.id_campa equals c.ID_Campamento into table1
                                from c in table1.DefaultIfEmpty()
                                select new TablaUnida
                                {
                                    nombre = u, edad = u, genero = u, estadoCivil = u, numIglesia = u,
                                    correo = u, camiseta = u, talla = u, descripcion = c
                                };
            return View(multipletable);
        }
        
        public JsonResult GetSearchingDataa(string SearchBy)
        {
            List<Usuarios> usuarios = cc.Usuario.ToList();
            List<Campamentos> campamentos = cc.Campamento.ToList();
            switch(SearchBy)
            {
                case "adolescentes":
                    usuarios = cc.Usuario.Where(x => x.id_campa == 1).ToList();
                    break;
                case "jovenes":
                    usuarios = cc.Usuario.Where(x => x.id_campa == 2).ToList();
                    break;
                case "damas":
                    usuarios = cc.Usuario.Where(x => x.id_campa == 3).ToList();
                    break;
                case "senores":
                    usuarios = cc.Usuario.Where(x => x.id_campa == 4).ToList();
                    break;
                case "MostrarTodo":
                    break;
            }
            var multipletable = from u in usuarios
                                join c in campamentos on u.id_campa equals c.ID_Campamento into table1
                                from c in table1.DefaultIfEmpty()
                                select new TablaUnida
                                {
                                    nombre = u, edad = u, genero = u, estadoCivil = u, numIglesia = u,
                                    correo = u, camiseta = u, talla = u, descripcion = c
                                };
            return Json(multipletable, JsonRequestBehavior.AllowGet);
        }
        
        public void DescargarPDF(string descargar)
        {
            To_pdf(descargar);
        }

        public void To_pdf(string descargar)
        {
            List<Usuarios> usuarios = cc.Usuario.ToList();
            List<Campamentos> campamentos = cc.Campamento.ToList();
            int numerodelista = 1;
            switch (descargar)
            {
                case "MostrarTodo":
                    descargar = "Mostrar todo";
                    break;
                case "adolescentes":
                    usuarios = cc.Usuario.Where(x => x.id_campa == 1).ToList();
                    descargar = "Adolescentes";
                    break;
                case "jovenes":
                    usuarios = cc.Usuario.Where(x => x.id_campa == 2).ToList();
                    descargar = "Jóvenes";
                    break;
                case "damas":
                    usuarios = cc.Usuario.Where(x => x.id_campa == 3).ToList();
                    descargar = "Damas";
                    break;
                case "senores":
                    usuarios = cc.Usuario.Where(x => x.id_campa == 4).ToList();
                    descargar = "Señores";
                    break;
            }

            var multipletable = from u in usuarios
                                join c in campamentos on u.id_campa equals c.ID_Campamento into table1
                                from c in table1.DefaultIfEmpty()
                                select new TablaUnida

                                {
                                    nombre = u, edad = u, genero = u, estadoCivil = u, numIglesia = u,
                                    correo = u, camiseta = u, talla = u, descripcion = c
                                };

            MemoryStream memoryS = new MemoryStream();
            Document doc = new Document(PageSize.A4, 10, 10, 10, 10);
            //Cambiar la orientación del pdf(horizontal)
            doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            PdfWriter writer = PdfWriter.GetInstance(doc, memoryS);
            doc.Open();

            // setting image path
            string imagePath = Server.MapPath("/Images/CampamentosLogo.png"); ;
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
            image.Alignment = Element.ALIGN_CENTER;
            // set width and height
            image.ScaleToFit(100f, 100f);
            // adding image to document
            doc.Add(image);
            doc.Add(new Paragraph("                                                        Iglesia Apostólica de la Fe en Cristo Jesús", FontFactory.GetFont("ARIAL", 16, iTextSharp.text.Font.BOLD)));

            doc.Add(new Paragraph("                                                             "));
            doc.Add(new Paragraph("                                                                                                    Tijuana, Baja California " + DateTime.Now.ToString(), FontFactory.GetFont("ARIAL", 16, iTextSharp.text.Font.BOLD)));
            doc.Add(new Paragraph("                                                             "));

            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("Lista Campamento: " + descargar + "", FontFactory.GetFont("ARIAL", 25, iTextSharp.text.Font.BOLD)));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));


            //Declara una tabla con 10 columnas
            PdfPTable table = new PdfPTable(10);
            //Añade la primera fila, correspondiente a los títulos
            table.AddCell("Número");
            table.AddCell("Nombre");
            table.AddCell("Edad");
            table.AddCell("Género");
            table.AddCell("Estado Civil");
            table.AddCell("Iglesia");
            table.AddCell("Correo");
            table.AddCell("Camiseta");
            table.AddCell("Talla");
            table.AddCell("Campamento");

            //Añade las siguientes filas, correspondientes a los registros
            foreach (TablaUnida t in multipletable)
            {
                table.AddCell(""+numerodelista);
                table.AddCell(""+t.nombre.Nombre);
                table.AddCell(""+t.edad.Edad);
                table.AddCell(""+t.genero.Genero);
                table.AddCell("" + t.estadoCivil.EstadoCivil);
                table.AddCell("" + t.numIglesia.NumIglesia);
                table.AddCell("" + t.correo.Correo);
                table.AddCell("" + t.camiseta.Camiseta);
                table.AddCell("" + t.talla.Talla);
                table.AddCell("" + t.descripcion.Descripcion);
                
                numerodelista++;
            }

            //Incluye la tabla recién creada al pdf
            doc.Add(table);
            
            doc.AddCreationDate();
            writer.CloseStream = false;
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));
            doc.Close();
            memoryS.Position = 0;
            enviodecorreo(memoryS, descargar);
        }

        public void enviodecorreo(MemoryStream memorystr, string descargar)
        {
            try
            {
                switch (descargar)
                {
                    case "Mostrar todo":
                        descargar = "TodosLosCampamentos";
                        break;
                    case "Adolescentes":
                        descargar = "CampamentoAdolescentes";
                        break;
                    case "Jóvenes":
                        descargar = "CampamentoJovenes";
                        break;
                    case "Damas":
                        descargar = "CampamentoDamas";
                        break;
                    case "Señores":
                        descargar = "CampamentoSenores";
                        break;
                }

                //Obtener el correo del administrador
                string mainconn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlconn = new SqlConnection(mainconn);
                string sqlquery = "select Correo from [dbo].[Admin] where Nombre = 'admin'";
                sqlconn.Open();
                SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                string correoAdmin = Convert.ToString(sqlcomm.ExecuteScalar());
                sqlconn.Close();

                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.To.Add(correoAdmin);
                msg.Subject = "Lista de asistencia a Campamentos IAFCJ ";
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Bcc.Add("prueba.correo.patrones@gmail.com");
                msg.Body = "Lista";
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.Attachments.Add(new System.Net.Mail.Attachment(memorystr, "Lista"+descargar+".pdf"));
                msg.From = new System.Net.Mail.MailAddress("prueba.correo.patrones@gmail.com");
                System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
                cliente.Credentials = new System.Net.NetworkCredential("prueba.correo.patrones@gmail.com", "prueba12345");
                cliente.Port = 587;
                cliente.EnableSsl = true;
                cliente.Host = "smtp.gmail.com";
                cliente.Send(msg);
            }
            catch (Exception)
            {

            }
        }
    }
}