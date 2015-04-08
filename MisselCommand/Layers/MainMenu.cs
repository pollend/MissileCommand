using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace missileCommand.Layers
{
    public class MainMenu : ILayer
    {
        private SpriteFont _font;
        private Texture2D _title;

        public string layerId()
        {
            return "Main-Menu";
        }

        public void load(ContentManager content)
        {
            _font = Game1.game.Content.Load<SpriteFont>("font");
            _title = Game1.game.Content.Load<Texture2D>("COMMAND");
        }

        public void update(LayerManager layerManager)
        {
           if( layerManager.keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
           {
               layerManager.clearLayers();
               layerManager.addLayer(new PlayField(true));
           }
        }

        public void draw()
        {
            Game1.game.spriteBatch.Begin();
            Vector2 lsize = _font.MeasureString("Press Enter To Start")*.9f;
            Game1.game.spriteBatch.DrawString(_font, "Press Enter To Start", new Vector2(-(lsize.X / 2.0f) + (Game1.game.GraphicsDevice.Viewport.Width / 2.0f), -(lsize.Y / 2.0f) + (Game1.game.GraphicsDevice.Viewport.Height / 2.0f)), Color.White,0.0f,Vector2.Zero,.9f,SpriteEffects.None,0);
            Game1.game.spriteBatch.End();
        }
    }
}
