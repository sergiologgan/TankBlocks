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
    public class Bot : Tank
    {
        private Rectangle clientRectangle;
        private Tank tank;
        private System.Windows.Forms.Timer timer;
        private bool completedFollow;
        private TimeSpan timeInc;
        private Task task;

        public Bot(Rectangle clientRectangle, Tank tank) : base(clientRectangle)
        {
            this.clientRectangle = clientRectangle;
            this.tank = tank;
            this.timer = new System.Windows.Forms.Timer();
            this.timer.Tick += Timer_Tick;
            this.timer.Interval = 1000;
            this.timeInc = new TimeSpan();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeInc += TimeSpan.Parse("00:00:01");
        }

        private async void FollowMainTank(TimeSpan time)
        {
            Point player = tank.GetLocation();
            Point bot = this.GetLocation();

            for (int i = bot.X; i < player.X; i += 2)
            {
                if (timeInc > time)
                {
                    this.timeInc = new TimeSpan();
                    return;
                }
                this.SetPosition(Direction.Right, i);
            }

            for (int i = bot.X; i > player.X; i -= 2)
            {
                if (timeInc > time)
                {
                    this.timeInc = new TimeSpan();
                    return;
                }
                this.SetPosition(Direction.Left, i);
            }

            for (int i = bot.Y; i < player.Y; i += 2)
            {
                if (timeInc > time)
                {
                    this.timeInc = new TimeSpan();
                    return;
                }
                this.SetPosition(Direction.Down, i);
            }

            for (int i = bot.Y; i > player.Y; i -= 2)
            {
                if (timeInc > time)
                {
                    this.timeInc = new TimeSpan();
                    await Work();
                }
                this.SetPosition(Direction.Up, i);
            }
        }

        private async Task<bool> Work()
        {
            return true;
        }

        public void SetStress(Stress stress)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    switch (stress)
                    {
                        case Stress.Low:
                            if (task.IsCompleted)
                            {
                                task = Task.Factory.FollowMainTank)
                            }
                            FollowMainTank(TimeSpan.Parse("00:00:50"));
                            break;
                        case Stress.Media:
                            FollowMainTank(TimeSpan.Parse("00:00:50"));
                            break;
                        case Stress.Hight:
                            FollowMainTank(TimeSpan.Parse("00:00:50"));
                            break;
                        case Stress.HiperHigh:
                            FollowMainTank(TimeSpan.Parse("00:00:50"));
                            break;
                        default:
                            break;
                    }
                }
            });
        }
    }
}
