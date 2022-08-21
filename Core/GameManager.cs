using Go.Interfaces;
using Go.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Go.Core
{
   //public class GameManager
   // {

   //     public event Action<int, ChessBoardMode> OnStart;

 
   //     private IGOObserver<Stone> _judge;

   //     private IGOAnalysis<ChessBoard> _situationAnalyzer;

   //     public event Action<List<Stone>> OnStonesKilled;

        
   //     public void Start(ChessBoardMode size)
   //     {
   //         _judge = new Judge(size);
   //         _situationAnalyzer = new SituationAnalyzer();
   //         int state = _judge.EvenGame();
   //         _judge.OnStoneKilledEvent += _judge_OnStoneKilledEvent;
   //         _judge.OnMoveEvent += _judge_OnMoveEvent;
   //         OnStart?.Invoke(state, size);          
   //     }

   //     public int[,] AnalysisForces => _situationAnalyzer.Analysis(((Judge)_judge).GetChessBoard);

   //     private void _judge_OnStoneKilledEvent(List<Stone> stones)
   //     {
   //         OnStonesKilled?.Invoke(stones);
   //     }

   //     private void _judge_OnMoveEvent(Stone stone)
   //     {
   //         Debug.WriteLine($"落子：{stone}");
   //     }

   //     public bool Move(Stone stone) {
   //        return _judge.Move(stone);
   //     }

   //     public void Stop() 
   //     { 
        
   //     }

   //     public void Over() 
   //     { 
        
   //     }



   // }
}
