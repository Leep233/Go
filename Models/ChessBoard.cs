using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Models
{
    public enum ChessBoardType { 
        None,
        Size_9x9    =9 ,
        Size_13x13  =13,
        Size_19x19  =19
    }

    public class ChessBoard
    {
        /// <summary>
        /// 棋盘大小
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// 棋盘类型
        /// </summary>
        public ChessBoardType BoardType { get; private set; }
        
        /// <summary>
        /// 棋盘落子点
        /// </summary>
        public StoneColor[,] Stones { get;private set; }

        public ChessBoard(ChessBoardType type)
        {
            BoardType = type;
            Size = (int)type;
            Stones = new StoneColor[Size, Size];
        }

        public StoneColor GetStoneColor(Vector2D vector) {
            return Stones[vector.x, vector.y];
        }
    }

}
 