using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Threading;

namespace missileCommand.Missile
{
    public class Missile
    {
        private Vector2 _startingPoint;
        private Vector2 _endingPoint;

        private bool _dead = false;
        //test if friendly missile
        protected bool _friendlyMissile = true;

        private Vector2 _oldLocation;

        //private float _arrivalTime;
        private DispatcherTimer _arrivialTime= new DispatcherTimer();
        private DispatcherTimer _timeBetweenLines = new DispatcherTimer();

        private Vector2 _Direction;

        protected Vector2 _location;

        protected float _velocity = 2;

        public Vector2 GetLocation
        {
            get { return _location; }
        }
        public bool GetFriendlyState
        {
            get { return _friendlyMissile; }
        }
        public bool DeathState
        {
            get { return _dead; }
            set { _dead = value; }
        }

        public virtual void Load(Vector2 StartingPoint, Vector2 EndingPoint, ContentManager Content)
        {


            this._endingPoint = EndingPoint;
            this._startingPoint = StartingPoint;
            this._oldLocation = StartingPoint;
            this._location = StartingPoint;
            //the direction of missile
            _Direction = Vector2.Normalize(_endingPoint - _startingPoint) * _velocity;
          
           //_arrivalTime = (Vector2.Distance(_startingPoint, _endingPoint) / _velocity);
           _arrivialTime.Interval = new TimeSpan(0, 0, 0, 0, (int)((Vector2.Distance(_startingPoint, _endingPoint) / _velocity)*16));
           _arrivialTime.Tick += new EventHandler(missileArrived);
           _arrivialTime.Start();

            //adds a starting line
           Lines.AddLine(this._oldLocation, _location);
            //sets up a timer to tell when a missile will put down a line
           _timeBetweenLines.Interval = new TimeSpan(0, 0, 0, 0,200);
           _timeBetweenLines.Tick += new EventHandler(_TimeBetweenLineDraw);
           _timeBetweenLines.Start();
           
        }

        //puts down a line on the map
        void _TimeBetweenLineDraw(object sender, EventArgs e)
        {
            Lines.AddLine(_oldLocation, _location);
            _oldLocation = _location;
        }

        void missileArrived(object sender, EventArgs e)
        {
            _dead = true;
            _arrivialTime.Stop();
            _timeBetweenLines.Stop();
        }
        public virtual void Update()
        {
            _location += _Direction;
            Lines.AddLine(_location, _Direction + _location);
        }
        public virtual void Draw(SpriteBatch SpriteBatch)
        {
        }
        public virtual void Collision()
        {
            _dead = true;
        }
    }
}
