using System.ComponentModel.DataAnnotations.Schema;

namespace FinalLastExam.Models
{
    public class Member:BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
