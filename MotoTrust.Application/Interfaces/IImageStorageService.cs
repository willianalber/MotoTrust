namespace MotoTrust.Application.Interfaces;

public interface IImageStorageService
{
    Task<string> SaveImageAsync(string base64Image, string fileName);
    Task<byte[]> GetImageAsync(string fileName);
    Task<bool> DeleteImageAsync(string fileName);
    Task<bool> ImageExistsAsync(string fileName);
}
