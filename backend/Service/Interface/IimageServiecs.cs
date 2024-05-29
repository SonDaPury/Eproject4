namespace backend.Service.Interface
{
    public interface IimageServiecs
    {
        string AddFile(IFormFile file);
        string UpdateFile(IFormFile file, string filename);
        string DeleteFile(string filename);

    }
}
