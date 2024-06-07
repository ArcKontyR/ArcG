using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcG
{
    public enum WeaponType
    {
        Melee,
        Range
    }

    public class Weapon
    {
        public double Damage = 3f;
        private WeaponType _type;
        public Vector2 Direction {  get; private set; }
        public Vector2 PlayerPosition { get; private set; }
        //private Bullet bullet;
        private bool _canAttack = true;
        private double _nextShotTime;
        private double _bulletsPerSecond = 3f;
        public Weapon(WeaponType wType) {
            _type = wType;
        }
        public void Attack(GameTime gameTime)
        {
            double currentShotTime = gameTime.TotalGameTime.TotalSeconds;
            //Trace.WriteLine($"Current shot time {currentShotTime}");
            //Trace.WriteLine($"Next shot time {_nextShotTime}");
            if (currentShotTime < _nextShotTime) return;
            
            _nextShotTime = currentShotTime + 1 / _bulletsPerSecond;
            //Trace.WriteLine($"Attacked to {Direction}");

            switch (_type)
            {
                case WeaponType.Melee:
                    new MeleeAttack(Direction, PlayerPosition);
                    break;

                case WeaponType.Range:
                    new Bullet(Direction, PlayerPosition);
                    break;
            }
               
            _canAttack = false;
        }

        public void UpdateDirection(Vector2 direction)
        {
            //Trace.WriteLine($"Changing weapon direction {direction}");
            if (direction == Direction) return;
            Direction = direction;
        }
        
        public void UpdatePlayerPosition(Vector2 playerPosition)
        {
            if (playerPosition == PlayerPosition) return;
            PlayerPosition = playerPosition;
        }
    }
}
