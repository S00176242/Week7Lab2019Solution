using AudioPlayer;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoDictionaryTextures
{
   public class CharacterSelection : DrawableGameComponent
    {
        public Texture2D Texture;
        public Rectangle Bound;
        public bool Selected = false;
        public string Name;
        public Vector2 Position;

        public CharacterSelection(Game g, Texture2D tx, Vector2 pos, string name) : base(g)
        {
            Texture = tx;
            Bound = new Rectangle(pos.ToPoint(), new Point(tx.Width, tx.Height));
            Name = name;
            g.Components.Add(this);
            Position = pos;
            Visible = false;
            DrawOrder = 1;
        }

        public override void Update(GameTime gameTime)

        {
            if (!Visible) return;
            if (InputEngine.IsMouseLeftClick())
            {
                Bound = new Rectangle(Position.ToPoint(), new Point(Texture.Width, Texture.Height));
                if (Bound.Contains(InputEngine.MousePosition))
                {
                    Selected = true;
                    SoundEffectInstance _player = null;
                    AudioManager.Play(ref _player, Name);
                }
            }
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            SpriteFont font = Game.Services.GetService<SpriteFont>();
            sp.Begin();
            if (Visible)
            {
                sp.Draw(Texture, Bound, Color.White);
                sp.DrawString(font, Name, Bound.Center.ToVector2() - font.MeasureString(Name) / 2, Color.White);
            }
            sp.End();
            base.Draw(gameTime);
        }
    }
}
