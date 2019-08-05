using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankBlocks.org.enums;

namespace TankBlocks.org.entity
{
    public class Tank : Vehicle
    {
        [NonSerialized]
        private System.Windows.Forms.Timer timer;
        private Size size;
        private int velocity, incY, incX;
        private Rectangle[,] visual, visualClone;

        public delegate void OnShoot(Tank tank);
        public event OnShoot Shoot;

        public Direction CurrentDirection { get; private set; }
        public Direction LastDirection { get; private set; }
        public Rectangle[,] Visual { get => visual; set => visual = value; }
        public int Velocity { get => velocity; set => velocity = value; }
        public bool IsOnline { get; set; }


        public Tank()
        {
            this.size = new Size(8, 8);
            visual = new Rectangle[3, 3]
            {
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()}
            };

            visualClone = new Rectangle[3, 3]
            {
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()}
            };

            int sumX = 0;
            int sumY = 0;
            incX = size.Width;
            incY = size.Height;
            for (int x = 0; x < visual.GetLength(0); x++)
            {
                for (int y = 0; y < visual.GetLength(1); y++)
                {
                    visualClone[x, y].Size = size;
                    visualClone[x, y].Location = new Point(sumX, sumY);
                    if (!(x == 0 && y == 0) || !(x == 2 && y == 0) || !(x == 1 && y == 0))
                    {
                        visual[x, y].Size = size;
                        visual[x, y].Location = new Point(sumX, sumY);
                        if (y > 3) sumY = 0;
                        sumY += incY;
                    }
                }
                if (x > 3) sumX = 0;
                sumX += incX;
                sumY = 0;
            }
        }

        public bool SetDirection(Direction direction)
        {
            for (int x = 0; x < visualClone.GetLength(0); x++)
            {
                for (int y = 0; y < visualClone.GetLength(1); y++)
                {
                    visual[x, y].Location = visualClone[x, y].Location;
                    visual[x, y].Size = visualClone[x, y].Size;
                }
            }

            switch (direction)
            {
                case Direction.Left:
                    visual[0, 0] = new Rectangle();
                    visual[2, 1] = new Rectangle();
                    visual[0, 2] = new Rectangle();
                    break;
                case Direction.Up:
                    visual[0, 0] = new Rectangle();
                    visual[2, 0] = new Rectangle();
                    visual[1, 2] = new Rectangle();
                    break;
                case Direction.Right:
                    visual[2, 0] = new Rectangle();
                    visual[0, 1] = new Rectangle();
                    visual[2, 2] = new Rectangle();
                    break;
                case Direction.Down:
                    visual[1, 0] = new Rectangle();
                    visual[0, 2] = new Rectangle();
                    visual[2, 2] = new Rectangle();
                    break;
                default:
                    break;
            }
            return false;
        }
        public void SetPosition(Direction direction, int velocity)
        {
            this.CurrentDirection = direction;
            this.velocity = velocity;
            this.velocity = (direction == Direction.Down || direction == Direction.Up) && this.velocity <= 1 ? incY : this.velocity;
            this.velocity = (direction == Direction.Right || direction == Direction.Left) && this.velocity <= 1 ? incX : this.velocity;

            switch (direction)
            {
                case Direction.Left:
                    if (LastDirection != direction)
                    {
                        SetDirection(direction);
                        LastDirection = direction;
                        return;
                    }
                    LastDirection = direction;
                    for (int y = 0; y < visual.GetLength(1); y++)
                    {
                        for (int x = 0; x < visual.GetLength(0); x++)
                        {
                            visualClone[x, y].X -= this.velocity;
                        }
                    }
                    SetDirection(direction);
                    break;
                case Direction.Up:
                    if (LastDirection != direction)
                    {
                        if (SetDirection(direction)) return;
                        LastDirection = direction;
                        return;
                    }
                    LastDirection = direction;
                    for (int x = 0; x < visual.GetLength(0); x++)
                    {
                        for (int y = 0; y < visual.GetLength(1); y++)
                        {
                            visualClone[x, y].Y -= this.velocity;
                        }
                    }
                    SetDirection(direction);
                    break;
                case Direction.Right:
                    if (LastDirection != direction)
                    {
                        if (SetDirection(direction)) return;
                        LastDirection = direction;
                        return;
                    }
                    LastDirection = direction;
                    for (int y = 0; y < visual.GetLength(1); y++)
                    {
                        for (int x = 0; x < visual.GetLength(0); x++)
                        {
                            visualClone[x, y].X += this.velocity;
                        }
                    }
                    SetDirection(direction);
                    break;
                case Direction.Down:
                    if (LastDirection != direction)
                    {
                        SetDirection(direction);
                        LastDirection = direction;
                        return;
                    }
                    LastDirection = direction;
                    for (int x = 0; x < visual.GetLength(0); x++)
                    {
                        for (int y = 0; y < visual.GetLength(1); y++)
                        {
                            visualClone[x, y].Y += this.velocity;
                        }
                    }
                    SetDirection(direction);
                    break;
                default:
                    break;
            }
        }
        public void SetPosition(Direction direction)
        {
            this.CurrentDirection = direction;
            this.velocity = 2;

            switch (direction)
            {
                case Direction.Left:
                    if (LastDirection != direction)
                    {
                        SetDirection(direction);
                        LastDirection = direction;
                        return;
                    }
                    LastDirection = direction;
                    for (int y = 0; y < visual.GetLength(1); y++)
                    {
                        for (int x = 0; x < visual.GetLength(0); x++)
                        {
                            visualClone[x, y].X -= this.velocity;
                        }
                    }
                    SetDirection(direction);
                    break;
                case Direction.Up:
                    if (LastDirection != direction)
                    {
                        if (SetDirection(direction)) return;
                        LastDirection = direction;
                        return;
                    }
                    LastDirection = direction;
                    for (int x = 0; x < visual.GetLength(0); x++)
                    {
                        for (int y = 0; y < visual.GetLength(1); y++)
                        {
                            visualClone[x, y].Y -= this.velocity;
                        }
                    }
                    SetDirection(direction);
                    break;
                case Direction.Right:
                    if (LastDirection != direction)
                    {
                        if (SetDirection(direction)) return;
                        LastDirection = direction;
                        return;
                    }
                    LastDirection = direction;
                    for (int y = 0; y < visual.GetLength(1); y++)
                    {
                        for (int x = 0; x < visual.GetLength(0); x++)
                        {
                            visualClone[x, y].X += this.velocity;
                        }
                    }
                    SetDirection(direction);
                    break;
                case Direction.Down:
                    if (LastDirection != direction)
                    {
                        SetDirection(direction);
                        LastDirection = direction;
                        return;
                    }
                    LastDirection = direction;
                    for (int x = 0; x < visual.GetLength(0); x++)
                    {
                        for (int y = 0; y < visual.GetLength(1); y++)
                        {
                            visualClone[x, y].Y += this.velocity;
                        }
                    }
                    SetDirection(direction);
                    break;
                default:
                    break;
            }
        }

        public Rectangle[] GetRectangles()
        {
            return visual.Cast<Rectangle>().ToArray();
        }
    }
}
