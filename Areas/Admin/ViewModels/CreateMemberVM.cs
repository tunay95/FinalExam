namespace FinalLastExam.Areas.Admin.ViewModels
{
    public class CreateMemberVM
    { 
        public string Name { get; set; }
        public string Work { get; set; }
        public string? ImgUrl { get; set; }
        public IFormFile? Image { get; set; }

    }
}
