using System;
using System.Diagnostics;
using System.IO;

namespace LeelaDemo
{
    internal class Program
    {
        const string ProgramName = @"D:\leela-zero-0.17-win64\leelaz.exe";

        const string ProgramWorkingDirectory = @"D:\leela-zero-0.17-win64";

        static int Visits = 200;

        static string WeigthPath = @"D:\leela-zero-0.17-win64\best-network";

        static int ThreadCount = 2;

        static void Main(string[] args)
        {

             Process process = new Process();
    
             process.StartInfo.FileName = ProgramName;

             process.StartInfo.WorkingDirectory = ProgramWorkingDirectory;

             process.StartInfo.Arguments = $@" -g -t {ThreadCount} -w {WeigthPath} -v {Visits}";

             process.StartInfo.UseShellExecute = false;

             process.StartInfo.Verb = "runas";

             process.StartInfo.CreateNoWindow = true;

             process.StartInfo.RedirectStandardOutput = true;

             process.StartInfo.RedirectStandardError = true;

             process.StartInfo.RedirectStandardInput = true;

             process.ErrorDataReceived += Process_ErrorDataReceived;

             process.OutputDataReceived += Process_OutputDataReceived;

             process.Start();

             process.BeginErrorReadLine();

             process.BeginOutputReadLine();

             StreamWriter writher =  process.StandardInput;

             while (true)
             {
               string cmd =  Console.ReadLine();

                writher.WriteLine(cmd);

                if (cmd.Equals("quit")) 
                {
                    break;
                }          
            }

            process.Close();

            Console.Read();      

        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
   
    }
}
