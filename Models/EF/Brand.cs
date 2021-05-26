namespace Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Brand")]
    public partial class Brand
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte ID { get; set; }

        [StringLength(10)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Logo { get; set; }
    }
}