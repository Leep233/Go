using Go.Interfaces;
using Go.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Go.Core
{
    public class Judge : IGOObserver<Stone>
    {
        /// <summary>
        /// 当前需要下的棋子颜色
        /// </summary>
        public StoneColor CurrentColor { get; private set; }

        private bool isPassed = false;

        public event Action<List<Stone>> OnStoneKilledEvent;

        public event Action<Stone> OnMoveEvent;

        private ChessBoard _chessBoard;
        public Judge(ChessBoardMode size)
        {
            _chessBoard = new ChessBoard(size);
        }

        public ChessBoard GetChessBoard => _chessBoard;

        public bool CanMove(Stone stone)
        {
            bool canMove = false;

            if (isPassed && stone.Color != CurrentColor)
            {
                return canMove;
            }

            Vector2D pos = stone.Position;

            int x = pos.x - 1;
            int y = pos.y;

            if (x >= 0)
            {
                StoneColor color = _chessBoard.Board[x, y];
                if (color == StoneColor.None || color == stone.Color) return true;
            }
            x = pos.x + 1;
            if (x < _chessBoard.Size)
            {
                StoneColor color = _chessBoard.Board[x, y];
                if (color == StoneColor.None || color == stone.Color) return true;
            }
            x = pos.x;
            y = pos.y - 1;
            if (y >= 0)
            {
                StoneColor color = _chessBoard.Board[x, y];
                if (color == StoneColor.None || color == stone.Color) return true;
            }
            y = pos.y + 1;
            if (y < _chessBoard.Size)
            {
                StoneColor color = _chessBoard.Board[x, y];
                if (color == StoneColor.None || color == stone.Color) return true;
            }

            isPassed = false;

            return canMove;
        }

        public int EvenGame()
        {
            CurrentColor = StoneColor.Black;
            Random random = new Random();
            return random.Next(0, 1);
        }

        public bool ExistLiberty(Stone stone)
        {
            int x = stone.Position.x + 1;
            int y = stone.Position.y;

            if (x < _chessBoard.Size)
            {
                StoneColor color = _chessBoard.GetChessColor(x, y);
                if (color == StoneColor.None)
                    return true;
            }

            x = stone.Position.x - 1;

            if (x >= 0)
            {
                StoneColor color = _chessBoard.GetChessColor(x, y);
                if (color == StoneColor.None)
                    return true;
            }

            x = stone.Position.x;
            y = stone.Position.y + 1;

            if (y < _chessBoard.Size)
            {
                StoneColor color = _chessBoard.GetChessColor(x, y);
                if (color == StoneColor.None)
                    return true;
            }

            y = stone.Position.y - 1;

            if (y >= 0)
            {
                StoneColor color = _chessBoard.GetChessColor(x, y);
                if (color == StoneColor.None)
                    return true;
            }
            return false;
        }

        public bool ExistsInBlock(Stone stone, List<Stone> block)
        {
            return block.Contains(stone);
        }

        public List<List<Stone>> FindDifferentBlocks(Stone startStone)
        {
            List<List<Stone>> blocks = new List<List<Stone>>();

            List<Stone> stones = new List<Stone>() { };

            StoneColor color = startStone.Color;

            switch (color)
            {
                case StoneColor.Black:
                    color = StoneColor.White;
                    break;
                case StoneColor.White:
                    color = StoneColor.Black;
                    break;
            }

            Vector2D pos = startStone.Position;

            int x = pos.x - 1;

            int y = pos.y;

            if (x >= 0)
            {
                if (_chessBoard.Board[x, y] == color)
                {
                    Stone stone = new Stone() { Position = new Vector2D(x, y), Color = color };
                    stones.Add(stone);
                }
            }

            x = pos.x + 1;
            if (x < _chessBoard.Size)
            {
                if (_chessBoard.Board[x, y] == color)
                {
                    Stone stone = new Stone() { Position = new Vector2D(x, y), Color = color };
                    stones.Add(stone);
                }
            }
            x = pos.x;
            y = pos.y - 1;
            if (y >= 0)
            {
                if (_chessBoard.Board[x, y] == color)
                {
                    Stone stone = new Stone() { Position = new Vector2D(x, y), Color = color };
                    stones.Add(stone);
                }
            }
            y = pos.y + 1;
            if (y < _chessBoard.Size)
            {
                if (_chessBoard.Board[x, y] == color)
                {
                    Stone stone = new Stone() { Position = new Vector2D(x, y), Color = color };
                    stones.Add(stone);
                }
            }

            bool isExsits = false;

            for (int i = 0; i < stones.Count; i++)
            {
                List<Stone> chesses = FindSameBlock(stones[i]);

                if (chesses is null || chesses.Count <= 0) continue;

                Stone chess = chesses[0];

                isExsits = false;

                for (int j = 0; j < blocks.Count; j++)
                {
                    if (ExistsInBlock(chess, blocks[j]))
                    {
                        isExsits = true;
                        break;
                    }
                }
                if (!isExsits)
                    blocks.Add(chesses);
            }

            return blocks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startStone"></param>
        /// <returns></returns>
        public List<Stone> FindSameBlock(Stone startStone)
        {
            List<Stone> block = new List<Stone>() { startStone };

            Queue<Stone> sticker = new Queue<Stone>();

            sticker.Enqueue(startStone);

            int x = -1;
            int y = -1;

            do
            {

                Stone target = sticker.Dequeue();

                Vector2D pos = target.Position;

                x = pos.x - 1;

                y = pos.y;

                if (x >= 0)
                {
                    if (_chessBoard.Board[x, y] == target.Color)
                    {
                        Stone stone = new Stone() { Position = new Vector2D(x, y), Color = target.Color };

                        if (!ExistsInBlock(stone, block))
                        {
                            block.Add(stone);
                            sticker.Enqueue(stone);
                        }
                    }
                }

                x = pos.x + 1;
                if (x < _chessBoard.Size)
                {
                    if (_chessBoard.Board[x, y] == target.Color)
                    {
                        Stone stone = new Stone() { Position = new Vector2D(x, y), Color = target.Color };

                        if (!ExistsInBlock(stone, block))
                        {
                            block.Add(stone);
                            sticker.Enqueue(stone);
                        }
                    }
                }
                x = pos.x;
                y = pos.y - 1;
                if (y >= 0)
                {
                    if (_chessBoard.Board[x, y] == target.Color)
                    {
                        Stone stone = new Stone() { Position = new Vector2D(x, y), Color = target.Color };

                        if (!ExistsInBlock(stone, block))
                        {
                            block.Add(stone);
                            sticker.Enqueue(stone);
                        }
                    }
                }
                y = pos.y + 1;
                if (y < _chessBoard.Size)
                {
                    if (_chessBoard.Board[x, y] == target.Color)
                    {
                        Stone stone = new Stone() { Position = new Vector2D(x, y), Color = target.Color };

                        if (!ExistsInBlock(stone, block))
                        {
                            block.Add(stone);
                            sticker.Enqueue(stone);
                        }
                    }
                }



            } while (sticker.Count > 0);


            return block;
        }

        public bool IsAlive(List<Stone> stones)
        {
            bool isAlive = false;
            for (int i = 0; i < stones.Count; i++)
            {
                Stone chess = stones[i];
                if (ExistLiberty(chess)) return true;
            }
            return isAlive;
        }

        public bool Move(Stone chessPieces)
        {
            Vector2D pos = chessPieces.Position;

            bool isSucc = false;

            if (_chessBoard.Board[pos.x, pos.y] != StoneColor.None) return isSucc;

            if (!CanMove(chessPieces)) return false;

            _chessBoard.Board[pos.x, pos.y] = chessPieces.Color;

            List<Stone> block = FindSameBlock(chessPieces);

            isSucc = IsAlive(block);

            //Debug.WriteLine(isSucc);

            List<List<Stone>> blocks = FindDifferentBlocks(chessPieces);

            Debug.WriteLine($"作为敌人数：{blocks.Count}");

            int killCount = 0;

            List<Stone> killStones = new List<Stone>();

            for (int i = 0; i < blocks.Count; i++)
            {
                if (!IsAlive(blocks[i]))
                {
                    killCount++;
                    killStones.AddRange(blocks[i]);
                }
            }

            for (int i = 0; i < killStones.Count; i++)
            {
                Vector2D V2 = killStones[i].Position;

                _chessBoard.Board[V2.x, V2.y] = StoneColor.None;
            }

            if (killStones.Count > 0)
            {
                OnStoneKilledEvent?.Invoke(killStones);
                Debug.WriteLine($"杀死{killCount}块， 杀死敌人数：{killStones.Count}");
            }


            ////获取到对应颜色的棋子块集合
            //ChessPiecesBlockCollection stoneCollection = _chessesBlock[chessPieces.Color];

            //stoneCollection.Add(chessPieces);

            if (isSucc)
            {
                _chessBoard.Board[pos.x, pos.y] = chessPieces.Color;

                switch (chessPieces.Color)
                {
  
                    case StoneColor.Black:
                        CurrentColor = StoneColor.White;
                        break;
                    case StoneColor.White:
                        CurrentColor = StoneColor.Black;
                        break;    
                }             

                OnMoveEvent?.Invoke(chessPieces);
            }
            else
            {
                _chessBoard.Board[pos.x, pos.y] = StoneColor.None;
            }

            return isSucc;

        }
    }
}