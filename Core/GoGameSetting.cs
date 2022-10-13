using Go.Interfaces;
using Go.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Core
{
    public class GoGameSetting
    {
        public ChessBoardType BoardSize { get; set; }

        public float Komi { get; set; }

        public TimeSpan Time { get; set; }   

    }
}
