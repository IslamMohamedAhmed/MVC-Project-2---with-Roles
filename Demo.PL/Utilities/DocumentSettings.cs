namespace Demo.PL.Utilities
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile formFile, string FolderName)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files", FolderName);

            string FileName = $"{Guid.NewGuid()}-{formFile.FileName}";

            string FilePath = Path.Combine(FolderPath, FileName);

            using var Stream = new FileStream(FilePath, FileMode.Create);

            formFile.CopyTo(Stream);

            return FileName;
        }


        public static void DeleteFile(string FolderName, string FileName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files", FolderName, FileName);

            if (File.Exists(FilePath)) { File.Delete(FilePath); }
        }
    }
}
