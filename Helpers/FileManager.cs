namespace FinalLastExam.Helpers
{
    public static class FileManager
    {
        public static string Upload(this IFormFile File,string envpath,string FolderName)
        {
            string filename = File.FileName;
            filename= Guid.NewGuid().ToString()+filename;
            string path = envpath+FolderName+filename;
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                File.CopyTo(fileStream);
            };
            return filename;
        }
        public static bool CheckContent(this IFormFile File , string content)
        {
;           return File.ContentType.Contains(content);
        }

        public static bool ChecckLength(this IFormFile File,int length)
        {
            return File.Length / 1024 / 1024 <= 3;
        }
    }
}
