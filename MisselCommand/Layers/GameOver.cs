using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MisselCommand.Layers
{
    public class GameOver : MisselCommand.ILayer
    {
        private SpriteFont _font;
        private int _score;
        public string layerId()
        {
            return "game-over";
        }

        public GameOver(int score)
        {
            _score = score;
        }

        public void load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _font = Game1.game.Content.Load<SpriteFont>("font");
        }

        public void update(MisselCommand.Layers.LayerManager layerManager)
        {
           if(layerManager.keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
           {
               layerManager.clearLayers();
               layerManager.addLayer(new PlayField(true));
           }
        }

        public void draw()
        {
            Game1.game.spriteBatch.Begin();
            Vector2 lsizeGameOver = _font.MeasureString("Game Over") * .9f;
            Game1.game.spriteBatch.DrawString(_font, "Game Over", new Vector2(-(lsizeGameOver.X / 2.0f) + (Game1.game.GraphicsDevice.Viewport.Width / 2.0f), -(lsizeGameOver.Y / 2.0f) + (Game1.game.GraphicsDevice.Viewport.Height / 2.0f)), Color.White, 0.0f, Vector2.Zero, .9f, SpriteEffects.None, 0);

            Vector2 lsizeScore = _font.MeasureString("Score:" + _score) * .5f;
            Game1.game.spriteBatch.DrawString(_font, "Score:" + _score, new Vector2(-(lsizeScore.X / 2.0f) + (Game1.game.GraphicsDevice.Viewport.Width / 2.0f), -(lsizeScore.Y / 2.0f) + lsizeGameOver.Y + 5f+ (Game1.game.GraphicsDevice.Viewport.Height / 2.0f)), Color.White, 0.0f, Vector2.Zero, .5f, SpriteEffects.None, 0);

            Vector2 lreset = _font.MeasureString("Enter To Restart" ) * .5f;
            Game1.game.spriteBatch.DrawString(_font, "Enter To Restart", new Vector2(-(lreset.X / 2.0f) + (Game1.game.GraphicsDevice.Viewport.Width / 2.0f), -(lreset.Y / 2.0f) + lsizeGameOver.Y + 5f + lsizeScore.Y + 5f + (Game1.game.GraphicsDevice.Viewport.Height / 2.0f)), Color.White, 0.0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
            
            
            Game1.game.spriteBatch.End();

        }
    }
}
