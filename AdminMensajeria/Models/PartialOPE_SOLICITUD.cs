using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdminMensajeria.Models
{
    [MetadataType(typeof(MetadataOP_SOLICITUD))]
    [DataContract(IsReference = true)]
    public partial class OPE_SOLICITUD
    {
        [DataContract(IsReference = true)]
        public class MetadataOP_SOLICITUD
        {
            public MetadataOP_SOLICITUD()
            {
                this.OPE_SOLICITUDPRODUCTO = new HashSet<OPE_SOLICITUDPRODUCTO>();
                this.OPE_SOLICITUDPUNTOSENTREC = new HashSet<OPE_SOLICITUDPUNTOSENTREC>();
            }

            [DataMember]
            public int IdSolicitud { get; set; }
            [DataMember]
            public int IdUsuario { get; set; }
            [DataMember]
            public Nullable<int> IdClientProvee { get; set; }
            [DataMember]
            public string Folio { get; set; }
            [DataMember]
            public System.DateTime FechaCreacion { get; set; }
            [DataMember]
            public bool RequiereAcuse { get; set; }
            [DataMember]
            public string Acuse { get; set; }
            [DataMember]
            public bool Emergencia { get; set; }
            [DataMember]
            public int EstatusSolicitud { get; set; }

            [DataMember]
            public virtual GEN_USUARIO GEN_USUARIO { get; set; }
            [DataMember]
            public virtual ICollection<OPE_SOLICITUDPRODUCTO> OPE_SOLICITUDPRODUCTO { get; set; }
            [JsonIgnore]
            public virtual ICollection<OPE_SOLICITUDPUNTOSENTREC> OPE_SOLICITUDPUNTOSENTREC { get; set; }
        }
    }
}