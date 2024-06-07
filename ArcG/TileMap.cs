using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArcG
{
    public class TileMap : GameObject, IDrawable
    {
        Dictionary<Vector2, int> map = new Dictionary<Vector2, int>();
        Dictionary<Vector2, int> collisionMap = new Dictionary<Vector2, int>();
        Dictionary<Vector2, int> spawnerMap = new Dictionary<Vector2, int>();
        private static int mapTileSize = 0;
        private static int mapTileSizeMultiplier = 3;
        public List<Tile> Tiles { get; private set; }
        public List<Tile> Spawners { get; private set; }
        private Dictionary<Vector2, bool> collisions;
        private Dictionary<Vector2, bool> spawners;

        public static readonly int MaxEntities = 60;
        public static readonly int MinEntities = 30;
        public static int RemainEntities = new Random().Next(MinEntities, MaxEntities);

        public TileMap(string _mapName, string _collisionMapName, string _spawnerMapName, int tileSize = 8, int tileSizeMultiplier = 2)
        {
            mapTileSize = tileSize;
            mapTileSizeMultiplier = tileSizeMultiplier;
            Tiles = new List<Tile>();
            Spawners = new List<Tile>();
            collisions = new Dictionary<Vector2, bool>();
            collisionMap = MapLoader.LoadMap(_collisionMapName);
            foreach (var tile in collisionMap)
            {
                if (!collisions.ContainsKey(tile.Key))
                    collisions[tile.Key] = Convert.ToBoolean(tile.Value);
            }
            spawners = new Dictionary<Vector2, bool>();
            spawnerMap = MapLoader.LoadMap(_spawnerMapName);
            foreach (var tile in spawnerMap)
            {
                if (!spawners.ContainsKey(tile.Key))
                    spawners[tile.Key] = Convert.ToBoolean(tile.Value);
            }
            map = MapLoader.LoadMap(_mapName);
            foreach (var tile in map)
            {
                Sprite tileSprite = new(Textures.MapTiles, tile.Value, mapTileSize, mapTileSizeMultiplier);
                Tile mapTile = new Tile(tileSprite, collisions[tileSprite.GetSheetPosition()], spawners[tileSprite.GetSheetPosition()]);
                Rectangle newDest = new((int)tile.Key.X * mapTileSize, (int)tile.Key.Y * mapTileSize, mapTileSize, mapTileSize);
                mapTile.Sprite.UpdateDestinationRectangle(newDest);
                Tiles.Add(mapTile);
                if (spawners[tileSprite.GetSheetPosition()]) Spawners.Add(mapTile);
            }
        }


        public override void Update(GameTime gameTime, IEnumerable<GameObject> objects = null)
        {
            foreach(Tile spawner in Spawners)
            {
                spawner.Update(gameTime);
            }
        }


        

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in Tiles)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.DrawString(Fonts.Jacquard, $"Remaining enemies: {RemainEntities}", new Vector2(0, 30), Color.White);
        }

    }
}
