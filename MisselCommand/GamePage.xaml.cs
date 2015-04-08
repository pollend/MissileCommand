using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using missileCommand.Missile;
using System.Windows.Media.Imaging;
using Microsoft.Devices;
using System.IO;
using System.IO.IsolatedStorage;

namespace missileCommand
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        private Random rand = new Random();
        private missileManager _missileManager;
        private Lines _line;

        //scoreItems
        private static int _score;
        private static int _oldScore;
        private static float _percentMultiplier;
        private static int _city;
        private static int _missilesBases;

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

        //the width and height of the phone texture
        public static int WIDTH;
        public static int HEIGHT;
        //the selected arrow
        private int _selectedArrow = 1;

        //the missile Ai 
        private MissileSpawningAI _missileAi;

        private UIElementRenderer _uiRender;

        public GamePage()
        {


            HEIGHT = SharedGraphicsDeviceManager.DefaultBackBufferWidth;
           WIDTH =SharedGraphicsDeviceManager.DefaultBackBufferHeight;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            _uiRender = new UIElementRenderer(this, WIDTH, HEIGHT);
            contentManager = (Application.Current as App).Content;
            InitializeComponent();
        }

        public static void AddScore(int score)
        {
            if (!_lose)
            {
                _score += (int)(score * _percentMultiplier);
            }
          
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            _line = new Lines(contentManager);

            //test if the game has been restored back to its oldstate
            if (_missileManager == null)
            {
                Load();
            }

            //cool down bar
            _leftBaseCoolDown = new CoolDownBar(contentManager, new Vector2(_leftBase.GetRectangle.Left - 20, _leftBase.GetRectangle.Top - 20));
            _middleBaseCoolDown = new CoolDownBar(contentManager, new Vector2(_middleBase.GetRectangle.Left - 20, _middleBase.GetRectangle.Top - 20));
            _rightBaseCoolDown = new CoolDownBar(contentManager, new Vector2(_rightBase.GetRectangle.Left - 20, _rightBase.GetRectangle.Top - 20));

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }
        /// <summary>
        /// load function
        /// </summary>
        private void Load()
        {
            _missileAi = new MissileSpawningAI();

            BorderLeftArrow.Visibility = System.Windows.Visibility.Visible;
            BorderMiddleArrow.Visibility = System.Windows.Visibility.Visible;
            BorderRightArrow.Visibility = System.Windows.Visibility.Visible;

            _missileManager = new Missile.missileManager(contentManager);

            CanvasGameOver.Visibility = System.Windows.Visibility.Collapsed;

            //set the score items
            _score = 0;
            _percentMultiplier = 1.0f;
            _missilesBases = 3;
            _city = 6;

            //sets up the missile bases
            Texture2D HomeBase = contentManager.Load<Texture2D>("Homebase");
            _leftBase = new Buildings(new Rectangle(30, 409, HomeBase.Width, HomeBase.Height), HomeBase);
            _middleBase = new Buildings(new Rectangle(385, 409, HomeBase.Width, HomeBase.Height), HomeBase);
            _rightBase = new Buildings(new Rectangle(725, 407, HomeBase.Width, HomeBase.Height), HomeBase);

            //sets up the selection of buildings
            Texture2D buildings = contentManager.Load<Texture2D>("building");
            _buildings[0] = new Buildings(new Rectangle(120, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings);
            _buildings[1] = new Buildings(new Rectangle(200, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings);
            _buildings[2] = new Buildings(new Rectangle(280, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings);

            _buildings[3] = new Buildings(new Rectangle(470, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings);
            _buildings[4] = new Buildings(new Rectangle(550, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings);
            _buildings[5] = new Buildings(new Rectangle(630, 480 - buildings.Height - 17, buildings.Width, buildings.Height), buildings);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);
            //tombstoneing

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            _leftBaseCoolDown.Update();
            _rightBaseCoolDown.Update();
            _middleBaseCoolDown.Update();

            //lose check
            if (_buildings[0].GetDeathState && _buildings[1].GetDeathState && _buildings[2].GetDeathState &&_buildings[3].GetDeathState &&_buildings[4].GetDeathState &&_buildings[5].GetDeathState )
            {
                _lose = true;
                TextBlockFinalScores.Text = _score.ToString();
                CanvasGameOver.Visibility = System.Windows.Visibility.Visible;
            }
            if (_leftBase.GetDeathState && _middleBase.GetDeathState && _rightBase.GetDeathState)
            {
                _lose = true;
                TextBlockFinalScores.Text = _score.ToString();
                CanvasGameOver.Visibility = System.Windows.Visibility.Visible;
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
                        UpdateOverlay();
                        _buildings[i].Destruction(Missile.missileManager.GetLandScape, 30, 30);
                    }
                }
            }

            //missile manager testcollision with buildings
            if (!_leftBase.GetDeathState)
            {
                if (_missileManager.TestForMissileRectangleCollisions(_leftBase.GetRectangle))
                {
                    BorderLeftArrow.Visibility = System.Windows.Visibility.Collapsed;

                    _missilesBases -= 1;
                    _percentMultiplier -= 0.05f;
                    _leftBase.Destruction(Missile.missileManager.GetLandScape, 40, 30);

                    UpdateOverlay();
                }
            }
            if (!_middleBase.GetDeathState)
            {
                if (_missileManager.TestForMissileRectangleCollisions(_middleBase.GetRectangle))
                {
                    BorderMiddleArrow.Visibility = System.Windows.Visibility.Collapsed;

                    _missilesBases -= 1;
                    _percentMultiplier -= 0.05f;
                    _middleBase.Destruction(Missile.missileManager.GetLandScape, 30, 30);

                    UpdateOverlay();
                }
            }
            if (!_rightBase.GetDeathState)
            {
                if (_missileManager.TestForMissileRectangleCollisions(_rightBase.GetRectangle))
                {
                    BorderRightArrow.Visibility = System.Windows.Visibility.Collapsed;

                    _missilesBases -= 1;
                    _percentMultiplier -= 0.05f;
                    _rightBase.Destruction(Missile.missileManager.GetLandScape, 30, 30);
                    UpdateOverlay();
                }
            }

            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {

                if (tl.State == TouchLocationState.Released)
                {
                    if (tl.Position.Y < (int)SharedGraphicsDeviceManager.DefaultBackBufferHeight - 40)
                    {
                        if (tl.Position.Y < HEIGHT - 50)
                        {
                            //adds a missile to the world
                            if (_selectedArrow == 1)
                            {
                                if (!_rightBaseCoolDown.IsOverHeated())
                                {
                                    if (!_rightBase.GetDeathState)
                                    {
                                        _rightBaseCoolDown.AddHeat(30);
                                        Missile.missileManager.Addmissile(new Vector2(_rightBase.GetRectangle.Left + _rightBase.GetRectangle.Width / 2, _rightBase.GetRectangle.Bottom - 2), tl.Position, new Missile.Player.PlayerNormalmissile());
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
                                        Missile.missileManager.Addmissile(new Vector2(_middleBase.GetRectangle.Left + _middleBase.GetRectangle.Width / 2, _middleBase.GetRectangle.Bottom - 2), tl.Position, new Missile.Player.PlayerNormalmissile());
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
                                        Missile.missileManager.Addmissile(new Vector2(_leftBase.GetRectangle.Left + _leftBase.GetRectangle.Width / 2, _leftBase.GetRectangle.Bottom - 2), tl.Position, new Missile.Player.PlayerNormalmissile());
                                    }
                                }
                            }
                        }
                    }
                }

            }
            _line.Update();
            // TODO: Add your update logic here
            _missileManager.Update();

            if (_oldScore != _score)
            {
                _oldScore = _score;
                UpdateOverlay();
            }

          
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Black);
            //draws the bases

            _uiRender.Render();
            spriteBatch.Begin();

            _line.Draw(spriteBatch);
            _missileManager.Draw(spriteBatch, SharedGraphicsDeviceManager.Current.GraphicsDevice);


            for (int i = 0; i < _buildings.Length; i++)
            {
                if (_buildings[i] != null)
                {
                    _buildings[i].Draw(spriteBatch);
                }

            }
            
            _leftBaseCoolDown.Draw(_leftBase.GetRectangle.Width + 40, spriteBatch);
            _middleBaseCoolDown.Draw(_leftBase.GetRectangle.Width + 40, spriteBatch);
            _rightBaseCoolDown.Draw(_leftBase.GetRectangle.Width + 40, spriteBatch);
            //draws the 3 bases
            _leftBase.Draw(spriteBatch);
            _rightBase.Draw(spriteBatch);
            _middleBase.Draw(spriteBatch);
            //draws overlay
            spriteBatch.Draw(_uiRender.Texture, Vector2.Zero, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here
        }

        private void UpdateOverlay()
        {
            TextBlockScore.Text = "Score:" + _score;
            TextBlockPercentMultiplier.Text = "PercentMultiplier:" + _percentMultiplier*100 + "%";
            TextBlockMissileBases.Text = "Bases:" + _missilesBases;
            TextBlockCities.Text = "Cities" + _city;
        }

        private void RightArrow(object sender, MouseEventArgs e)
        {
            BorderLeftArrow.Background = _setImage("Arrow.png");
            BorderMiddleArrow.Background = _setImage("Arrow.png");
            BorderRightArrow.Background = _setImage("SelectedArrow.png");
            //far right arrow
            _selectedArrow = 1;
            //causes the phone to vibrate when the 3rd arrow is selected
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(50));
        
        }

        private void MiddleArrow(object sender, MouseEventArgs e)
        {
            if (_selectedArrow != 2)
            {
                BorderLeftArrow.Background = _setImage("Arrow.png");
                BorderMiddleArrow.Background = _setImage("SelectedArrow.png");
                BorderRightArrow.Background = _setImage("Arrow.png");
                //middle arrow left
                _selectedArrow = 2;
                //causes the phone to vibrate when the 3rd arrow is selected
                VibrateController vibrate = VibrateController.Default;
                vibrate.Start(TimeSpan.FromMilliseconds(50));
            }
        }

        private void LeftArrow(object sender, MouseEventArgs e)
        {
            BorderLeftArrow.Background = _setImage("SelectedArrow.png");
            BorderMiddleArrow.Background = _setImage("Arrow.png");
            BorderRightArrow.Background = _setImage("Arrow.png");
            //far left arrow
            _selectedArrow = 3;
            //causes the phone to vibrate when the 3rd arrow is selected
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(50));
        }

        private System.Windows.Media.ImageBrush _setImage(string location)
        {
            System.Windows.Media.ImageBrush imagebrush = new System.Windows.Media.ImageBrush();
            imagebrush.ImageSource = new BitmapImage(new Uri(location, UriKind.Relative));
            return imagebrush;
        }

        private void TextBlockRestart_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            using (var isoFileStream = new IsolatedStorageFileStream("score.txt", FileMode.OpenOrCreate, myStore))
            {
                // Write the data
                using (var isoFileWriter = new StreamWriter(isoFileStream))
                {
                    isoFileWriter.WriteLine(_score.ToString());
                }
            }
            Load();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }


    }
}