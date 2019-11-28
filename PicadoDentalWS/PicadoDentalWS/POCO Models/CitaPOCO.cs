using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicadoDentalWS.POCO_Models
{
    public class CitaPOCO
    {
        public DateTime Fecha { get; set; }

        public String ClienteNombre { get; set; }
        public String ClienteApellidos{ get; set; }
        public String DoctorNombre { get; set; }
        public String DoctorApellidos{ get; set; }
        public String Detalles { get; set; }


    }
}