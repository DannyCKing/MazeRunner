using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Maze.Model
{
    class MazeBoardModel
    {
        public Node[,] board
        {
            get;
            private set;
        }

        public readonly int Width;
        public readonly int Height;

        Random random;

        Node Start;
        Node End;

        List<Node> unvisited;

        public delegate void MyFunction(int i, int j);

        MyFunction myfunction;

        #region Constructors

        public MazeBoardModel(int width, int height, MyFunction function )
        {
            Contract.Requires(width > 0 && height > 0);

            unvisited = new List<Node>();
            random = new Random(DateTime.Now.Millisecond);

            if (width % 2 == 0)
            {
                width++;
            }

            if (height % 2 == 0)
            {
                height++;
            }
            Width = width;
            Height = height;

            myfunction = function;
          
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            board = new Node[Width, Height];
            for(int i=0; i<Width; i++)
            {
                for(int j=0; j<Height;j++)
                {
                    board[i,j] = new Node(i,j, "Node" + i + j);
                    unvisited.Add(board[i, j]);
                }
            }

            board[0, 0].IsStart = true;
            board[Width - 1, Height - 1].IsEnd = true;

            AddPaths();

        }

		private bool _HasBegun = false;
        public void Begin()
        {
			if (!_HasBegun)
			{
				_HasBegun = true;
				BeginMaze();
			}
		}

        private void BeginMaze()
        {
            DFS();
            //using dijkstra's!
            
            //for (int i = 0; i < Width; i++)
            //{
            //    for (int j = 0; j < Height; j++)
            //    {
            //        if (!board[i, j].IsStart)
            //        {
            //            board[i, j].Dist = int.MaxValue;
            //            board[i, j].Prev = null;
            //        }
            //        else
            //        {
            //            board[i, j].Dist = 0;
            //        }
            //    }
            //}




        }

        public void DFS()
        {
			//clear all visited
			foreach (var node in board)
			{
				node.Visited = false;
			}

            Stack<Node> stack = new Stack<Node>();
            stack.Push(board[0, 0]);
            Node v = new Node(0, 0, "");
            while (stack.Count > 0 && !v.IsEnd)
            {
                v = stack.Pop();
                if (!v.Visited)
                {
                    Thread.Sleep(5);
                    myfunction(v.Location.X, v.Location.Y);
                    v.Visited = true;
                    foreach (Node n in v.GetAllNeighbors())
                    {
                        stack.Push(n);
                    }
                }
            }
        }

        private void AddPaths()
        {
            Stack<Node> stack = new Stack<Node>();

            Node currentNode = board[0, 0];
            while (unvisited.Count > 0 )
            {
                if (HasUnvisitedNeighboor(currentNode))
                {
                    Direction direction;
                    Node chosen = GetRandomNeighboor(currentNode, out direction);
                    stack.Push(currentNode);
                    currentNode.SetNode(direction, chosen);
                    currentNode = chosen;
                    currentNode.Visited = true;
                    unvisited.Remove(currentNode);
                }
                else if (stack.Count > 0)
                {
                    currentNode = stack.Pop();
                }
                else
                {
                    currentNode = unvisited[random.Next(unvisited.Count)];
                    currentNode.Visited = true;
                    unvisited.Remove(currentNode);
                }
            }
        }

        private bool HasUnvisitedNeighboor(Node currentNode)
        {
            Node n = null;
            foreach (Direction dir in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                int x = 0;
                int y = 0;
                try
                {
                    switch (dir)
                    {
                        case Direction.Up:
                            x = currentNode.Location.X;
                            y = currentNode.Location.Y - 1;
                            break;
                        case Direction.Right:
                            x = currentNode.Location.X + 1;
                            y = currentNode.Location.Y;
                            break;
                        case Direction.Down:
                            x = currentNode.Location.X;
                            y = currentNode.Location.Y + 1;
                            break;
                        case Direction.Left:
                            x = currentNode.Location.X - 1;
                            y = currentNode.Location.Y;
                            break;
                    }
                    if (board[x, y].Visited == false)
                    {
                        return true;
                    };
                }
                catch
                {
                    continue;
                }

            }
            return false;
        }

        private Node GetRandomNeighboor(Node currentNode, out Direction direction)
        {
            Node n = null;
            direction = Direction.Up;
            List<Direction> directions = DirectionUtils.GetDirectionsInRandomOrder();
            foreach (Direction dir in directions)
            {
                direction = dir;
                try
                {
                    switch (dir)
                    {
                        case Direction.Up:
                            n = board[currentNode.Location.X, currentNode.Location.Y - 1];
                            break;
                        case Direction.Right:
                            n = board[currentNode.Location.X + 1, currentNode.Location.Y];
                            break;
                        case Direction.Down:
                            n = board[currentNode.Location.X, currentNode.Location.Y + 1];
                            break;
                        case Direction.Left:
                            n = board[currentNode.Location.X - 1, currentNode.Location.Y];
                            break;
                    }
                }
                catch
                {
                    continue;
                }
                if (n != null && !n.Visited)
                {
                    break;
                }
            }
            return n;
        }

        #endregion

        /*

        #region IsPathMethods

        private bool IsPathFromStartToEnd()
        {
            Stack<SpaceItem> stack = new Stack<SpaceItem>();
            foreach (SpaceItem s in nodes)
            {
                s.Visited = false;
                s.Connected = false;
            }

            stack.Push(Start);
            while (stack.Count != 0)
            {
                SpaceItem v = stack.Pop();
                if (!v.Visited)
                {
                    v.Visited = true;
                    v.Connected = true;

                    if (board[v.Location.X, v.Location.Y].Equals(End))
                    {
                        return true;
                    }

                    List<Point> adj = GetAdjacentsNodes(v);
                    foreach (Point item in adj)
                    {
                        stack.Push(board[item.X, item.Y]);
                    }
                }
            }

            return false;
        }

        private bool IsPathFromXToY(SpaceItem x, SpaceItem y)
        {
            Stack<SpaceItem> stack = new Stack<SpaceItem>();
            foreach (SpaceItem s in nodes)
            {
                GetNode(s).Visited = false;
                s.Visited = false;
            }

            stack.Push(x);
            while (stack.Count != 0)
            {
                SpaceItem v = stack.Pop();
                if (!GetNode(v).Visited)
                {
                    GetNode(v).Visited = true;

                    if (board[v.Location.X, v.Location.Y].Equals(y))
                    {
                        return true;
                    }

                    List<Point> adj = GetAdjacentsNodes(v);
                    foreach (Point item in adj)
                    {
                        stack.Push(board[item.X, item.Y]);
                    }
                }
            }

            return false;
        }

        #endregion


        private void AddInPath()
        {
            List<SpaceItem> allNodes = new List<SpaceItem>(nodes);
            GetNode(Start).Connected = true;
            while (nodes.Count >  0)
            {
                foreach (SpaceItem temp in allNodes)
                {
                    GetNode(temp).Visited = false;
                }
                int loc = random.Next(nodes.Count);
                SpaceItem s = nodes[loc];
                ConnectNodeAt(s);
                nodes.RemoveAt(loc);
            }
        }

        private void ConnectNodeAt(SpaceItem s)
        {
            if (s.Connected == true)
            {
                return;
            }
            else
            {
                FindNodeToConnectTo(s);
            }
        }

        private void FindNodeToConnectTo(SpaceItem x)
        {
            x.Visited = true;
            SpaceItem y = null;
            if (x.Connected)
            {
                return;
            }
            else
            {
                List<Direction> directions = new List<Direction>(){Direction.West,Direction.South,Direction.North, Direction.East};
                while (directions.Count > 0 && x.Connected == false && y != null && !y.Visited)
                {
                    Direction d = directions[random.Next(directions.Count)];
                    directions.Remove(d);
                }
                if (y == null)
                {
                    return;
                }

                FindNodeToConnectTo(y);
            }
            ConnectNodetoNode(x, y);
        }

        private void ConnectNodetoNode(SpaceItem item1, SpaceItem item2)
        {
            int x = (item1.Location.X + item2.Location.X) / 2;
            int y = (item1.Location.Y + item2.Location.Y) / 2;

            board[x, y].Type = SpaceType.Edge;
            board[item1.Location.X, item1.Location.Y].Type = board[item2.Location.X, item2.Location.Y].Type;
        }

        #region GetMethods

        public SpaceItem GetNodeAtLocation(int x, int y)
        {
            return board[x, y];
        }

        public SpaceItem GetNode(SpaceItem s)
        {
            return GetNodeAtLocation(s.Location.X, s.Location.Y);
        }

        private SpaceItem GetNodeFromNode(SpaceItem s, Direction direction, SpaceType type)
        {
            int x = 0 , y = 0;
            int mult = 1;
            SpaceItem ret;
            if(type == SpaceType.Node)
            {
                mult = 2;
            }
            switch (direction)
            {
                case Direction.North:
                    y = -1;
                    break;
                case Direction.South:
                    y = 1;
                    break;
                case Direction.East:
                    x = -1;
                    break;
                case Direction.West:
                    x = 1;
                    break;
            }
            try
            {
                ret = board[s.Location.X + (x*mult), s.Location.Y + (y*mult)];
            }
            catch
            {
                return null;
            }
            return ret;
        }

        private SpaceItem GetRandomNodeFromNode(SpaceItem s, SpaceType type)
        {
            SpaceItem ret;

            int count = 0;
            do
            {
                if (count > 4)
                {
                    return null;
                }
                Direction d = GetRandomDirection();
                ret = GetNodeFromNode(s, d, type);
                count++;
            }while((ret == null || ret.Type != type || ret.Visited == true));
            
            return GetNode(ret);
        }

        private Direction GetRandomDirection()
        {
            switch(random.Next(4))
            {
                case 0:
                    return Direction.North;
                case 1:
                    return Direction.East;
                case 2:
                    return Direction.South;
                default:
                    return Direction.West;
            }
        }

        private List<Point> GetAdjacentsNodes(SpaceItem v)
        {
            List<Point> ret = new List<Point>();

            foreach (Direction dir in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                Point? point = GetRealNode(v,dir);
                if(point != null)
                {
                    ret.Add(new Point(point.Value.X, point.Value.Y));
                }
            }

            return ret;
        }

        private Point? GetRealNode(SpaceItem currentNode, Direction direction)
        {
            int x = 0, y = 0;
            switch(direction)
            {
                case Direction.North:
                    y = -1;
                    break;
                case Direction.South:
                    y = 1;
                    break;
                case Direction.East:
                    x = -1;
                    break;
                case Direction.West:
                    x = 1;
                    break;
            }

            try
            {
                SpaceItem edge = board[currentNode.Location.X + x, currentNode.Location.Y + y];
                SpaceItem otherNode = board[currentNode.Location.X + (2*x), currentNode.Location.Y + (2*y)];

                //add top
                if (edge.Type == SpaceType.Edge && otherNode.Type == SpaceType.Node && otherNode.Visited == false)
                {
                    return new Point(otherNode.Location.X,otherNode.Location.Y);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Set Spaces Initializers

        private SpaceItem SetSpaceType(int i, int j)
        {
            if (i % 2 == 1 && j % 2 == 1)
            {
                return new SpaceItem(i,j,SpaceType.Node);
            }
            else
            {
                return new SpaceItem(i,j,SpaceType.Wall);
            }
        }

        private bool IsWall(int i, int j)
        {
            if (IsWallException(i, j))
            {
                return false;
            }
            else if (i % 2 == 0 && j % 2 == 0)
            {
                return true;
            }
            else if (i == 0 || j == 0 || i == Width - 1 || j == Height - 1)
            {
                return true;
            }
            else if (i % 2 != 1 && j % 2 != 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsWallException(int i, int j)
        {
            if (i == Width - 2 && j == Height - 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        

        #endregion
         * */
    }
}
