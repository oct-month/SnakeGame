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
using System.Windows.Threading;

namespace SnakeGame
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // 蛇
        private Snake snake;
        // 食物
        private Food food;
        // 随机数生成器
        private Random random;

        public MainWindow()
        {
            InitializeComponent();
            Title = Data.WindowTitle;
            GameArea.Width = Config.MapCellWidth * Config.SquareSize;
            GameArea.Height = Config.MapCellHeight * Config.SquareSize;
            random = new Random();
        }

        /// <summary>
        /// 窗口内容加载后
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.DrawGameArea();
            this.CreateSnake();
            this.CreateFood();
            this.snake.start();
        }

        /// <summary>
        /// 绘制地图
        /// </summary>
        private void DrawGameArea()
        {
            int X = 0;
            int Y = 0;
            bool isOdd = true;

            while (true)
            {
                Rectangle rectange = new Rectangle
                {
                    Width = Config.SquareSize,
                    Height = Config.SquareSize,
                    Fill = isOdd ? Brushes.White : Brushes.LightGray
                };

                GameArea.Children.Add(rectange);
                Canvas.SetTop(rectange, Y);
                Canvas.SetLeft(rectange, X);

                isOdd = !isOdd;

                X += Config.SquareSize;
                if (X >= GameArea.ActualWidth)
                {
                    X = 0;
                    Y += Config.SquareSize;
                    isOdd = !isOdd;
                }
                if (Y >= GameArea.ActualHeight)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 创建蛇
        /// </summary>
        private void CreateSnake()
        {
            this.snake = new Snake(Config.MapCellWidth / 2, Config.MapCellHeight / 2, Config.SnakeInitLength);
            SnakeBody snakeBody = this.snake.tail;
            for (int i = 0; i < snake.length; i++)
            {
                GameArea.Children.Add(snakeBody.rectangle);
                snakeBody = snakeBody.prev;
            }
            this.snake.onSnakeTimerTick += this.onSnakeTimerTick;
            this.snake.onSnakeDead += this.onSnakeIsDead;
        }

        /// <summary>
        /// 创建一个食物
        /// </summary>
        private void CreateFood()
        {
            int x = random.Next(0, Config.MapCellWidth);
            int y = random.Next(0, Config.MapCellHeight);
            // TODO 生成的食物不应该和蛇重合
            this.food = new Food(x, y);
            GameArea.Children.Add(this.food.rectangle);
            this.food.onFoodEated += this.onFoodEated;
        }

        /// <summary>
        /// 键盘按键被按下事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void MainWindows_Keydown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    this.snake.direction = Direction.UP;
                    break;
                case Key.Down:
                case Key.S:
                    this.snake.direction = Direction.DOWN;
                    break;
                case Key.Left:
                case Key.A:
                    this.snake.direction = Direction.LEFT;
                    break;
                case Key.Right:
                case Key.D:
                    this.snake.direction = Direction.RIGHT;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 蛇的定时事件
        /// </summary>
        private void onSnakeTimerTick()
        {
            if (this.snake.isWillEated(this.food))
            {
                Rectangle newRec = this.snake.eat(this.food);
                GameArea.Children.Add(newRec);
            }
            else
                this.snake.move();
        }

        /// <summary>
        /// 蛇死亡事件
        /// </summary>
        private void onSnakeIsDead()
        {
            snake.stop();
        }

        /// <summary>
        /// 食物被吃事件
        /// </summary>
        private void onFoodEated()
        {
            GameArea.Children.Remove(this.food.rectangle);
            Data.Score += this.food.score;
            Title = Data.WindowTitle;
            //DataContext = Data.getData();
            this.CreateFood();
        }
    }
}
