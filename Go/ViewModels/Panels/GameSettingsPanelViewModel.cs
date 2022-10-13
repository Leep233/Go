using Go.Core;
using Go.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.ViewModels.Panels
{
    public class GameSettingsPanelViewModel : BindableBase
    {
        private int selectedTime = 0;
        public int SelectedTime
        {
            get { return selectedTime; }
            set { selectedTime = value; RaisePropertyChanged("SelectedTime"); }
        }

        private float komi = 7.5f;
        public float Komi
        {
            get { return komi; }
            set { komi = value; RaisePropertyChanged("Komi"); }
        }

        private int selectedSize = 2;
        public int SelectedSize
        {
            get { return selectedSize; }
            set { selectedSize = value; RaisePropertyChanged("SelectedSize"); }
        }

        public PlayerSettingPanelViewModel Player01 { get; set; }

        public PlayerSettingPanelViewModel Player02 { get; set; }
    
        public GoGameSetting Settings => new GoGameSetting()
        {
            BoardSize = GetBoardType(),
            Komi = Komi,
            Time = GetGameTime()
        };

        public GameSettingsPanelViewModel()
        {
            Player01 = new PlayerSettingPanelViewModel("玩家1") { StoneColor = StoneColor.Black };
            Player02 = new PlayerSettingPanelViewModel("玩家2") { StoneColor = StoneColor.White };
        }

        public ChessBoardType GetBoardType() {

            ChessBoardType boardSize = ChessBoardType.Size_19x19;

            switch (SelectedSize)
            {
                case 0:
                    boardSize = ChessBoardType.Size_9x9;
                    break;
                case 1:
                    boardSize = ChessBoardType.Size_13x13;
                    break;
                case 2:
                    boardSize = ChessBoardType.Size_19x19;
                    break;
            }

            return boardSize;
        }

        internal TimeSpan GetGameTime()
        {
            int time = 30;

            switch (SelectedTime)
            {
                case 0:
                    time = 30;
                    break;
                case 1:
                    time = 60;
                    break;
                case 2:
                    time = 120;
                    break;
            }

          return new TimeSpan(0, time, 0);

        }
    
        
    
    }
}
