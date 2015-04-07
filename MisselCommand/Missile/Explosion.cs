using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MisselCommand.Missile
{
    public class Explosion
    {

        private Texture2D _texture;

            public float MaxWidth;
            public float Width;
            public bool Dead = false;
            private Vector2 _location;
            private Color _color;

            public Vector2 GetLocation
            {
                get { return _location; }
            }
            public Explosion(Vector2 location, float MaxWidth, ContentManager Content, Color _color)
            {
               _texture= Content.Load<Texture2D>("Ring");
                this._location = location;
                this.MaxWidth = MaxWidth;
                this._color = _color;
            }
            public void Update()
            {
                Width += 5;
                if (Width > MaxWidth)
                {
                    Dead = true;
                }
                
            }
            public void Draw(SpriteBatch sprite)
            {
                sprite.Draw(_texture, _location, null, _color * (((Width / MaxWidth) - 1.0f) * -1*1.5f), 0.0f, new Vector2(_texture.Width / 2, _texture.Width / 2), (Width / _texture.Width), SpriteEffects.None, 0);
            }
    }
}
