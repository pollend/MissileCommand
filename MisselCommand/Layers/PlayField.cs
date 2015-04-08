
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using MisselCommand;
using MisselCommand.Missile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MisselCommand.Missile.Player;

namespace MisselCommand.Layers
{
    public class PlayField : ILayer
    {
        private SpriteFont _font;

        private MouseState _mouse;

        private Random rand = new Random();
        private MisselManager _missileManager;
        private Lines _line;

        //scoreItems
        private  int _score;
        private  float _percentMultiplier;
        private  int _city;
        private  int _missileBases;

        private ButtonState _oldState = ButtonState.Released;

        //loseState
        private static bool _lose = false;

        //the bases coolDownBar
        private static CoolDownBar _leftBaseCoolDown;
        private static CoolDownBar _middleBaseCoolDown;
        private static CoolDownBar _rightBaseCoolDown;

        //the bases
        private static Buildings _leftBase;
        private static Buildings _middleBase;
        private static Buildings _rightBase;

        private static Buildings[] _buildings = new Buildings[6];


        //the selected arrow
        private int _selectedArrow = 1;

        //the missile Ai 
        private MissileSpawningAI _missileAi;

        private bool _isInPlay = false;

        public string layerId()
        {
            return "playfield";
        }

        public PlayField(bool isInPlay)
        {
            _isInPlay = isInPlay;
        }

        public void AddScore(int score)
        {
            if (!_lose)
            {
                _score += (int)(score * _percentMultiplier);
            }

        }

        public void load(ContentManager content)
        {
        

            _missileManager = new MisselManager(Game1.game.Content);

            _font = Game1.game.Content.Load<SpriteFont>("font");

            //set the score items
            _score = 0;
            _percentMultiplier = 1.0f;
            _missileBases = 3;
            _city = 6;
            _missileManager = new MisselManager(Game1.game.Content);
         
            
            //sets up the missile bases
            Texture2D HomeBase = Game1.game.Content.Load<Texture2D>("Homebase");
            _leftBase = new Buildings(new Rectangle(30, 409, HomeBase.Width, HomeBase.Height), HomeBase, _missileManager);
            _middleBase = new Buildings(new Rectangle(385, 409, HomeBase.Width, HomeBase.Height), HomeBase, _missileManager);
            _rightBase = new Buildings(new Rectangle(725, 407, HomeBase.Width, HomeBase.Height), HomeBase, _missileManager);

            //sets up the selection of buildings
            Texture2D buildings = Game1.game.Content.Load<Texture2D>("building");
            _buildings[0] = new Buildings(new Rectangle(120, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings, _missileManager);
            _buildings[1] = new Buildings(new Rectangle(200, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings, _missileManager);
            _buildings[2] = new Buildings(new Rectangle(280, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings, _missileManager);

            _buildings[3] = new Buildings(new Rectangle(470, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings, _missileManager);
            _buildings[4] = new Buildings(new Rectangle(550, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings, _missileManager);
            _buildings[5] = new Buildings(new Rectangle(630, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings, _missileManager);

            _line = new Lines(Game1.game.Content);

            //cool down bar
            _leftBaseCoolDown = new CoolDownBar(Game1.game.Content, new Vector2(_leftBase.GetRectangle.Left - 20, _leftBase.GetRectangle.Top - 20));
            _middleBaseCoolDown = new CoolDownBar(Game1.game.Content, new Vector2(_middleBase.GetRectangle.Left - 20, _middleBase.GetRectangle.Top - 20));
            _rightBaseCoolDown = new CoolDownBar(Game1.game.Content, new Vector2(_rightBase.GetRectangle.Left - 20, _rightBase.GetRectangle.Top - 20));


            _missileAi = new MissileSpawningAI(_missileManager);

            //set the score items
            _score = 0;
            _percentMultiplier = 1.0f;
            _missileBases = 3;
            _city = 6;

        }

        public void update(LayerManager layerManager)
        {
            _mouse = Mouse.GetState();
           

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                this._selectedArrow = 3;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                this._selectedArrow = 2;

            if (Keyboard.GetState().IsKeyDown(Keys.E))
                this._selectedArrow = 1;

            _leftBaseCoolDown.Update();
            _rightBaseCoolDown.Update();
            _middleBaseCoolDown.Update();

            //lose check
            if (_buildings[0].GetDeathState && _buildings[1].GetDeathState && _buildings[2].GetDeathState && _buildings[3].GetDeathState && _buildings[4].GetDeathState && _buildings[5].GetDeathState)
            {
                _lose = true;
                //TextBlockFinalScores.Text = _score.ToString();
                //CanvasGameOver.Visibility = System.Windows.Visibility.Visible;
            }
            if (_leftBase.GetDeathState && _middleBase.GetDeathState && _rightBase.GetDeathState)
            {
                _lose = true;
                // TextBlockFinalScores.Text = _score.ToString();
                // CanvasGameOver.Visibility = System.Windows.Visibility.Visible;
            }

            //mini buildings to test collisions
            for (int i = 0; i < _buildings.Length; i++)
            {
                if (!_buildings[i].GetDeathState)
                {
                    if (_missileManager.TestForMissileRectangleCollisions(_buildings[i].GetRectangle))
                    {
                        _city -= 1;
                        _percentMultiplier -= 0.1f;

                        _buildings[i].Destruction(MisselCommand.Missile.MisselManager.GetLandScape, 30, 30);
                    }
                }
            }

            //missile manager testcollision with buildings
            if (!_leftBase.GetDeathState)
            {
                if (_missileManager.TestForMissileRectangleCollisions(_leftBase.GetRectangle))
                {
                    //BorderLeftArrow.Visibility = System.Windows.Visibility.Collapsed;

                    _missileBases -= 1;
                    _percentMultiplier -= 0.05f;
                    _leftBase.Destruction(MisselCommand.Missile.MisselManager.GetLandScape, 40, 30);


                }
            }
            if (!_middleBase.GetDeathState)
            {
                if (_missileManager.TestForMissileRectangleCollisions(_middleBase.GetRectangle))
                {
                    // BorderMiddleArrow.Visibility = System.Windows.Visibility.Collapsed;

                    _missileBases -= 1;
                    _percentMultiplier -= 0.05f;
                    _middleBase.Destruction(MisselCommand.Missile.MisselManager.GetLandScape, 30, 30);


                }
            }
            if (!_rightBase.GetDeathState)
            {
                if (_missileManager.TestForMissileRectangleCollisions(_rightBase.GetRectangle))
                {
                    //BorderRightArrow.Visibility = System.Windows.Visibility.Collapsed;

                    _missileBases -= 1;
                    _percentMultiplier -= 0.05f;
                    _rightBase.Destruction(MisselCommand.Missile.MisselManager.GetLandScape, 30, 30);

                }
            }

            if (_isInPlay)
            {
                if (_mouse.LeftButton == ButtonState.Pressed && _oldState == ButtonState.Released)
                {

                    if (_mouse.Position.Y < Game1.game.GraphicsDevice.Viewport.Height - 40)
                    {
                        if (_mouse.Position.Y < Game1.game.GraphicsDevice.Viewport.Height - 50)
                        {
                            //adds a missel to the world
                            if (_selectedArrow == 1)
                            {
                                if (!_rightBaseCoolDown.IsOverHeated())
                                {
                                    if (!_rightBase.GetDeathState)
                                    {
                                        _rightBaseCoolDown.AddHeat(30);


                                        _missileManager.AddMissel(new Vector2(_rightBase.GetRectangle.Left + _rightBase.GetRectangle.Width / 2, _rightBase.GetRectangle.Bottom - 2), _mouse.Position.ToVector2(), new PlayerNormalMissel(_missileManager));
                                    }
                                }
                            }
                            else if (_selectedArrow == 2)
                            {
                                if (!_middleBaseCoolDown.IsOverHeated())
                                {
                                    if (!_middleBase.GetDeathState)
                                    {
                                        _middleBaseCoolDown.AddHeat(30);
                                        _missileManager.AddMissel(new Vector2(_middleBase.GetRectangle.Left + _middleBase.GetRectangle.Width / 2, _middleBase.GetRectangle.Bottom - 2), _mouse.Position.ToVector2(), new PlayerNormalMissel(_missileManager));
                                    }
                                }
                            }
                            else
                            {
                                if (!_leftBaseCoolDown.IsOverHeated())
                                {
                                    if (!_leftBase.GetDeathState)
                                    {

                                        _leftBaseCoolDown.AddHeat(30);

                                        _missileManager.AddMissel(new Vector2(_leftBase.GetRectangle.Left + _leftBase.GetRectangle.Width / 2, _leftBase.GetRectangle.Bottom - 2), _mouse.Position.ToVector2(), new PlayerNormalMissel(_missileManager));
                                    }
                                }
                            }
                        }
                    }
                }
            }


             if ((_missileBases <= 0 || _city <= 0) && _isInPlay)
            {
                if( layerManager.getLayerById("game-over") == null)
                layerManager.addLayer(new GameOver(_score));
            }
            


            _oldState = _mouse.LeftButton;

            _line.Update();
            // TODO: Add your update logic here
            _missileManager.Update(this);

        }

        public void draw()
        {
            Game1.game.spriteBatch.Begin();

            if (_isInPlay)
            {
                Game1.game.spriteBatch.DrawString(_font, "Score:" + _score, new Vector2(0, 20), Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
                Game1.game.spriteBatch.DrawString(_font, "Percent Multiplier:" + _percentMultiplier * 100 + "%", new Vector2(0, 0), Color.White, 0, Vector2.Zero, .5f,SpriteEffects.None,0);
                Game1.game.spriteBatch.DrawString(_font, "Bases:" + _missileBases, new Vector2(Game1.game.GraphicsDevice.Viewport.Width - _font.MeasureString("Bases:" + _missileBases).X* .5f - 10f, 0), Color.White,0,Vector2.Zero,.5f,SpriteEffects.None,0);
                Game1.game.spriteBatch.DrawString(_font, "Cities:" + _city, new Vector2(Game1.game.GraphicsDevice.Viewport.Width - _font.MeasureString("Cities" + _city).X * .5f - 10f, 20), Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }

            _line.Draw(Game1.game.spriteBatch);
            _missileManager.Draw(Game1.game.spriteBatch, Game1.game.GraphicsDevice);


            for (int i = 0; i < _buildings.Length; i++)
            {
                if (_buildings[i] != null)
                {
                    _buildings[i].Draw(Game1.game.spriteBatch);
                }

            }

            if (_isInPlay)
            {
                _leftBaseCoolDown.Draw(_leftBase.GetRectangle.Width + 40, Game1.game.spriteBatch);
                _middleBaseCoolDown.Draw(_leftBase.GetRectangle.Width + 40, Game1.game.spriteBatch);
                _rightBaseCoolDown.Draw(_leftBase.GetRectangle.Width + 40, Game1.game.spriteBatch);
            }
            //draws the 3 bases
            _leftBase.Draw(Game1.game.spriteBatch);
            _rightBase.Draw(Game1.game.spriteBatch);
            _middleBase.Draw(Game1.game.spriteBatch);

            if (this._selectedArrow == 3)
            {
                Game1.game.spriteBatch.DrawString(_font, "Q", new Vector2(_leftBase.GetRectangle.Left, _leftBase.GetRectangle.Bottom + 10), Color.Red, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }
            else
            {
                Game1.game.spriteBatch.DrawString(_font, "Q", new Vector2(_leftBase.GetRectangle.Left, _leftBase.GetRectangle.Bottom + 10), Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }

            if (this._selectedArrow == 2)
            {
                Game1.game.spriteBatch.DrawString(_font, "W", new Vector2(_middleBase.GetRectangle.Left, _middleBase.GetRectangle.Bottom + 10), Color.Red, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }
            else
            {
                Game1.game.spriteBatch.DrawString(_font, "W", new Vector2(_middleBase.GetRectangle.Left, _middleBase.GetRectangle.Bottom + 10), Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }

            if (this._selectedArrow == 1)
            {
                Game1.game.spriteBatch.DrawString(_font, "E", new Vector2(_rightBase.GetRectangle.Left, _rightBase.GetRectangle.Bottom + 10), Color.Red, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }
            else
            {
                Game1.game.spriteBatch.DrawString(_font, "E", new Vector2(_rightBase.GetRectangle.Left, _rightBase.GetRectangle.Bottom + 10), Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }
            //draws overlay
            // spriteBatch.Draw(_uiRender.Texture, Vector2.Zero, Color.White);
            Game1.game.spriteBatch.End();
        }
    }
}
