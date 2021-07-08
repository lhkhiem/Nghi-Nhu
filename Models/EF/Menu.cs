namespace Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Menu")]
    public partial class Menu
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50)]
        public string Text { get; set; }

        [StringLength(250)]
        public string Link { get; set; }

        public int DisplayOrder { get; set; }

        [StringLength(50)]
        public string Target { get; set; }

        public bool Status { get; set; }

        public int MenuTypeID { get; set; }
    }
}