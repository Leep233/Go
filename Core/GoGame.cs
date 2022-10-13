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

        public ChessBoardType BoardSize => Setting?.BoardSize ?? ChessBoardType.None;

        public event Action<IEnumerable<Stone>> StoneKilledEvent;

        public event Action<Stone> PressedEvent;

        public IGOAnalysis<ChessBoard> SituationAnalyzer { get; private set; }

        public IGOObserver<Stone> Judge { get; set; }

        private GoPlayer[] Players;

        public void SetPlayers(params GoPlayer[] players)
        {
            Players = players;

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].Judge = Judge;
            }
        }

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
            return SituationAnalyzer.Analysis(((Judge)Judge).ChessBoard);
        }

        /// <summary>
        /// 游戏初始化
        /// </summary>
        /// <param name="setting"></param>
        /// <exception cref="Exception"></exception>
        public void Init(GoGameSetting setting = null, GoPlayer [] players = null)
        {
            if (setting != null)
                Setting = setting;

            if (Setting is null) throw new Exception("Game Setting is Empty!");

          //  if (Players is null || Players.Length != PlayerMaxCount) throw new Exception("Player's Count Error!");

            Judge = new Judge(Setting.BoardSize);

            SetPlayers(players);

            SituationAnalyzer = new SituationAnalyzer();

            Judge.StoneKilledEvent += OnStoneKilledEventCallback;

            Judge.PressedEvent += OnMoveEventCallback;

        }

        private void OnMoveEventCallback(Stone stone)
        {
            PressedEvent?.Invoke(stone);
        }

        private void OnStoneKilledEventCallback(IEnumerable<Stone> stones)
        {
            StoneKilledEvent?.Invoke(stones);
        }

        public override void Start()
        {
            //int state = Judge.EvenGame();

            //Players[state].StoneColor = StoneColor.Black;

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
