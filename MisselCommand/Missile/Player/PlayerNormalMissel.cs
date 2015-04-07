using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MisselCommand.Missile;

namespace MisselCommand.Missile.Player
{
    public class PlayerNormalMissel : Missile
    {
        private Random _rand;
        Texture2D texture;
        MisselManager _misselManager;
        public PlayerNormalMissel(MisselManager manager)
        {
            _misselManager = manager;
        }
        public override void Load(Vector2 StartingPoint, Vector2 EndingPoint, ContentManager Content)
        {
            _rand = new Random((int)(StartingPoint.X + EndingPoint.X));
            texture = Content.Load<Texture2D>("missile");
            _velocity = 3;
            base.Load(StartingPoint, EndingPoint, Content);
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Collision()
        {
            _misselManager.AddExplosion(_location, 200, new Color(115, 155, 155, 255));
            base.Collision();
        }
        public override void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(texture, _location, Color.Blue);
            base.Draw(SpriteBatch);
        }
      
    }
}
