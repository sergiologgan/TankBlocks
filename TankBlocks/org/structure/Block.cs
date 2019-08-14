using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBlocks.org.structure
{
    public class Block
    {
        private Rectangle rectangle;
        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }

        private Point pixelLocation;
        public int X { get => pixelLocation.X; set => pixelLocation.X = value; }
        public int Y { get => pixelLocation.Y; set => pixelLocation.Y = value; }
        public Point PixelLocation
        {
            get => pixelLocation;
            set
            {
                X = value.X;
                Y = value.Y;
                this.rectangle.Location = value;
            }
        }

        private Size pixelSize;
        public int Width { get => pixelSize.Width; set => pixelSize.Width = value; }
        public int Height { get => pixelSize.Height; set => pixelSize.Height = value; }
        public Size PixelSize
        {
            get => pixelSize;
            set
            {
                Width = value.Width;
                Height = value.Height;
                this.rectangle.Size = pixelSize;
            }
        }

        public Color Color { get; set; }
        public Point Location { get; set; }

        public Block(int x, int y)
        {
            this.rectangle = new Rectangle();
            this.X = x;
            this.Y = y;
        }

        public Block()
        {
            this.rectangle = new Rectangle();
        }
    }
}
