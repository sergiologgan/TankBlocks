using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TankBlocks.org.entity;
using TankBlocks.org.structure;

namespace TankBlocks
{
    public partial class Form1 : Form
    {
        private Map map;
        private Tank tank;
        private bool up, down, left, right;
        private System.Windows.Forms.Timer timer;
        private List<Bot> bots;

        public Form1()
        {
            InitializeComponent();

            this.timer = new System.Windows.Forms.Timer();
            this.timer.Interval = 1;
            this.timer.Tick += Timer_Tick;

            map = new Map(24, 24, 55, 40, this);

            this.tank = new Tank(new Point(8,16), map, this.ClientRectangle);
            this.bots = new List<Bot>();
            tank.Color = Color.Black;

            Task.Run(() =>
            {
                for (int i = 0; i < 1; i++)
                {
                    Bot bot = new Bot(this.ClientRectangle, tank, map);
                    bot.Color = Color.Red;
                    bot.Start();
                    this.bots.Add(bot);
                }
            });
            
            this.DoubleBuffered = true;
        }

        private Random r = new Random();
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            List<Rectangle> rs = new List<Rectangle>();
            foreach (Bot bot in bots)
            {
                Rectangle[] rb = bot.GetRectangles();
                for (int i = 0; i < rb.Length; i++)
                {
                    rs.Add(rb[i]);
                }
            }

            Rectangle[] rt = tank.GetRectangles();
            for (int i = 0; i < rt.Length; i++)
            {
                rs.Add(rt[i]);
            }

            e.Graphics.FillRectangles(new SolidBrush(Color.Black), map.GetAllRectangles());

            e.Graphics.FillRectangles(new SolidBrush(Color.Red), tank.GetAreaIntersect());

            if (tank != null)
            {                
                e.Graphics.FillRectangles(new SolidBrush(Color.Yellow), rs.ToArray());
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
                case Keys.Space:
                    tank.Shoot();
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
