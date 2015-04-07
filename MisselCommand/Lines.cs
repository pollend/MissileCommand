using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace MisselCommand
{
    public class Lines
    {
            private static LinkedList<Line> _lines = new LinkedList<Line>();
            Texture2D _lineTexture;
            List<Line> _removeLine = new List<Line>();

            public Lines(ContentManager ContentManager)
            {
                _lineTexture = ContentManager.Load<Texture2D>("line");
            }

            public static void AddLine(Vector2 Start, Vector2 End)
            {
                _lines.AddFirst( new Line(Start, End));
            }

            private class Line
            {
                public Vector2 _start;
                public float _rotation;
                public float _width;
                public float opacity;
                public Line(Vector2 Start, Vector2 End)
                {
                    this._width = Vector2.Distance(Start, End);
                    this._start = Start;
                    this._rotation = (float)Math.Atan2((End.Y - Start.Y), (End.X-Start.X));
                    opacity = 100;
                }
            }

            public void Update()
            {
                foreach (Line l in _lines)
                {
                    l.opacity -= 1;
                    if (l.opacity <= 0)
                    {
                        _removeLine.Add(l);
                    }
                }
                for (int i = 0; i < _removeLine.Count; i++)
                {
                    _lines.Remove(_removeLine[i]);
                }
                _removeLine.Clear();
            }
            public void Draw(SpriteBatch SpriteBatch)
            {
                foreach (Line l in _lines)
                {
                    if(l.opacity >= 0)
                    SpriteBatch.Draw(_lineTexture, l._start, null, Color.Blue * (l.opacity/100), l._rotation, Vector2.Zero, new Vector2((l._width/_lineTexture.Width), 1),SpriteEffects.None, 0);
                }
            }
    }
}
