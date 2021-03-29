using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SnakeGame
{
    /// <summary>
    /// 移动方向
    /// </summary>
    public enum Direction
    {
        LEFT = 0,       // 左移
        UP = 1,         // 上移
        RIGHT = 2,      // 右移
        DOWN = 3        // 下移
    }

    public delegate void SnakeTimerTickHandler();
    public delegate void SnakeDeadEventHandler();

    class Snake
    {
        // 蛇头
        public SnakeBody head;
        // 蛇尾
        public SnakeBody tail;
        // 蛇长
        public int length;
        // 蛇死亡事件处理
        public SnakeDeadEventHandler onSnakeDead;
        // 蛇的定时事件
        public SnakeTimerTickHandler onSnakeTimerTick;

        private double speed;
        private DispatcherTimer timer;
        private Direction nowDirection;

        // 蛇当前的移动方向
        public Direction direction
        {
            set
            {
                if (this.nowDirection + 2 != value && this.nowDirection - 2 != value)
                    this.nowDirection = value;
            }
            get => this.nowDirection;
        }
        
        // 蛇的移动速度（越小越大）
        public double Speed
        {
            set
            {
                this.speed = value;
                this.timer.Interval = TimeSpan.FromSeconds(value);
            }
            get => this.speed;
        }

        /// <summary>
        /// 蛇
        /// </summary>
        /// <param name="x">蛇头位置X</param>
        /// <param name="y">蛇头位置Y</param>
        /// <param name="length">初始蛇长</param>
        public Snake(int x, int y, int length)
        {
            this.head = new SnakeBody(x, y, Config.SnakeHeadColor);
            SnakeBody bodyPre = head;
            for (int i = 1; i < length; i++)
            {
                SnakeBody body =  new SnakeBody(--x, y, Config.SnakeBodyColor, ref bodyPre);
                bodyPre = body;
            }
            this.tail = bodyPre;
            this.length = length;
            this.nowDirection = Direction.RIGHT;

            this.speed = Config.SnakeInitSpeed;
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1);
            this.timer.Tick += new EventHandler((Object sender, EventArgs e) => this.onSnakeTimerTick());
        }

        /// <summary>
        /// 蛇开始移动
        /// </summary>
        public void start()
        {
            this.timer.Start();
        }

        /// <summary>
        /// 蛇停止移动
        /// </summary>
        public void stop()
        {
            this.timer.Stop();
        }

        /// <summary>
        /// 移动蛇
        /// </summary>
        /// <param name="d">移动的方向</param>
        public void move()
        {
            // 移动蛇身
            SnakeBody b = this.tail;
            while (b != this.head)
            {
                b.move();
                b = b.prev;
            }
            // 移动蛇头
            switch (this.direction)
            {
                case Direction.LEFT:
                    this.head.X--;
                    break;
                case Direction.RIGHT:
                    this.head.X++;
                    break;
                case Direction.UP:
                    this.head.Y--;
                    break;
                case Direction.DOWN:
                    this.head.Y++;
                    break;
                default:
                    break;
            }
            // 判断蛇是否死亡
            if (this.isDead())
                this.onSnakeDead();
        }

        /// <summary>
        /// 蛇吃到了食物变长
        /// </summary>
        public Rectangle eat(Food food)
        {
            SnakeBody newHead = new SnakeBody(food.X, food.Y, Config.SnakeHeadColor);
            this.head.prev = newHead;
            this.head.Color = Config.SnakeBodyColor;
            this.head = newHead;
            this.length++;
            food.eated();
            return this.head.rectangle;
        }

        /// <summary>
        /// 判断蛇是否撞墙或者咬到自己
        /// </summary>
        /// <returns>true-dead false-safe</returns>
        public bool isDead()
        {
            SnakeBody head = this.head;
            SnakeBody body = this.tail;
            if (head.X < 0 || head.X >= Config.MapCellWidth || head.Y < 0 || head.Y >= Config.MapCellHeight)
                return true;
            while (body != head)
            {
                if (head.X == body.X && head.Y == body.Y)
                    return true;
                body = body.prev;
            }
            return false;
        }

        /// <summary>
        /// 判断蛇是否将要吃到食物
        /// </summary>
        /// <returns>true-eated false-no</returns>
        public bool isWillEated(Food food)
        {
            int x = this.head.X;
            int y = this.head.Y;
            switch (this.direction)
            {
                case Direction.LEFT:
                    x--;
                    break;
                case Direction.RIGHT:
                    x++;
                    break;
                case Direction.UP:
                    y--;
                    break;
                case Direction.DOWN:
                    y++;
                    break;
                default:
                    break;
            }
            if (food.X == x && food.Y == y)
                return true;
            return false;
        }
    }

    class SnakeBody
    {
        private int x;
        private int y;
        private SolidColorBrush color;

        // 蛇身的X位置
        public int X
        {
            set
            {
                this.x = value;
                Canvas.SetLeft(this.rectangle, this.x * Config.SquareSize);
            }
            get => this.x;
        }

        // 蛇身的Y位置
        public int Y
        {
            set
            {
                this.y = value;
                Canvas.SetTop(this.rectangle, this.y * Config.SquareSize);
            }
            get => this.y;
        }
        public SolidColorBrush Color
        {
            set
            {
                this.color = value;
                this.rectangle.Fill = this.color;
            }
            get => this.color;
        }
        public SnakeBody prev { set; get; }
        public Rectangle rectangle { get; }

        /// <summary>
        /// 蛇身
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="color">颜色</param>
        public SnakeBody(int x, int y, SolidColorBrush color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            this.rectangle = new Rectangle
            {
                Width = Config.SquareSize,
                Height = Config.SquareSize,
                Fill = color
            };
            this.prev = null;
            Canvas.SetLeft(this.rectangle, this.x * Config.SquareSize);
            Canvas.SetTop(this.rectangle, this.y * Config.SquareSize);
        }

        /// <summary>
        /// 蛇身
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="prev">前一个蛇身</param>
        /// <param name="color">颜色</param>
        public SnakeBody(int x, int y, SolidColorBrush color, ref SnakeBody prev) : this(x, y, color)
        {
            this.prev = prev;
        }

        /// <summary>
        /// 本蛇身移动到前一个蛇身的位置
        /// </summary>
        public void move()
        {
            this.X = this.prev.X;
            this.Y = this.prev.Y;
        }
    }
}
