using Go.Interfaces;
using Go.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Core
{
    public class GoPlayer:Player
    {
        public StoneColor StoneColor { get; set; } = StoneColor.White;

        public GoPlayer(string name, IGOObserver<Stone> judge):base(judge)
        {
            Name = name;
        }

        public GoPlayer(string name, StoneColor color)
        {
            Name = name;
            StoneColor = color;
        }

        public override bool Press(object arg)
        {
            Vector2D position = (Vector2D)arg;

            Stone stone = new Stone(position, StoneColor);

            return base.Press(stone);
        }

    }


}
