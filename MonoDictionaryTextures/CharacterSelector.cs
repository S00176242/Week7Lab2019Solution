using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoDictionaryTextures
{
    public class CharacterSelector : DrawableGameComponent
    {
        Texture2D _background;
        bool HudUp = false;
        public static CharacterSelection current;

        static public Dictionary<string, CharacterSelection> Characters = new Dictionary<string, CharacterSelection>();
                

        public CharacterSelector(Game g, Texture2D backgound) : base(g)
        {
            _background = backgound;
            g.Components.Add(this);
            DrawOrder = 0;
            
        }
        // This static method must be called after the class is added as a game component
        public static void SetupCharacterMenu()
        {
            Vector2 pos = new Vector2(50, 50);
            // Draw 5 badges per row
            int ColCount = 5;
            foreach (KeyValuePair<string, CharacterSelection> entry in Characters)
            {
                entry.Value.Position = pos;
                if (ColCount-- > 1)
                    pos += new Vector2(entry.Value.Texture.Width + 10, 0);
                else
                {
                    ColCount = 5;
                    pos += new Vector2(-pos.X + 50, entry.Value.Texture.Height + 10);
                }
            }
            current = Characters.First().Value;
        }
        public override void Update(GameTime gameTime)
        {
            if (InputEngine.IsKeyPressed(Keys.C))
                HudUp = !HudUp;
            if(HudUp)
            {
                foreach (KeyValuePair<string, CharacterSelection> entry in Characters)
                    entry.Value.Visible = true;
                // Change up current Selection if another is selected
                var found = Characters.FirstOrDefault(c => c.Value.Selected && c.Value.Name != current.Name).Value;
                    // if changed
                    if(found != null)
                    {
                    // Find the player 
                    var p = Game.Components.FirstOrDefault(c => c.GetType() == typeof(Player));
                    // Update the current Selection 
                    current.Selected = false;
                    current = found;
                    // Update the player texture
                        if (p != null)
                        {
                        CharacterSelection projectile = new CharacterSelection(Game, current.Texture, Vector2.Zero, current.Name);
                            ((Player)p).Projectiles.Enqueue(projectile);
                        }
                    }

            } else
            {
                foreach (KeyValuePair<string, CharacterSelection> entry in Characters)
                    entry.Value.Visible = false;

            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (HudUp)
            {
                SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
                SpriteFont font = Game.Services.GetService<SpriteFont>();
                sp.Begin();
                sp.Draw(_background, Game.GraphicsDevice.Viewport.Bounds, Color.White);
                //sp.Draw(current.Texture, current.Position, Color.White);
                sp.End();
            }
            base.Draw(gameTime);
        }
    }
}
