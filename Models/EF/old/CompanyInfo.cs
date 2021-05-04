namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CompanyInfo")]
    public partial class CompanyInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte ID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        public string Address { get; set; }

        [StringLength(50)]
        public string Tax { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(15)]
        public string Phone2 { get; set; }

        [StringLength(15)]
        public string Phone3 { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Email2 { get; set; }

        [StringLength(100)]
        public string YoutubeLink { get; set; }

        [StringLength(100)]
        public string FaceboolLink { get; set; }

        [StringLength(100)]
        public string Logo { get; set; }

        [StringLength(100)]
        public string favicon { get; set; }
    }
}