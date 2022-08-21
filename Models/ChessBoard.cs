using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Models
{
    public enum ChessBoardMode { 
        None,
        Size_9x9    =9 ,
        Size_13x13  =13,
        Size_19x19  =19
    }

    public class ChessBoard
    {
        public ChessBoardMode Mode { get; private set; }
        public StoneColor[,] Board { get;private set; }

        public int Size { get; private set; }

        public ChessBoard(ChessBoardMode sizeMode)
        {
            Mode = sizeMode;
            Size = (int)sizeMode;
            Board = new StoneColor[Size, Size];
        }

        public StoneColor GetChessColor(int x, int y) {
            return Board[x, y];


        }

    }

}
 