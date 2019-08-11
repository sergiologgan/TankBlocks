using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TankBlocks.org.enums;

namespace TankBlocks.org.entity
{
    public class Tank : Vehicle
    {
        [NonSerialized]
        private Size size;
        private int velocity, incY, incX;
        private Rectangle[,] rectTank, rectTankClone;
        private Rectangle clientRectangle;

        public Direction CurrentDirection { get; private set; }
        public Direction LastDirection { get; private set; }
        public Rectangle[,] Visual { get => rectTank; set => rectTank = value; }
        public int Velocity { get => velocity; set => velocity = value; }
        public bool IsOnline { get; set; }
        


        public Tank(Rectangle clientRectangle)
        {
            this.clientRectangle = clientRectangle;
            this.size = new Size(8, 8);
            this.rs = new Dictionary<string, List<Rectangle>>()
            {
                { "tank", new List<Rectangle>() },
                { "shot", new List<Rectangle>() },
            };
            rectTank = new Rectangle[3, 3]
            {
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()}
            };

            rectTankClone = new Rectangle[3, 3]
            {
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()}
            };

            int sumX = 0;
            int sumY = 0;
            incX = size.Width;
            incY = size.Height;
            for (int x = 0; x < rectTank.GetLength(0); x++)
            {
                for (int y = 0; y < rectTank.GetLength(1); y++)
                {
                    rectTankClone[x, y].Size = size;
                    rectTankClone[x, y].Location = new Point(sumX, sumY);
                    if (!(x == 0 && y == 0) || !(x == 2 && y == 0) || !(x == 1 && y == 0))
                    {
                        rectTank[x, y].Size = size;
                        rectTank[x, y].Location = new Point(sumX, sumY);
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
            if (!TankExceededLimit(rectTankClone))
            {
                for (int x = 0; x < rectTankClone.GetLength(0); x++)
                {
                    for (int y = 0; y < rectTankClone.GetLength(1); y++)
                    {
                        rectTank[x, y].Location = rectTankClone[x, y].Location;
                        rectTank[x, y].Size = rectTankClone[x, y].Size;
                    }
                }

                switch (direction)
                {
                    case Direction.Left:
                        rectTank[0, 0] = new Rectangle();
                        rectTank[2, 1] = new Rectangle();
                        rectTank[0, 2] = new Rectangle();
                        break;
                    case Direction.Up:
                        rectTank[0, 0] = new Rectangle();
                        rectTank[2, 0] = new Rectangle();
                        rectTank[1, 2] = new Rectangle();
                        break;
                    case Direction.Right:
                        rectTank[2, 0] = new Rectangle();
                        rectTank[0, 1] = new Rectangle();
                        rectTank[2, 2] = new Rectangle();
                        break;
                    case Direction.Down:
                        rectTank[1, 0] = new Rectangle();
                        rectTank[0, 2] = new Rectangle();
                        rectTank[2, 2] = new Rectangle();
                        break;
                    default:
                        break;
                }
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
                        {
                            LastDirection = direction;
                        }
                    }
                    else
                    {
                        LastDirection = direction;
                        for (int y = 0; y < rectTank.GetLength(1); y++)
                        {
                            for (int x = 0; x < rectTank.GetLength(0); x++)
                            {
                                rectTankClone[x, y].X -= this.velocity;
                            }
                        }
                        SetDirection(direction);
                    }
                    break;
                case Direction.Up:
                    if (LastDirection != direction)
                    {
                        if (SetDirection(direction))
                        {
                            LastDirection = direction;
                        }
                    }
                    else
                    {
                        LastDirection = direction;
                        for (int x = 0; x < rectTank.GetLength(0); x++)
                        {
                            for (int y = 0; y < rectTank.GetLength(1); y++)
                            {
                                rectTankClone[x, y].Y -= this.velocity;
                            }
                        }
                        SetDirection(direction);
                    }
                    break;
                case Direction.Right:
                    if (LastDirection != direction)
                    {
                        if (SetDirection(direction))
                        {
                            LastDirection = direction;
                        }
                    }
                    else
                    {
                        LastDirection = direction;
                        for (int y = 0; y < rectTank.GetLength(1); y++)
                        {
                            for (int x = 0; x < rectTank.GetLength(0); x++)
                            {
                                rectTankClone[x, y].X += this.velocity;
                            }
                        }
                        SetDirection(direction);
                    }
                    break;
                case Direction.Down:
                    if (LastDirection != direction)
                    {
                        SetDirection(direction);
                        {
                            LastDirection = direction;
                        }
                    }
                    else
                    {
                        LastDirection = direction;
                        for (int x = 0; x < rectTank.GetLength(0); x++)
                        {
                            for (int y = 0; y < rectTank.GetLength(1); y++)
                            {
                                rectTankClone[x, y].Y += this.velocity;
                            }
                        }
                        SetDirection(direction);
                    }
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
                    for (int y = 0; y < rectTank.GetLength(1); y++)
                    {
                        for (int x = 0; x < rectTank.GetLength(0); x++)
                        {
                            rectTankClone[x, y].X -= this.velocity;
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
                    for (int x = 0; x < rectTank.GetLength(0); x++)
                    {
                        for (int y = 0; y < rectTank.GetLength(1); y++)
                        {
                            rectTankClone[x, y].Y -= this.velocity;
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
                    for (int y = 0; y < rectTank.GetLength(1); y++)
                    {
                        for (int x = 0; x < rectTank.GetLength(0); x++)
                        {
                            rectTankClone[x, y].X += this.velocity;
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
                    for (int x = 0; x < rectTank.GetLength(0); x++)
                    {
                        for (int y = 0; y < rectTank.GetLength(1); y++)
                        {
                            rectTankClone[x, y].Y += this.velocity;
                        }
                    }
                    SetDirection(direction);
                    break;
                default:
                    break;
            }
        }
        private Dictionary<string, List<Rectangle>> rs;
        public Rectangle[] GetRectangles()
        {
            try
            {
                rs["tank"] = new List<Rectangle>(rectTank.Cast<Rectangle>());
                return rs.Values.SelectMany(x => x).Select(x => x).ToArray();
            }
            catch
            {
                return null;
            }
        }

        public void Shoot()
        {
            int sleep = 10;
            Task.Run(() =>
            {
                switch (CurrentDirection)
                {
                    case Direction.Left:
                        {
                            Rectangle shot = new Rectangle();
                            shot.Location = new Point(rectTank[0, 1].X, rectTank[0, 1].Y);
                            shot.Size = new Size(rectTank[0, 1].Width, rectTank[0, 1].Height);
                            int inc = shot.Size.Width;
                            rs["shot"].Add(shot);
                            int index = rs["shot"].Count - 1;                           
                            for (int i = shot.X; i > this.clientRectangle.X; i -= inc)
                            {
                                Thread.Sleep(sleep);
                                shot.X = i;
                                rs["shot"][index] = shot;
                            }
                            rs["shot"][index] = new Rectangle();
                        }
                        break;
                    case Direction.Right:
                        {
                            Rectangle shot = new Rectangle();
                            shot.Location = new Point(rectTank[2, 1].X, rectTank[2, 1].Y);
                            shot.Size = new Size(rectTank[2, 1].Width, rectTank[2, 1].Height);
                            int inc = shot.Size.Width;
                            rs["shot"].Add(shot);
                            int index = rs["shot"].Count - 1;
                            for (int i = shot.X; i < clientRectangle.X + clientRectangle.Width; i += inc)
                            {
                                Thread.Sleep(sleep);
                                shot.X = i;
                                rs["shot"][index] = shot;
                            }
                            rs["shot"][index] = new Rectangle();
                        }
                        break;
                    case Direction.Up:
                        {
                            Rectangle shot = new Rectangle();
                            shot.Location = new Point(rectTank[1, 0].X, rectTank[1, 0].Y);
                            shot.Size = new Size(rectTank[1, 0].Width, rectTank[1, 0].Height);
                            int inc = shot.Size.Height;
                            rs["shot"].Add(shot);
                            int index = rs["shot"].Count - 1;
                            for (int i = shot.Y; i > this.clientRectangle.Y; i -= inc)
                            {
                                Thread.Sleep(sleep);
                                shot.Y = i;
                                rs["shot"][index] = shot;
                            }
                            rs["shot"][index] = new Rectangle();
                        }
                        break;
                    case Direction.Down:
                        {
                            Rectangle shot = new Rectangle();
                            shot.Location = new Point(rectTank[1, 2].X, rectTank[1, 2].Y);
                            shot.Size = new Size(rectTank[1, 2].Width, rectTank[1, 2].Height);
                            int inc = shot.Size.Height;
                            rs["shot"].Add(shot);
                            int index = rs["shot"].Count - 1;
                            for (int i = shot.Y; i < this.clientRectangle.Y + clientRectangle.Height; i += inc)
                            {
                                Thread.Sleep(sleep);
                                shot.Y = i;
                                rs["shot"][index] = shot;
                            }
                            rs["shot"][index] = new Rectangle();
                        }
                        break;
                    default:
                        break;
                }
            });
        }

        public Point GetLocation()
        {
            return rectTank[1, 0].Location;
        }

        private bool TankExceededLimit(Rectangle[,] rectangles)
        {
            switch (CurrentDirection)
            {
                case Direction.Left:
                    return rectangles[0, 1].X < this.clientRectangle.X;
                case Direction.Right:
                    return rectangles[2, 1].X > this.clientRectangle.X + this.clientRectangle.Width;
                case Direction.Up:
                    return rectangles[1, 0].Y < this.clientRectangle.Y;
                case Direction.Down:
                    return rectangles[1, 2].Y > this.clientRectangle.Y + this.clientRectangle.Height;
            }
            return false;
        }

        //private bool ShotExceededLimit()
        //{
        //    switch (CurrentDirection)
        //    {
        //        case Direction.Left:
        //            return shot.X < this.clientRectangle.X;
        //        case Direction.Right:
        //            return shot.X > this.clientRectangle.X + this.clientRectangle.Width;
        //        case Direction.Up:
        //            return shot.Y < this.clientRectangle.Y;
        //        case Direction.Down:
        //            return shot.Y > this.clientRectangle.Y + this.clientRectangle.Height;
        //    }
        //    return false;
        //}
    }
}
