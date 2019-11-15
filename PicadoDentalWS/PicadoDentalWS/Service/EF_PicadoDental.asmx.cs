using PicadoDentalWS.Models;
using PicadoDentalWS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace PicadoDentalWS.Service
{
    /// <summary>
    /// Summary description for EF_PicadoDental
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class EF_PicadoDental : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] LogIn(string usuario, string contrasena)
        {
            string[] info = new string[2];

            try
            {
                using (PD_Entities e = new PD_Entities())
                {
                    var user = (from u in e.Personas
                                join Credencial in e.Credencials on u.CredencialID equals Credencial.CredencialID
                                where u.Credencial.Usuario == usuario && u.Credencial.Password == contrasena
                                select new
                                {
                                    u.TipoCuentaID,
                                    u.Credencial.Usuario
                                }).ToList();

                    if (user.FirstOrDefault() != null)
                    {
                        info[0] = user.FirstOrDefault().Usuario.ToString();
                        info[1] = user.FirstOrDefault().TipoCuentaID.ToString();
                        return info;
                    }
                }
            }
            catch (Exception)
            {
                info[2] = "error";
                return info;
            }
            return info;
        }


        /// <summary>
        /// devuelve lista de clientes
        /// </summary>
        /// <returns>lista de clientes</returns>
        [WebMethod]
        public List<Cliente> ClientList()
        {
            using (PD_Entities e = new PD_Entities())
            {
                var userList = from Cliente in e.Clientes
                               select Cliente;
                if (userList.Count() != 0)
                {
                    return userList.ToList();
                }
            }
            return null;
        }


        /// <summary>
        /// Agrega nuevo cliente
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="primerApellido"></param>
        /// <param name="segundoApellido"></param>
        /// <param name="telefono"></param>
        /// <param name="correo"></param>
        /// <param name="generoID"></param>
        [WebMethod]
        public void NewClient(string nombre, string primerApellido, string segundoApellido, string telefono, string correo, int generoID)
        {
            using (PD_Entities e = new PD_Entities())
            {
                try
                {
                    int contactoID = 0;
                    int personaID = 0;

                    e.Contactoes.Add(new Contacto(){
                        ContactoID = contactoID,
                        Telefono = telefono,
                        Correo = correo
                    });
                    e.Personas.Add(new Persona()
                    {
                        PersonaID = personaID,
                        Nombre = nombre,
                        PrimerApellido = primerApellido,
                        SegundoApellido = segundoApellido,
                        GeneroID = generoID,
                        ContactoID = contactoID,
                        TipoCuentaID = 4
                    });
                    e.Clientes.Add(new Cliente()
                    {
                        PersonaID = personaID
                    });
                    e.SaveChanges();
                }
                catch (Exception)
                {
                   
                }
            }
        }

        //[WebMethod]
        //public void DeleteClient(short personaID)
        //{
        //    using (PD_Entities e = new PD_Entities())
        //    {
        //        try
        //        {
        //            var client = new Persona { PersonaID = personaID };
        //            e.Personas.Attach(client);
        //            e.Personas.Remove(client);

        //            var person = new Cliente { PersonaID = personaID };
        //            e.Clientes.Attach(person);
        //            e.Clientes.Remove(person);

        //            var 

        //            e.SaveChanges();
        //        }
        //        catch (Exception)
        //        {
        //            return;
        //        }
        //    }
        //}


        /// <summary>
        /// Modifica datos de cliente
        /// </summary>
        /// <param name = "personaId" ></ param >
        /// < param name="telefono"></param>
        /// <param name = "correo" ></ param >
        /// < param name="generoID"></param>
        [WebMethod]
        public void ModifyClient(short personaId, string telefono, string correo, byte generoID)
        {
            using (PDEntities e = new PDEntities())
            {
                try
                {
                    var persona = e.Personas.Where(s => s.PersonaId == personaId).First();

                    persona.Telefono = telefono;
                    persona.Correo = correo;
                    e.SaveChanges();
                }
                catch (Exception)
                {
                    return;
                }

            }
        }







        //Citas








        /// <summary>
        /// Crea nueva cita
        /// </summary>
        /// <param name = "clienteId" ></ param >
        /// < param name="doctorId"></param>
        /// <param name = "fechaHora" ></ param >
        /// < param name="descripcion"></param>
        /// <param name = "comentarios" ></ param >
        //[WebMethod]
        //public void NewAppointment(short clienteId, short doctorId, DateTime fechaHora, string descripcion, string comentarios)
        //{
        //    using (PDEntities e = new PDEntities())
        //    {
        //        try
        //        {
        //            e.Citas.Add(new Cita()
        //            {
        //                ClienteId = clienteId,
        //                DoctorId = doctorId,
        //                FechaHora = DateTime.Now,
        //                Descripcion = descripcion,
        //                Comentarios = comentarios
        //            });
        //            e.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex);
        //        }
        //    }
        //}

        /// <summary>
        /// Modifica las citas
        /// </summary>
        /// <param name = "citaId" ></ param >
        /// < param name="doctorId"></param>
        /// <param name = "fechaHora" ></ param >
        /// < param name="descripcion"></param>
        /// <param name = "comentarios" ></ param >
        //[WebMethod]
        //public void ModifyAppointment(short citaId, short doctorId, DateTime fechaHora, string descripcion, string comentarios)
        //{
        //    using (PDEntities e = new PDEntities())
        //    {
        //        try
        //        {



        //            var cita = e.Citas.Where(s => s.CitaId == citaId).First();
        //            cita.DoctorId = doctorId;
        //            cita.FechaHora = fechaHora;
        //            cita.Descripcion = descripcion;
        //            cita.Comentarios = comentarios;
        //            e.SaveChanges();
        //        }
        //        catch (Exception)
        //        {
        //            return;
        //        }

        //    }
        //}
    }
}
