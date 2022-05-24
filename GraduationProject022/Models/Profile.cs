using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject022.Models
{
    public class Profile
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="هذا الحقل مطلوب"),MaxLength(150)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب"), EmailAddress(ErrorMessage ="البريد الالكتروني غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب"), MaxLength(150)]
        public string Qualification { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Address { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب"),MaxLength(11, ErrorMessage = "رقم الهاتف غير صحيح"), MinLength(11, ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNo { get; set; }

        public string ExtraInfo { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public DateTime Age { get; set; }

        public bool IsVaccine { get; set; }=false;  

        public string ProfilePictureUrl { get; set; }

        [NotMapped]
        public IFormFile ProfilePicture { get; set; }
    }
}
