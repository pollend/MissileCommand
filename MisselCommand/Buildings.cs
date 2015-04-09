using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using missileCommand.Missile;


namespace missileCommand
{

    public class Buildings
    {
        private Texture2D _buildingTexture;
        private Rectangle _rect;
        private Random _rand;
        private bool _dead = false;
        private missileManager _missileManager;
        public Buildings(Rectangle rect, Texture2D texture,missileManager missileManager)
        {
            _missileManager = missileManager;
            _rand = new Random(System.DateTime.Now.Millisecond);
            _rect = rect;
            _buildingTexture = texture;
        }



        /// <summary>
        /// gets tje rectangle
        /// </summary>
        public Rectangle GetRectangle
        {
            get { return _rect; }
        }
       
        public bool GetDeathState
        {
            get { return _dead; }
            set { _dead = value; }
        }
        /// <summary>
        /// causes the building to explod destrroying the terrain in the process
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="TerrainDestruction"></param>
        /// <param name="amountOfExplosions"></param>
        public void Destruction(Grid grid, int TerrainDestruction, int amountOfExplosions )
        {
            for (int i = 0; i < amountOfExplosions; i++)
			{
                _missileManager.AddExplosion(new Vector2(_rand.Next(_rect.Left, _rect.Right), _rand.Next(_rect.Top, _rect.Bottom)), 50 + _rand.Next(0, 200), new Color(115, 30, 30, 30));
			}
            for (int i = 0; i < TerrainDestruction; i++)
			{
                grid.ImpactPosion(new Vector2(_rand.Next(_rect.Left-10, _rect.Right+10), _rand.Next(_rect.Bottom-10, _rect.Bottom + 10)), 10);
             
			}
            _dead = true;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if (!_dead)
            {
                spritebatch.Draw(_buildingTexture, _rect, Color.White);
            }
        }
    }
    
}
