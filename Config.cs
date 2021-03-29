using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SnakeGame
{
    class Config
    {
        // 方格大小
        public static int SquareSize = 20;
        // 游戏地图的宽度（格子数）
        public static int MapCellWidth = 20;
        // 游戏地图的高度（格子数）
        public static int MapCellHeight = 20;
        // 蛇头颜色
        public static SolidColorBrush SnakeHeadColor = Brushes.Blue;
        // 蛇尾颜色
        public static SolidColorBrush SnakeBodyColor = Brushes.Green;
        // 蛇的初始长度
        public static int SnakeInitLength = 3;
        // 蛇的初始速度（>0）（越小越快）
        public static double SnakeInitSpeed = 0.3;
        // 食物的颜色
        public static SolidColorBrush FoodColor = Brushes.Yellow;
    }
}
