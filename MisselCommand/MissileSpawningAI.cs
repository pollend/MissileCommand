using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MisselCommand.Missile;
using System.Threading;
using System.Windows.Threading;
using MisselCommand.Missile.Computer;

namespace MisselCommand
{
    public class MissileSpawningAI
    {

        private DispatcherTimer _spawnNormalMissels = new DispatcherTimer();
        private Random rand = new Random();
        private int maxMillisecondsSpanMissile = 5000;
        public MisselManager _misselManager;
        public MissileSpawningAI(MisselManager misselManager)
        {
            _misselManager = misselManager;
            maxMillisecondsSpanMissile = 6000;
            _spawnNormalMissels.Interval = new TimeSpan(0, 0, 0, 0, 5000);
            _spawnNormalMissels.Tick += new EventHandler(_spawnNormalMissels_Tick);
            _spawnNormalMissels.Start();
        }

        void _spawnNormalMissels_Tick(object sender, EventArgs e)
        {

            _misselManager.AddMissel(new Vector2(rand.Next(0, Game1.game.GraphicsDevice.Viewport.Width), 0), new Vector2(rand.Next(0, Game1.game.GraphicsDevice.Viewport.Width), Game1.game.GraphicsDevice.Viewport.Height), new ComputerNormalMissile(_misselManager));
            if (maxMillisecondsSpanMissile <= 1000)
            {
                maxMillisecondsSpanMissile -= 100;
                _spawnNormalMissels.Interval = new TimeSpan(0, 0, 0, 0, rand.Next(1000, maxMillisecondsSpanMissile));
            }
            if (maxMillisecondsSpanMissile < 1000)
            {
                maxMillisecondsSpanMissile = 1000;
            }
        }
    }
}
