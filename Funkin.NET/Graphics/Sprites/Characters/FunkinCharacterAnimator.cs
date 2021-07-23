using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Funkin.NET.Graphics.Sprites.Characters
{
    public static class FunkinCharacterAnimator
    {
        public static Dictionary<string, TextureAnimation> LoadTextures(this CharacterDrawable drawable,
            TextureStore textures)
        {
            Dictionary<string, TextureAnimation> collection = new();

            switch (drawable.Character)
            {
                case "gf":
                    drawable.RegisterGirlfriend(textures);
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

        #region Asset Retrieval

        private static string GetPath(string character) => $"Characters/{character}";

        private static string GetPath(string character, string image) => $"{GetPath(character)}/{image}";

        private static string FrameAsString(int frame)
        {
            if (frame == 0)
                return "0000";

            int zeroCount = 4;
            int frameLength = frame.ToString().Length;
            zeroCount -= frameLength;

            StringBuilder builder = new();

            return zeroCount <= 0
                ? builder.Append(frame).ToString()
                : builder.Append('0', zeroCount).Append(frame).ToString();
        }

        private static TextureAnimation RetrieveByPrefix(this CharacterDrawable drawable, TextureStore textures,
            string prefix, int frameLengthDivider, int frameCount, bool looped)
        {
            TextureAnimation animation = Create();

            for (int i = 0; i < frameCount; i++)
            {
                string texturePath = $"{GetPath(drawable.Character, prefix)}{FrameAsString(i)}";
                animation.AddFrame(textures.Get(texturePath), 1D / frameLengthDivider * 1000D);
            }

            animation.Loop = looped;

            return animation;
        }

        private static void AddByPrefix(this CharacterDrawable drawable, TextureStore textures,
            string identifier, string prefix, int frameLengthProvider, int frameCount, bool looped = false) =>
            drawable.Animations[identifier] =
                drawable.RetrieveByPrefix(textures, prefix, frameLengthProvider, frameCount, looped);

        private static TextureAnimation RetrieveByIndices(this CharacterDrawable drawable, TextureStore textures,
            string prefix, int frameLengthDivider, bool looped, params int[] indices)
        {
            TextureAnimation animation = Create();

            foreach (int frame in indices)
            {
                string texturePath = $"{GetPath(drawable.Character, prefix)}{FrameAsString(frame)}";
                animation.AddFrame(textures.Get(texturePath), 1D / frameLengthDivider * 1000D);
            }

            animation.Loop = looped;

            return animation;
        }

        private static void AddByIndices(this CharacterDrawable drawable, TextureStore textures, string identifier,
            string prefix, int frameLengthDivider, bool looped = false, params int[] indices) =>
            drawable.Animations[identifier] =
                drawable.RetrieveByIndices(textures, prefix, frameLengthDivider, looped, indices);

        private static TextureAnimation Create() => new()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            IsPlaying = false,
            Loop = false
        };

        #endregion

        #region Animation Population

        // ==============
        // prefix regex -
        // search: animation\.addByPrefix\('(.*?)', '(.*?)', (.*?), false\);
        //  $1 = identifier
        //  $2 = prefix
        //  $3 = frame divider
        //  $4 = looped
        //
        // replace: drawable.AddByPrefix(textures, "$1", "$2", $3, $4);
        //
        // notes: frame count must be specified manually
        // ==============

        // ==============
        // indices regex -
        // search: animation\.addByIndices\('(.*?)', '(.*?)', \[(.*?)\], "", (.*?), (.*?)\);
        //  $1 = identifier
        //  $2 = prefix
        //  $3 = indices
        //  $4 = frame divider
        //  $5 = looped
        //
        // replace: drawable.AddByIndices(textures, "$1", "$2", $4, $5, $3);
        //
        // notes: frame count not needed, indices provided. may need to swap ' with ". may need to remove false check
        // ==============

        // ==============
        // offset regex -
        // search: addOffset\('(.*?)', (.*?), (.*?)\);
        //  $1 = identifier
        //  $2 = x
        //  $3 = y
        //
        // replace: drawable.AssignOffset("$1", new Vector2($2f, $3f));
        //
        // notes: automatically appends "f"
        // ==============

        public static void RegisterGirlfriend(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "cheer", "GF Cheer", 24, 21);
            drawable.AddByPrefix(textures, "singLEFT", "GF left note", 24, 15);
            drawable.AddByPrefix(textures, "singRIGHT", "GF Right Note", 24, 15);
            drawable.AddByPrefix(textures, "singUP", "GF Up Note", 24, 7);
            drawable.AddByPrefix(textures, "singDOWN", "GF Down Note", 24, 20);
            drawable.AddByPrefix(textures, "scared", "GF FEAR", 24, 4, true);
            drawable.AddByIndices(textures, "sad", "gf sad", 24, false, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            drawable.AddByIndices(textures, "danceLeft", "GF Dancing Beat", 24, false, 30, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            drawable.AddByIndices(textures, "danceRight", "GF Dancing Beat", 24, false, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29);
            drawable.AddByIndices(textures, "hairBlow", "GF Dancing Beat Hair blowing", 24, true, 0, 1, 2, 3);
            drawable.AddByIndices(textures, "hairFall", "GF Dancing Beat Hair Landing", 24, false, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            drawable.AssignOffset("sad", new Vector2(-2f, -2f));
            drawable.AssignOffset("danceLeft", new Vector2(0f, -9f));
            drawable.AssignOffset("danceRight", new Vector2(0f, -9f));
            drawable.AssignOffset("singUP", new Vector2(0f, 4f));
            drawable.AssignOffset("singRIGHT", new Vector2(0f, -20f));
            drawable.AssignOffset("singLEFT", new Vector2(0f, -19f));
            drawable.AssignOffset("singDOWN", new Vector2(0f, -20f));
            drawable.AssignOffset("hairBlow", new Vector2(45f, -8f));
            drawable.AssignOffset("hairFall", new Vector2(0f, -9f));
            drawable.AssignOffset("scared", new Vector2(-2f, -17f));
        }

        #endregion
    }
}