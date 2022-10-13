using Go.Core;
using Go.Models;
using Go.ViewModels.Panels;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.ViewModels.Windows
{
    public class MainWindowViewModel:BindableBase
    {
        public event Action<GoGameSetting> OnStart;

        private GameSettingsPanelViewModel gameSettings;

        public GameSettingsPanelViewModel GameSettings
        {
            get { return gameSettings; }
            set { gameSettings = value;RaisePropertyChanged("GameSettings"); }
        }

        public DelegateCommand GameStartCommand { get; set; }

        public DelegateCommand<Vector2D?> PressCommand { get; set; }

        public GoGameSetting Settings { get; set; }

        private GoGame game;//{ get; set; }

        public event EventHandler<Stone> StonePressed;

        public event EventHandler<IEnumerable<Stone>> StoneKilled;

        public MainWindowViewModel()
        {
            GameSettings = new GameSettingsPanelViewModel();
            GameStartCommand = new DelegateCommand(ExecuteGameStartCommand);
            PressCommand = new DelegateCommand<Vector2D?>(ExecutePressCommand);
        }

        private void ExecutePressCommand(Vector2D? stone)
        {
            game.GetEnablePlayer().Press(stone);
        }

        private void ExecuteGameStartCommand()
        {

            Settings = GameSettings.Settings;
   
            game = new GoGame();

            GoPlayer[] players = new GoPlayer[2];

            players[0] = new GoPlayer(GameSettings.Player01.Name, GameSettings.Player01.StoneColor);
            players[1] = new LeelaPlayer(GameSettings.Player02.Name, GameSettings.Player02.StoneColor);// new GoPlayer(GameSettings.Player02.Name, GameSettings.Player02.StoneColor);

    

            game.Init(Settings,players);

         //  game.SetPlayers();

            game.Start();

            game.PressedEvent += OnPlayerMovedCallback;

            game.StoneKilledEvent += OnStoneKilledEventCallback;

            OnStart.Invoke(Settings);
        }

        private void OnStoneKilledEventCallback(IEnumerable<Stone> stones)
        {
            StoneKilled?.Invoke(this, stones);
        }

        private void OnPlayerMovedCallback(Stone stone)
        {
            StonePressed?.Invoke(this,stone);

        }
    }
}
