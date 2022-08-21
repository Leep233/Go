using System;
using System.Collections.Generic;
using System.Text;
using Go.Interfaces;
using Go.Models;

namespace Go.Core
{
    class SituationAnalyzer : IGOAnalysis<ChessBoard>
    {
  
        public SituationAnalyzer()
        {

        }


        public int[,] Analysis(ChessBoard chessBoard)
        {
            int[,] forces = new int[chessBoard.Size, chessBoard.Size];

            int[,] corners = new int[chessBoard.Size, chessBoard.Size];

            CornerForces(new Stone() { Position = new Vector2D(0, 0) }, ref corners);

            CornerForces(new Stone() { Position = new Vector2D(0, chessBoard.Size - 1) }, ref corners);

            CornerForces(new Stone() { Position = new Vector2D(chessBoard.Size - 1, 0) }, ref corners);

            CornerForces(new Stone() { Position = new Vector2D(chessBoard.Size - 1, chessBoard.Size - 1) }, ref corners);

            //  return corners;

            for (int i = 0; i < chessBoard.Size; i++)
            {
                for (int j = 0; j < chessBoard.Size; j++)
                {
                    if(chessBoard.Board[i, j] == StoneColor.Black || chessBoard.Board[i, j] == StoneColor.White)
                        AnlysisStoneForces(new Stone() {Position= new Vector2D(i,j),Color= chessBoard.Board[i, j] }, corners, ref forces);
                }
            }          

            return forces;
        }


        private void CornerForces(Stone stone,ref int [,]forces)
        {
            int size = forces.GetLength(0);

            int radius = (size >> 1)+2 ;

            int maxIntensity = 1<< radius;

            int value = maxIntensity >> 4;

            Vector2D pos = stone.Position;

            int symbol = 1;

            int x = pos.x;
            int y = pos.y;

            int intensity = 0;

            for (int i = 0; i < radius; i++)
            {
                for (int j = 0; j < radius-i; j++)
                {

                    intensity = maxIntensity >> (i + j);

                    //右下
                    x = pos.x + i;
                    y = pos.y + j;
                    if (x < size && y < size)
                    {
                        forces[x, y] = intensity * symbol;

                      //  forces[x, y] = maxIntensity * symbol;

                    }

                    //左下
                    x = pos.x - i;
                    y = pos.y + j;
                    if (x >= 0 && y < size)
                    {
                        forces[x, y] = intensity * symbol;
                    }

                    //右上
                    x = pos.x + i;
                    y = pos.y - j;
                    if (y >= 0 && x < size)
                    {
                        forces[x, y] = intensity * symbol;
                    }
                    //左上
                    x = pos.x - i;
                    y = pos.y - j;
                    if (x >= 0 && y >= 0)
                    {
                        forces[x, y] = intensity * symbol;
                    }

                }
            }
        }

        public void AnlysisStoneForces(Stone stone, int[,]corner, ref int[,] forces)
        {

            int size = forces.GetLength(0);

            int radius = (size >> 1);

            int maxIntensity = radius<<1;

            int value = maxIntensity >> 4;

            Vector2D pos = stone.Position;

            int symbol = 0;

            switch (stone.Color)
            {

                case StoneColor.Black:
                    symbol = 1;
                    break;
                case StoneColor.White:
                    symbol = -1;
                    break;
                case StoneColor.None:
                default:
                    symbol = 0;
                    break;
            }

            int x = pos.x;
            int y = pos.y;
            int intensity = 0;

            int dirL = x - 0;
            int dirU = y - 0;
            int dirR = size - x;
            int dirD = size - y;

            for (int i = 0; i < radius; i++)
            {
                for (int j = 0; j < radius; j++)
                {

                    intensity = maxIntensity >> (i + j);

                    //右下
                    x = pos.x + i;
                    y = pos.y + j;
                    if (x < size && y < size)
                    {
                        forces[x, y] += (int)(( dirL* 0.9 * intensity) * symbol);
                    }

                    //左下
                    x = pos.x - i;
                    y = pos.y + j;
                    if (x >= 0 && y < size)
                    {
                        forces[x, y] += (int)((dirR * 0.9 * intensity) * symbol);
                    }

                    //右上
                    x = pos.x + i;
                    y = pos.y - j;
                    if (y >= 0 && x < size)
                    {
                        forces[x, y] += (corner[x, y] + intensity) * symbol;
                    }
                    //左上
                    x = pos.x - i;
                    y = pos.y - j;
                    if (x >= 0 && y >= 0)
                    {
                        forces[x, y] += (corner[x, y] + intensity) * symbol;
                    }

                }
            }
        }

      
    }
}

