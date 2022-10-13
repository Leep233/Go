using Go.Interfaces;
using Go.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Go.Core
{
    public class LeelaPlayer : GoPlayer
    {
  
        private Leela leela = new Leela();

        public LeelaPlayer(string name, IGOObserver<Stone> judge) : base(name, judge)
        {
            leela.Init();
        }


        public LeelaPlayer(string name, StoneColor color): base(name, color)
        {
            leela.Init();
        }

        private void LeelaWriteCommand(LeelaCommand command) 
        {
           Debug.WriteLine($"命令:{command}");

            leela.ExecuteCommand(command);
        }

        private Stone LeelaAnalyzer() 
        {
            LeelaCommand command = new LeelaCommand("123", "lz-analyze", null);           

            LeelaWriteCommand(command);

            return new Stone() { Color= StoneColor ,Position = new Vector2D {x=1,y=3 } };
        }

        public override bool Press(object arg)
        {

            Vector2D position = (Vector2D)arg;

            Stone stone = new Stone(position, StoneColor);

           // bool isSucce = base.Press(stone);

            bool isSucce = base.Press(arg);

            LeelaCommand command = new LeelaCommand("123", "play", stone.ToString());

            LeelaWriteCommand(command);

            return isSucce;
        }

    }
}
