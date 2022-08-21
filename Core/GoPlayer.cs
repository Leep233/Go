using Go.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Core
{
    public class GoPlayer:Player
    {
        public event Func<Stone,bool> MoveEvent;

        public StoneColor StoneColor { get; set; } = StoneColor.White;

        public GoPlayer(string name)
        {
            Name = name;
        }

        public override bool Move(object arg) 
        {
            bool canMove = false;
            if (arg is Stone stone) 
            {
                stone.Color = StoneColor;
                canMove = MoveEvent?.Invoke(stone) ?? false;
            }
           
            return canMove;
        }

    }
}
