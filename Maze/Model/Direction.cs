using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Model
{
    public enum Direction
    {
        Up, Right, Down, Left
    }

    public static class DirectionUtils
    {
        static Random rng = new Random(DateTime.Now.Millisecond);

        public static List<Direction> GetDirectionsInRandomOrder()
        {
            List<Direction> directions = new List<Direction>() { Direction.Up,Direction.Down, Direction.Right, Direction.Left };
            DirectionUtils.Shuffle(directions);
            return directions;
        }

        public static void Shuffle<Node>(this IList<Node> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(100);
                k = k / 10;
                if (k > 3)
                {
                    k = k % 2 + 2;
                }
                Node value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
