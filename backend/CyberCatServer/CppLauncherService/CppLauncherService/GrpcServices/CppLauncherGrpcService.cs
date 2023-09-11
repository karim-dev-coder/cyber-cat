using CppLauncherService.InternalModels;
using CppLauncherService.Services;
using Shared.Server.Dto;
using Shared.Server.ProtoHelpers;
using Shared.Server.Services;

namespace CppLauncherService.GrpcServices;

internal class CppLauncherGrpcService : ICodeLauncherGrpcService
{
    private readonly IProcessExecutorProxy _processExecutorProxy;
    private readonly ICppFileCreator _cppFileCreator;
    private readonly ICppErrorFormatService _errorFormatService;

    public CppLauncherGrpcService(IProcessExecutorProxy processExecutorProxy, ICppFileCreator cppFileCreator, ICppErrorFormatService errorFormatService)
    {
        _processExecutorProxy = processExecutorProxy;
        _cppFileCreator = cppFileCreator;
        _errorFormatService = errorFormatService;
    }

    public async Task<Response<OutputDto>> Launch(LaunchCodeArgs args)
    {
        var (solution, input) = args;
        return await Launch(solution, input);
    }

    private async Task<OutputDto> Launch(string sourceCode, string input = null)
    {
        var compileResult = await CompileCode(sourceCode);
        if (compileResult.Output.HasError)
        {
            await _processExecutorProxy.Run(RunCommand.DeleteFile(compileResult.ObjectFileName));
            await _processExecutorProxy.Run(RunCommand.DeleteFile($"{Path.GetFileNameWithoutExtension(compileResult.ObjectFileName)}.cpp"));
            return new OutputDto
            {
                StandardError = compileResult.Output.StandardError
            };
        }

        var launchOutput = await LaunchCode(compileResult.ObjectFileName, input);
        launchOutput = _errorFormatService.Format(launchOutput);

         await _processExecutorProxy.Run(RunCommand.DeleteFile(compileResult.ObjectFileName));
         await _processExecutorProxy.Run(RunCommand.DeleteFile($"{Path.GetFileNameWithoutExtension(compileResult.ObjectFileName)}.cpp"));

        return new OutputDto
        {
            StandardOutput = launchOutput.StandardOutput,
            StandardError = launchOutput.StandardError
        };
    }

    private async Task<CompileCppResult> CompileCode(string sourceCode)
    {
        var cppFileName = await _cppFileCreator.CreateCppWithText(sourceCode);
        var objectFileName = _cppFileCreator.GetObjectFileName(cppFileName);

        var output = await _processExecutorProxy.Run(RunCommand.CreateCompile(cppFileName, objectFileName));

        return new CompileCppResult
        {
            Output = output,
            ObjectFileName = objectFileName
        };
    }

    private async Task<Output> LaunchCode(string objectFileName, string input)
    {
        return await _processExecutorProxy.Run(RunCommand.CreateLaunch(objectFileName, input));
    }
}