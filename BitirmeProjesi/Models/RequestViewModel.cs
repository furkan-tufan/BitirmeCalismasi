using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BitirmeProjesi.Models
{
    public class RequestViewModel
    {
        public RequestViewModel()
        {
            Approve = false;
            Check = false;
        }

        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public bool Approve { get; set; }
        public bool Check { get; set; }
    }
}
