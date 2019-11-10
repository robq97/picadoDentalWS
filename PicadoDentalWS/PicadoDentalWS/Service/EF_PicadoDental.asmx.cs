using PicadoDentalWS.Models;
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
        PicadoDentalEntities db = new PicadoDentalEntities();

        [WebMethod]
        public List<Cliente> ClientList()
        {
            return db.Clientes.ToList();
        }

       




    }
}
