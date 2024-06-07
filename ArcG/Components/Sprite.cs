using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcG
{
    public class Sprite : IDrawable
    {
        private Texture2D _spriteSheet = null;
        private int _rowCount = 0;
        private int _columnCount = 0;

        public Rectangle Rectangle;
        
        private Vector2 _size;
        private Vector2 _position;
        private Rectangle? _destination;
        private Rectangle? _source;
        private int _rowPos;
        private int _columnPos;
        private int _spriteIndex;
        private int _sizeMultiplier;
        private float _rotation = 0f;

        public Sprite(Texture2D spriteSheet, int spriteIndex, Vector2 spriteSize, int sizeMultiplier = 3)
        {
            _spriteSheet = spriteSheet;
            _spriteIndex = spriteIndex;
            _sizeMultiplier = sizeMultiplier;
            _size = spriteSize;
            _rotation = 0f;
            UpdatePosition(Vector2.Zero);
           // Trace.WriteLine($"Sprite created with {spriteSheet} texture with {spriteSize} size");
            //Trace.WriteLine($"Sprite id = {spriteIndex}");
            _columnCount = (int)(spriteSheet.Width / _size.X);
            _rowCount = (int)(spriteSheet.Height / _size.Y);
            for (_rowPos = 0; _rowPos < _rowCount; _rowPos++)
            {
                bool indexFound = false;
                for (_columnPos = 0; _columnPos < _columnCount; _columnPos++){
                    //Trace.Write($"{_rowPos * _columnCount + _columnPos} ");
                    if (_rowPos * _columnCount + _columnPos != _spriteIndex) continue;
                    indexFound = true;
                    break;
                }
                //Trace.WriteLine("");
                if (indexFound) break;
            }
            //Trace.WriteLine($"Sheet contains {_rowCount} rows and {_columnCount} columns");
            //Trace.WriteLine($"Sprite position in sheet is {_rowPos} rows and {_columnPos} columns");
        }

        public Sprite(Texture2D spriteSheet, int spriteIndex, int spriteSize, int sizeMultiplier = 3) : this(spriteSheet, spriteIndex, new Vector2(spriteSize, spriteSize), sizeMultiplier) { }


        public Vector2 Size {
            get => _size;
            private set
            {
                if (value == Vector2.Zero) return;
                _size = value;
            }
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            if (_position == newPosition) return;
            _position = newPosition;
            Rectangle.X = (int)_position.X;
            Rectangle.Y = (int)_position.Y;
        }

        public void SetRotation(float angle)
        {
            _rotation = angle;
        }

        public void UpdateDestinationRectangle(Rectangle newDestination) {
            if (_destination == newDestination) return;
            _destination = newDestination;
        }

        public void UpdateSourceRectangle(Rectangle newSource)
        {
            if (_source == newSource) return;
            _source = newSource;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = (_destination == null) ? 
                new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X * _sizeMultiplier, (int)_size.Y * _sizeMultiplier) : 
                new Rectangle(_destination.Value.X * _sizeMultiplier, _destination.Value.Y * _sizeMultiplier, _destination.Value.Width * _sizeMultiplier, _destination.Value.Height * _sizeMultiplier);
            Rectangle = destination;
            Rectangle src = (Rectangle)((_source == null) ? new Rectangle(_columnPos * (int)_size.X, _rowPos * (int)_size.X, (int)_size.X, (int)_size.Y) : _source);
            spriteBatch.Draw(_spriteSheet,
                destination,
                src,
                Color.White,
                _rotation,
                Vector2.Zero,
                SpriteEffects.None,
                0
                ); ;
        }

        public Vector2 GetSheetPosition() => new Vector2(_columnPos, _rowPos);
    }
}
    