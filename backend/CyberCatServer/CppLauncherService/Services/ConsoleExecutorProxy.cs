﻿using System.Diagnostics;
using System.Text;
using CompilerServiceAPI.InternalModels;
using Microsoft.Extensions.Options;

namespace CompilerServiceAPI.Services
{
    internal class ConsoleExecutorProxy : IProcessExecutorProxy
    {
        private readonly TimeSpan _timeOut;

        public ConsoleExecutorProxy(IOptions<CppLauncherAppSettings> appSettings)
        {
            _timeOut = appSettings.Value.ProcessTimeout;
        }

        public async Task<Output> Run(string command, string arguments)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardErrorEncoding = Encoding.Default
            };

            var process = Process.Start(startInfo);
            var output = await WaitForExit(process, _timeOut);
            if (output.HasError)
            {
                return output;
            }

            while (!process.HasExited)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            if (process.ExitCode != 0)
            {
                return await process.ReadError();
            }

            return await process.ReadOk();
        }

        private async Task<Output> WaitForExit(Process process, TimeSpan timeOut)
        {
            var ct = new CancellationTokenSource(timeOut).Token;
            try
            {
                await process.WaitForExitAsync(ct);
                return Output.Empty;
            }
            catch (TaskCanceledException _)
            {
                Kill(process, timeOut);
                return process.ReadError("The process took more than 5 seconds");
            }
        }

        private void Kill(Process process, TimeSpan timeOut)
        {
            if (!process.HasExited)
            {
                process.Kill(true);
            }

            // Синхронно ждем завершения процесса.
            process.WaitForExit(timeOut);

            if (!process.HasExited)
            {
                throw new InvalidOperationException("Process not killed");
            }
        }
    }
}