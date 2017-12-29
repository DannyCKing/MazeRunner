using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Maze.Model;

namespace Maze
{

    public class Node
    {
        #region Properties/Fields

        string NodeID; 

        public Point Location
        {
            get;
            private set;
        }

        public bool Visited;

        public bool Connected;

        public Node left;
        public Node right;
        public Node up;
        public Node down;

        public Node Prev;
        public int Dist;

        public bool IsStart = false;
        public bool IsEnd = false;

        #endregion

        #region Constructors

        public Node(int i, int j, string id)
        {
            this.NodeID = id; 
            Location = new Point(i, j);
            Connected = false;
            Visited = false;

            left = null;
            right = null;
            up = null;
            down = null;
        }

        public void SetNode(Direction direction, Node node)
        {
            switch (direction)
            {
                case Direction.Up:
                    up = node;
                    up.down = this;
                    break;
                case Direction.Left:
                    left = node;
                    left.right = this;
                    break;
                case Direction.Down:
                    down = node;
                    down.up = this;
                    break;
                case Direction.Right:
                    right = node;
                    right.left = this;
                    break;
            }
        }

        public bool HasUnvisitedNeighboors()
        {
            return (left!= null && left.Visited == false) || 
                (right != null && right.Visited == false) || 
                (up != null && up.Visited == false) || 
                (down!= null && down.Visited == false);
        }


        #endregion

        public List<Node> GetAllNeighbors()
        {
            List<Node> nodes = new List<Node>();
            if (right != null && !right.Visited)
            {
                nodes.Add(right);
            }
            if (down != null && !down.Visited)
            {
                nodes.Add(down);
            }
            if (up != null && !up.Visited)
            {
                nodes.Add(up);
            }
            if (left != null && !left.Visited)
            {
                nodes.Add(left);
            }
            return nodes;
        }
    }
}
