using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcG
{
    public class GameObject
    {
        public static List<GameObject> Objects = new List<GameObject>();
        public GameObject() { 
        }

        public virtual void Init() { }

        public virtual void Update(GameTime gameTime, IEnumerable<GameObject> objects = null) { }

        public static float CalculateVerticalMovement() => MapKeysToValue(Keys.W, Keys.S);

        public static float CalculateHorizontalMovement() => MapKeysToValue(Keys.A, Keys.D);
        
        private static float MapKeysToValue(Keys leftKey, Keys rightKey)
        {
            var state = Keyboard.GetState();
            //Trace.WriteLine($"Keyboard state - {state.IsKeyDown(leftKey)}");
            int toLeftKey = state.IsKeyDown(leftKey) ? -1 : 0;
            //Trace.WriteLine($"To left - {toLeftKey}");
            int toRightKey = state.IsKeyDown(rightKey) ? 1 : 0;
            //Trace.WriteLine($"To right - {toRightKey}");
            return toLeftKey + toRightKey;
        }

        public static double GetAngle(Vector2 start, Vector2 end)
        {
            double angle = Math.Atan2(start.Y - end.Y, start.X - end.X);
            //Trace.WriteLine($"To mouse x {mousePos.X - Position.X}, to mouse y {mousePos.Y - Position.Y}");
            if (angle < 0)
                angle = Math.Abs(angle);
            else
                angle = 2 * Math.PI - angle;
            return angle;
        }
    }
}
