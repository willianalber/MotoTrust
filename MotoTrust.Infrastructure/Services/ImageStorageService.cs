using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MotoTrust.Application.Interfaces;
using System.Text.RegularExpressions;

namespace MotoTrust.Infrastructure.Services;

public class ImageStorageService : IImageStorageService
{
    private readonly string _storagePath;
    private readonly ILogger<ImageStorageService> _logger;

    public ImageStorageService(ILogger<ImageStorageService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Images");
        
        // Cria o diretório se não existir
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
            _logger.LogInformation("Diretório de storage criado: {StoragePath}", _storagePath);
        }
    }

    public async Task<string> SaveImageAsync(string base64Image, string fileName)
    {
        try
        {
            // Valida se é uma imagem válida (PNG ou BMP)
            if (!IsValidImageFormat(base64Image))
                throw new ArgumentException("Formato de imagem inválido. Apenas PNG e BMP são aceitos.");

            // Remove o prefixo data:image/...;base64, se existir
            var cleanBase64 = CleanBase64String(base64Image);
            
            // Converte base64 para bytes
            var imageBytes = Convert.FromBase64String(cleanBase64);
            
            // Gera nome único para o arquivo
            var uniqueFileName = GenerateUniqueFileName(fileName);
            var filePath = Path.Combine(_storagePath, uniqueFileName);
            
            // Salva o arquivo
            await File.WriteAllBytesAsync(filePath, imageBytes);
            
            _logger.LogInformation("Imagem salva com sucesso: {FileName}", uniqueFileName);
            return uniqueFileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar imagem: {FileName}", fileName);
            throw;
        }
    }

    public async Task<byte[]> GetImageAsync(string fileName)
    {
        try
        {
            var filePath = Path.Combine(_storagePath, fileName);
            
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Imagem não encontrada: {fileName}");
            
            return await File.ReadAllBytesAsync(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recuperar imagem: {FileName}", fileName);
            throw;
        }
    }

    public async Task<bool> DeleteImageAsync(string fileName)
    {
        try
        {
            var filePath = Path.Combine(_storagePath, fileName);
            
            if (!File.Exists(filePath))
                return false;
            
            File.Delete(filePath);
            _logger.LogInformation("Imagem removida: {FileName}", fileName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover imagem: {FileName}", fileName);
            return false;
        }
    }

    public Task<bool> ImageExistsAsync(string fileName)
    {
        var filePath = Path.Combine(_storagePath, fileName);
        return Task.FromResult(File.Exists(filePath));
    }

    private bool IsValidImageFormat(string base64Image)
    {
        // Verifica se contém os headers de PNG ou BMP
        var cleanBase64 = CleanBase64String(base64Image);
        var imageBytes = Convert.FromBase64String(cleanBase64);
        
        // PNG: 89 50 4E 47
        if (imageBytes.Length >= 4 && 
            imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && 
            imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
            return true;
        
        // BMP: 42 4D
        if (imageBytes.Length >= 2 && 
            imageBytes[0] == 0x42 && imageBytes[1] == 0x4D)
            return true;
        
        return false;
    }

    private string CleanBase64String(string base64Image)
    {
        // Remove prefixo data:image/...;base64, se existir
        if (base64Image.Contains(","))
            return base64Image.Split(',')[1];
        
        return base64Image;
    }

    private string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        if (string.IsNullOrEmpty(extension))
            extension = ".png"; // Default para PNG
        
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var guid = Guid.NewGuid().ToString("N")[..8];
        
        return $"{fileNameWithoutExtension}_{timestamp}_{guid}{extension}";
    }
}
