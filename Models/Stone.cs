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



  public  class Stone
    {

        public StoneColor Color { get; set; }

        public Vector2D  Position { get; set; }

        public override bool Equals(object obj)
        {
            Stone s1= obj as Stone;


            if (s1.Color != this.Color) return false;

            if (!this.Position.Equals( s1.Position)) return false;

            return true;
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
