using System;
using TilausDB1.Models;

namespace TilausDB1.ViewModels
{
    public class TilausTietoDataBase
    {
        public string AsiakasID { get; set; }
        public string Nimi { get; set; }
        public string Osoite { get; set; }
        public string Postinumero { get; set; }
        public string Vastuumyyja { get; set; }
        public Nullable<decimal> Luottoraja { get; set; }
        public string Websivusto { get; set; }

        //public virtual Postitoimipaikat Postitoimipaikat { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Tilaukset> Tilaukset { get; set; }

        public int TilausID { get; set; }

        public string Toimitusosoite { get; set; }
        public Nullable<System.DateTime> Tilauspvm { get; set; }
        public Nullable<System.DateTime> Toimituspvm { get; set; }

        public virtual Asiakkaat Asiakkaat { get; set; }
        public virtual Postitoimipaikat Postitoimipaikat { get; set; }
    }
}