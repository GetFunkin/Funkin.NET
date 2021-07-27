using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Funkin.NET.Game.Graphics.Composites.Characters
{
    public class HealthIconDrawable : CompositeDrawable
    {
        public string Character { get; protected set; }

        public bool Flipped { get; protected set; }

        public Sprite Alive { get; protected set; }

        public Sprite Dead { get; protected set; }

        public HealthIconDrawable(string character, bool flip = false)
        {
            Character = character;
            Flipped = flip;
        }

        public void Set(Type type)
        {
            switch (type)
            {
                case Type.Alive:
                    Alive.Show();
                    Dead.Hide();
                    break;

                case Type.Dead:
                    Alive.Hide();
                    Dead.Show();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {

            Alive = new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AlwaysPresent = true
            };

            Dead = new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AlwaysPresent = true
            };

            switch (Character)
            {
                case "pico":
                    Alive.Texture = textures.Get("Icons/pico-funny");
                    Dead.Texture = textures.Get("Icons/pico-unfunny");
                    break;

                default:
                    Alive.Texture = textures.Get($"Icons/{Character}-alive");
                    Dead.Texture = textures.Get($"Icons/{Character}-dead");
                    break;
            }

            AddInternal(Alive);
            AddInternal(Dead);

            Alive.Show();
            Dead.Hide();

            if (!Flipped)
                return;

            Alive.Rotation = 180f;
            Dead.Rotation = 180f;
            Alive.Scale = new Vector2(Alive.Scale.X, -Alive.Scale.Y);
            Dead.Scale = new Vector2(Dead.Scale.X, -Dead.Scale.Y);
        }

        public enum Type
        {
            Alive,
            Dead
        }
    }
}