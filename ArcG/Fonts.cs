using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcG
{
    public static class Fonts
    {
        public static SpriteFont Arial;
        public static SpriteFont Jacquard ;

        public static void LoadFonts()
        {
            Arial = MainGame.MainContent.Load<SpriteFont>("Fonts/Arial");
            Jacquard = MainGame.MainContent.Load<SpriteFont>("Fonts/Jacquard");
        }
    }
}
