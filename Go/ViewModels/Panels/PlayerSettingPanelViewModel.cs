using Go.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.ViewModels.Panels
{
    public class PlayerSettingPanelViewModel:BindableBase
    {
        private int selectedPlayerType=0;
        public int SelectedPlayerType
        {
            get { return selectedPlayerType; }
            set { selectedPlayerType = value; RaisePropertyChanged("SelectedPlayerType"); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged("Name"); }
        }

        private int selectedStone = 0;
        public int SelectedStone
        {
            get { return selectedStone; }
            set { selectedStone = value; RaisePropertyChanged("SelectedStone"); }
        }

  
        public StoneColor StoneColor
        {
            get { return (StoneColor)(SelectedStone + 1); }
            set { SelectedStone = (int)value -1; }
        }


        public PlayerSettingPanelViewModel()
        {

        }


        public PlayerSettingPanelViewModel(string name)
        {
            this.Name = name;
        }
    }
}
