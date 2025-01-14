﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitirmeProjesi.Models
{
    public class Request
    {
        public Request() { 
            Approve = false;
            Check = false;
        }

        public int Id { get; set; }
        [Display(Name = "İzin Başlangıç Tarihi")]
        [Required(ErrorMessage = "Tarih Girin")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "İzin Bitiş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public bool Approve { get; set; }
        public bool Check { get; set; }

        [NotMapped]
        public virtual List<IFormFile>? Files { get; set; } = new List<IFormFile>();
        public virtual List<FileModel>? FileList { get; set; } = new List<FileModel>();
    }
}
