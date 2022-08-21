using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Interfaces
{
   public interface IGOAnalysis<T_chessBoard>
    {
        int[,] Analysis(T_chessBoard chessBoard);
    }
}
