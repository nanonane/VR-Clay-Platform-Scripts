using System;
using System.Diagnostics;

namespace ShellCommand
{
    public static class ShellHelper
    {
        public static void Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = false,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            // string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // return result;
        }
    }
}
