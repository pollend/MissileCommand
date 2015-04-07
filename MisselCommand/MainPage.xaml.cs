using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MisselCommand.Missile;
using System.IO;
using System.IO.IsolatedStorage;



namespace MisselCommand
{
    public partial class MainPage : PhoneApplicationPage
    {
        private UIElementRenderer _uiRender;
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        private Random rand = new Random();
        private MisselManager _misselManager;
    

        private Lines _line;


        private void Read()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            int score;

            try
            {
                using (var isoFileStream = new IsolatedStorageFileStream("score.txt", FileMode.Open, myStore))
                {
                    using (var isoFileReader = new StreamReader(isoFileStream))
                    {
                        score = Int32.Parse(isoFileReader.ReadLine());
                    }
                }
            }
            catch
            {
                score = 0;
            }

            HighScore.Text = "HIGHSCORE:" + score;
        }

        // Constructor
        public MainPage()
        {
         
            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
            contentManager = (Application.Current as App).Content;
            InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
     
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);


            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            _misselManager = new Missile.MisselManager(contentManager);
            _line = new Lines(contentManager);

            _uiRender = new UIElementRenderer(this, 800, 480);

            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }




        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            _misselManager.Update();
      
            _line.Update();
            if (rand.Next(0, 1000) < 100)
            {

                Missile.MisselManager.AddMissel(new Vector2(rand.Next(0, 800), 0), new Vector2(rand.Next(0, 800), 480), new Missile.Computer.ComputerNormalMissile());
            }
        }


        private void OnDraw(object sender, GameTimerEventArgs e)
        {
           
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
            //draws the bases

            _uiRender.Render();
            spriteBatch.Begin();
            _misselManager.Draw(spriteBatch, SharedGraphicsDeviceManager.Current.GraphicsDevice);
            _line.Draw(spriteBatch);
            spriteBatch.Draw(_uiRender.Texture, Vector2.Zero, Microsoft.Xna.Framework.Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }





    }
}