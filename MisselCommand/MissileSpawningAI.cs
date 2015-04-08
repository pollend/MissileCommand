using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using missileCommand.Missile;
using System.Threading;
using System.Windows.Threading;
using missileCommand.Missile.Computer;

namespace missileCommand
{
    public class MissileSpawningAI
    {

        private DispatcherTimer _spawnNormalmissiles = new DispatcherTimer();
        private Random rand = new Random();
        private int maxMillisecondsSpanMissile = 5000;
        public missileManager _missileManager;
        public MissileSpawningAI(missileManager missileManager)
        {
            _missileManager = missileManager;
            maxMillisecondsSpanMissile = 6000;
            _spawnNormalmissiles.Interval = new TimeSpan(0, 0, 0, 0, 5000);
            _spawnNormalmissiles.Tick += new EventHandler(_spawnNormalmissiles_Tick);
            _spawnNormalmissiles.Start();
        }

        void _spawnNormalmissiles_Tick(object sender, EventArgs e)
        {

            _missileManager.Addmissile(new Vector2(rand.Next(0, Game1.game.GraphicsDevice.Viewport.Width), 0), new Vector2(rand.Next(0, Game1.game.GraphicsDevice.Viewport.Width), Game1.game.GraphicsDevice.Viewport.Height), new ComputerNormalMissile(_missileManager));
            if (maxMillisecondsSpanMissile <= 1000)
            {
                maxMillisecondsSpanMissile -= 100;
                _spawnNormalmissiles.Interval = new TimeSpan(0, 0, 0, 0, rand.Next(1000, maxMillisecondsSpanMissile));
            }
            if (maxMillisecondsSpanMissile < 1000)
            {
                maxMillisecondsSpanMissile = 1000;
            }
        }
    }
}
