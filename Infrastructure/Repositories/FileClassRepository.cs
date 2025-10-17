using Domain.Entities.FileEntity.IRepository;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shared;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Infrastructure.Repositories;

public class FileClassRepository : IFileClassRepository
{
    private readonly IHostingEnvironment _hostingEnvironment;
    private readonly IConfiguration _config;
    private List<string> ExtList = new List<string>() {
    "pdf", "doc", "docx", "xls", "xlsx", "csv",
    "ppt", "pptx", "txt", "rtf", "odt", "ods", "odp",
    "md", "markdown",
    "png", "jpg", "jpeg", "gif", "bmp", "tiff", "svg", "webp", "ico", "heic",
    "zip", "rar", "7z", "tar", "gz", "bz2", "xz"};
    public FileClassRepository(IHostingEnvironment hostingEnvironment, IConfiguration config)
    {
        _hostingEnvironment = hostingEnvironment;
        _config = config;
    }

    private string GetSolutionFileFolderDirectory()
    {
        string fileDir = "\\Files\\";

        DirectoryInfo dir = new DirectoryInfo(_hostingEnvironment.ContentRootPath + fileDir);
        return dir.FullName;
    }


    public async Task<(CommandExecutionResult result, string filePath)> SaveFile(string folderName, string base64String, string fileName, string ext, bool? skipCheck)
    {
        //var directoryPath =;
        fileName = Regex.Replace(fileName, @"[<>:""/\\|?*]", "_");


        string FullFolderPath = Path.Combine(GetSolutionFileFolderDirectory(), folderName);

        if (skipCheck == null || skipCheck == false)
        {
            if (ExtList.FirstOrDefault(x => x == ext).IsNull())
            //if (ext != "png" && ext != "jpg" && ext != "docx" && ext != "pdf" && ext != "xlsx")
            {
                return (new CommandExecutionResult { Success = false, ErrorMessage = "File format error" }, null);

            }
        }

        if (!Directory.Exists(FullFolderPath))
        {
            Directory.CreateDirectory(FullFolderPath);
        }
        string generatedName = Guid.NewGuid().ToString();
        string filePath = Path.Combine(FullFolderPath, $"{fileName}_{generatedName}.{ext}");
        string fileAddres = "Files/" + folderName + $"/{fileName}_{generatedName}.{ext}";
        try
        {
            byte[] fileBytes = Convert.FromBase64String(base64String);
            File.WriteAllBytes(filePath, Convert.FromBase64String(base64String));
            string ApiAddress = _config.GetSection("SendVerifyMailLinks:ApiAddress")?.Value.ToString();

            return (new CommandExecutionResult { Success = true }, ApiAddress + fileAddres);
        }
        catch (Exception ex)
        {
            return (new CommandExecutionResult { Success = false, ErrorMessage = $"File Write error " }, null);
        }
    }

    public async Task<(CommandExecutionResult result, string filePath)> SaveFile(string FilePathForDb, string hostingEnvironmentPath, string folderName, string base64String, string fileName, string ext, string OldFileName = null)
    {
        //var directoryPath =; //"\\Files\\Employees"
        try
        {
            string FullFolderPath = Path.Combine(new DirectoryInfo(_hostingEnvironment.ContentRootPath + hostingEnvironmentPath)?.FullName ?? throw new Exception("_hostingEnvironment fullname is null "), folderName);

            //if (ext != "png" && ext != "jpg" && ext != "docx" && ext != "pdf" && ext != "xlsx" && ext != "jpeg" && ext != "pptx" && ext != "ppt")
            if (ExtList.FirstOrDefault(x => x == ext).IsNull())

            {
                return (new CommandExecutionResult { Success = false, ErrorMessage = "File format error" }, null);
            }

            if (!Directory.Exists(FullFolderPath))
            {
                Directory.CreateDirectory(FullFolderPath);
            }
            // თუ OldFileName არ იქნება ნალი გადააწერს  თავზე   OldFileName მიხედვით
            string generatedName = Guid.NewGuid().ToString();
            string filePath = OldFileName.IsNull() ? Path.Combine(FullFolderPath, $"{fileName}_{generatedName}.{ext}")
                                                     : Path.Combine(FullFolderPath, OldFileName);
            string fileAddres = OldFileName.IsNull() ? FilePathForDb + folderName + $"/{fileName}_{generatedName}.{ext}"
                                                      : FilePathForDb + folderName + $"/{OldFileName}";

            byte[] fileBytes = Convert.FromBase64String(base64String);
            string ApiAddress = _config.GetSection("SendVerifyMailLinks:ApiAddress").Value?.ToString() ?? throw new Exception("ApiAddress  is null ");
            File.WriteAllBytes(filePath, Convert.FromBase64String(base64String));

            return (new CommandExecutionResult { Success = true }, ApiAddress + fileAddres);
        }
        catch (Exception ex)
        {
            return (new CommandExecutionResult { Success = false, ErrorMessage = $"File Write error" }, null);
        }
    }

    public async Task<byte[]> GenerateAndDownloadZipAsync(List<string> FileUrls)
    {
        var SaveFilePath = new DirectoryInfo(_hostingEnvironment.ContentRootPath).FullName + "Files\\ZipFiles";

        if (!Directory.Exists(SaveFilePath))
        {
            Directory.CreateDirectory(SaveFilePath);
        }


        var filename = DateTime.Now.ToString("MM_dd_yyyy") + "_" + Guid.NewGuid().ToString() + ".zip";
        var tempOut = Path.Combine(SaveFilePath, filename);

        using (ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(tempOut)))
        {
            zipOutputStream.SetLevel(9);
            byte[] buffer = new byte[4096];

            foreach (string fileUrl in FileUrls)
            {
                string fileName = Path.GetFileName(fileUrl);

                if (!string.IsNullOrEmpty(fileName))
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        using (HttpResponseMessage response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                        using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                        {
                            ZipEntry entry = new(fileName);
                            entry.DateTime = DateTime.Now;
                            entry.IsUnicodeText = true;
                            zipOutputStream.PutNextEntry(entry);

                            using (FileStream fileStream = File.OpenWrite(Path.Combine(SaveFilePath, fileName)))
                            {
                                int bytesRead;
                                do
                                {
                                    bytesRead = streamToReadFrom.Read(buffer, 0, buffer.Length);
                                    zipOutputStream.Write(buffer, 0, bytesRead);
                                } while (bytesRead > 0);
                            }
                            if (File.Exists(Path.Combine(SaveFilePath, fileName)))
                            {
                                File.Delete(Path.Combine(SaveFilePath, fileName));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and continue to the next file
                        Console.WriteLine($"Error downloading file '{fileName}': {ex.Message}");
                    }
                }
            }
        }

        if (!Directory.Exists(SaveFilePath))
        {
            Directory.CreateDirectory(SaveFilePath);
        }

        byte[] finalResult = File.ReadAllBytes(tempOut);

        if (finalResult == null || finalResult.Length == 0)
        {
            throw new Exception("Error creating the zip file.");
        }
        return finalResult;
    }



    public async Task<FileResult> GenerateAndDownloadZipAsync(List<string> FileUrls, string SaveFilePath)
    {
        if (!Directory.Exists(SaveFilePath))
        {
            Directory.CreateDirectory(SaveFilePath);
        }
        var filename = DateTime.Now.ToString("MM_dd_yyyy") + "_" + Guid.NewGuid().ToString() + ".zip";
        var tempOut = Path.Combine(SaveFilePath, filename);

        using (ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(tempOut)))
        {
            zipOutputStream.SetLevel(9);
            byte[] buffer = new byte[4096];

            foreach (string fileUrl in FileUrls)
            {
                string fileName = Path.GetFileName(fileUrl);

                if (!string.IsNullOrEmpty(fileName))
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        using (HttpResponseMessage response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                        using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                        {
                            ZipEntry entry = new(fileName);
                            entry.DateTime = DateTime.Now;
                            entry.IsUnicodeText = true;
                            zipOutputStream.PutNextEntry(entry);

                            using (FileStream fileStream = File.OpenWrite(Path.Combine(SaveFilePath, fileName)))
                            {
                                int bytesRead;
                                do
                                {
                                    bytesRead = streamToReadFrom.Read(buffer, 0, buffer.Length);
                                    zipOutputStream.Write(buffer, 0, bytesRead);
                                } while (bytesRead > 0);
                            }
                            if (File.Exists(Path.Combine(SaveFilePath, fileName)))
                            {
                                File.Delete(Path.Combine(SaveFilePath, fileName));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and continue to the next file
                        Console.WriteLine($"Error downloading file '{fileName}': {ex.Message}");
                    }
                }
            }
        }

        byte[] finalResult = File.ReadAllBytes(tempOut);

        if (finalResult == null || finalResult.Length == 0)
        {
            throw new Exception("Error creating the zip file.");
        }

        return new FileContentResult(finalResult, "application/zip")
        {
            FileDownloadName = filename
        };
    }



    public async Task<CommandExecutionResult> DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return new CommandExecutionResult { Success = true };
            }
            else
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = "File not found" };
            }
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = $"File deletion error \n{ex.Message}" };
        }
    }


}
