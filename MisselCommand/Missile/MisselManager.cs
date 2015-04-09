using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;

using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using missileCommand.Missile;
using missileCommand.Layers;


namespace missileCommand.Missile
{
    public class missileManager
    {
        //the grid with the texture to present
        private static Grid _landScapeGrid;
        Texture2D _landScape;

        //explosion
        private LinkedList<Explosion> _explosion = new LinkedList<Explosion>();
        private List<Explosion> _explosionToRemove = new List<Explosion>();
        //missiles
        private LinkedList<Missile> _missile = new LinkedList<Missile>();
        private List<Missile> _missilesToRemove = new List<Missile>();

       

        private static ContentManager _content;

        public static Grid GetLandScape
        {
            get { return _landScapeGrid; }
        }

        public missileManager(ContentManager content)
        {
            _explosion = new LinkedList<Explosion>();
            _missile = new LinkedList<Missile>();
            _missilesToRemove = new List<Missile>();
            _explosionToRemove = new List<Explosion>();

            _landScape = content.Load<Texture2D>("backdrop");
             _landScapeGrid = new Grid(_landScape, new Vector2(0,480 - _landScape.Height));

            _content = content;
        }
        public void Addmissile(Vector2 start, Vector2 end,Missile type)
        {
            Missile missile = type;
            missile.Load(start, end, _content);
            _missile.AddLast(missile);
        }
        public void AddExplosion(Vector2 location, float maxwidth, Color clr)
        {
            _explosion.AddLast(new Explosion(location, maxwidth, _content, clr));
        }

        /// <summary>
        /// test for rectangle collisions
        /// </summary>
        public bool TestForMissileRectangleCollisions(Microsoft.Xna.Framework.Rectangle rect)
        {
            foreach (Missile m in _missile)
            {
                if (!m.GetFriendlyState)
                {
                    if (m.GetLocation.X > rect.Left && m.GetLocation.X < rect.Right)
                    {
                        if (m.GetLocation.Y > rect.Top && m.GetLocation.Y < rect.Bottom)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// update loop
        /// </summary>
        public void Update(PlayField field)
        {

            foreach (Missile m in _missile)
            {
                m.Update();
                if (m.DeathState)
                {
                    _missilesToRemove.Add(m);
                }
                foreach (Explosion e in _explosion)
                {
                    if (new Rectangle((int)m.GetLocation.X, (int)m.GetLocation.Y, 2, 2).Intersects(new Rectangle((int)(e.GetLocation.X - (e.Width / 2)), (int)(e.GetLocation.Y -( e.Width / 2)), (int)e.Width, (int)e.Width))) ;
                    if (Vector2.Distance(e.GetLocation, m.GetLocation) < (e.Width / 2))
                    {
                        //explosion causes the missile to explode
                        if (!m.GetFriendlyState)
                        {
                            field.AddScore(100);
                            
                        }
                        _missilesToRemove.Add(m);
                    }
                }

                //checks if the missile collides with the terrain
                if (!m.GetFriendlyState)
                {
                    if (_landScapeGrid.CollisionCheck(m.GetLocation))
                    {
                        m.DeathState = true;
                        _landScapeGrid.ImpactPosion(m.GetLocation,10);
                    }

                }
            }
            foreach (Explosion e in _explosion)
            {
                e.Update();
                if (e.Dead)
                {
                    _explosionToRemove.Add(e);
                }
            }

            for (int i = 0; i < _missilesToRemove.Count; i++)
            {
                _missilesToRemove[i].Collision();
                _missile.Remove(_missilesToRemove[i]);
            }
            for (int i = 0; i < _explosionToRemove.Count; i++)
            {
                _explosion.Remove(_explosionToRemove[i]);
            }
            _missilesToRemove.Clear();
            _explosionToRemove.Clear();
            _landScapeGrid.update();
        }
        /// <summary>
        /// draws the textures of the missiles and landscape
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphicsDevice"></param>
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            _landScapeGrid.GetTexture(graphicsDevice, spriteBatch);
            foreach (Missile m in _missile)
            {
                m.Draw(spriteBatch);
            }
            foreach (Explosion e in _explosion)
            {
                e.Draw(spriteBatch);
            }
        }

    }
}
