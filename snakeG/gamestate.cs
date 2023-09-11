using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeG
{
    public class gamestate
    {
        public int Rows { get; }
        public int Col { get; }
        public gridV[,] Grid { get; }
        public Direction dirc { get; private set; }
        public int score { get; private set; }
        public bool gameover { get; private set; }


        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
        private readonly LinkedList<position> snakeposition = new LinkedList<position>();

        private readonly Random rand = new Random();

        public gamestate(int rows, int cols)
        {
            Rows = rows;
            Col = cols;
            Grid = new gridV[rows, cols];
            dirc = Direction.right;

            addSnake();
            addFood();
        }

        private void addSnake()
        {
            int r = Rows / 2;
            for(int i = 1; i <= 3; i++)
            {
                Grid[r, i] = gridV.Snake;
                snakeposition.AddFirst(new position(r, i));
            }
        }

        private IEnumerable<position> EmptyPosit()
        {

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    if (Grid[i, j] == gridV.Empty)
                    {
                        yield return new position(i, j);
                    }
                }
            }
        }

        private void addFood()
        {
            List<position> empty = new List<position>(EmptyPosit());
            if(empty.Count == 0)
            {
                return;
            }

            position pos = empty[rand.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = gridV.Food;
        }

        public position headPosition()
        {
            return snakeposition.First.Value;
        }

        public position tailPosition()
        {
            return snakeposition.Last.Value;
        }

        public IEnumerable<position> snakePosition()
        {
            return snakeposition;
        }

        private void addHead(position pos)
        {
            snakeposition.AddFirst(pos);
            Grid[pos.Row, pos.Col] = gridV.Snake;
        }

        private void removeTail()
        {
            position tail = snakeposition.Last.Value;
            Grid[tail.Row, tail.Col] = gridV.Empty;
            snakeposition.RemoveLast();
        }
        private Direction getLastDir()
        {
            if(dirChanges.Count == 0)
            {
                return dirc;
            }
            return dirChanges.Last.Value;
        }

        private bool canchangeDirection(Direction newDir)
        {
            if(dirChanges.Count == 2)
            {
                return false;
            }

            Direction lastDir = getLastDir();
            return newDir != lastDir && newDir != lastDir.opposite();
        }
        public void changeDirection(Direction dir)
        {
            if (canchangeDirection(dir))
            {
                dirChanges.AddLast(dir);
            }
            
        }

        private bool outsideGrid(position post)
        {
            return post.Row < 0 || post.Row >= Rows || post.Col < 0 || post.Col >= Col;
        }

        private gridV willHit(position newHead)
        {
            if (outsideGrid(newHead))
            {
                return gridV.Outside;
            }

            if(newHead == tailPosition())
            {
                return gridV.Empty;
            }
            return Grid[newHead.Row, newHead.Col];
        }

        public void move()
        {
            if(dirChanges.Count > 0)
            {
                dirc = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }

            position newHeadPost = headPosition().Translate(dirc);
            gridV hit = willHit(newHeadPost);

            if(hit == gridV.Outside || hit == gridV.Snake)
            {
                gameover = true;
            }
            else if(hit == gridV.Empty)
            {
                removeTail();
                addHead(newHeadPost);
            }
            else if(hit == gridV.Food)
            {
                addHead(newHeadPost);
                score++;
                addFood();
            }
        }
    }
}
