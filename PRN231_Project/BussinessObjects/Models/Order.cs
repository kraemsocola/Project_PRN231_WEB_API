using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BussinessObjects.Models
{
    public class Order
    {
        public Order() { }

        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderDate { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public int TotalMoney { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [ForeignKey(nameof(UserId))]
        public AppUser AppUser { get; set; }

    }
}
