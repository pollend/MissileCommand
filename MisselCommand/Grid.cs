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
        public void ImpactPosion(Vector2 Location, int radius)
        {
        
                int locationOnArray = _getLocationOnArray((int)(Location.X - _location.X), (int)(Location.Y - _location.Y));
                for (int x = ((int)Location.X) - radius; x < ((int)Location.X) + radius; x++ )
                {
                    for (int y = ((int)Location.Y) - radius; y < ((int)Location.Y) + radius; y++)
                    {

                            Vector2 lrelativeLocation = new Vector2(x - _location.X, y - _location.Y);
                            if (new Vector2((float)x - Location.X, (float)y - Location.Y).Length() < radius)
                            {
                                int llocation = _getLocationOnArray((int)(lrelativeLocation.X), (int)(lrelativeLocation.Y));
                                if(llocation != -1)
                                {
                                   
                                    _color[llocation] = new Color(0, 0, 0, 0);
                                   
                                }
                            }
                        
                    }

                }

        }
        public  bool CollisionCheck(Vector2 Location)
        {
            int locationOnArray = _getLocationOnArray((int)(Location.X - _location.X), (int)(Location.Y - _location.Y));
            if (locationOnArray != -1)
            {
                if (_color[locationOnArray].A > 5)
                {
                    return true;
                }
            }
            return false;
        }

        public void update()
        {

        }
        public void GetTexture(GraphicsDevice GraphicsDevice, SpriteBatch sprites)
        {
      
            // GraphicsDevice.Textures[0] = null;
            if (_texture == null)
            {
                _texture = new Texture2D(GraphicsDevice, _width, _height);
            }
            _texture.SetData(_color);
            sprites.Draw(_texture, _location, Color.White);

      
        }
    }
}
