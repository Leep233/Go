using Go.Models;
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

namespace Go.Views.Panels
{
    /// <summary>
    /// ChessboardPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ChessboardPanel : UserControl
    {

        public readonly static RoutedEvent PressedEvent = EventManager.RegisterRoutedEvent("Pressed", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Vector2D>), typeof(ChessboardPanel));

        public event RoutedPropertyChangedEventHandler<Vector2D> Pressed {
            add { this.AddHandler(PressedEvent, value); }
            remove { this.RemoveHandler(PressedEvent, value); }
        }

        public static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Colors.Transparent);

        /// <summary>
        /// 棋盘每个方格的大小
        /// </summary>
        public int BlockSize { get; private set; }



        /// <summary>
        /// 星位显示像素
        /// </summary>
        public readonly int StarSize = 8;

        /// <summary>
        /// 星位像素一半
        /// </summary>
        public readonly float HalfStarSize = 8 * 0.5f;

        ///// <summary>
        ///// 棋盘坐标轴的偏移单位
        ///// </summary>
        //public readonly double AxisLabelOffset = -25;

        public readonly double LabelSize = 20;

        private AxisLabelCollection axisLabels;

        /// <summary>
        /// 棋子层
        /// </summary>
        private BorderLayerCollection StoneLayer;

        /// <summary>
        /// 势力判断层
        /// </summary>
        private BorderLayerCollection ForcesLayer;

        /// <summary>
        ///  预测层
        /// </summary>
        private BorderLayerCollection PredictLayer;

        public ChessBoardType BoardSize { get; set; }

        public ChessboardPanel(ChessBoardType mode, int size = 25)
        {
            InitializeComponent();

            InitChessboard(mode, size);
        }

        public ChessboardPanel()
        {
            InitializeComponent();
        }


        internal void RemoveStones(IEnumerable<Stone> stones)
        {
            foreach (var stone in stones)
            {
                RemoveStone(stone);
            }
        }

        public void InitChessboard(ChessBoardType mode, int size = 35)
        {
            BoardSize = mode;

            this.BlockSize = size;

            int lineCount = (int)mode;

            axisLabels = new AxisLabelCollection();

            StoneLayer = new StoneBorderLayerCollection(lineCount);

            ForcesLayer = new ForcesBorderLayerCollection(lineCount);

            PredictLayer = new PredictBorderLayerCollection(lineCount);

            int width = BlockSize * (lineCount - 1);

            double spacing = BlockSize * 0.5f;

            DrawVerticalAxisAndLabels(lineCount, spacing, width);

            DrawHorizontalAxisAndLabels(lineCount, spacing, width);

            double pressPointSize = BlockSize - 1.5;

            for (int x = 0; x < lineCount; x++)
            {
                for (int y = 0; y < lineCount; y++)
                {
                    DrawStar(x, y);

                    DrawPressPoint(x, y, pressPointSize, spacing, OnPressedCallback);
                }
            }

            double chessboardGridWidth = width + BlockSize;
        }

        private void OnPressedCallback(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && sender is Border parent)
            {
                Vector2D point = (Vector2D)parent.Tag;

                RoutedPropertyChangedEventArgs<Vector2D> eventArgs = new RoutedPropertyChangedEventArgs<Vector2D>(point, point, PressedEvent);

                this.RaiseEvent(eventArgs);
            }
        }

        public void AddStone(Stone stone)
        {
            StoneLayer?.AddBorderChild(stone);
        }

        public void RemoveStone(Stone stone)
        {
            StoneLayer?.RemoveBorderChild(stone.Position);
        }

        public void ClearStones() {
            StoneLayer?.ClearBorderChildren();
        }

        private void DrawPressPoint(int x, int y, double size, double spacing, Action<object, MouseButtonEventArgs> onPressedCallback)
        {
            double leftOffset = (x * BlockSize) - spacing;
            double topOffset= (y * BlockSize) - spacing;

            Border stoneBorder = new Border()
            {
                Tag = new Vector2D(x, y),
                Background = TransparentBrush,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = size,
                Height = size,
                Margin = new Thickness(leftOffset, topOffset, 3, 3)
            };
            stoneBorder.MouseDown += new MouseButtonEventHandler(onPressedCallback);
            StoneLayer.Add(stoneBorder);
            chessboardGrid.Children.Add(stoneBorder);

            Border forcesBorder = new Border()
            {
                Tag = new Vector2D(x, y),
                Background = TransparentBrush,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = size,
                Height = size,
                Margin = new Thickness(leftOffset, topOffset, 3, 3),
                IsHitTestVisible = false                
            };
            ForcesLayer.Add(forcesBorder);
            chessboardGrid.Children.Add(forcesBorder);

            Border predictBorder = new Border()
            {
                Tag = new Vector2D(x, y),
                Background = TransparentBrush,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = size,
                Height = size,
                Margin = new Thickness(leftOffset, topOffset, 3, 3),
                IsHitTestVisible = false
            };
            PredictLayer.Add(predictBorder);
            chessboardGrid.Children.Add(predictBorder);
        }

        /// <summary>
        /// 绘制水平轴和标签
        /// </summary>
        /// <param name="lineCount"></param>
        /// <param name="spacing"></param>
        /// <param name="width"></param>
        private void DrawHorizontalAxisAndLabels(int lineCount, double spacing, int width)
        {
            double fontSize = BlockSize * 0.35;

            for (int i = 0; i < lineCount; i++)
            {
                int offset = i * BlockSize;

                Label label = new Label()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = LabelSize,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    Padding = new Thickness(0),
                    FontSize = fontSize,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(-BlockSize - 10, offset - spacing + 8, 0, 0),
                    Content = (i + 1).ToString()
                };

                axisLabels.Add(label);

                chessboardGrid.Children.Add(label);

                chessboardGrid.Children.Add(BoardDrawer.BuildLine(0, width, offset, offset));
            }
        }

        /// <summary>
        /// 绘制垂直轴和标签
        /// </summary>
        /// <param name="lineCount"></param>
        /// <param name="spacing"></param>
        /// <param name="width"></param>
        private void DrawVerticalAxisAndLabels(int lineCount, double spacing, double width)
        {
            for (int i = 0; i < lineCount; i++)
            {
                int offset = i * BlockSize;

                Label label = new Label()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(offset - spacing + 8, -BlockSize - 10, 0, 0),
                    FontWeight = FontWeights.Bold,
                    FontSize = 12,
                    Content = Char.ConvertFromUtf32(i + ((i > 7) ? 66 : 65)),
                };

                axisLabels.Add(label);

                chessboardGrid.Children.Add(label);

                chessboardGrid.Children.Add(BoardDrawer.BuildLine(offset, offset, 0, width));
            }
        }

        /// <summary>
        /// 绘制星位点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="offset"></param>
        private void DrawStar(int x, int y)
        {

            if ((x == 3 || x == 9 || x == 15) && (y == 3 || y == 9 || y == 15))
            {
                float offset = HalfStarSize;

                Thickness margin = new Thickness((x * BlockSize) - offset, (y * BlockSize) - offset, 3, 3);

                Ellipse starPoint = BoardDrawer.BuildEllipse(BoardDrawer.BlackBrush, margin, StarSize);

                starPoint.HorizontalAlignment = HorizontalAlignment.Left;

                starPoint.VerticalAlignment = VerticalAlignment.Top;

                chessboardGrid.Children.Add(starPoint);
            }
        }

        internal class AxisLabelCollection : List<UIElement>
        {
            public virtual void Hide()
            {
                foreach (var item in this)
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }

            public virtual void Show()
            {
                foreach (var item in this)
                {
                    item.Visibility = Visibility.Visible;
                }
            }
        }


        public abstract class BorderLayerCollection : List<Border>
        {
            public int BoardSize { get; set; }

            public BorderLayerCollection(int boardSize)
            {
                BoardSize = boardSize;
            }

            /// <summary>
            /// 根据坐标获取Border
            /// </summary>
            /// <param name="stone"></param>
            /// <returns></returns>
            public Border FindBorderByPosition(Vector2D position)
            {
                int index = position.x * BoardSize + position.y;

                return this[index];
            }

            public virtual void AddBorderChild(object arg)
            {

            }

            public virtual void RemoveBorderChild(Vector2D position)
            {
                Border parent = FindBorderByPosition(position);
                parent.Child = null;
            }

            public virtual void ClearBorderChildren()
            {
                foreach (Border border in this)
                {
                    border.Child = null;
                }
            }
        }


        internal class ForcesBorderLayerCollection : BorderLayerCollection
        {
            public ForcesBorderLayerCollection(int boardSize) : base(boardSize)
            {
            }
        }

        internal class PredictBorderLayerCollection : BorderLayerCollection
        {
            public PredictBorderLayerCollection(int boardSize) : base(boardSize)
            {
            }

            public override void AddBorderChild(object arg)
            {
                Predict predict = (Predict)arg;

                Border parent = FindBorderByPosition(predict.posiction);

                parent.Child = new PredictItem(predict.rate);
            }
        }

        /// <summary>
        /// 下落点集合
        /// </summary>
        internal class StoneBorderLayerCollection : BorderLayerCollection
        {
 
            public StoneBorderLayerCollection(int boardSize):base(boardSize)
            {
 
            }    

            public override void AddBorderChild(object arg)
            {
                Stone stone = arg as Stone;

                Border parent = FindBorderByPosition(stone.Position);

                parent.Child = new StoneItem(stone);
            }          
           
        }

    }
}
