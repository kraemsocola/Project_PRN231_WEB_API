using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BussinessObjects.Models
{
    public class OrderDetail
    {
        public OrderDetail() { }
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
        [Required]

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
