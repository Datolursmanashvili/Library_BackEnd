using Shared;
using System.Web.Mvc;

namespace Domain.Entities.FileEntity.IRepository;

public interface IFileClassRepository
{
    //Task<CommandExecutionResult>  SaveFile(string folderName, string base64String, string FileName, string Ext);

    Task<(RepositoryExecutionResult result, string filePath)> SaveFile(string folderName, string base64String, string fileName, string ext, bool? skipChec);
    Task<(RepositoryExecutionResult result, string filePath)> SaveFile(string FilePathForDb, string hostingEnvironmentPath, string folderName, string base64String, string fileName, string ext, string OldFileName = null);
    Task<FileResult> GenerateAndDownloadZipAsync(List<string> FileUrls, string SaveFilePath);
    Task<byte[]> GenerateAndDownloadZipAsync(List<string> FileUrls);
    Task<RepositoryExecutionResult> DeleteFile(string filePath);
}
