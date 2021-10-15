namespace Paperwork.Core.Interfaces
{
    public interface IIOService
    {
        string GetTemporaryDirectory();
        void ZipFolderIntoFile(string folderPath, string zipFilePath);
    }
}