using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArcG
{
    public class MainGame : Game
    {
        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;
        public static ContentManager MainContent;
        public static TileMap Map;
        public static Player Player;


        public static int Score;
        public static bool IsGameActive;

        public MainGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            MainContent = this.Content;
            MainContent.RootDirectory = "Content";
            
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Textures.LoadTextures();
            Fonts.LoadFonts();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Reset();
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                Reset();

            if (TileMap.RemainEntities == 0 && Enemy.Instances.Count == 0) IsGameActive = false;
            if (!Player.Health.IsAlive) IsGameActive = false;

            if (IsGameActive)
            {
                Player.Update(gameTime, Map.Tiles);
                Map.Update(gameTime);
                UpdateObjects(Enemy.Instances, gameTime);
                UpdateObjects(Bullet.Instances, gameTime);
                UpdateObjects(MeleeAttack.Instances, gameTime);
            }

            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            //Trace.WriteLine("Drawing...");

            SpriteBatch.Begin();
            Map.Draw(SpriteBatch);
            //Player.Draw(SpriteBatch);
            SpriteBatch.DrawString(Fonts.Jacquard, $"Score: {Score}", Vector2.Zero, Color.White);
            //DrawObjects(Enemy.Instances);
            //DrawObjects(Bullet.Instances);
            //DrawObjects(MeleeAttack.Instances);
            DrawObjects(GameObject.Objects);

            if (!IsGameActive)
            {
                SpriteBatch.DrawString(Fonts.Jacquard, (!Player.Health.IsAlive) ? $"You were killed!" : $"You win! Score: {Score}", new Vector2(Graphics.PreferredBackBufferWidth * 0.35f, Graphics.PreferredBackBufferHeight * 0.70f), Color.White);
                
                SpriteBatch.DrawString(Fonts.Jacquard, $"Game is over, press R to restart", new Vector2(Graphics.PreferredBackBufferWidth * 0.25f, Graphics.PreferredBackBufferHeight * 0.80f), Color.White);
                //SpriteBatch.DrawString(Fonts.Jacquard, $"Score: {Score}", new Vector2(Graphics.PreferredBackBufferWidth * 0.25f, Graphics.PreferredBackBufferHeight * 0.90f), Color.White);
            }
            SpriteBatch.End();



            base.Draw(gameTime);
        }

        private void DrawObjects(IEnumerable<GameObject> list)
        {
            int objCount = list.Count();
            IEnumerable<GameObject> newList = list.Take(objCount);
            foreach (GameObject obj in newList)
            {
                //Trace.WriteLine($"...{obj}");
                if (list.Count() != objCount) break;
                ((IDrawable)obj).Draw(SpriteBatch);
                //Trace.WriteLine($"at position {(obj as Player).Position}");
            }
        }

        private void UpdateObjects<T>(IEnumerable<T> list, GameTime gameTime) where T : GameObject
        {
            int objCount = list.Count();
            IEnumerable<GameObject> newList = list.Take(objCount);
            foreach (GameObject obj in newList)
            {
                //Trace.WriteLine($"...{obj}");
                if (list.Count() != objCount) break;

                if (obj is Enemy) 
                    ((Enemy)obj).Update(gameTime, Map.Tiles);
                //else if(obj is Bullet)
                //    ((Bullet)obj).Update(gameTime, Enemy.Instances);
                else 
                    obj.Update(gameTime, GameObject.Objects);
                //Trace.WriteLine($"at position {(obj as Player).Position}");
            }
        }

        private void Reset()
        {
            IsGameActive = true;
            Map = new("../../../Maps/1.csv", "../../../Maps/Collisions/tilesetCollisions.csv", "../../../Maps/Spawners/tilesetSpawners.csv", 16, 3);
            TileMap.RemainEntities = new Random().Next(TileMap.MinEntities, TileMap.MaxEntities);
            Enemy.Instances.Clear();
            Bullet.Instances.Clear();
            GameObject.Objects.Clear();

            Player = new Player(new Vector2(100, 100));
            //GameObject.Objects.Add(Player);
            for (int i = 0; i < 3; i++) new Enemy();
        }
    }
}
