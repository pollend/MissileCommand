using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace missileCommand
{
    public class Grid
    {
        private Color[] _color;
        private int _width;
        private int _height;
        private Vector2 _location;
        private Random _rand;
        private Texture2D _texture; 

        public Grid(Texture2D texture, Vector2 location)
        {
            //set the color
            _color = new Color[texture.Width * texture.Height];
            texture.GetData(_color);

            //sets the random
            _rand = new Random(System.DateTime.Now.Millisecond);

            //set the width height and location
            _width = texture.Width;
            _height = texture.Height;
            _location = location;

    
        }
        private int _getLocationOnArray(int x, int y)
        {

            int test = (y * _width) + x;
            if (test >= 0 && test <= _width * _height)
            {
                return test;
            }
            return -1;
            
        }
        public void ImpactPosion(Vector2 Location, int radius, int amount)
        {
        
                int locationOnArray = _getLocationOnArray((int)(Location.X - _location.X), (int)(Location.Y - _location.Y));
                if (locationOnArray >= 0)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        int CircleLocation = _rand.Next(0, 7);
                        locationOnArray = _getLocationOnArray((int)((Location.X - _location.X) + (Math.Cos(CircleLocation) * _rand.Next(0, radius))), (int)((Location.Y - _location.Y) + (Math.Sin(CircleLocation) * _rand.Next(0, radius))));
                        if (locationOnArray >= 0)
                        {
                            _color[locationOnArray] = new Color(0, 0, 0, 0);
                        }
                    }
                }
        }
        public  bool CollisionCheck(Vector2 Location)
        {
            int locationOnArray = _getLocationOnArray((int)(Location.X - _location.X), (int)(Location.Y - _location.Y));
            if (locationOnArray >= 0)
            {
                if (_color[_getLocationOnArray((int)(Location.X - _location.X), (int)(Location.Y - _location.Y))].A > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void GetTexture(GraphicsDevice GraphicsDevice, SpriteBatch sprites)
        {
            GraphicsDevice.Textures[0] = null;
            if (_texture == null)
            {
                _texture = new Texture2D(GraphicsDevice, _width, _height);
            }
            _texture.SetData(_color);
            sprites.Draw(_texture, _location, Color.White);
        }
    }
}
