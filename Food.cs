using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SnakeGame
{
    public delegate void FoodEatedHandler();

    public class Food
    {
        private int x;
        private int y;
        public FoodEatedHandler onFoodEated;

        public int X
        {
            set
            {
                this.x = value;
                Canvas.SetLeft(this.rectangle, this.x * Config.SquareSize);
            }
            get => this.x;
        }

        public int Y
        {
            set
            {
                this.y = value;
                Canvas.SetTop(this.rectangle, this.y * Config.SquareSize);
            }
            get => this.y;
        }

        public int score { get; }
        public Rectangle rectangle { get; }

        public Food(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.score = Data.FoodScore;
            this.rectangle = new Rectangle
            {
                Width = Config.SquareSize,
                Height = Config.SquareSize,
                Fill = Config.FoodColor
            };
            Canvas.SetLeft(this.rectangle, this.x * Config.SquareSize);
            Canvas.SetTop(this.rectangle, this.y * Config.SquareSize);
        }

        public Food(int x, int y, int score) : this(x, y)
        {
            this.score = score;
        }

        public void eated()
        {
            this.onFoodEated();
        }
    }
}
