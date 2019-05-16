using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//
using CampIAFCJ.Models;
using System.Configuration;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.IO;


namespace CampIAFCJ.Controllers
{
    public class ValidarJovenController : Controller
    {
        // GET: ValidarJoven
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormularioJovenes vf)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            using (sqlconn)
            {
                try
                {
                    if(ModelState.IsValid)

                    if (vf.Camiseta == "N")
                    {
                        vf.Talla = "N/A";
                    }

                    string sqlquery = "Insert INTO [dbo].[Usuarios] (Nombre, Edad, Genero, EstadoCivil, NumIglesia,  Correo, Camiseta, Talla, id_campa) VALUES (@Nombre, @Edad, @Genero,  @EstadoCivil, @NumIglesia, @Correo, @Camiseta, @Talla,@id_campa )";
                    SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                    sqlconn.Open();
                    sqlcomm.Parameters.AddWithValue("@Nombre", vf.Nombre);
                    sqlcomm.Parameters.AddWithValue("@Edad", vf.Edad);
                    sqlcomm.Parameters.AddWithValue("@Genero", vf.Genero);
                    sqlcomm.Parameters.AddWithValue("@EstadoCivil", "Soltero");
                    sqlcomm.Parameters.AddWithValue("@NumIglesia", vf.NumeroIglesia);
                    sqlcomm.Parameters.AddWithValue("@Correo", vf.CorreoElectronico);
                    sqlcomm.Parameters.AddWithValue("@Camiseta", vf.Camiseta);
                    sqlcomm.Parameters.AddWithValue("@Talla", vf.Talla);
                    sqlcomm.Parameters.AddWithValue("@id_campa", 2);
                    sqlcomm.ExecuteNonQuery();
                    ViewData["Message"] = "Insercion correcta";
                    sqlconn.Close();
                    To_pdf(vf);
                    ModelState.Clear();
                }
                catch (SqlException)
                {
                    ModelState.AddModelError("", "Inserción incorrecta");
                    
                }
            }
            return View();
        }
        
        public void To_pdf(FormularioJovenes vf)
        {
            string gener = "", camisa = "", talla = "";
            switch (vf.Genero)
            {
                case "H":
                {
                    gener = "Hombre";
                }
                    break;
                case "M":
                {
                    gener = "Mujer";
                }
                    break;
            }

            switch (vf.Camiseta)
            {
                case "S":
                {
                    camisa = "Si";
                }
                    break;
                case "N":
                {
                    camisa = "No";
                    talla = "N/A";
                }
                    break;
            }

            if (vf.Talla != "N/A")
            {
                switch (vf.Talla)
                {
                    case "C":
                    {
                        talla = "Chica";
                    }
                        break;
                    case "M":
                    {
                        talla = "Mediana";
                    }
                        break;
                    case "G":
                    {
                        talla = "Grande";
                    }
                        break;
                }
            }
            
            MemoryStream memoryS = new MemoryStream();
            Document doc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            PdfWriter writer = PdfWriter.GetInstance(doc, memoryS);
            doc.Open();
            
            Chunk chunk = new Chunk("Registro", FontFactory.GetFont("ARIAL", 28, iTextSharp.text.Font.BOLD));
            doc.Add(new Paragraph(chunk));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("---------------------------------------------------------------------------------------------------------"));
            doc.Add(new Paragraph("Tijuana, Baja California " + DateTime.Now.ToString()));
            doc.Add(new Paragraph("---------------------------------------------------------------------------------------------------------"));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       " + vf.Nombre));
            doc.Add(new Paragraph("                       " + "IAFCJ No: " + vf.NumeroIglesia));
            doc.Add(new Paragraph("                       " + "Edad: " + vf.Edad));
            doc.Add(new Paragraph("                       " + "Genero: " + gener));
            doc.Add(new Paragraph("                       " + "Camisa: " + camisa));
            doc.Add(new Paragraph("                       " + "Talla: " + talla));
            doc.Add(new Paragraph("                       " + "Correo: " +vf.CorreoElectronico));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("Firma del pastor: ________________________________________"));
            doc.AddCreationDate();
            writer.CloseStream = false;
            doc.Add(new Paragraph("                       "));
            doc.Add(new Paragraph("                       "));
            doc.Close();
            memoryS.Position = 0;
            enviodecorreo(memoryS, vf);
        }
        
        public void enviodecorreo(MemoryStream memorystr, FormularioJovenes vf)
        {
            try
            {
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

                msg.To.Add(vf.CorreoElectronico);
                msg.Subject = "Confirmación de registro a Campamento ";
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Bcc.Add("prueba.correo.patrones@gmail.com");
                msg.Body = "Permiso";
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = true;


                msg.Attachments.Add(new System.Net.Mail.Attachment(memorystr, "Permiso.pdf"));
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