using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicadoDentalWS.POCOModels
{
    public class ClientePOCO
    {
        public int ClienteID { get; set; }
        public int PersonaID { get; set; }
        public string Nombre { get; set; }
    }
}