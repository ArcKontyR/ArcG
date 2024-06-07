using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcG
{
    public class Player : GameObject, IDrawable
    {
        private Vector2 _position;
        public Vector2 Position { get => _position; private set => _position = value; }

        private Vector2 _velocity;
        public Vector2 Velocity { get => _velocity; private set => _velocity = value; }
        public Sprite Sprite { get; private set; }
        public float Speed { get; private set; }

        public Health Health { get; private set; }

        public Weapon Weapon { get; private set; }

        public Player() {
            Init();
            Objects.Add(this);
        }

        public Player(Vector2 position) : this() {
            _position = position;
        }

        public override void Init()
        {
            //Trace.WriteLine($"Player initiated");
            //_position = Vector2.Zero;
            Sprite = new Sprite(Textures.Characters, 2, new Vector2(16,16));
            //Trace.WriteLine(Texture);
            Weapon = new Weapon(WeaponType.Range);
            Health = new Health();
            Speed = 200f;
        }

        public override void Update(GameTime gameTime, IEnumerable<GameObject> objects)
        {
            if (!Health.IsAlive)
            {
                Objects.Remove(this);
                return;
            }
            Velocity = CalculateVelocity(gameTime.ElapsedGameTime.TotalSeconds);
            float moveX = Velocity.X;
            float moveY = Velocity.Y;

            _position.X += moveX; 
            Sprite.UpdatePosition(Position);
            var tiles = objects.Select(x => x).Where(x => x is Tile);
            foreach (Tile tile in tiles)
            {
                //Trace.WriteLine($"{tile.Value.CollisionRect}");
                if (!(tile.HasCollision && tile.Sprite.Rectangle.Intersects(Sprite.Rectangle))) continue;
                
                if (Velocity.X < 0)
                {
                    _position.X = tile.Sprite.Rectangle.Right; 
                    Sprite.UpdatePosition(Position);
                    moveX = 0f;
                } else
                if (Velocity.X > 0)
                {
                    _position.X = tile.Sprite.Rectangle.Left - Sprite.Rectangle.Width; 
                    Sprite.UpdatePosition(Position);
                    moveX = 0f;
                }
                //Trace.WriteLine($"Collision {tile.Sprite.Rectangle} with {Texture.Rectangle}");
            }

            _position.Y += moveY;
            Sprite.UpdatePosition(_position);
            foreach (Tile tile in tiles)
            {
                if (!(tile.HasCollision && tile.Sprite.Rectangle.Intersects(Sprite.Rectangle))) continue;

                if (Velocity.Y < 0)
                {
                    _position.Y = tile.Sprite.Rectangle.Bottom; 
                    Sprite.UpdatePosition(Position);
                    moveY = 0f;
                }
                else
                if (Velocity.Y > 0)
                {
                    _position.Y = tile.Sprite.Rectangle.Top - Sprite.Rectangle.Height;
                    Sprite.UpdatePosition(Position); 
                    moveY = 0f;
                }
            }

            Vector2 lookDirection = CalculateLookDirection();

            //Trace.WriteLine($"Player velocity = {Velocity}");

            Weapon.UpdateDirection(lookDirection);
            Weapon.UpdatePlayerPosition(Position);

            CheckAttackState(gameTime);
        }

        private Vector2 CalculateVelocity(double deltaTime)
        {
            Vector2 movementDirection = CalculateNormalizedDirection();
            float xPos = (float)(movementDirection.X * Speed * deltaTime);
            float yPos = (float)(movementDirection.Y * Speed * deltaTime);
            return new Vector2(xPos, yPos);
        }

        private void CheckAttackState(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                Weapon.Attack(gameTime);
        }

        private Vector2 CalculateLookDirection()
        {
            var mousePos = Mouse.GetState().Position;
            //Trace.WriteLine($"Mouse position - {mousePos}");
            double angle = GetAngle(mousePos.ToVector2(), Position);
            //double angle = Math.Atan2(mousePos.Y - Position.Y , mousePos.X - Position.X);
            //Trace.WriteLine($"To mouse x {mousePos.X - Position.X}, to mouse y {mousePos.Y - Position.Y}");
            //if (angle < 0)
            //    angle = Math.Abs(angle);
            //else
            //    angle = 2 * Math.PI - angle;
            Vector2 lookDirection = new Vector2((float)Math.Cos(angle), -(float)Math.Sin(angle));
            //if (lookDirection != Vector2.Zero) 
                //lookDirection.Normalize();
            //Trace.WriteLine($"Look direction - {lookDirection}");
            return lookDirection;
        }

        

        private Vector2 CalculateNormalizedDirection()
        {
            Vector2 direction = Vector2.Zero;
            float x = CalculateHorizontalMovement();
            float y = CalculateVerticalMovement();
            direction = new Vector2(x, y);
            if (direction != Vector2.Zero)
                direction.Normalize();
            return direction;
        }

       

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Health != null)
            {
                Vector2 healthPos = Sprite.Rectangle.Location.ToVector2();
                healthPos.Y -= 30;
                healthPos.X += Sprite.Rectangle.Width * 0.25f;
                spriteBatch.DrawString(Fonts.Jacquard, Health.Current.ToString(), healthPos, Color.White);
            }
            ((IDrawable)Sprite).Draw(spriteBatch);
        }
    }
}
