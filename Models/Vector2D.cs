using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Models
{
    public struct Vector2D
    {
        public int x;
        public int y;
        public Vector2D(int x,int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            Vector2D v2 = (Vector2D)obj;
            return this.x == v2.x&& this.y == v2.y;
        }

        public override int GetHashCode()
        {          
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{char.ConvertFromUtf32(x + ((x > 7) ? 66 : 65))}{y+1}";
        }
    }
}
