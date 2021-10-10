using Funkin.NET.Resources;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Funkin.NET.Default.Graphics.Composites.Characters
{
    public static class FunkinCharacterAnimator
    {
        public const float PixelZoom = 6f;

        public static void LoadTextures(this CharacterDrawable drawable, TextureStore textures)
        {
            switch (drawable.Character)
            {
                case "gf":
                    drawable.RegisterGirlfriend(textures);
                    break;

                case "gf-christmas":
                    drawable.RegisterGirlfriendChristmas(textures);
                    break;

                case "gf-car":
                    drawable.RegisterGirlfriendCar(textures);
                    break;

                case "gf-pixel":
                    drawable.RegisterGirlfriendPixel(textures);
                    break;

                case "dad":
                    drawable.RegisterDad(textures);
                    break;

                case "spooky":
                    drawable.RegisterSpooky(textures);
                    break;

                case "mom":
                    drawable.RegisterMom(textures);
                    break;

                case "mom-car":
                    drawable.RegisterMomCar(textures);
                    break;

                case "monster":
                    drawable.RegisterMonster(textures);
                    break;

                case "monster-christmas":
                    drawable.RegisterMonsterChristmas(textures);
                    break;

                case "pico":
                    drawable.RegisterPico(textures);
                    break;

                case "bf":
                    drawable.RegisterBoyfriend(textures);
                    break;

                case "bf-christmas":
                    drawable.RegisterBoyfriendChristmas(textures);
                    break;

                case "bf-car":
                    drawable.RegisterBoyfriendCar(textures);
                    break;

                case "bf-pixel":
                    drawable.RegisterBoyfriendPixel(textures);
                    break;

                case "bf-pixel-dead":
                    drawable.RegisterBoyfriendPixelDead(textures);
                    break;

                case "senpai":
                    drawable.RegisterSenpai(textures);
                    break;

                case "senpai-angry":
                    drawable.RegisterSenpaiAngry(textures);
                    break;

                case "spirit":
                    drawable.RegisterSpirit(textures);
                    break;

                case "parents-christmas":
                    drawable.RegisterParentsChristmas(textures);
                    break;
            }
        }

        #region Asset Retrieval

        private static string GetPath(string character) => $"Characters/{character}";

        private static string GetPath(string character, string image) => $"{GetPath(character)}/{image}";

        private static TextureAnimation RetrieveByPrefix(this CharacterDrawable drawable, TextureStore textures,
            string prefix, int frameLengthDivider, int frameCount, bool looped)
        {
            TextureAnimation animation = Create();

            for (int i = 0; i < frameCount; i++)
            {
                string texturePath = $"{GetPath(drawable.Character, prefix)}{PathHelper.Atlas.FrameAsString(i)}";
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
                string texturePath = $"{GetPath(drawable.Character, prefix)}{PathHelper.Atlas.FrameAsString(frame)}";
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
        // search: animation\.addByPrefix\('(.*?)', '(.*?)', (.*?), (.*?)\);
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
            drawable.AddByIndices(textures, "danceLeft", "GF Dancing Beat", 24, false, 
                30, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            drawable.AddByIndices(textures, "danceRight", "GF Dancing Beat", 24, false, 
                15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29);
            drawable.AddByIndices(textures, "hairBlow", "GF Dancing Beat Hair blowing", 24, true, 0, 1, 2, 3);
            drawable.AddByIndices(textures, "hairFall", "GF Dancing Beat Hair Landing", 24, false, 
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
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

            drawable.SetAnimation("danceRight");
        }

        public static void RegisterGirlfriendChristmas(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "cheer", "GF Cheer", 24, 21);
            drawable.AddByPrefix(textures, "singLEFT", "GF left note", 24, 15);
            drawable.AddByPrefix(textures, "singRIGHT", "GF Right Note", 24, 15);
            drawable.AddByPrefix(textures, "singUP", "GF Up Note", 24, 7);
            drawable.AddByPrefix(textures, "singDOWN", "GF Down Note", 24, 20);
            drawable.AddByIndices(textures, "sad", "gf sad", 24, false, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            drawable.AddByIndices(textures, "danceLeft", "GF Dancing Beat", 24, false,
                30, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            drawable.AddByIndices(textures, "danceRight", "GF Dancing Beat", 24, false, 
                15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29);
            drawable.AddByIndices(textures, "hairBlow", "GF Dancing Beat Hair blowing", 24, true, 0, 1, 2, 3);
            drawable.AddByIndices(textures, "hairFall", "GF Dancing Beat Hair Landing", 24, false, 
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            drawable.AddByPrefix(textures, "scared", "GF FEAR", 24, 4, true);
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

            drawable.SetAnimation("danceRight");
        }

        public static void RegisterGirlfriendCar(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByIndices(textures, "singUP", "GF Dancing Beat Hair blowing CAR", 24, false, 0);
            drawable.AddByIndices(textures, "danceLeft", "GF Dancing Beat Hair blowing CAR", 24, false, 
                30, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            drawable.AddByIndices(textures, "danceRight", "GF Dancing Beat Hair blowing CAR", 24, false, 
                15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29);

            drawable.SetAnimation("danceRight");
        }

        public static void RegisterGirlfriendPixel(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByIndices(textures, "singUP", "GF IDLE", 24, false, 2);
            drawable.AddByIndices(textures, "danceLeft", "GF IDLE", 24, false,
                30, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            drawable.AddByIndices(textures, "danceRight", "GF IDLE", 24, false,
                15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29);

            drawable.PixelScaling = true;
            drawable.SetAnimation("danceRight");
        }

        public static void RegisterDad(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "Dad idle dance", 24, 13, true);
            drawable.AddByPrefix(textures, "singUP", "Dad Sing Note UP", 24, 5, true);
            drawable.AddByPrefix(textures, "singRIGHT", "Dad Sing Note RIGHT", 24, 19, true);
            drawable.AddByPrefix(textures, "singDOWN", "Dad Sing Note DOWN", 24, 7, true);
            drawable.AddByPrefix(textures, "singLEFT", "Dad Sing Note LEFT", 24, 1, true);
            drawable.AssignOffset("singUP", new Vector2(-6f, 50f));
            drawable.AssignOffset("singRIGHT", new Vector2(0f, 27f));
            drawable.AssignOffset("singLEFT", new Vector2(-10f, 10f));
            drawable.AssignOffset("singDOWN", new Vector2(0f, -30f));

            drawable.SetAnimation("idle");
        }

        public static void RegisterSpooky(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "singUP", "spooky UP NOTE", 24, 5);
            drawable.AddByPrefix(textures, "singDOWN", "spooky DOWN note", 24, 25);
            drawable.AddByPrefix(textures, "singLEFT", "note sing left", 24, 15);
            drawable.AddByPrefix(textures, "singRIGHT", "spooky sing right", 24, 25);
            drawable.AddByIndices(textures, "danceLeft", "spooky dance idle", 12, false, 0, 2, 6);
            drawable.AddByIndices(textures, "danceRight", "spooky dance idle", 12, false, 8, 10, 12, 14);
            drawable.AssignOffset("singUP", new Vector2(-20f, 26f));
            drawable.AssignOffset("singRIGHT", new Vector2(-130f, -14f));
            drawable.AssignOffset("singLEFT", new Vector2(130f, -10f));
            drawable.AssignOffset("singDOWN", new Vector2(-50f, -130f));

            drawable.SetAnimation("danceRight");
        }

        public static void RegisterMom(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "Mom Idle", 24, 14);
            drawable.AddByPrefix(textures, "singUP", "Mom Up Pose", 24, 15);
            drawable.AddByPrefix(textures, "singDOWN", "MOM DOWN POSE", 24, 15);
            drawable.AddByPrefix(textures, "singLEFT", "Mom Left Pose", 24, 10);
            // ANIMATION IS CALLED MOM LEFT POSE BUT ITS FOR THE RIGHT
            // CUZ DAVE IS DUMB!
            drawable.AddByPrefix(textures, "singRIGHT", "Mom Pose Left", 24, 10);
            drawable.AssignOffset("singUP", new Vector2(14f, 71f));
            drawable.AssignOffset("singRIGHT", new Vector2(10f, -60f));
            drawable.AssignOffset("singLEFT", new Vector2(250f, -23f));
            drawable.AssignOffset("singDOWN", new Vector2(20f, -160f));

            drawable.SetAnimation("idle");
        }

        public static void RegisterMomCar(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "Mom Idle", 24, 14);
            drawable.AddByPrefix(textures, "singUP", "Mom Up Pose", 24, 15);
            drawable.AddByPrefix(textures, "singDOWN", "MOM DOWN POSE", 24, 15);
            drawable.AddByPrefix(textures, "singLEFT", "Mom Left Pose", 24, 10);
            // ANIMATION IS CALLED MOM LEFT POSE BUT ITS FOR THE RIGHT
            // CUZ DAVE IS DUMB!
            drawable.AddByPrefix(textures, "singRIGHT", "Mom Pose Left", 24, 10);
            drawable.AssignOffset("singUP", new Vector2(14f, 71f));
            drawable.AssignOffset("singRIGHT", new Vector2(10f, -60f));
            drawable.AssignOffset("singLEFT", new Vector2(250f, -23f));
            drawable.AssignOffset("singDOWN", new Vector2(20f, -160f));

            drawable.SetAnimation("idle");
        }

        public static void RegisterMonster(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "monster idle", 24, 15);
            drawable.AddByPrefix(textures, "singUP", "monster up note", 24, 24);
            drawable.AddByPrefix(textures, "singDOWN", "monster down", 24, 30);
            drawable.AddByPrefix(textures, "singLEFT", "Monster left note", 24, 20);
            drawable.AddByPrefix(textures, "singRIGHT", "Monster Right note", 24, 15);
            drawable.AssignOffset("singUP", new Vector2(-20f, 50f));
            drawable.AssignOffset("singRIGHT", new Vector2(-51f, 0f));
            drawable.AssignOffset("singLEFT", new Vector2(-30f, 0f));
            drawable.AssignOffset("singDOWN", new Vector2(-30f, -40f));

            drawable.SetAnimation("idle");
        }

        public static void RegisterMonsterChristmas(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "monster idle", 24, 14);
            drawable.AddByPrefix(textures, "singUP", "monster up note", 24, 24);
            drawable.AddByPrefix(textures, "singDOWN", "monster down", 24, 30);
            drawable.AddByPrefix(textures, "singLEFT", "Monster left note", 24, 20);
            drawable.AddByPrefix(textures, "singRIGHT", "Monster Right note", 24, 15);
            drawable.AssignOffset("singUP", new Vector2(-20f, 50f));
            drawable.AssignOffset("singRIGHT", new Vector2(-51f, 0f));
            drawable.AssignOffset("singLEFT", new Vector2(-30f, 0f));
            drawable.AssignOffset("singDOWN", new Vector2(-40f, -94f));

            drawable.SetAnimation("idle");
        }

        public static void RegisterPico(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "Pico Idle Dance", 24, 14, true);
            drawable.AddByPrefix(textures, "singUP", "pico Up note", 24, 14);
            drawable.AddByPrefix(textures, "singDOWN", "Pico Down Note", 24, 25);
            drawable.AddByPrefix(textures, "singUPmiss", "pico Up note miss", 24, 11, true);
            drawable.AddByPrefix(textures, "singDOWNmiss", "Pico Down Note MISS", 24, 25, true);
            drawable.AssignOffset("singUP", new Vector2(-29f, 27f));
            drawable.AssignOffset("singRIGHT", new Vector2(-68f, -7f));
            drawable.AssignOffset("singLEFT", new Vector2(65f, 9f));
            drawable.AssignOffset("singDOWN", new Vector2(200f, -70f));
            drawable.AssignOffset("singUPmiss", new Vector2(-19f, 67f));
            drawable.AssignOffset("singRIGHTmiss", new Vector2(-60f, 41f));
            drawable.AssignOffset("singLEFTmiss", new Vector2(62f, 64f));
            drawable.AssignOffset("singDOWNmiss", new Vector2(210f, -28f));

            if (drawable.Type == CharacterType.Boyfriend)
            {
                drawable.AddByPrefix(textures, "singLEFT", "Pico NOTE LEFT", 24, 17);
                drawable.AddByPrefix(textures, "singRIGHT", "Pico Note Right", 24, 17);
                drawable.AddByPrefix(textures, "singRIGHTmiss", "Pico Note Right Miss", 24, 25);
                drawable.AddByPrefix(textures, "singLEFTmiss", "Pico NOTE LEFT miss", 24, 25);
            }
            else
            {
                // Need to be flipped! REDO THIS LATER!
                drawable.AddByPrefix(textures, "singLEFT", "Pico Note Right", 24, 17);
                drawable.AddByPrefix(textures, "singRIGHT", "Pico NOTE LEFT", 24, 17);
                drawable.AddByPrefix(textures, "singRIGHTmiss", "Pico NOTE LEFT miss", 24, 25);
                drawable.AddByPrefix(textures, "singLEFTmiss", "Pico Note Right Miss", 24, 25);
            }

            drawable.Flip = true;
            drawable.SetAnimation("idle");
        }

        public static void RegisterBoyfriend(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "BF idle dance", 24, 14);
            drawable.AddByPrefix(textures, "singUP", "BF NOTE UP", 24, 15);
            drawable.AddByPrefix(textures, "singLEFT", "BF NOTE LEFT", 24, 15);
            drawable.AddByPrefix(textures, "singRIGHT", "BF NOTE RIGHT", 24, 62);
            drawable.AddByPrefix(textures, "singDOWN", "BF NOTE DOWN", 24, 30);
            drawable.AddByPrefix(textures, "singUPmiss", "BF NOTE UP MISS", 24, 24);
            drawable.AddByPrefix(textures, "singLEFTmiss", "BF NOTE LEFT MISS", 24, 34);
            drawable.AddByPrefix(textures, "singRIGHTmiss", "BF NOTE RIGHT MISS", 24, 46);
            drawable.AddByPrefix(textures, "singDOWNmiss", "BF NOTE DOWN MISS", 24, 29);
            drawable.AddByPrefix(textures, "hey", "BF HEY!!", 24, 26);
            drawable.AddByPrefix(textures, "firstDeath", "BF dies", 24, 58);
            drawable.AddByPrefix(textures, "deathLoop", "BF Dead Loop", 24, 34, true);
            drawable.AddByPrefix(textures, "deathConfirm", "BF Dead confirm", 24, 34);
            drawable.AddByPrefix(textures, "scared", "BF idle shaking", 24, 4, true);
            drawable.AssignOffset("idle", new Vector2(-5f, 0f));
            drawable.AssignOffset("singUP", new Vector2(-29f, 27f));
            drawable.AssignOffset("singRIGHT", new Vector2(-38f, -7f));
            drawable.AssignOffset("singLEFT", new Vector2(12f, -6f));
            drawable.AssignOffset("singDOWN", new Vector2(-10f, -50f));
            drawable.AssignOffset("singUPmiss", new Vector2(-29f, 27f));
            drawable.AssignOffset("singRIGHTmiss", new Vector2(-30f, 21f));
            drawable.AssignOffset("singLEFTmiss", new Vector2(12f, 24f));
            drawable.AssignOffset("singDOWNmiss", new Vector2(-11f, -19f));
            drawable.AssignOffset("hey", new Vector2(7f, 4f));
            drawable.AssignOffset("firstDeath", new Vector2(37f, 11f));
            drawable.AssignOffset("deathLoop", new Vector2(37f, 5f));
            drawable.AssignOffset("deathConfirm", new Vector2(37f, 69f));
            drawable.AssignOffset("scared", new Vector2(-4f, 0f));

            drawable.Flip = true;
            drawable.SetAnimation("idle");
        }

        public static void RegisterBoyfriendChristmas(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "BF idle dance", 24, 14, true);
            drawable.AddByPrefix(textures, "singUP", "BF NOTE UP", 24, 15);
            drawable.AddByPrefix(textures, "singLEFT", "BF NOTE LEFT", 24, 15);
            drawable.AddByPrefix(textures, "singRIGHT", "BF NOTE RIGHT", 24, 62);
            drawable.AddByPrefix(textures, "singDOWN", "BF NOTE DOWN", 24, 30);
            drawable.AddByPrefix(textures, "singUPmiss", "BF NOTE UP MISS", 24, 24);
            drawable.AddByPrefix(textures, "singLEFTmiss", "BF NOTE LEFT MISS", 24, 34);
            drawable.AddByPrefix(textures, "singRIGHTmiss", "BF NOTE RIGHT MISS", 24, 46);
            drawable.AddByPrefix(textures, "singDOWNmiss", "BF NOTE DOWN MISS", 24, 29);
            drawable.AddByPrefix(textures, "hey", "BF HEY!!", 24, 26);
            drawable.AssignOffset("idle", new Vector2(-5f, 0f));
            drawable.AssignOffset("singUP", new Vector2(-29f, 27f));
            drawable.AssignOffset("singRIGHT", new Vector2(-38f, -7f));
            drawable.AssignOffset("singLEFT", new Vector2(12f, -6f));
            drawable.AssignOffset("singDOWN", new Vector2(-10f, -50f));
            drawable.AssignOffset("singUPmiss", new Vector2(-29f, 27f));
            drawable.AssignOffset("singRIGHTmiss", new Vector2(-30f, 21f));
            drawable.AssignOffset("singLEFTmiss", new Vector2(12f, 24f));
            drawable.AssignOffset("singDOWNmiss", new Vector2(-11f, -19f));
            drawable.AssignOffset("hey", new Vector2(7f, 4f));

            drawable.Flip = true;
            drawable.SetAnimation("idle");
        }

        public static void RegisterBoyfriendCar(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "BF idle dance", 24, 14);
            drawable.AddByPrefix(textures, "singUP", "BF NOTE UP", 24, 15);
            drawable.AddByPrefix(textures, "singLEFT", "BF NOTE LEFT", 24, 16);
            drawable.AddByPrefix(textures, "singRIGHT", "BF NOTE RIGHT", 24, 62);
            drawable.AddByPrefix(textures, "singDOWN", "BF NOTE DOWN", 24, 30);
            drawable.AddByPrefix(textures, "singUPmiss", "BF NOTE UP MISS", 24, 24);
            drawable.AddByPrefix(textures, "singLEFTmiss", "BF NOTE LEFT MISS", 24, 34);
            drawable.AddByPrefix(textures, "singRIGHTmiss", "BF NOTE RIGHT MISS", 24, 46);
            drawable.AddByPrefix(textures, "singDOWNmiss", "BF NOTE DOWN MISS", 24, 29);
            drawable.AssignOffset("idle", new Vector2(-5f, 0f));
            drawable.AssignOffset("singUP", new Vector2(-29f, 27f));
            drawable.AssignOffset("singRIGHT", new Vector2(-38f, -7f));
            drawable.AssignOffset("singLEFT", new Vector2(12f, -6f));
            drawable.AssignOffset("singDOWN", new Vector2(-10f, -50f));
            drawable.AssignOffset("singUPmiss", new Vector2(-29f, 27f));
            drawable.AssignOffset("singRIGHTmiss", new Vector2(-30f, 21f));
            drawable.AssignOffset("singLEFTmiss", new Vector2(12f, 24f));
            drawable.AssignOffset("singDOWNmiss", new Vector2(-11f, -19f));

            drawable.Flip = true;
            drawable.SetAnimation("idle");
        }

        public static void RegisterBoyfriendPixel(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "BF IDLE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singUP", "BF UP NOTE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singLEFT", "BF LEFT NOTE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singRIGHT", "BF RIGHT NOTE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singDOWN", "BF DOWN NOTE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singUPmiss", "BF UP MISS instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singLEFTmiss", "BF LEFT MISS instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singRIGHTmiss", "BF RIGHT MISS instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singDOWNmiss", "BF DOWN MISS instance 1", 24, 14);

            drawable.Flip = true;
            drawable.PixelScaling = true;
            drawable.SetAnimation("idle");
        }

        public static void RegisterBoyfriendPixelDead(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "singUP", "BF Dies pixel", 24, 58);
            drawable.AddByPrefix(textures, "firstDeath", "BF Dies pixel", 24, 58);
            drawable.AddByPrefix(textures, "deathLoop", "Retry Loop", 24, 34, true);
            drawable.AddByPrefix(textures, "deathConfirm", "RETRY CONFIRM", 24, 34);
            drawable.AssignOffset("deathLoop", new Vector2(-37f, 0f));
            drawable.AssignOffset("deathConfirm", new Vector2(-37f, 0f));

            drawable.Flip = true;
            drawable.PixelScaling = true;
            drawable.SetAnimation("firstDeath");
        }

        public static void RegisterSenpai(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "Senpai Idle instance 1", 24, 28);
            drawable.AddByPrefix(textures, "singUP", "SENPAI UP NOTE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singLEFT", "SENPAI LEFT NOTE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singRIGHT", "SENPAI RIGHT NOTE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singDOWN", "SENPAI DOWN NOTE instance 1", 24, 14);
            drawable.AssignOffset("singUP", new Vector2(5f, 37f));
            drawable.AssignOffset("singLEFT", new Vector2(40f, 0f));
            drawable.AssignOffset("singDOWN", new Vector2(14f, 0f));

            drawable.PixelScaling = true;
            drawable.SetAnimation("idle");
        }

        public static void RegisterSenpaiAngry(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "Angry Senpai Idle instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singUP", "Angry Senpai UP NOTE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singLEFT", "Angry Senpai LEFT NOTE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singRIGHT", "Angry Senpai RIGHT NOTE instance 1", 24, 14);
            drawable.AddByPrefix(textures, "singDOWN", "Angry Senpai DOWN NOTE instance 1", 24, 14);
            drawable.AssignOffset("singUP", new Vector2(5f, 37f));
            drawable.AssignOffset("singLEFT", new Vector2(40f, 0f));
            drawable.AssignOffset("singDOWN", new Vector2(14f, 0f));

            drawable.PixelScaling = true;
            drawable.SetAnimation("idle");
        }

        public static void RegisterSpirit(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "idle spirit_", 24, 20);
            drawable.AddByPrefix(textures, "singUP", "up_", 24, 20);
            drawable.AddByPrefix(textures, "singRIGHT", "right_", 24, 20);
            drawable.AddByPrefix(textures, "singLEFT", "left_", 24, 20);
            drawable.AddByPrefix(textures, "singDOWN", "spirit down_", 24, 20);
            drawable.AssignOffset("idle", new Vector2(-220f, -280f));
            drawable.AssignOffset("singUP", new Vector2(-220f, -240f));
            drawable.AssignOffset("singRIGHT", new Vector2(-220f, -280f));
            drawable.AssignOffset("singLEFT", new Vector2(-200f, -280f));
            drawable.AssignOffset("singDOWN", new Vector2(170f, 110f));

            drawable.PixelScaling = true;
            drawable.SetAnimation("idle");
        }

        public static void RegisterParentsChristmas(this CharacterDrawable drawable, TextureStore textures)
        {
            drawable.AddByPrefix(textures, "idle", "Parent Christmas Idle", 24, 14);
            drawable.AddByPrefix(textures, "singUP", "Parent Up Note Dad", 24, 14);
            drawable.AddByPrefix(textures, "singDOWN", "Parent Down Note Dad", 24, 15);
            drawable.AddByPrefix(textures, "singLEFT", "Parent Left Note Dad", 24, 16);
            drawable.AddByPrefix(textures, "singRIGHT", "Parent Right Note Dad", 24, 16);
            drawable.AddByPrefix(textures, "singUP-alt", "Parent Up Note Mom", 24, 14);
            drawable.AddByPrefix(textures, "singDOWN-alt", "Parent Down Note Mom", 24, 15);
            drawable.AddByPrefix(textures, "singLEFT-alt", "Parent Left Note Mom", 24, 15);
            drawable.AddByPrefix(textures, "singRIGHT-alt", "Parent Right Note Mom", 24, 15);
            drawable.AssignOffset("singUP", new Vector2(-47f, 24f));
            drawable.AssignOffset("singRIGHT", new Vector2(-1f, -23f));
            drawable.AssignOffset("singLEFT", new Vector2(-30f, 16f));
            drawable.AssignOffset("singDOWN", new Vector2(-31f, -29f));
            drawable.AssignOffset("singUP-alt", new Vector2(-47f, 24f));
            drawable.AssignOffset("singRIGHT-alt", new Vector2(-1f, -24f));
            drawable.AssignOffset("singLEFT-alt", new Vector2(-30f, 15f));
            drawable.AssignOffset("singDOWN-alt", new Vector2(-30f, -27f));

            drawable.SetAnimation("idle");
        }

        #endregion
    }
}