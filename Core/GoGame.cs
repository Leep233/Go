using Go.Interfaces;
using Go.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Core
{
    public class GoGame : Game<GoGameSetting>
    {
        public const int PlayerMaxCount = 2;

        public ChessBoardMode BoardSize => Setting?.BoardSize ?? ChessBoardMode.None;

        public event Action<List<Stone>> OnStoneKilledEvent;

        public event Action<Stone> OnMoveEvent;

        public IGOAnalysis<ChessBoard> SituationAnalyzer { get; private set; }

        public IGOObserver<Stone> Judge { get; set; }

        public GoPlayer[] Players { get; set; }



        public GoGame(GoGameSetting setting) : base(setting)
        {

        }

        public StoneColor GetCurrentStoneColor() {

            return ((Judge)Judge).CurrentColor;
        }

        public GoPlayer GetEnablePlayer() 
        {

            GoPlayer goPlayer = null;

            for (int i = 0; i < Players.Length; i++)
            { 
                if (Players[i].StoneColor == GetCurrentStoneColor()) 
                {
                    goPlayer = Players[i];
                    break;
                }
            }

            return goPlayer;
        }

        public GoGame() : base()
        {

        }

        /// <summary>
        /// 局势判断
        /// </summary>
        /// <returns></returns>
        public int[,] GetAnalysisForces()
        {
            return SituationAnalyzer.Analysis(((Judge)Judge).GetChessBoard);
        }

        /// <summary>
        /// 游戏初始化
        /// </summary>
        /// <param name="setting"></param>
        /// <exception cref="Exception"></exception>
        public void Init(GoGameSetting setting = null)
        {
            if (setting != null)
                Setting = setting;

            if (Setting is null) throw new Exception("Game Setting is Empty!");

            if (Players is null || Players.Length != PlayerMaxCount) throw new Exception("Player's Count Error!");

            Judge = new Judge(Setting.BoardSize);

            SituationAnalyzer = new SituationAnalyzer();

            Judge.OnStoneKilledEvent += OnStoneKilledEventCallback;

            Judge.OnMoveEvent += OnMoveEventCallback;

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].MoveEvent += Judge.Move;
            }

        }

        private void OnMoveEventCallback(Stone stone)
        {
            OnMoveEvent?.Invoke(stone);
        }

        private void OnStoneKilledEventCallback(List<Stone> stones)
        {
            OnStoneKilledEvent?.Invoke(stones);
        }

        public override void Start()
        {
            int state = Judge.EvenGame();

            Players[state].StoneColor = StoneColor.Black;

            Console.WriteLine("Start");

            base.Start();
        }

        public override void Pause()
        {
            Console.WriteLine("Pause");
            base.Pause();
        }
        public override void Over()
        {
            Console.WriteLine("Over");
            base.Over();
        }
    }
}
