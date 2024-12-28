using Application.Contracts.FileService.Models;

namespace Application.Contracts.FileService.Interfaces;

public interface IFileService
{
    Task<List<SaveFileModelResult>> saveFilesAsync(List<SaveFileModel> files,CancellationToken cancellationToken = default);
    Task<List<GetFileModel>> GetFilesByNameAsync(List<string> fileNames,CancellationToken cancellationToken = default);
}
