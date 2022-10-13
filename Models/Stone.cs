using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Models
{
    public enum StoneColor
    {
        None,
        Black,
        White
    }

    public class Stone
    {
        public StoneColor Color { get; set; }

        public Vector2D Position { get; set; }

        public Stone()
        {

        }

        public Stone(Vector2D position, StoneColor color)
        {
            Color = color;  
            Position = position;
        }
        public Stone(int x,int y, StoneColor color)
        {
            Color = color;
            Position = new Vector2D(x,y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Stone stone)
            {
                if (stone.Color != this.Color) return false;

                if (!this.Position.Equals(stone.Position)) return false;

                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string str = string.Empty;

            switch (Color)
            {

                case StoneColor.Black:
                    str = "B";
                    break;
                case StoneColor.White:
                    str = "W";
                    break;
                case StoneColor.None:
                default:
                    str = "pass";
                    break;
            }
            return $"{str} {Position}";
        }
    }
}
