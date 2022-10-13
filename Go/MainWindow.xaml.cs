using Go.Core;
using Go.Models;
using Go.Views.Panels;
using Go.ViewModels.Windows;
using Go.Views.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Go
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly MainWindowViewModel viewModel;

        public int BlockSize { get; private set; } 

        public MainWindow()
        {           
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            viewModel.OnStart += OnGameStartCallback;
            viewModel.StonePressed += OnStonePressedCallback;
            viewModel.StoneKilled += OnStoneKilledCallback;
            this.DataContext = viewModel;
        }

        private void OnStoneKilledCallback(object sender, IEnumerable<Stone> e)
        {
            chessboardPanle.RemoveStones(e);
        }

        private void OnStonePressedCallback(object sender, Stone e)
        {
            Debug.WriteLine($"落子：{e}");
            chessboardPanle.AddStone(e);
        }

        private void OnGameStartCallback(GoGameSetting setting)
        {
            chessboardPanle.InitChessboard(setting.BoardSize, 30);
        }

        private void OnClickBuildFightButton(object sender, RoutedEventArgs e)
        {
            gameSettingBorder.Visibility = Visibility.Visible;
        }

        private void OnClickGameStartButton(object sender, RoutedEventArgs e)
        {
            gameSettingBorder.Visibility = Visibility.Collapsed;
        }

        private void OnPressedChessboardCallback(object sender, RoutedPropertyChangedEventArgs<Vector2D> e)
        {
            Debug.WriteLine(e.NewValue);

            viewModel.PressCommand.Execute(e.NewValue);
        }

    }
}
