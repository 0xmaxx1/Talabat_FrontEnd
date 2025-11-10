using System.Threading.Tasks;

namespace Admin_Dashboard.Helpers
{
    public class PictureSettings
    {
        // Method to upload a file and return the file path
        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 2. Create Folder if it doesn't exist
            string fileName = file.FileName;

            // 3. Get File Path
            string filePath = Path.Combine(folderPath, fileName);

            // 4. Save File as Stream
            using FileStream fs = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(fs);

            string relativePath = Path.Combine("images", folderName, fileName).Replace("\\", "/");

            return relativePath;
        }


        // Method to Delete File
        public static async Task DeleteFile(string relativePath)
        {
            // 1. Get Folder Path
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            // 2. Check if file exists
            if (File.Exists(filePath))
                await Task.Run(() => File.Delete(filePath));
        }
    }
}
