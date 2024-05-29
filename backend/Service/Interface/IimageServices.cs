namespace backend.Service.Interface
{
    public interface IimageServices
    {
        string AddFile(IFormFile file,string rootFolder, string subfolder);
        //string UpdateFile(IFormFile file, string filename , string subfolder);
        string DeleteFile(string filename);
        string GetFile(string relativePath);
        string UpdateFile(IFormFile file, string existingFilePath, string rootFolder, string subFolder);
    }
}
