namespace mvvm
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        public int ProductId { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        public int? year { get; set; }

        public decimal? price { get; set; }
    }
}
