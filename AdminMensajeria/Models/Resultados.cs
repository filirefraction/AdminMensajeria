using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminMensajeria.Models
{
    public class Resultados
    {
            public bool Result { get; set; }
            public int  Valor { get; set; }
            public string Mensaje { get; set; }
            public bool Redirecciona { get; set; }
            public int Fila { get; set; }
            public int? Opcional { get; set; }


    }
}