namespace FinalLastExam.Helpers
{
    public static class FileManager
    {
        public static string Upload(this IFormFile File, string envpath, string folderName)
        {
            string fileName = File.FileName;
            fileName = Guid.NewGuid().ToString() + fileName;
            string path = envpath + folderName + fileName;
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                File.CopyTo(stream);

            }
            return fileName;
        }

        public static bool CheckContent(this IFormFile File, string content)
        {
            return File.ContentType.Contains(content);
        }

        public static bool CheckLength(this IFormFile File, int length)
        {
            return File.Length <= length *1024 *1024;
        }
    }
}
