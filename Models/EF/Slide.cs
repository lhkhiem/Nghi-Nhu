namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Slide")]
    public partial class Slide
    {
        [Key]
        public int ID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [StringLength(500)]
        public string Caption1 { get; set; }

        [StringLength(500)]
        public string Caption2 { get; set; }

        [StringLength(500)]
        public string Caption3 { get; set; }

        [StringLength(500)]
        public string Caption4 { get; set; }

        [StringLength(500)]
        public string Caption5 { get; set; }

        [StringLength(500)]
        public string Caption6 { get; set; }

        [StringLength(250)]
        public string Link { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public int DisplayOrder { get; set; }

        public bool Status { get; set; }
    }
}