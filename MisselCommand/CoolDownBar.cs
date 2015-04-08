using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MisselCommand.Missile;
using System.Windows.Threading;

namespace MisselCommand
{
    public class CoolDownBar
    {
       

        private Texture2D _texture;
        private int _heat;
        private Vector2 _location;
        private bool _isRed = false;

        private int _overheat;
        private float _delaySwitch = 0.0f;

        private SpriteFont _font;

        private DispatcherTimer _coolDown = new DispatcherTimer();
        private DispatcherTimer _overheatClock = new DispatcherTimer(); 

        public CoolDownBar(ContentManager Content, Vector2 Location)
        {
            _font = Game1.game.Content.Load<SpriteFont>("font");

            _texture = Content.Load<Texture2D>("line");
            _location = Location;


            _coolDown.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _coolDown.Tick += new EventHandler(coolDown);
            _coolDown.Start();

            _overheatClock.Interval = new TimeSpan(0, 0, 0, 0, 50);
            _overheatClock.Tick += new EventHandler(overHeat);

        }

        private void overHeat(object sender, EventArgs e)
        {
            if (_overheat > 0)
            {
                _overheat -= 1;
            }
            else
            {
                _overheatClock.Stop();
                _coolDown.Start();
            }
        }

        private void coolDown(object sender, EventArgs e)
        {
            if (_heat > 0)
            {
                _heat -= 1;
            }
        }

        public void Update()
        {
            if (_heat >= 100)
            {
                _overheat = 100;
                _heat = 70;
                _coolDown.Stop();
                _overheatClock.Start();
            }
        }

        public void AddHeat(int heat)
        {
            if (_overheat <= 0)
            {
                _heat += heat;
                if (_heat >= 100)
                    _heat = 100;
            }
        }

        public bool IsOverHeated()
        {
            if (_overheat <= 0)
            {
                return false;
            }
            return true;
        }

        public void Draw(int WidthOfOverHeatBar, SpriteBatch spritebatch)
        {
       
            if (_overheat <= 0)
            {
                float widthOfBar = (((_heat / 100f) * WidthOfOverHeatBar) / 2);
                spritebatch.Draw(_texture, _location, null, Color.Yellow, 0.0f, Vector2.Zero, new Vector2(widthOfBar, 1), SpriteEffects.None, 0);
                spritebatch.DrawString(_font, _heat + "%", _location, Color.Yellow, 0.0f, Vector2.Zero, .3f, SpriteEffects.None, 0);
            }
            else
            {
                float widthOfBar = (((_overheat / 100f) * WidthOfOverHeatBar) / 2);
                _delaySwitch += 1f;
                if (_isRed)
                {
                    if (_delaySwitch > 5)
                    {
                        _delaySwitch = 0;

                        _isRed = false;
                    }
                        spritebatch.Draw(_texture, _location, null, Color.Red, 0.0f, Vector2.Zero, new Vector2(widthOfBar, 1), SpriteEffects.None, 0);
                        spritebatch.DrawString(_font, _overheat + "%", _location, Color.IndianRed, 0.0f, Vector2.Zero, .3f, SpriteEffects.None, 0);
                    
                }
                else
                {
                    if (_delaySwitch > 5)
                    {
                        _delaySwitch = 0;
                        _isRed = true;
                    }
                        spritebatch.Draw(_texture, _location, null, Color.Yellow, 0.0f, Vector2.Zero, new Vector2(widthOfBar, 1), SpriteEffects.None, 0);
                        spritebatch.DrawString(_font, _overheat + "%", _location, Color.Yellow, 0.0f, Vector2.Zero, .3f, SpriteEffects.None, 0);
                    
                }
            }

            
        }
    }
}
