using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Go.Core
{
    public class Leela: IDisposable
    {
 
        /// <summary>
        /// 执行程序名称
        /// </summary>
        public string FileName { get; set; } = @"D:\leela-zero-0.17-win64\leelaz.exe";

        /// <summary>
        /// 启动的线程数
        /// </summary>
        public int ThreadCount { get; set; } = 2;
        /// <summary>
        /// 权重文件路径
        /// </summary>
        public string WeigthFile { get; set; } = @"D:\leela-zero-0.17-win64\best-network";
        /// <summary>
        /// 计算深度
        /// </summary>
        public int Visits { get; set; } = 200;
        /// <summary>
        /// 工作文件夹啊
        /// </summary>
        public string WorkingDirectory { get; set; } = @"D:\leela-zero-0.17-win64";

        private Process processHandler;

        private string commandId;

        private bool done = false;

        public void Init() {
            processHandler = LeelaProcess();

        }

        private Process LeelaProcess()
        {

            Process process = new Process();

            process.StartInfo.FileName = FileName;

            process.StartInfo.WorkingDirectory = WorkingDirectory;

            process.StartInfo.Arguments = $@" -g -t {ThreadCount} -w {WeigthFile} -v {Visits}";

            process.StartInfo.UseShellExecute = false;

            process.StartInfo.Verb = "runas";

            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.RedirectStandardOutput = true;

            process.StartInfo.RedirectStandardError = true;

            process.StartInfo.RedirectStandardInput = true;

            process.ErrorDataReceived += OnErrorDataReceivedCallback;

            process.OutputDataReceived += OnOutputDataReceivedCallback;

            process.Start();

            process.BeginErrorReadLine();

            process.BeginOutputReadLine();

            return process;
        }

        private void OnOutputDataReceivedCallback(object sender, DataReceivedEventArgs e)
        {
            string message = e.Data;

            HandleMessage(message);
        }

        private void HandleMessage(string message)
        {
            Debug.Write(message);
        }

        private void OnErrorDataReceivedCallback(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine($"[Error] :{e.Data}");
        }

        public void ExecuteCommand(LeelaCommand command)         
        {
          //  if (!done) return;

          //  done = false;

            commandId = command.id.ToString();

            processHandler?.StandardInput.WriteLine($"{command}");

            //do
            //{

            //} while (!done);

        }

        public async Task ExecuteCommandTaskAsync(LeelaCommand command)
        {
            commandId = command.id;

            processHandler?.StandardInput.WriteLine($"{command}");

             await Task.Factory.StartNew(() => { });

        }

        public void Dispose()
        {
            processHandler?.Close();
        }
    }
}
