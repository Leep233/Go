using Go.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Go.Core
{
    public class LeelaPlayer : GoPlayer, IDisposable, ILeelaMessageHandler
    {
        const string ProgramName = @"D:\leela-zero-0.17-win64\leelaz.exe";

        const string ProgramWorkingDirectory = @"D:\leela-zero-0.17-win64";

        static int Visits = 200;

        static string WeigthPath = @"D:\leela-zero-0.17-win64\best-network";

        static int ThreadCount = 2;

        private int currentOperationIndex=0;
        protected class LeelaStartInfo
        {
            public string FileName { get; set; }

            public string ProgramWorkingDirectory { get; set; }

            public int Visits { get; set; } = 200;

            public string WeigthPath { get; set; } 

            public int ThreadCount { get; set; } = 2;
        }

        private Process leelaProcessHandler;

        public Action<Stone> OnMovedDelegate { get; private set; }

        public LeelaPlayer(string name = "leela") : base(name)
        {
        
                OnMovedDelegate = new Action<Stone>(OnMovedCallback);
            

            LeelaStartInfo startInfo = new LeelaStartInfo()
            {
                FileName = @"D:\leela-zero-0.17-win64\leelaz.exe",

                ProgramWorkingDirectory = @"D:\leela-zero-0.17-win64",

                WeigthPath = @"D:\leela-zero-0.17-win64\best-network",
            };

            leelaProcessHandler = LeelaProcess();// CreateLeelaProcess(startInfo);
        }

        private Process LeelaProcess() {

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

            return process;
        }

        private LeelaOperationObject currentOperation;

        private  void OnMovedCallback(Stone stone)
        {
            Task.Factory.StartNew(() => 
            {
                if (currentOperation != null) return;

                currentOperation = new LeelaOperationObject()
                {
                    Id = new Random().Next(0, int.MaxValue),
                    Command = "play",
                    Arg = stone
                };

                LeelaWriteCommand(currentOperation);

              //  leelaProcessHandler.StandardInput.WriteLine($"{currentOperationIndex} play {stone}");

              //  string message = ReadLeelaMessage();

              //  LeelaMessageHandler(message);
            });          
        }

        private void LeelaWriteCommand(LeelaOperationObject operation) 
        {
            Debug.WriteLine(operation);

            leelaProcessHandler.StandardInput.WriteLine($"{operation}");
        }

        private Stone LeelaAnalyzer() 
        {
          //  leelaProcessHandler.StandardInput.WriteLine($"{currentOperationIndex} lz-analyze");


            currentOperation = new LeelaOperationObject()
            {
                Id = currentOperationIndex,
                Command = "lz-analyze",               
            };

            LeelaWriteCommand(currentOperation);

     

          //  string[] messages = message.Split(@"\n");

            return new Stone() { Color= StoneColor ,Position = new Vector2D {x=1,y=3 } };
        }

        private void LeelaMessageHandler(string message)
        {
         
            if (message.StartsWith("="))
            {
                if (currentOperation != null)
                {
                    currentOperation = null;

                    LeelaAnalyzer();
                }              
            }
        }

      

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);

            Handle(e.Data);    
           
            //if (e.Data.StartsWith(currentOperationIndex.ToString())) 
            //{
            //   
            //}          
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
        }


        public override bool Move(object arg)
        {
            return base.Move(arg);
        }

        public void Dispose()
        {
            leelaProcessHandler?.Close();
        }

        public void Handle(string message)
        {
           
        }

        class LeelaOperationObject
        {
            public int Id { get; set; }

            public string Command { get; set; }
            public object Arg { get; set; }

            public override string ToString()
            {
                return $"{Id} {Command} {Arg??""}";
            }
        }
    
    }
}
