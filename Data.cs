using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    class Data
    {
        // 玩家分数
        public static int Score = 0;
        // 当前一个食物的分数
        public static int FoodScore = 1;
        // 当前蛇移动的速度
        //public static double SnakeSpeed = Config.SnakeInitSpeed;

        public static string WindowTitle
        {
            get => "WPF 贪吃蛇：" + Score;
        }
    }
}
