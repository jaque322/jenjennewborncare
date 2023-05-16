
using jenjennewborncare.Areas.Identity.Data;

namespace jenjennewborncare.Models
{
    public class Invoice { 
    public int Id { get; set; }

        public string Name { get; set; }
    public string PaymentId{ get; set; }
  
    public DateTime Date { get; set; }
    public float TotalAmount;

    public User User { get; set; }

    }

}
