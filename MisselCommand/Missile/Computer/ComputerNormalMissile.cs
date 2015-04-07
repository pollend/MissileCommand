using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MisselCommand.Missile.Computer
{
    public class ComputerNormalMissile : Missile
    {
        private Texture2D texture;
        private MisselManager _misselManager;
        public ComputerNormalMissile(MisselManager misselManager)
        {
            _misselManager = misselManager;
        }
        public override void Load(Vector2 StartingPoint, Vector2 EndingPoint, ContentManager Content)
        {
            _friendlyMissile = false;
            texture = Content.Load<Texture2D>("Missile");
            _velocity = 1;
            base.Load(StartingPoint, EndingPoint, Content);
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Collision()
        {

            _misselManager.AddExplosion(_location, 100, new Color(115, 155, 155, 255));
            base.Collision();
        }
        public override void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(texture, _location, Color.White);
            base.Draw(SpriteBatch);
        }
    }
}
