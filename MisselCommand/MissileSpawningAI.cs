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
        private DispatcherTimer _difficulty = new DispatcherTimer();
        private Random rand = new Random();
        private int maxMillisecondsSpanMissile = 5000;
        public missileManager _missileManager;
        public MissileSpawningAI(missileManager missileManager)
        {
            _missileManager = missileManager;
            maxMillisecondsSpanMissile = 5000;

            _difficulty.Interval = new TimeSpan(0, 0, 1);
            _spawnNormalmissiles.Tick += new EventHandler(_difficultyTick);
            _spawnNormalmissiles.Start();

            _spawnNormalmissiles.Interval = new TimeSpan(0, 0, 0, 0, 5000);
            _spawnNormalmissiles.Tick += new EventHandler(_spawnNormalmissiles_Tick);
            _spawnNormalmissiles.Start();
        }
        private void _spawnNormalmissiles_Tick(object sender, EventArgs e)
        {
            System.Console.WriteLine(maxMillisecondsSpanMissile);
            _missileManager.Addmissile(new Vector2(rand.Next(0, Game1.game.GraphicsDevice.Viewport.Width), 0), new Vector2(rand.Next(0, Game1.game.GraphicsDevice.Viewport.Width), Game1.game.GraphicsDevice.Viewport.Height + 20), new ComputerNormalMissile(_missileManager));
            _spawnNormalmissiles.Interval = new TimeSpan(0, 0, 0, 0, rand.Next(200, maxMillisecondsSpanMissile));
        }

        private void _difficultyTick(object sender, EventArgs e)
        {
            if (maxMillisecondsSpanMissile >= 200)
            {
                maxMillisecondsSpanMissile -= 20;
            }
            else
                maxMillisecondsSpanMissile = 300;


        }
    }
}
