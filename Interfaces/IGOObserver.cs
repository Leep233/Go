using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Interfaces
{
    public interface IGOObserver<T_Stone>
    {

        event Action<IEnumerable<T_Stone>> StoneKilledEvent;

        event Action<T_Stone> PressedEvent;
        /// <summary>
        /// 分先
        /// </summary>
        /// <returns></returns>
        int EvenGame();

       // bool Pressable(T_Stone stone);

        bool ExistsInBlock(T_Stone stone, IEnumerable<T_Stone> block);

        List<T_Stone> FindSameBlock(T_Stone stone);

        List<List<T_Stone>> FindDifferentBlocks(T_Stone stone);

        bool IsAlive(IEnumerable<T_Stone> stones);

        bool ExistLibertyFromAround(T_Stone stone);

        bool Press(T_Stone chessPieces);
    }
}
