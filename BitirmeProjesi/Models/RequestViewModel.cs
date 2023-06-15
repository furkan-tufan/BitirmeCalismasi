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
        [Display(Name = "İzin Başlangıç Tarihi")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "İzin Bitiş Tarihi")]
        public DateTime? EndDate { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public bool Approve { get; set; }
        public bool Check { get; set; }
        public virtual List<IFormFile>? Files { get; set; } = new List<IFormFile>();
        public virtual List<FileModel>? FileList { get; set; } = new List<FileModel>();
    }
}
