using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TankBlocks.org.entity;

namespace TankBlocks
{
    public partial class Form1 : Form
    {
        private Tank tank;
        private bool up, down, left, right;
        private Timer timer;
        public Form1()
        {
            InitializeComponent();

            this.tank = new Tank();
            tank.Color = Color.Black;

            this.timer = new Timer();
            this.timer.Interval = 1;
            this.timer.Tick += Timer_Tick;

            this.DoubleBuffered = true;
        }

        

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (tank != null)
            {
                e.Graphics.FillRectangles(new SolidBrush(tank.Color), tank.GetRectangles());
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    up = true;
                    break;
                case Keys.S:
                    down = true;
                    break;
                case Keys.D:
                    right = true;
                    break;
                case Keys.A:
                    left = true;
                    break;
                default:
                    break;
            }
            if (!(up || down || left || right))
            {
                timer.Stop();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    up = false;
                    break;
                case Keys.S:
                    down = false;
                    break;
                case Keys.D:
                    right = false;
                    break;
                case Keys.A:
                    left = false;
                    break;
                default:
                    break;
            }
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (up)
            {
                tank.SetPosition(org.enums.Direction.Up);
            }
            else if (down)
            {
                tank.SetPosition(org.enums.Direction.Down);
            }
            else if (left)
            {
                tank.SetPosition(org.enums.Direction.Left);
            }
            else if (right)
            {
                tank.SetPosition(org.enums.Direction.Right);
            }
            this.Invalidate();
        }
    }
}
