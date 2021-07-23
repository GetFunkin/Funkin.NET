using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Graphics.Sprites.Characters
{
    public static class FunkinCharacterAnimator
    {
        public static Dictionary<string, TextureAnimation> LoadTextures(this CharacterDrawable drawable, TextureStore textures)
        {
            Dictionary<string, TextureAnimation> collection = new();

            switch (drawable.Character)
            {
                case "gf":
                    break;
                case "gf-christmas":
                    break;
                case "gf-car":
                    break;
                case "gf-pixel":
                    break;
                case "dad":
                    break;
                case "spooky":
                    break;
                case "mom":
                    break;
                case "mom-car":
                    break;
                case "monster":
                    break;
                case "pico":
                    break;
                case "bf":
                    break;
                case "bf-christmas":
                    break;
                case "bf-car":
                    break;
                case "bf-pixel":
                    break;
                case "bf-pixel-dead":
                    break;
                case "senpai":
                    break;
                case "senpai-angry":
                    break;
                case "spirit":
                    break;
                case "parents-christmas":
                    break;
            }

            return collection;
        }

        private static TextureAnimation RetrieveByPrefix(this CharacterDrawable drawable, TextureStore textures, string identifier, string prefix, int frameCount)
        {
            TextureAnimation animation = Create();

            return animation;
        }

        private static TextureAnimation Create() => new()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            IsPlaying = false,
            Loop = false
        };
    }
}