using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFPlayground
{
    public class StageDrawer
    {
        private Stage _stage;
        public StageDrawer(Stage stage)
        {
            _stage = stage;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public void Draw(Canvas canvas, int x, int y, bool pulse)
        {
            TextBlock text = GetNewTextBlock();

            text.Text = _stage.Label;
            int start = 0;
            Canvas.SetLeft(text, x + start);
            Canvas.SetTop(text, y);

            var rect = new System.Windows.Shapes.Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);

            System.Windows.Media.Color color;
            switch (_stage.Result)
            {
                case StageResult.Error:
                    color = Colors.Red;
                    color.A = 160;
                    break;
                case StageResult.Warning:
                    color = Colors.Yellow;
                    color.A = 80;
                    break;
                case StageResult.AllGood:
                    color = Colors.LimeGreen;
                    color.A = 80;
                    break;
                case StageResult.None:
                    {
                        color = Colors.Blue;
                        if (_stage.IsRunning && pulse)
                        {
                            color.A = 80;
                        }
                        else
                        {
                            color.A = 50;
                        }
                        break;
                    }
            }
            rect.Fill = new SolidColorBrush(color);
            rect.Width = Width;
            rect.Height = Height;
            rect.RadiusX = 10;
            rect.RadiusY = 10;

            TextBlock resultText = GetNewTextBlock();
            resultText.Text = _stage.Time;
            Canvas.SetLeft(resultText, x);
            Canvas.SetTop(resultText, y + 5 + Height/2);

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y + 25);

            canvas.Children.Add(text);
            canvas.Children.Add(rect);
            canvas.Children.Add(resultText);
        }

        private TextBlock GetNewTextBlock()
        {
            TextBlock text = new TextBlock();
            text.Width = Width;
            text.FontFamily = new FontFamily("Century Gothic");
            text.FontSize = 12;
            text.FontStretch = FontStretches.UltraExpanded;
            text.FontStyle = FontStyles.Italic;
            text.FontWeight = FontWeights.UltraBold;


            text.LineHeight = Double.NaN;
            text.Padding = new Thickness(5, 10, 5, 10);
            text.TextAlignment = TextAlignment.Center;
            text.TextWrapping = TextWrapping.NoWrap;

            text.Typography.NumeralStyle = FontNumeralStyle.OldStyle;
            text.Typography.SlashedZero = true;

            return text;
        }
    }
}
