using Go.Models;
using Go.ViewModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Go.Views.Components
{
    /// <summary>
    /// StoneItem.xaml 的交互逻辑
    /// </summary>
    public partial class StoneItem : UserControl
    {
        private bool deviator = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stone"></param>
        /// <param name="deviator">是否进行一定的棋子偏差</param>
        public StoneItem(Stone stone,bool deviator = true)
        {
            InitializeComponent();

            this.deviator = deviator;    

            this.DataContext = new StoneItemViewModel(stone);

            this.Loaded += OnLoadedCallback;
            
        }

        private void OnLoadedCallback(object sender, RoutedEventArgs e)
        {
            if (deviator)
                SetDeviator();
        }

        public void SetDeviator() {

            Random random = new Random();

            VerticalAlignment = VerticalAlignment.Center;

            HorizontalAlignment = HorizontalAlignment.Center;

            int scalar = (int)(this.ActualWidth * 0.07f);

            int verticalOffset = random.Next(-scalar, scalar);

            int horizontalOffset = random.Next(-scalar, scalar);      

            Margin = new Thickness(verticalOffset, horizontalOffset, -verticalOffset, -horizontalOffset);

        }
    }
}
