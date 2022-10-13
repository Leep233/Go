using Go.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Go.ViewModels.Components
{
    public class StoneItemViewModel:BindableBase
    {

        private ImageSource stoneMaterial;

        public ImageSource StoneMaterial
        {
            get { return stoneMaterial; }
            set { stoneMaterial = value;RaisePropertyChanged("StoneMaterial"); }
        }


        public readonly Stone stone;

        public StoneItemViewModel(Stone stone)
        {
            this.stone = stone;

            LoadStoneMaterial(stone.Color);
        }

        private void LoadStoneMaterial(StoneColor color) {

            string url = "";

            switch (color)
            {
                case StoneColor.Black:
                    url = Path.Combine(System.Environment.CurrentDirectory, @"Resources\black.png");
                    break;
                case StoneColor.White:
                    url = Path.Combine(System.Environment.CurrentDirectory, @"Resources\white.png");
                    break;
            }

            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            StoneMaterial = bitmap;

        }

    }
}
