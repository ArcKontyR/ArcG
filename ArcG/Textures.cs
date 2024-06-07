using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcG
{
    public static class Textures
    {
        public static Texture2D Characters;
        public static Texture2D MapTiles ;
        public static Texture2D Arrow;

        public static void LoadTextures()
        {
            Characters = MainGame.MainContent.Load<Texture2D>("Sprites\\2D Pixel Dungeon Asset Pack\\character and tileset\\Dungeon_Character_2");
            MapTiles = MainGame.MainContent.Load<Texture2D>("Sprites\\2D Pixel Dungeon Asset Pack\\character and tileset\\Dungeon_Tileset");
            Arrow = MainGame.MainContent.Load<Texture2D>("Sprites\\2D Pixel Dungeon Asset Pack\\items and trap_animation\\arrow\\Just_arrow");
        }
    }
}
