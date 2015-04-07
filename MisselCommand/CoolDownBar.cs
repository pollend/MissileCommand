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

        private DispatcherTimer _coolDown = new DispatcherTimer();

        public CoolDownBar(ContentManager Content, Vector2 Location)
        {
            _texture = Content.Load<Texture2D>("line");
            _location = Location;


            _coolDown.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _coolDown.Tick += new EventHandler(coolDown);
            _coolDown.Start();

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
            if (_overheat >= 0)
            {
                _overheat -= 1;
            }
            else
            {
               
                 if (_heat >= 100)
                {
                    _overheat = 100;
                    _heat = 100;
                }
            }
        }

        public void AddHeat(int heat)
        {
            if (_overheat <= 0)
            {
                _heat += heat;
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
            float widthOfBar = ( ((_heat/100f)*WidthOfOverHeatBar)  / 2);
            if (_overheat <= 0)
            {
                spritebatch.Draw(_texture, _location, null, Color.Yellow, 0.0f, Vector2.Zero, new Vector2(widthOfBar, 1), SpriteEffects.None, 0);
            }
            else
            {
                if (_isRed)
                {
                    _isRed = false;
                    spritebatch.Draw(_texture, _location, null, Color.Red, 0.0f, Vector2.Zero, new Vector2(widthOfBar, 1), SpriteEffects.None, 0);
                }
                else
                {
                    _isRed = true;
                    spritebatch.Draw(_texture, _location, null, Color.LightYellow, 0.0f, Vector2.Zero, new Vector2(widthOfBar, 1), SpriteEffects.None, 0);
                }
            }
        }
    }
}
