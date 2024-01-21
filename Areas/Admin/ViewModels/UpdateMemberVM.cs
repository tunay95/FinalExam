namespace FinalLastExam.Areas.Admin.ViewModels
{
    public class UpdateMemberVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Work { get; set; }
        public string? ImgUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
