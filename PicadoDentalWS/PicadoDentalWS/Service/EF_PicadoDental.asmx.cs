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


        //[WebMethod]
        //public List<Cliente> ClientList()
        //{
        //    ClienteViewModel clienteList = new ClienteViewModel();
        //    return db.Clientes.ToList();
        //}


        [WebMethod]
        public List<Cliente> Read()
        {
            using (PDEntities e = new PDEntities())
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

        [WebMethod]
        public void NewClient(short personaID, string nombre, string primerApellido, string segundoApellido, string telefono, string correo, byte generoID)
        {
            using (PDEntities e = new PDEntities())
            {
                try
                {
                    e.Personas.Add(new Persona()
                    {
                        PersonaId = personaID,
                        Nombre = nombre,
                        PrimerApellido = primerApellido,
                        SegundoApellido = segundoApellido,
                        Telefono = telefono,
                        Correo = correo,
                        GeneroId = generoID
                    });
                    e.SaveChanges();
                }
                catch (Exception)
                {
                    return;
                }
                
            }
        }

        [WebMethod]
        public void DeleteClient(short personaID)
        {
            using (PDEntities e = new PDEntities())
            {
                try
                {
                    var client = new Persona { PersonaId = personaID };
                    e.Personas.Attach(client);
                    e.Personas.Remove(client);
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
