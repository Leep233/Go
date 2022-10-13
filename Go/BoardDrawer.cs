using Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Go
{
    public static class BoardDrawer
    {
        public readonly static SolidColorBrush BlackBrush = new SolidColorBrush(Colors.Black);

        public readonly static SolidColorBrush BlackForcesBrush = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));

        public readonly static SolidColorBrush WhiteForcesBrush = new SolidColorBrush(Color.FromArgb(150, 255, 255, 255));

        public readonly static SolidColorBrush TransparentBrush = new SolidColorBrush(Colors.Transparent);
        public static Line BuildLine(double x1, double x2, double y1, double y2, double strokeThickness = 1.5)
        {
           return new Line() 
            {
               Fill = BlackBrush,
               Stroke = BlackBrush,
               StrokeThickness = strokeThickness,
               Y1 = y1,
               Y2 = y2,
               X1 = x1,              
               X2 = x2,
           };         
        }

        public static UIElement BuildStoneItem(Stone pieces)
        {

            string imgUrl = string.Empty;

            switch (pieces.Color)
            {
                case StoneColor.Black:
                    imgUrl = @".\Resources\black.png";
                    break;
                case StoneColor.White:
                    imgUrl = @".\Resources\white.png";
                    break;
            }

            // <Image Width="300" Height="300" Source="pack://application:,,,/Go.Resources;component/black.png"></Image>
            Image image = new Image()
            {
                Tag = pieces,
                HorizontalAlignment = HorizontalAlignment.Stretch,

                VerticalAlignment = VerticalAlignment.Stretch,

                Margin = new Thickness(1),

                Stretch = Stretch.Fill,

                Source = new BitmapImage(new Uri(imgUrl, UriKind.RelativeOrAbsolute))
            };

 
            return image;
        }

        public static Ellipse BuildEllipse(Brush color, Thickness thickness, int size = 5)
        {

            return new Ellipse()
            {
                Fill = color,
                Stroke = color,
                Width = size,
                Height = size,
                Margin = thickness

            };
         
        }

        public static Border BuildAnalysisForcesBorder(double size,int weight, Vector2D position, Thickness margin)
        {

            TextBlock textBlock = new TextBlock()
            {
                Text = weight.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
            };

            Border border = new Border()
            {
                CornerRadius = new CornerRadius(15),
                IsHitTestVisible = false,
                Tag = position,
                Background = weight > 0 ? BlackForcesBrush : WhiteForcesBrush,
                BorderBrush = TransparentBrush,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = size,
                Height = size,
                Margin = margin,
                Child = textBlock
            };


            return border;
        }
    }
}
