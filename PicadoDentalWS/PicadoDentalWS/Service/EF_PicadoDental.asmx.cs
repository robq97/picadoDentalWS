﻿using PicadoDentalWS.Models;
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
        /// <summary>
        /// Here we realice the validation of the login 
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="contrasena"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] LogIn(string usuario, string contrasena)
        {
            string[] info = new string[3];

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
                                    u.Credencial.Usuario,
                                    u.PersonaID
                                }).ToList();
                    if (user.FirstOrDefault() != null)
                    {
                        info[0] = user.FirstOrDefault().PersonaID.ToString();
                        info[1] = user.FirstOrDefault().TipoCuentaID.ToString();
                        return info;
                    }
                    else
                    {
                        info[2] = "error";
                        return info;
                    }
                }
            }
            catch (Exception)
            {
                info[2] = "reload";
                return info;
            }
        }

        /// <summary>
        /// This method returns the client list 
        /// </summary>
        /// <returns></returns>

        [WebMethod]
        public List<ClientePOCO> ClientList()
        {
            using (PD_Entities e = new PD_Entities())
            {
                List<ClientePOCO> clientes = e.Clientes
                    .Select(c => new ClientePOCO() {
                        ClienteID = c.ClienteID,
                        PersonaID = c.PersonaID,
                        Nombre = c.Persona.Nombre,
                        Apellidos = c.Persona.PrimerApellido + " " + c.Persona.SegundoApellido,
                        Cedula = c.Persona.Cedula,
                        Genero = c.Persona.Genero.DescGenero,
                        Correo = c.Persona.Contacto.Correo,
                        Telefono = c.Persona.Contacto.Telefono
                    })
                    .ToList();
                return clientes;
            }
        }
        /// <summary>
        /// Returns the client lists for appoinment dropdown
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<CitaPOCO> ListaClientes()
        {
            try
            {
                using (PD_Entities e = new PD_Entities())
                {
                    List<CitaPOCO> lista = (from c in e.Clientes
                                            join p in e.Personas
                                               on c.PersonaID equals p.PersonaID into InfoClientePersona
                                            from p in InfoClientePersona.DefaultIfEmpty()
                                            select new CitaPOCO
                                            {
                                                ClienteID = c.ClienteID,
                                                ClienteNombre = p.Nombre + " " + p.PrimerApellido + " " + p.SegundoApellido
                                            })
                             .ToList();
                    return lista;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
        /// <summary>
        ///Returns the client lists for Gender dropdown
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<ClientePOCO> ListaGenero()
        {
            try
            {
                using (PD_Entities e = new PD_Entities())
                {
                    List<ClientePOCO> lista = (from c in e.Generoes
                                            select new ClientePOCO
                                            {
                                                GeneroID = c.GeneroID,
                                                Genero = c.DescGenero
                                            })
                             .ToList();
                    return lista;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        /// <summary>
        /// Returns the client lists for appointment dropdown
        /// </summary>
        /// <returns>Info de un cliente en específico</returns>
        [WebMethod]
        public List<ClientePOCO> ObtenerInfoCliente(int id)
        {
            try
            {
                using (PD_Entities e = new PD_Entities())
                {
                    List<ClientePOCO> clientes = e.Clientes
                    .Select(c => new ClientePOCO()
                    {
                        ClienteID = c.ClienteID,
                        PersonaID = c.PersonaID,
                        Nombre = c.Persona.Nombre,
                        Apellidos = c.Persona.PrimerApellido + " " + c.Persona.SegundoApellido,
                        Cedula = c.Persona.Cedula,
                        Genero = c.Persona.Genero.DescGenero,
                        Correo = c.Persona.Contacto.Correo,
                        Telefono = c.Persona.Contacto.Telefono
                    })
                    .ToList();
                    foreach (var i in clientes)
                    {
                        if (i.PersonaID == id)
                        {
                            List<ClientePOCO> encontrado = new List<ClientePOCO>();
                            encontrado.Add(i);
                            return encontrado;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        /// <summary>
        /// devuelve la información de un doctor en específico
        /// </summary>
        /// <returns>Info de un doctor en específico</returns>
        [WebMethod]
        public List<DoctorPOCO> ObtenerInfoDoctor(int id)
        {
            try
            {
                using (PD_Entities e = new PD_Entities())
                {
                    List<DoctorPOCO> doctor = e.Personas
                    .Select(c => new DoctorPOCO()
                    {
                        PersonaID = c.PersonaID,
                        Nombre = c.Nombre,
                        Apellidos = c.PrimerApellido + " " + c.SegundoApellido,
                        Cedula = c.Cedula,
                        Genero = c.Genero.DescGenero,
                        Correo = c.Contacto.Correo,
                        Telefono = c.Contacto.Telefono
                    })
                    .ToList();
                    foreach (var i in doctor)
                    {
                        if (i.PersonaID == id)
                        {
                            List<DoctorPOCO> encontrado = new List<DoctorPOCO>();
                            encontrado.Add(i);
                            return encontrado;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        /// <summary>
        /// Method to create a new person.
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
                    int credencialID = 0;

                    e.Contactoes.Add(new Contacto()
                    {
                        ContactoID = contactoID,
                        Telefono = telefono,
                        Correo = correo
                    });
                  
                    e.Credencials.Add(new Credencial()
                    {
                        CredencialID = credencialID,
                        Usuario = usuario,
                        Password = contrasena
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
                        TipoCuentaID = tipoCuentaID,
                        CredencialID = credencialID

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

        /// <summary>
        /// Method to modify the dates of a  client 
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

       


        /// <summary>
        /// returns the appointment list
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<CitaPOCO> CitaList()
        {
            using (PD_Entities e = new PD_Entities())
            {
                List<CitaPOCO> lista = (from c in e.Citas
                                        join s in e.Clientes
                                           on c.ClienteID equals s.ClienteID into InfoCliente
                                        from s in InfoCliente.DefaultIfEmpty()
                                        join p in e.Personas
                                            on c.PersonaID equals p.PersonaID into InfoClientePersona
                                        from p in InfoClientePersona.DefaultIfEmpty()
                                        join g in e.Contactoes
                                            on p.ContactoID equals g.ContactoID into InfoClientePersonaContacto
                                        from g in InfoClientePersonaContacto.DefaultIfEmpty()
                                        join d in e.Personas
                                            on c.PersonaID equals d.PersonaID into InfoDoctor
                                        from d in InfoDoctor.DefaultIfEmpty()
                                        join y in e.Contactoes
                                            on d.ContactoID equals y.ContactoID into InfoDoctorContacto
                                        from y in InfoDoctorContacto.DefaultIfEmpty()
                                        select new CitaPOCO
                                        {
                                            Fecha = c.FechaHora,
                                            ClienteNombre = c.Cliente.Persona.Nombre,
                                            ClienteApellidos = c.Cliente.Persona.PrimerApellido + " " + c.Cliente.Persona.SegundoApellido,
                                            DoctorID = d.PersonaID,
                                            DoctorNombre = d.Nombre,
                                            DoctorApellidos = d.PrimerApellido + " " + d.SegundoApellido,
                                            Detalles = c.DescCita,
                                            ClienteID = c.ClienteID,
                                            DoctorTelefono = y.Telefono,
                                            DoctorCorreo = y.Correo,
                                            ClienteTelefono = g.Telefono,
                                            ClienteCorreo = g.Correo,
                                            Comentarios = c.Comentarios,
                                            CitaID = c.CitaID
                                        })
                             .OrderBy(x => x.Fecha).ToList();
                return lista;
            }
        }
        /// <summary>
        /// returns the appoinment list
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<DoctorPOCO> DoctorList()
        {
            using (PD_Entities e = new PD_Entities())
            {
                List<DoctorPOCO> lista = (
                                        from d in e.Personas where d.TipoCuenta.DescTipoCuenta.Equals("Doctor")
                                        join y in e.Contactoes
                                            on d.ContactoID equals y.ContactoID into InfoDoctorContacto
                                        from y in InfoDoctorContacto.DefaultIfEmpty()
                                        select new DoctorPOCO
                                        {
                                            PersonaID = d.PersonaID,
                                            Nombre = d.Nombre,
                                            Apellidos = d.PrimerApellido + " " + d.SegundoApellido,
                                            Telefono = y.Telefono,
                                            Correo = y.Correo,
                                            Cedula = d.Cedula

                                        })
                             .ToList();
                return lista;
            }
        }
        /// <summary>
        /// Modifica datos de un doctor
        /// </summary>
        /// <param name = "personaId" ></ param >
        /// < param name="telefono"></param>
        /// <param name = "correo" ></ param >
        /// < param name="generoID"></param>
        [WebMethod]
        public void ModifyDoctor(int personaId, string telefono, string correo)
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

        /// <summary>
        /// returns the appoinment list by the id 
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<CitaPOCO> CitaListByID(int id)
        {
            try
            {
                using (PD_Entities e = new PD_Entities())
                {
                    List<CitaPOCO> lista = (from c in e.Citas
                                            join s in e.Clientes
                                               on c.ClienteID equals s.ClienteID into InfoCliente
                                            from s in InfoCliente.DefaultIfEmpty()
                                            join p in e.Personas
                                                on s.PersonaID equals p.PersonaID into InfoClientePersona
                                            from p in InfoClientePersona.DefaultIfEmpty()
                                            join g in e.Contactoes
                                                on p.ContactoID equals g.ContactoID into InfoClientePersonaContacto
                                            from g in InfoClientePersonaContacto.DefaultIfEmpty()
                                            join d in e.Personas
                                                on c.PersonaID equals d.PersonaID into InfoDoctor
                                            from d in InfoDoctor.DefaultIfEmpty()
                                            join y in e.Contactoes
                                                on d.ContactoID equals y.ContactoID into InfoDoctorContacto
                                            from y in InfoDoctorContacto.DefaultIfEmpty()
                                            select new CitaPOCO
                                            {
                                                Fecha = c.FechaHora,
                                                ClienteNombre = p.Nombre,
                                                ClienteApellidos = p.PrimerApellido + " " + p.SegundoApellido,
                                                DoctorNombre = d.Nombre,
                                                DoctorApellidos = d.PrimerApellido + " " + d.SegundoApellido,
                                                Detalles = c.DescCita,
                                                ClienteID = c.ClienteID,
                                                DoctorTelefono = y.Telefono,
                                                DoctorCorreo = y.Correo,
                                                ClienteTelefono = g.Telefono,
                                                ClienteCorreo = g.Correo,
                                                Comentarios = c.Comentarios,
                                                CitaID = c.CitaID
                                            })
                             .OrderBy(x => x.Fecha).ToList();

                    foreach (var i in lista)
                    {
                        if (i.CitaID == id)
                        {
                            List<CitaPOCO> encontrado = new List<CitaPOCO>();
                            encontrado.Add(i);
                            return encontrado;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        /// <summary>
        /// retunrs the doctor list to the appointment dropdown
        /// </summary>
        /// <returns></returns>

        [WebMethod]
        public List<CitaPOCO> ListaDoctores()
        {
            try
            {
                using (PD_Entities e = new PD_Entities())
                {
                    List<CitaPOCO> lista = (from c in e.Personas
                                                      where c.TipoCuenta.DescTipoCuenta.Equals("Doctor")

                                                      select new CitaPOCO
                                                      {
                                                           DoctorID = c.PersonaID,
                                                           DoctorNombre = c.Nombre + " " + c.PrimerApellido + " " + c.SegundoApellido
                                                      })
                             .ToList();
                    return lista;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }


        

        /// <summary>
        /// Method to create a new appointment
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
        /// Method to modify the appointments
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
                    DateTime fechaHoraConvertida = DateTime.Parse(fechaHora);
                    int personIdInt = Convert.ToInt32(doctorId);

                    var cita = e.Citas.Where(s => s.CitaID == citaId).First();


                    if (doctorId != "0")
                    {
                        cita.PersonaID = personIdInt;
                    }
                    if (fechaHoraConvertida.Year!= 1)
                    {
                        cita.FechaHora = fechaHoraConvertida;
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
