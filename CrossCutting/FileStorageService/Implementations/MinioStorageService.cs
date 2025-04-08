using Application.Contracts.FileService.Interfaces;
using Application.Contracts.FileService.Models;
using FileTypeChecker;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.DataModel.Args;
using System.Net.Security;

namespace CrossCutting.FileStorageService.Implementations;

internal class MinioStorageService(IMinioClient minioClient,
    [FromKeyedServices(key:"SasMinioClient")] IMinioClient sasClient) : IFileService
{
    private const string CleanBucketName = "cleanfiles";
    private async Task CreateBucketIfMissing(CancellationToken token = default)
    {
        var checkBucketExistsArg = new BucketExistsArgs().WithBucket(CleanBucketName);

        if (await minioClient.BucketExistsAsync(checkBucketExistsArg))
            return;

        var createBucketArg = new MakeBucketArgs().WithBucket(CleanBucketName);
        await minioClient.MakeBucketAsync(createBucketArg, token);
    }
    public async Task<List<GetFileModel>> GetFilesByNameAsync(List<string> fileNames, CancellationToken cancellationToken = default)
    {
        await CreateBucketIfMissing(cancellationToken);
        var result = new List<GetFileModel>();
        foreach (var fileName in fileNames)
        {
            var objectInfo = new StatObjectArgs().WithBucket(CleanBucketName).WithObject(fileName);

            var objectInfoResult = await minioClient.StatObjectAsync(objectInfo, cancellationToken);

            if (objectInfoResult is null) { continue; }

            var sasUrlArgs = new PresignedGetObjectArgs()
                .WithBucket(CleanBucketName)
                .WithObject(fileName)
                .WithExpiry((int)TimeSpan.FromMinutes(30).TotalSeconds);

            var fileUrl = await sasClient.PresignedGetObjectAsync(sasUrlArgs);

            result.Add(new GetFileModel(fileUrl, objectInfoResult.ContentType, objectInfoResult.ObjectName));
        }

        return result;

    }

    public async Task RemoveFileAsync(string[] removeFiles, CancellationToken cancellationToken = default)
    {
        await CreateBucketIfMissing(cancellationToken);
        var filesToRemove = new List<string>();
        foreach (var fileName in removeFiles)
        {
            var objectInfo = new StatObjectArgs().WithBucket(CleanBucketName).WithObject(fileName);

            var objectInfoResult = await minioClient.StatObjectAsync(objectInfo, cancellationToken);

            if (objectInfoResult is null) { continue; }

            filesToRemove.Add(fileName);

            var removeFileArgs = new RemoveObjectArgs().WithBucket(CleanBucketName).WithObject(objectInfoResult.ObjectName);

            await minioClient.RemoveObjectAsync(removeFileArgs,cancellationToken);
        }

    }

    public async Task<List<SaveFileModelResult>> SaveFilesAsync(List<SaveFileModel> files, CancellationToken cancellationToken = default)
    {
        await CreateBucketIfMissing(cancellationToken);
        var list = new List<SaveFileModelResult>();

        foreach (var file in files)
        {
            await using var memoryStream = new MemoryStream(Convert.FromBase64String(file.Base64File));

            var fileName = $"{Guid.NewGuid():N}.{FileTypeValidator.GetFileType(memoryStream).Extension}";

            var fileType = !string.IsNullOrEmpty(file.FileContent) ? file.FileContent : "application/octet-stream";

            memoryStream.Position = 0;

            var createFileArg = new PutObjectArgs()
                .WithBucket(CleanBucketName)
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length)
                .WithObject(fileName)
                .WithContentType(fileType);

            await minioClient.PutObjectAsync(createFileArg, cancellationToken);
            list.Add(new SaveFileModelResult(fileName, fileType));
        }
        return list;
    }
}
