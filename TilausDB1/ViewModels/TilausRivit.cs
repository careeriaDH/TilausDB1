namespace TilausDB1.ViewModels
{
    using System;
    using System.Drawing;
    using TilausDB1.Models;

    public class TilausRivit
    {
        public int? AsiakasID { get; set; }
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
        public virtual Asiakkaat Asiakkaat { get; set; }
        public virtual Postitoimipaikat Postitoimipaikat { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public int TilausriviID { get; set; }
        public Nullable<int> TuoteID { get; set; }
        public Nullable<int> Maara { get; set; }
        public Nullable<decimal> Ahinta { get; set; }

        public virtual Tilaukset Tilaukset { get; set; }
        public virtual Tuotteet Tuotteet { get; set; }
    }
}