using Go.Core;
using Go.Models;
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

        private readonly static SolidColorBrush ChessBGBrush = new SolidColorBrush(Color.FromArgb(1,1,1,1));

        private List<UIElement> stoneGrids = new List<UIElement>();

        private List<UIElement> forcesBoaders = new List<UIElement>();

        private bool forcesVisible = false;

        public int BlockSize { get; private set; } 

        private GoGame game;

        private Grid chessboardGrid;

        GoPlayer player;

        public MainWindow()
        {
            InitializeComponent();

           // Init();

           // this.Loaded += MainWindow_Loaded;
        }



        private void Init()
        {
            GoGameSetting setting = new GoGameSetting();

            setting.BoardSize = ChessBoardMode.Size_19x19;

            game = new GoGame(setting);

            player = new GoPlayer("玩家A");

            Action<Stone> onPlayerDelegate = new Action<Stone>(o => { });



            LeelaPlayer playerB = new LeelaPlayer();

            game.Players = new GoPlayer[GoGame.PlayerMaxCount] { player, playerB };

            game.OnMoveEvent += playerB.OnMovedDelegate;

            game.OnMoveEvent += OnPlayerMovedCallback;

            game.OnStoneKilledEvent += OnStoneKilledEventCallback;

            grid.Children.Add(CreateChessBorder(25, setting.BoardSize));
        }

        private void OnPlayerMovedCallback(Stone stone)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                FindStoneParentBorder(stone).Child = BoardDraw.BuildStoneItem(stone);
            }));
        }

        private void OnStoneKilledEventCallback(List<Stone> stones)
        {
            for (int i = 0; i < stones.Count; i++)
            {
                for (int j   = 0; j < stoneGrids.Count; j++)
                {
                    Border border = stoneGrids[j] as Border;
                    if (stones[i].Position.Equals(border.Tag))
                        border.Child = null;
                }              
            }         
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            game.Init();    

            game.Start();
        }

        private Grid CreateChessBorder(int size, ChessBoardMode mode)
        {
            BlockSize = size;
          
            chessboardGrid = new Grid();

            Image image = new Image() {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(-size, -size, 0, 0),
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri(@".\Resources\chessboard_bg.jpg", UriKind.RelativeOrAbsolute)),
            };

           chessboardGrid.Children.Add(image);

            int lineCount = (int)mode;

            int width = size * (lineCount - 1);          

            double halfSize = size * 0.5f;

            for (int i = 0; i < lineCount; i++)
            {
                int offset = i * size;

                TextBlock text = new TextBlock()
                {
                    Text = Char.ConvertFromUtf32(i + ((i > 7) ? 66 : 65)),
                    Margin = new Thickness(offset - halfSize + 2, -18, 0, 0)
                };

                chessboardGrid.Children.Add(text);

                chessboardGrid.Children.Add(BoardDraw.BuildLine(offset, offset, 0, width));
            }

            for (int i = 0; i < lineCount; i++)
            {
                int offset = i * size;

                TextBlock text = new TextBlock()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = size,
                    Height = size,
                    Text = (i + 1).ToString(),
                    Margin = new Thickness(-18, offset - halfSize + 2, 2, 2)
                };

                chessboardGrid.Children.Add(text);
                chessboardGrid.Children.Add(BoardDraw.BuildLine(0, width, offset, offset));
            }
            int ellipseSize = 8;

            int halfEllipseSize = ellipseSize >> 1;

            int bSize = size - 1;

            for (int x = 0; x < lineCount; x++)
            {
                for (int y = 0; y < lineCount; y++)
                {
                  

                    if ((x == 3 || x == 9 || x == 15) && (y == 3 || y == 9 || y == 15))
                    {
                        Thickness margin = new Thickness((x * size) - halfEllipseSize, (y * size) - halfEllipseSize, 3, 3);

                        Ellipse ellipse = BoardDraw.BuildEllipse(BoardDraw.BlackBrush, margin, ellipseSize);

                        ellipse.HorizontalAlignment = HorizontalAlignment.Left;

                        ellipse.VerticalAlignment = VerticalAlignment.Top;
                
                        chessboardGrid.Children.Add(ellipse);
                    }

                    Border border = new Border() {             
                        Tag = new Vector2D(x, y),
                        Background = ChessBGBrush,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Width = bSize,
                        Height = bSize,
                        Margin = new Thickness((x * size) - halfSize, (y * size) - halfSize, 3, 3)
                    };

                    border.MouseDown += OnClickPosition;

                    stoneGrids.Add(border);

                    chessboardGrid.Children.Add(border);

                }
            }

            double chessboardGridWidth = width + size;

            chessboardGrid.Width = chessboardGridWidth;

            chessboardGrid.Height = chessboardGridWidth;

            return chessboardGrid;
        }

        public Border FindStoneParentBorder(Stone stone) 
        {
            Vector2D position = stone.Position;

            int index = position.x * (int)(game.BoardSize) + position.y;

            return stoneGrids[index] as Border;
        }

        private void OnClickPosition(object sender, MouseButtonEventArgs e)
        {
            if (forcesVisible)
                ClearForcesBoarder();

            if (e.ChangedButton == MouseButton.Left) 
            {
                if (sender is Border parent) 
                {
                    Stone chessPieces = new Stone() { Position = (Vector2D)parent.Tag };

                    if (player.Move(chessPieces))
                    {
                       // parent.Child = BoardDraw.BuildStoneItem(chessPieces);

                    }
                    else
                    {
                        Debug.WriteLine($"您不能下在{parent.Tag.ToString()}");
                    }

                }

            }          
           
        }

        private void ClearForcesBoarder() {

            if (forcesBoaders is null || forcesBoaders.Count <= 0) return;
            for (int i = 0; i < forcesBoaders.Count; i++)
            {
                chessboardGrid.Children.Remove(forcesBoaders[i]);
            }
            forcesVisible = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (forcesVisible)
                ClearForcesBoarder();

            forcesVisible = true;

            int [,] index = game.GetAnalysisForces();

            int lineCount = (int)(game.BoardSize);

            int halfSize = BlockSize >> 1;

            for (int x = 0; x < lineCount; x++)
            {
                for (int y = 0; y < lineCount; y++)
                {
                    //|| Math.Abs(index[x, y])<200
                    if (index[x, y] == 0 ) continue;

                    Border border = BoardDraw.BuildAnalysisForcesBorder(BlockSize,index[x, y], new Vector2D(x, y), new Thickness((x * BlockSize) - halfSize, (y * BlockSize) - halfSize, 3, 3));
                   
                    forcesBoaders.Add(border);


                    chessboardGrid.Children.Add(border);

                }
            }
        }

        private void OnClickStartGameMenuItem(object sender, RoutedEventArgs e)
        {
            Init();

            game.Init();

            game.Start();
        }
    }
}
