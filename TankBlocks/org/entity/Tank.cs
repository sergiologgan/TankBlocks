using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TankBlocks.org.enums;
using TankBlocks.org.structure;

namespace TankBlocks.org.entity
{
    public class Tank : Vehicle
    {
        [NonSerialized]
        private Size size;
        private int velocity, incY, incX;
        private Rectangle[,] tank, tankfull;
        private Rectangle clientRectangle;
        private Dictionary<string, List<Rectangle>> rectangles;
        private Map map;
        private Point location;
        private Point lastLocation;
        public Map Map { get => map; }
        public Point Location { get => location; }

        public Direction CurrentDirection { get; private set; }
        public Direction LastDirection { get; private set; }

       

        public Tank(Point location, Map map, Rectangle clientRectangle)
        {
            this.clientRectangle = clientRectangle;
            this.size = new Size(8, 8);
            this.map = map;
            this.lastLocation = location;
            this.location = location;

            this.rectangles = new Dictionary<string, List<Rectangle>>()
            {
                { "tank", new List<Rectangle>() },
                { "shot", new List<Rectangle>() },
            };

            tank = new Rectangle[3, 3]
            {
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()}
            };

            tankfull = new Rectangle[3, 3]
            {
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()},
                { new Rectangle(),new Rectangle(),new Rectangle()}
            };

            Point convertPoint = map.Blocks[location.X, location.Y].PixelLocation;
            int sumX = convertPoint.X;
            int sumY = convertPoint.Y;
            incX = size.Width;
            incY = size.Height;
            for (int x = 0; x < tank.GetLength(0); x++)
            {
                for (int y = 0; y < tank.GetLength(1); y++)
                {
                    tankfull[x, y].Size = size;
                    tankfull[x, y].Location = new Point(sumX, sumY);
                    if (!(x == 0 && y == 0) || !(x == 2 && y == 0) || !(x == 1 && y == 0))
                    {
                        tank[x, y].Size = size;
                        tank[x, y].Location = new Point(sumX, sumY);
                        if (y > 3) sumY = convertPoint.Y;
                        sumY += incY;
                    }
                }
                if (x > 3) sumX = convertPoint.X;
                sumX += incX;
                sumY = convertPoint.Y;
            }
        }

        public bool SetDirection(Direction direction)
        {
            for (int x = 0; x < tankfull.GetLength(0); x++)
            {
                for (int y = 0; y < tankfull.GetLength(1); y++)
                {
                    tank[x, y].Location = tankfull[x, y].Location;
                    tank[x, y].Size = tankfull[x, y].Size;
                }
            }

            switch (direction)
            {
                case Direction.Left:
                    tank[0, 0] = new Rectangle();
                    tank[2, 1] = new Rectangle();
                    tank[0, 2] = new Rectangle();
                    break;
                case Direction.Up:
                    tank[0, 0] = new Rectangle();
                    tank[2, 0] = new Rectangle();
                    tank[1, 2] = new Rectangle();
                    break;
                case Direction.Right:
                    tank[2, 0] = new Rectangle();
                    tank[0, 1] = new Rectangle();
                    tank[2, 2] = new Rectangle();
                    break;
                case Direction.Down:
                    tank[1, 0] = new Rectangle();
                    tank[0, 2] = new Rectangle();
                    tank[2, 2] = new Rectangle();
                    break;
                default:
                    break;
            }
            return false;
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
                    }
                    else
                    {
                        LastDirection = direction;
                        for (int y = 0; y < tank.GetLength(1); y++)
                        {
                            for (int x = 0; x < tank.GetLength(0); x++)
                            {
                                tankfull[x, y].X -= this.velocity;
                            }
                        }
                        SetDirection(direction);
                    }
                    break;
                case Direction.Up:
                    if (LastDirection != direction)
                    {
                        SetDirection(direction);
                        LastDirection = direction;
                    }
                    else
                    {
                        LastDirection = direction;
                        for (int x = 0; x < tank.GetLength(0); x++)
                        {
                            for (int y = 0; y < tank.GetLength(1); y++)
                            {
                                tankfull[x, y].Y -= this.velocity;
                            }
                        }
                        SetDirection(direction);
                    }
                    break;
                case Direction.Right:
                    if (LastDirection != direction)
                    {
                        SetDirection(direction);
                        LastDirection = direction;
                    }
                    else
                    {
                        LastDirection = direction;
                        for (int y = 0; y < tank.GetLength(1); y++)
                        {
                            for (int x = 0; x < tank.GetLength(0); x++)
                            {
                                tankfull[x, y].X += this.velocity;
                            }
                        }
                        SetDirection(direction);
                    }
                    break;
                case Direction.Down:
                    if (LastDirection != direction)
                    {
                        SetDirection(direction);
                        LastDirection = direction;
                    }
                    else
                    {
                        LastDirection = direction;
                        for (int x = 0; x < tank.GetLength(0); x++)
                        {
                            for (int y = 0; y < tank.GetLength(1); y++)
                            {
                                tankfull[x, y].Y += this.velocity;
                            }
                        }
                        SetDirection(direction);
                    }
                    break;
                default:
                    break;
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
                            shot.Location = new Point(tank[0, 1].X, tank[0, 1].Y);
                            shot.Size = new Size(tank[0, 1].Width, tank[0, 1].Height);
                            int inc = shot.Size.Width;
                            rectangles["shot"].Add(shot);
                            int index = rectangles["shot"].Count - 1;
                            for (int i = shot.X; i > this.clientRectangle.X; i -= inc)
                            {
                                Thread.Sleep(sleep);
                                shot.X = i;
                                rectangles["shot"][index] = shot;
                            }
                            rectangles["shot"][index] = new Rectangle();
                        }
                        break;
                    case Direction.Right:
                        {
                            Rectangle shot = new Rectangle();
                            shot.Location = new Point(tank[2, 1].X, tank[2, 1].Y);
                            shot.Size = new Size(tank[2, 1].Width, tank[2, 1].Height);
                            int inc = shot.Size.Width;
                            rectangles["shot"].Add(shot);
                            int index = rectangles["shot"].Count - 1;
                            for (int i = shot.X; i < clientRectangle.X + clientRectangle.Width; i += inc)
                            {
                                Thread.Sleep(sleep);
                                shot.X = i;
                                rectangles["shot"][index] = shot;
                            }
                            rectangles["shot"][index] = new Rectangle();
                        }
                        break;
                    case Direction.Up:
                        {
                            Rectangle shot = new Rectangle();
                            shot.Location = new Point(tank[1, 0].X, tank[1, 0].Y);
                            shot.Size = new Size(tank[1, 0].Width, tank[1, 0].Height);
                            int inc = shot.Size.Height;
                            rectangles["shot"].Add(shot);
                            int index = rectangles["shot"].Count - 1;
                            for (int i = shot.Y; i > this.clientRectangle.Y; i -= inc)
                            {
                                Thread.Sleep(sleep);
                                shot.Y = i;
                                rectangles["shot"][index] = shot;
                            }
                            rectangles["shot"][index] = new Rectangle();
                        }
                        break;
                    case Direction.Down:
                        {
                            Rectangle shot = new Rectangle();
                            shot.Location = new Point(tank[1, 2].X, tank[1, 2].Y);
                            shot.Size = new Size(tank[1, 2].Width, tank[1, 2].Height);
                            int inc = shot.Size.Height;
                            rectangles["shot"].Add(shot);
                            int index = rectangles["shot"].Count - 1;
                            for (int i = shot.Y; i < this.clientRectangle.Y + clientRectangle.Height; i += inc)
                            {
                                Thread.Sleep(sleep);
                                shot.Y = i;
                                rectangles["shot"][index] = shot;
                            }
                            rectangles["shot"][index] = new Rectangle();
                        }
                        break;
                    default:
                        break;
                }
            });
        }

        public Point GetShooter()
        {
            switch (CurrentDirection)
            {
                case Direction.Left:
                    return tank[0, 1].Location;
                case Direction.Right:
                    return tank[2, 1].Location;
                case Direction.Up:
                    return tank[1, 0].Location;
                case Direction.Down:
                    return tank[1, 2].Location;
                default:
                    break;
            }
            return new Point();
        }

        public Rectangle[] GetRectangles()
        {
            try
            {
                rectangles["tank"] = new List<Rectangle>(tank.Cast<Rectangle>());
                return rectangles.Values.SelectMany(x => x).Select(x => x).ToArray();
            }
            catch
            {
                return null;
            }
        }
        public Rectangle[] GetAreaIntersect()
        {
            var t = map.GetAllRectangles().ToList();
            switch (CurrentDirection)
            {
                case Direction.Left:
                    t = t.Where(l => l.IntersectsWith(tankfull[0, 1])).ToList();
                    return t.ToArray();
                case Direction.Right:
                    t = t.Where(l => l.IntersectsWith(tankfull[2, 1])).ToList();
                    return t.ToArray();
                case Direction.Up:
                    t = t.Where(l => l.IntersectsWith(tankfull[1, 0])).ToList();
                    return t.ToArray();
                case Direction.Down:
                    t = t.Where(l => l.IntersectsWith(tankfull[1, 2])).ToList();
                    return t.ToArray();
                default:
                    return null;
            }
        }
    }
}
