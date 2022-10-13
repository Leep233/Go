using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Models
{
   public class ChessStoneBlockCollection
    {
        public List<ChessStoneBlock> Blocks { get; set; }
        public ChessStoneBlockCollection()
        {
            Blocks = new List<ChessStoneBlock>();
        }
     
        public void Add(Stone chessPieces)
        {
            bool isExistsBlocks = false;

            for (int i = 0; i < Blocks.Count; i++)
            {
                if (Blocks[i].IsChild(chessPieces)) 
                { 
                //判断是否存在
                }
            }
            if (isExistsBlocks)
            {
                ChessStoneBlock block = new ChessStoneBlock();
                block.Add(chessPieces);
                Blocks.Add(block);
            }
        }
    }

    public class ChessStoneBlock 
    {
        public List<Stone> Stones { get; set; }

        public ChessStoneBlock()
        {
            Stones = new List<Stone>();
        }

        public bool IsChild(Stone chessPieces)
        {
            bool isChild = false;

            foreach (var item in Stones)
            {
                if (IsNear(item, chessPieces)) {
                    isChild = true;
                    break;
                }
            }
            return isChild;
        }
        private bool IsNear(Stone chess1, Stone chess2)
        {

            bool isNear = false;

            Vector2D v2 = chess2.Position;

            Vector2D v1 = new Vector2D(chess1.Position.x + 1, chess1.Position.y);

            if (v2.Equals(v1))
                return true;

            v1 = new Vector2D(chess1.Position.x - 1, chess1.Position.y);
            if (v2.Equals(v1))
                return true;

            v1 = new Vector2D(chess1.Position.x, chess1.Position.y - 1);
            if (v2.Equals(v1))
                return true;

            v1 = new Vector2D(chess1.Position.x, chess1.Position.y + 1);
            if (v2.Equals(v1))
                return true;

            return isNear;
        }
        public void Remove(Stone chessPieces) {
            Stones.Remove(chessPieces);
        }
        public void Add(Stone chessPieces)
        {
            Stones.Add(chessPieces);
        }
    }
}
