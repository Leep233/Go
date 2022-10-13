using Go.Interfaces;
using Go.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Go.Core
{
    public class Judge : IGOObserver<Stone>
    {
        /// <summary>
        /// 当前需要下的棋子颜色
        /// </summary>
        public StoneColor CurrentColor { get; private set; }

        public event Action<IEnumerable<Stone>> StoneKilledEvent;

        public event Action<Stone> PressedEvent;
        public ChessBoard ChessBoard { get; private set; }
        public Judge(ChessBoardType size)
        {
            ChessBoard = new ChessBoard(size);
            CurrentColor = StoneColor.Black;
        }

        /// <summary>
        /// 判断四周是否存在相同的棋子
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="targetColor"></param>
        /// <returns></returns>
        public bool ExistSameColorFromAround(Vector2D pos, StoneColor targetColor)
        {
            return BorderDetection(pos, color => color == targetColor).state;

            //左边
            int x = pos.x - 1;
            int y = pos.y;
            if (x >= 0)
            {
                StoneColor color = ChessBoard.Stones[x, y];
                if (color == targetColor) return true;
            }

            x = pos.x + 1;
            //右边
            if (x < ChessBoard.Size)
            {
                StoneColor color = ChessBoard.Stones[x, y];
                if (color == targetColor) return true;
            }

            //下边
            x = pos.x;
            y = pos.y - 1;
            if (y >= 0)
            {
                StoneColor color = ChessBoard.Stones[x, y];
                if (color == targetColor) return true;
            }

            //上边
            y = pos.y + 1;
            if (y < ChessBoard.Size)
            {
                StoneColor color = ChessBoard.Stones[x, y];
                if (color == targetColor) return true;
            }
            return false;
        }

        /// <summary>
        /// 是否存在气
        /// </summary>
        /// <param name="stone"></param>
        /// <returns></returns>
        public bool ExistLibertyFromAround(Stone stone)
        {

            return BorderDetection(stone.Position, color => color == StoneColor.None).state;

            int x = stone.Position.x + 1;
            int y = stone.Position.y;

            if (x < ChessBoard.Size)
            {
                StoneColor color = ChessBoard.Stones[x, y];
                if (color == StoneColor.None)
                    return true;
            }

            x = stone.Position.x - 1;

            if (x >= 0)
            {
                StoneColor color = ChessBoard.Stones[x, y];
                if (color == StoneColor.None)
                    return true;
            }

            x = stone.Position.x;
            y = stone.Position.y + 1;

            if (y < ChessBoard.Size)
            {
                StoneColor color = ChessBoard.Stones[x, y];
                if (color == StoneColor.None)
                    return true;
            }

            y = stone.Position.y - 1;

            if (y >= 0)
            {
                StoneColor color = ChessBoard.Stones[x, y];
                if (color == StoneColor.None)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 边缘检测 （检测目标上下左右的情况）
        /// </summary>
        /// <param name="position">需要检测的坐标</param>
        /// <param name="conditions">检测的条件</param>
        /// <returns></returns>
        public (bool state,Vector2D point) BorderDetection(Vector2D position, Func<StoneColor, bool> conditions) 
        {
            //左边
            int x = position.x - 1;
            int y = position.y;

            if (x >= 0)
            {  
                if (conditions(ChessBoard.Stones[x, y])) return (true,new Vector2D(x,y));
            }

            x = position.x + 1;
            //右边
            if (x < ChessBoard.Size)
            {
                if (conditions(ChessBoard.Stones[x, y])) return (true, new Vector2D(x, y));
            }

            //下边
            x = position.x;
            y = position.y - 1;
            if (y >= 0)
            {
                if (conditions(ChessBoard.Stones[x, y])) return (true, new Vector2D(x, y));
            }

            //上边
            y = position.y + 1;
            if (y < ChessBoard.Size)
            {
                if (conditions(ChessBoard.Stones[x, y])) return  (true, new Vector2D(x, y));
            }
            return (false, new Vector2D(-1, -1));
        }


        /// <summary>
        /// 分先
        /// </summary>
        /// <returns></returns>
        public int EvenGame()
        {
            CurrentColor = StoneColor.Black;
            Random random = new Random();
            return random.Next(0, 1);
        }

 

        /// <summary>
        /// 是否属于一片棋
        /// </summary>
        /// <param name="stone"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public bool ExistsInBlock(Stone stone, IEnumerable<Stone> block)
        {
            return block.Contains(stone);
        }

        /// <summary>
        /// 寻找不同一片的棋
        /// </summary>
        /// <param name="startStone"></param>
        /// <returns></returns>
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

            Vector2D position = startStone.Position;

            //1.检测到相同颜色的棋子 就添加到列表内

            var detResult = BorderDetection(position, c => c == color);

            if (detResult.state) 
            {
                Stone stone = new Stone() { Position = detResult.point, Color = color };
                stones.Add(stone);
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
        /// 寻找同一片棋
        /// </summary>
        /// <param name="startStone"></param>
        /// <returns></returns>
        public List<Stone> FindSameBlock(Stone startStone)
        {
            List<Stone> block = new List<Stone>() {startStone};

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
                    if (ChessBoard.Stones[x, y] == target.Color)
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

                if (x < ChessBoard.Size)
                {
                    if (ChessBoard.Stones[x, y] == target.Color)
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
                    if (ChessBoard.Stones[x, y] == target.Color)
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
                if (y < ChessBoard.Size)
                {
                    if (ChessBoard.Stones[x, y] == target.Color)
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

        /// <summary>
        /// 是否是活棋
        /// </summary>
        /// <param name="stones"></param>
        /// <returns></returns>
        public virtual bool IsAlive(IEnumerable<Stone> stones)
        {
            bool isAlive = false;

            foreach (Stone stone in stones)
            {
                if (ExistLibertyFromAround(stone))
                {
                    isAlive = true;
                    break;
                }
            }

            return isAlive;
        }

        public virtual void OnStoneKilled(IEnumerable<Stone> killedStones, int blocks)
        {
            int killCount = 0;

            foreach (var stones in killedStones)
            {
                Vector2D V2 = stones.Position;

                ChessBoard.Stones[V2.x, V2.y] = StoneColor.None;

                killCount++;
            }

            Debug.WriteLine($"杀死{blocks}块， 杀死敌人数：{killCount}");

            StoneKilledEvent?.Invoke(killedStones);
        }

        /// <summary>
        /// 落子
        /// </summary>
        /// <param name="stone"></param>
        /// <returns>是否落子成功</returns>
        public bool Press(Stone stone)
        {
            bool pressable = false;

            Vector2D position = stone.Position;

            StoneColor stoneColor = stone.Color;

            if (stoneColor != CurrentColor) 
            {
                Debug.WriteLine($"不是 {stoneColor} 的回合");

                return pressable = false;
            }

            //1.坐标上 有子 不能下

            if (ChessBoard.GetStoneColor(position) != StoneColor.None)
            {
                Debug.WriteLine($"此处已有棋子:{position}");

                return pressable = false;
            }

            //2.判断4周是否存在气
            if (BorderDetection(position, color => color == StoneColor.None).state)
            {
                pressable = true;

            } else 
            {
                //3.判断四周是否有同色棋子
                if (!BorderDetection(position, color => color == stoneColor).state) 
                {
                    Debug.WriteLine("禁入点");

                    return pressable = false;
                }
            }


            //这里暂时将坐标位置 设置成棋子，便于后面的检测，如果检测不通过需要将这个棋子的位置 恢复原来的状态
            StoneColor stoneStatus = ChessBoard.Stones[position.x, position.y];

            ChessBoard.Stones[position.x, position.y] = stoneColor;

            //4.判断是否能杀死对方的棋子
            List<List<Stone>> enemyBlocks = FindDifferentBlocks(stone);

            int killedBlockNumber = 0;

            List<Stone> killedStones = new List<Stone>();

            for (int i = 0; i < enemyBlocks.Count; i++)
            {
                if (!IsAlive(enemyBlocks[i]))
                {
                    killedBlockNumber++;
                    killedStones.AddRange(enemyBlocks[i]);
                }
            }

            if (killedBlockNumber > 0) 
            {
                OnStoneKilled(killedStones, killedBlockNumber);
                pressable = true;

            } else 
            {
                //5.四周有自己的棋子 需要判断的如果下这个位置会不会导致这片棋一口气都没有（自杀）

                List<Stone> sameBlock = FindSameBlock(stone);

                pressable = IsAlive(sameBlock);
              
            }

            if (pressable)
            {
                ChessBoard.Stones[position.x, position.y] = stone.Color;

                switch (stone.Color)
                {

                    case StoneColor.Black:
                        CurrentColor = StoneColor.White;
                        break;
                    case StoneColor.White:
                        CurrentColor = StoneColor.Black;
                        break;
                }

                PressedEvent?.Invoke(stone);
            }
            else
            {
                ChessBoard.Stones[position.x, position.y] = StoneColor.None;
            }

            //////获取到对应颜色的棋子块集合
            //ChessStoneBlockCollection stoneCollection = _chessesBlock[Color];

            //stoneCollection.Add(chessPieces);


            return pressable;
        }
    }
}