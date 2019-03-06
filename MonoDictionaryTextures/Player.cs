using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoDictionaryTextures;
using Engine.Engines;
using Microsoft.Xna.Framework.Input;

namespace Sprites
{
    public class Player : DrawableGameComponent
    {
        public Texture2D playercharacter;
        public Vector2 Position;
        public Rectangle BoundingRect;
        private float speed = 5f;
        public Queue<CharacterSelection> Projectiles = new Queue<CharacterSelection>();
        bool ShowProjectiles = false;
        // Constructor expects to see a loaded Texture
        // and a start position
        public Player(Game g, Vector2 startPosition,Texture2D texture) : base(g)
        {
            //
            // Take a copy of the texture passed down
            playercharacter = texture;
            // Take a copy of the start position
            Position = startPosition;
            // Calculate the bounding rectangle
            //BoundingRect = new Rectangle((int)startPosition.X, (int)startPosition.Y, Image.Width, Image.Height);
            g.Components.Add(this);
        }
        public override void Update(GameTime gameTime)
        {
            if (InputEngine.IsKeyHeld(Keys.Z))
            {
                ShowProjectiles = true;
            }
            else ShowProjectiles = false;

            if(ShowProjectiles)
            {
                if(InputEngine.IsKeyPressed(Keys.X))
                {
                    Projectiles.Enqueue(Projectiles.Dequeue());
                }
            }
            else hideQueue(); 

            if (InputEngine.IsKeyHeld(Keys.D))
                Position += new Vector2(speed,0);
            if (InputEngine.IsKeyHeld(Keys.A))
                Position -= new Vector2(speed, 0);
            if (InputEngine.IsKeyHeld(Keys.W))
                Position -= new Vector2(0,speed);
            if (InputEngine.IsKeyHeld(Keys.S))
                Position += new Vector2(0,speed);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            SpriteFont font = Game.Services.GetService<SpriteFont>();
            sp.Begin();
            if (Visible)
            {
                sp.Draw(playercharacter, Position, Color.White);
                if (ShowProjectiles)
                    drawQueue(sp);
            }
            sp.End();

            base.Draw(gameTime);
        }

        public void drawQueue(SpriteBatch sp)
        {
            //Vector2 startpos = Position - new Vector2(Position.X, 100);
            //foreach (var item in Projectiles)
            //{
            //    item.Visible = true;
            //    sp.Draw(item.Texture, startpos, Color.White);
            //    startpos += new Vector2(item.Texture.Width + 10, 0);
            //}
            sp.Draw(Projectiles.Peek().Texture, Position, Color.White);
        }

        public void hideQueue()
        {
            foreach (var item in Projectiles)
                item.Visible = false;

        }
    }
}
