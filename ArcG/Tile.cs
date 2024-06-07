using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArcG
{
    public class Tile : GameObject, IDrawable
    {
        public Sprite Sprite;

        public bool HasCollision;
        public bool CanSpawnEntities;


        private double _spawnCooldown = 8f;
        private int _minSpawnCooldown = 5;
        private int _maxSpawnCooldown = 15;
        private double _spawnTime = -1f;


        public Tile(Sprite sprite, bool hasCollision, bool canSpawnEntities)
        {
            Sprite = sprite;
            HasCollision = hasCollision;
            CanSpawnEntities = canSpawnEntities;

            if (CanSpawnEntities) 
                _spawnCooldown = new Random().Next(_minSpawnCooldown, _maxSpawnCooldown);
            //Trace.WriteLine($"Tile with collision {HasCollision} at {Sprite.Rectangle} is spawner - {CanSpawnEntities}");
        }

        public override void Update(GameTime gameTime, IEnumerable<GameObject> objects = null)
        {
            if (!CanSpawnEntities || TileMap.RemainEntities == 0) return;

            double currentTime = gameTime.TotalGameTime.TotalSeconds;
            //Trace.WriteLine($"Current shot time {currentShotTime}");
            //Trace.WriteLine($"Next shot time {_nextShotTime}");
            if (currentTime < _spawnTime) return;

            if (_spawnTime < 0) _spawnTime = currentTime;
            if (currentTime > _spawnTime + _spawnCooldown)
            {

                new Enemy(Sprite.Rectangle.Location.ToVector2());
                TileMap.RemainEntities--;
                _spawnTime += _spawnCooldown;
                _spawnCooldown = new Random().Next(_minSpawnCooldown, _maxSpawnCooldown);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ((IDrawable)Sprite).Draw(spriteBatch);
        }
    }
}
