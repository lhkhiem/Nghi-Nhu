namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Visitor")]
    public partial class Visitor
    {
        [Key]
        [StringLength(50)]
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
