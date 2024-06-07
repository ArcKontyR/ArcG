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
    public class MeleeAttack : GameObject, IDrawable
    {
        public static List<MeleeAttack> Instances = new List<MeleeAttack>();

        private Vector2 _velocity;
        public Vector2 Velocity { get => _velocity; private set => _velocity = value; }

        private Vector2 _position;
        public Vector2 Position { get => _position; private set => _position = value; }
        public Sprite Sprite { get; private set; }
        public float Speed { get; private set; }

        private Vector2 _direction;
        private double _lifeSpan = 0.2f;
        private double _spawnTime = -1f;

        private int _damagePower = 8;

        public MeleeAttack(Vector2 direction, Vector2 spawnPosition)
        {
            Instances.Add(this);
            Objects.Add(this);
            //Trace.WriteLine($"Player initiated");
            Position = spawnPosition;
            _direction = direction;
            Sprite = new Sprite(new Texture2D(MainGame.Graphics.GraphicsDevice, 1, 1), 0, new Vector2(16, 16));
            Sprite.UpdatePosition(Position);
            
            Sprite.SetRotation(-(float)GetAngle(Vector2.Zero, _direction) + 90);

            
            //Trace.WriteLine(Texture);
            Speed = 150f;
        }

        

        public override void Update(GameTime gameTime, IEnumerable<GameObject> objects)
        {
            double currentTime = gameTime.TotalGameTime.TotalSeconds;
            if (_spawnTime < 0) _spawnTime = currentTime;
            if (currentTime > _spawnTime + _lifeSpan)
            {
                Instances.Remove(this);
                Objects.Remove(this);
            }

            Velocity = CalculateVelocity(gameTime.ElapsedGameTime.TotalSeconds);
            
            _position.X += Velocity.X;
            _position.Y += Velocity.Y;
            Sprite.UpdatePosition(Position);
            Player player = objects.Select(x => x).Where(x => x is Player).First() as Player;

            //Trace.WriteLine($"{tile.Value.CollisionRect}");
            if (!player.Sprite.Rectangle.Intersects(Sprite.Rectangle)) return;

            player.Health.Damage(_damagePower);
            Instances.Remove(this);
            Objects.Remove(this);
            
            
            //Trace.WriteLine($"Bullet direction - {_direction}, angle: {-(float)GetAngle(Vector2.UnitY, _direction)}");
        }

        private Vector2 CalculateVelocity(double deltaTime)
        {
            Vector2 movementDirection = _direction;
            float xPos = (float)(movementDirection.X * Speed * deltaTime);
            float yPos = (float)(movementDirection.Y * Speed * deltaTime);
            return new Vector2(xPos, yPos);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Trace.WriteLine($"Drawing bullet");
            Sprite.Draw(spriteBatch);
        }
    }
}
