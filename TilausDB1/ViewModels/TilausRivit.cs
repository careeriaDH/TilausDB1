namespace TilausDB1.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TilausRivit
    {
        [Key]
        public int TilausriviId { get; set; }

        public int Maara { get; set; }
        public int TilausID { get; set; }
        public int TuoteID { get; set; }

        public string Nimi { get; set; }
        public Nullable<decimal> Ahinta { get; set; }

        public Nullable<int> Kategoria { get; set; }
        public int KategoriaId { get; set; }

        //public TilausRivit(int kategoriaId) => KategoriaId = kategoriaId;

        public string KategoriaNimi { get; set; }



    }
}