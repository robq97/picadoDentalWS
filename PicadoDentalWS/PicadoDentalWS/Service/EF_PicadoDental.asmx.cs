using PicadoDentalWS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using PicadoDentalWS.POCOModels;
using PicadoDentalWS.POCO_Models;

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
        public List<ClientePOCO> ClientList()
        {
            using (PD_Entities e = new PD_Entities())
            {
                List<ClientePOCO> clientes = e.Clientes
                    .Select(c => new ClientePOCO() {
                        ClienteID = c.ClienteID,
                        PersonaID = c.PersonaID,
                        Nombre = c.Persona.Nombre
                    })
                    .ToList();
                return clientes;
            }
        }

        /// <summary>
        /// Crea nueva persona
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="primerApellido"></param>
        /// <param name="segundoApellido"></param>
        /// <param name="telefono"></param>
        /// <param name="correo"></param>
        /// <param name="generoID"></param>
        /// <param name="cedula"></param>
        /// <param name="tipoCuentaID"></param>
        /// <param name="usuario"></param>
        /// <param name="contrasena"></param>
        [WebMethod]
        public void NewPerson(string nombre, string primerApellido, string segundoApellido, string telefono, string correo, int generoID, int cedula, int tipoCuentaID, string usuario, string contrasena)
        {
            using (PD_Entities e = new PD_Entities())
            {
                try
                {
                    int contactoID = 0;
                    int personaID = 0;

                    e.Contactoes.Add(new Contacto()
                    {
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
                        Cedula = cedula,
                        TipoCuentaID = tipoCuentaID
                    });
                    e.Clientes.Add(new Cliente()
                    {
                        PersonaID = personaID
                    });
                    if (!usuario.Equals("") && !contrasena.Equals(""))
                    {
                        e.Credencials.Add(new Credencial()
                        {
                            Usuario = usuario,
                            Password = contrasena
                        });
                    }
                    e.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// Modifica datos de cliente
        /// </summary>
        /// <param name = "personaId" ></ param >
        /// < param name="telefono"></param>
        /// <param name = "correo" ></ param >
        /// < param name="generoID"></param>
        [WebMethod]
        public void ModifyClient(int personaId, string telefono, string correo)
        {
            using (PD_Entities e = new PD_Entities())
            {
                try
                {
                    var user = (from u in e.Personas
                                join Contacto in e.Contactoes on u.ContactoID equals Contacto.ContactoID
                                where u.PersonaID == personaId
                                select u
                                ).SingleOrDefault();
                    if (correo != null)
                    {
                        user.Contacto.Correo = correo;
                    }
                    if (telefono != null)
                    {
                        user.Contacto.Telefono = telefono;
                    }
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
        /// devuelve lista de citas
        /// </summary>
        /// <returns>lista de citas</returns>

        [WebMethod]
        public List<CitaPOCO> CitaList()
        {
            using (PD_Entities e = new PD_Entities())
            {
                List<CitaPOCO> lista = (from c in e.Citas
                                        join s in e.Clientes
                                           on c.ClienteID equals s.ClienteID into asd1
                                        from s in asd1.DefaultIfEmpty()
                                        join p in e.Personas
                                            on s.PersonaID equals p.PersonaID into asd2
                                        from p in asd2.DefaultIfEmpty()
                                        join d in e.Personas
                                            on c.PersonaID equals d.PersonaID into asd3
                                        from d in asd3.DefaultIfEmpty()
                                        select new CitaPOCO
                                        {
                                            Fecha = c.FechaHora,
                                            ClienteNombre = p.Nombre,
                                            ClienteApellidos = p.PrimerApellido + " " + p.SegundoApellido,
                                            DoctorNombre = d.Nombre,
                                            DoctorApellidos = d.PrimerApellido + " " + d.SegundoApellido,
                                            Detalles = c.DescCita
                                        })
                             .OrderBy(x => x.Fecha).ToList();
                return lista;
            }
        }

        /// <summary>
        /// devuelve lista de citas por id
        /// </summary>
        /// <returns>lista de citas</returns>

        [WebMethod]
        public List<CitaPOCO> CitaListByID(int id)
        {
            try { 
            using (PD_Entities e = new PD_Entities())
            {
                List<CitaPOCO> lista = (from c in e.Citas
                                        join s in e.Clientes
                                           on c.ClienteID equals s.ClienteID into asd1
                                        from s in asd1.DefaultIfEmpty()
                                        join p in e.Personas
                                            on s.PersonaID equals p.PersonaID into asd2
                                        from p in asd2.DefaultIfEmpty()
                                        join d in e.Personas
                                            on c.PersonaID equals d.PersonaID into asd3
                                        from d in asd3.DefaultIfEmpty()
                                        select new CitaPOCO
                                        {
                                            Fecha = c.FechaHora,
                                            ClienteNombre = p.Nombre,
                                            ClienteApellidos = p.PrimerApellido + " " + p.SegundoApellido,
                                            DoctorNombre = d.Nombre,
                                            DoctorApellidos = d.PrimerApellido + " " + d.SegundoApellido,
                                            Detalles = c.DescCita,
                                            ClienteID = p.Cedula
                                        })
                             .OrderBy(x => x.Fecha).ToList();
                
                foreach (var i in lista)
                {
                    if(i.ClienteID == id)
                    {
                        List<CitaPOCO> encontrado = new List<CitaPOCO>();
                        encontrado.Add(i);
                        return encontrado;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Crea nueva cita
        /// </summary>
        /// <param name = "clienteId" ></ param >
        /// < param name="doctorId"></param>
        /// <param name = "fechaHora" ></ param >
        /// < param name="descripcion"></param>
        /// <param name = "comentarios" ></ param >
        [WebMethod]
        public void NewAppointment(int clienteId, int doctorId, string fechaHora, string descripcion, string comentarios)
        {
            using (PD_Entities e = new PD_Entities())
            {
                try
                {
                    e.Citas.Add(new Cita()
                    {
                        ClienteID = clienteId,
                        PersonaID = doctorId,
                        FechaHora = DateTime.Parse(fechaHora),
                        DescCita = descripcion,
                        Comentarios = comentarios
                    });
                    e.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Modifica las citas
        /// </summary>
        /// <param name = "citaId" ></ param >
        /// < param name="doctorId"></param>
        /// <param name = "fechaHora" ></ param >
        /// < param name="descripcion"></param>
        /// <param name = "comentarios" ></ param >
        [WebMethod]
        public void ModifyAppointment(int citaId, string doctorId, string fechaHora, string descripcion, string comentarios)
        {
            using (PD_Entities e = new PD_Entities())
            {
                try
                {
                    int personIdInt = Convert.ToInt32(doctorId);

                    var cita = e.Citas.Where(s => s.CitaID == citaId).First();
                    if (doctorId != null)
                    {
                        cita.PersonaID = personIdInt;
                    }
                    if (fechaHora != null)
                    {
                        cita.FechaHora = DateTime.Parse(fechaHora);
                    }
                    if (descripcion != null)
                    {
                        cita.DescCita = descripcion;
                    }
                    if (comentarios!= null)
                    {
                        cita.Comentarios = comentarios;
                    }
          
                    e.SaveChanges();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
