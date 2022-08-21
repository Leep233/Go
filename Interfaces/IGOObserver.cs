using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Interfaces
{
    public interface IGOObserver<T_Stone>
    {

        event Action<List<T_Stone>> OnStoneKilledEvent;

        event Action<T_Stone> OnMoveEvent;
        /// <summary>
        /// 分先
        /// </summary>
        /// <returns></returns>
        int EvenGame();

        bool CanMove(T_Stone stone);

        bool ExistsInBlock(T_Stone stone, List<T_Stone> block);

        List<T_Stone> FindSameBlock(T_Stone stone);

        List<List<T_Stone>> FindDifferentBlocks(T_Stone stone);

        bool IsAlive(List<T_Stone> stones);

        bool ExistLiberty(T_Stone stone);

        bool Move(T_Stone chessPieces);
    }
}
