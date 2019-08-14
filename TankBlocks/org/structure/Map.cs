using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankBlocks.org.entity;

namespace TankBlocks.org.structure
{
    public class Map
    {
        private Block[,] blocks;
        private Tank tank;
        public Form1 Form { get; private set; }

        public Block[,] Blocks { get => blocks; }

        public Map(int widthRect, int heightRect, int countX, int countY, Form1 form1)
        {
            blocks = new Block[countX, countY];
            form1.Size = new Size((widthRect * countX) - 8, (heightRect * countY) - 12);
            this.Form = form1;
            Random r = new Random();

            int setY = 0, setX = 0;
            for (int y = 0; y < blocks.GetLength(1); y++)
            {                
                for (int x = 0; x < blocks.GetLength(0); x++)
                {
                    blocks[x, y] = new Block();
                    blocks[x, y].Location = new Point(x, y);
                    blocks[x, y].PixelSize = new Size(widthRect, heightRect);
                    blocks[x, y].PixelLocation = new Point(setX, setY);
                    blocks[x, y].Color = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                    setX += widthRect;
                }
                setY += heightRect;
                setX = 0;
            }
        }

        //public void AddTank(Tank tank)
        //{
        //    this.tank = tank;
        //}

        //public void AddBot(Bot bot)
        //{
        //    this.bots.Add(bot);
        //}

        //public void Add<E>(E entity) where E : Vehicle
        //{
        //    if (entity is Tank)
        //    {
        //        this.tank = entity as Tank;
        //    }
        //    else if (entity is Bot)
        //    {
        //        this.bots.Add(entity as Bot);
        //    }
        //}

        public Rectangle[] GetAllRectangles()
        {
            List<Rectangle> rs = new List<Rectangle>();
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                for (int x = 0; x < blocks.GetLength(0); x++)
                {
                    rs.Add(blocks[x, y].Rectangle);
                }
            }
            return rs.ToArray();
        }
    }
}
