using Application.Shared;
using Shared;
using System.Web.Mvc;

namespace Application.Commands.FileCommands;

public class SaveFileCommand : Command<FileCommandResult>
{
    public string Name { get; set; }
    public string Base64 { get; set; }
    public string Ext { get; set; }
    public override async Task<CommandExecutionResultGeneric<FileCommandResult>> ExecuteCommandLogicAsync()
    {
        var fileResult = await _fileClassRepository.SaveFile(Guid.NewGuid().ToString(), Base64, Name.ToString(), Ext, false);

        if (fileResult.result.Success == false) return await Fail<FileCommandResult>(fileResult.result.ErrorMessage);

        return await Ok(new FileCommandResult() { FilePath = fileResult.filePath });
    }
}


public class FileCommandResult
{
    public string FilePath { get; set; }
}