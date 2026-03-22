using Application.Shared;
using Shared;

namespace Application.Commands.FileCommands;

/// <summary>
/// Upload a file using a base64-encoded payload (POST JSON).
/// </summary>
public class SaveFileCommand : Command<FileCommandResult>
{
    /// <summary>Logical file name (without path).</summary>
    public string Name { get; set; }

    /// <summary>File content encoded as base64.</summary>
    public string Base64 { get; set; }

    /// <summary>File extension without dot (e.g. pdf, png).</summary>
    public string Ext { get; set; }

    /// <inheritdoc />
    public override async Task<CommandExecutionResultGeneric<FileCommandResult>> ExecuteCommandLogicAsync()
    {
        var fileResult = await _fileClassRepository.SaveFile(Guid.NewGuid().ToString(), Base64, Name.ToString(), Ext, false);

        if (fileResult.result.Success == false) return await Fail<FileCommandResult>(fileResult.result.ErrorMessage);

        return await Ok(new FileCommandResult() { FilePath = fileResult.filePath });
    }
}


/// <summary>Result of a successful file save.</summary>
public class FileCommandResult
{
    /// <summary>Public or API-relative URL/path to the stored file.</summary>
    public string FilePath { get; set; }
}
