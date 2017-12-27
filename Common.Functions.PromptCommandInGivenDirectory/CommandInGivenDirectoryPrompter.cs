using System.Diagnostics;
using System.IO;
using System.Linq;
using static Common.Functions.PromptCommand.CommandPrompter;

namespace Common.Functions.PromptCommandInGivenDirectory
{
    public static class CommandInGivenDirectoryPrompter
    {
        private const string ChangeDirCommand = "cd";
        private const char Colon = ':';

        public static Process PromptIn(string directory)
        {
            var cmd = GetPrompt();
            ChangeDriveIfNeeded(cmd, directory);
            ChangeDirectory(cmd, directory);
            return cmd;
        }

        private static void ChangeDirectory(Process cmd, string directory)
            => WriteToStandardInput(cmd, $"{ChangeDirCommand} {directory}");

        private static void ChangeDrive(Process cmd, string drive)
            => WriteToStandardInput(cmd, $"{drive}{Colon}");

        private static void ChangeDriveIfNeeded(Process cmd, string directory)
        {
            if (!directory.Contains($"{Colon}")) return;
            var currentDrive = GetDrive(cmd.StartInfo.WorkingDirectory);
            var targetDrive = GetDrive(directory);
            if (currentDrive != targetDrive) ChangeDrive(cmd, targetDrive);
        }

        private static string GetDrive(string path)
            => Directory.GetDirectoryRoot(path)
                .Split(Colon)
                .First()
                .ToLower();

        private static Process GetPrompt()
        {
            var cmd = Prompt();
            cmd.StandardInput.AutoFlush = true;
            return cmd;
        }

        private static void WriteToStandardInput(Process cmd, string input)
            => cmd.StandardInput.WriteLine(input);
    }
}
