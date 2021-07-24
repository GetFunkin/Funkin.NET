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

        public static void RegisterGirlfriendChristmas(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * 				animation.addByPrefix('cheer', 'GF Cheer', 24, false);
				animation.addByPrefix('singLEFT', 'GF left note', 24, false);
				animation.addByPrefix('singRIGHT', 'GF Right Note', 24, false);
				animation.addByPrefix('singUP', 'GF Up Note', 24, false);
				animation.addByPrefix('singDOWN', 'GF Down Note', 24, false);
				animation.addByIndices('sad', 'gf sad', [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], "", 24, false);
				animation.addByIndices('danceLeft', 'GF Dancing Beat', [30, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14], "", 24, false);
				animation.addByIndices('danceRight', 'GF Dancing Beat', [15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29], "", 24, false);
				animation.addByIndices('hairBlow', "GF Dancing Beat Hair blowing", [0, 1, 2, 3], "", 24);
				animation.addByIndices('hairFall', "GF Dancing Beat Hair Landing", [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11], "", 24, false);
				animation.addByPrefix('scared', 'GF FEAR', 24);

				addOffset('cheer');
				addOffset('sad', -2, -2);
				addOffset('danceLeft', 0, -9);
				addOffset('danceRight', 0, -9);

				addOffset("singUP", 0, 4);
				addOffset("singRIGHT", 0, -20);
				addOffset("singLEFT", 0, -19);
				addOffset("singDOWN", 0, -20);
				addOffset('hairBlow', 45, -8);
				addOffset('hairFall', 0, -9);

				addOffset('scared', -2, -17);

				playAnim('danceRight');
             */
        }

        public static void RegisterGirlfriendCar(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByIndices('singUP', 'GF Dancing Beat Hair blowing CAR', [0], "", 24, false);
				animation.addByIndices('danceLeft', 'GF Dancing Beat Hair blowing CAR', [30, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14], "", 24, false);
				animation.addByIndices('danceRight', 'GF Dancing Beat Hair blowing CAR', [15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29], "", 24,
					false);

				addOffset('danceLeft', 0);
				addOffset('danceRight', 0);

				playAnim('danceRight');
             */
        }

        public static void RegisterGirlfriendPixel(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByIndices('singUP', 'GF IDLE', [2], "", 24, false);
				animation.addByIndices('danceLeft', 'GF IDLE', [30, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14], "", 24, false);
				animation.addByIndices('danceRight', 'GF IDLE', [15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29], "", 24, false);

				addOffset('danceLeft', 0);
				addOffset('danceRight', 0);

				playAnim('danceRight');

				setGraphicSize(Std.int(width * PlayState.daPixelZoom));
				updateHitbox();
				antialiasing = false;
             */
        }

        public static void RegisterDad(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', 'Dad idle dance', 24);
				animation.addByPrefix('singUP', 'Dad Sing Note UP', 24);
				animation.addByPrefix('singRIGHT', 'Dad Sing Note RIGHT', 24);
				animation.addByPrefix('singDOWN', 'Dad Sing Note DOWN', 24);
				animation.addByPrefix('singLEFT', 'Dad Sing Note LEFT', 24);

				addOffset('idle');
				addOffset("singUP", -6, 50);
				addOffset("singRIGHT", 0, 27);
				addOffset("singLEFT", -10, 10);
				addOffset("singDOWN", 0, -30);

				playAnim('idle');
             */
        }

        public static void RegisterSpooky(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('singUP', 'spooky UP NOTE', 24, false);
				animation.addByPrefix('singDOWN', 'spooky DOWN note', 24, false);
				animation.addByPrefix('singLEFT', 'note sing left', 24, false);
				animation.addByPrefix('singRIGHT', 'spooky sing right', 24, false);
				animation.addByIndices('danceLeft', 'spooky dance idle', [0, 2, 6], "", 12, false);
				animation.addByIndices('danceRight', 'spooky dance idle', [8, 10, 12, 14], "", 12, false);

				addOffset('danceLeft');
				addOffset('danceRight');

				addOffset("singUP", -20, 26);
				addOffset("singRIGHT", -130, -14);
				addOffset("singLEFT", 130, -10);
				addOffset("singDOWN", -50, -130);

				playAnim('danceRight');
             */
        }

        public static void RegisterMom(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', "Mom Idle", 24, false);
				animation.addByPrefix('singUP', "Mom Up Pose", 24, false);
				animation.addByPrefix('singDOWN', "MOM DOWN POSE", 24, false);
				animation.addByPrefix('singLEFT', 'Mom Left Pose', 24, false);
				// ANIMATION IS CALLED MOM LEFT POSE BUT ITS FOR THE RIGHT
				// CUZ DAVE IS DUMB!
				animation.addByPrefix('singRIGHT', 'Mom Pose Left', 24, false);

				addOffset('idle');
				addOffset("singUP", 14, 71);
				addOffset("singRIGHT", 10, -60);
				addOffset("singLEFT", 250, -23);
				addOffset("singDOWN", 20, -160);

				playAnim('idle');
             */
        }

        public static void RegisterMomCar(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', "Mom Idle", 24, false);
				animation.addByPrefix('singUP', "Mom Up Pose", 24, false);
				animation.addByPrefix('singDOWN', "MOM DOWN POSE", 24, false);
				animation.addByPrefix('singLEFT', 'Mom Left Pose', 24, false);
				// ANIMATION IS CALLED MOM LEFT POSE BUT ITS FOR THE RIGHT
				// CUZ DAVE IS DUMB!
				animation.addByPrefix('singRIGHT', 'Mom Pose Left', 24, false);

				addOffset('idle');
				addOffset("singUP", 14, 71);
				addOffset("singRIGHT", 10, -60);
				addOffset("singLEFT", 250, -23);
				addOffset("singDOWN", 20, -160);

				playAnim('idle');
             */
        }

        public static void RegisterMonster(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', 'monster idle', 24, false);
				animation.addByPrefix('singUP', 'monster up note', 24, false);
				animation.addByPrefix('singDOWN', 'monster down', 24, false);
				animation.addByPrefix('singLEFT', 'Monster left note', 24, false);
				animation.addByPrefix('singRIGHT', 'Monster Right note', 24, false);

				addOffset('idle');
				addOffset("singUP", -20, 50);
				addOffset("singRIGHT", -51);
				addOffset("singLEFT", -30);
				addOffset("singDOWN", -30, -40);
				playAnim('idle');
             */
        }

        public static void RegisterMonsterChristmas(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', 'monster idle', 24, false);
				animation.addByPrefix('singUP', 'monster up note', 24, false);
				animation.addByPrefix('singDOWN', 'monster down', 24, false);
				animation.addByPrefix('singLEFT', 'Monster left note', 24, false);
				animation.addByPrefix('singRIGHT', 'Monster Right note', 24, false);

				addOffset('idle');
				addOffset("singUP", -20, 50);
				addOffset("singRIGHT", -51);
				addOffset("singLEFT", -30);
				addOffset("singDOWN", -40, -94);
				playAnim('idle');
             */
        }

        public static void RegisterPico(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', "Pico Idle Dance", 24);
				animation.addByPrefix('singUP', 'pico Up note0', 24, false);
				animation.addByPrefix('singDOWN', 'Pico Down Note0', 24, false);
				if (isPlayer)
				{
					animation.addByPrefix('singLEFT', 'Pico NOTE LEFT0', 24, false);
					animation.addByPrefix('singRIGHT', 'Pico Note Right0', 24, false);
					animation.addByPrefix('singRIGHTmiss', 'Pico Note Right Miss', 24, false);
					animation.addByPrefix('singLEFTmiss', 'Pico NOTE LEFT miss', 24, false);
				}
				else
				{
					// Need to be flipped! REDO THIS LATER!
					animation.addByPrefix('singLEFT', 'Pico Note Right0', 24, false);
					animation.addByPrefix('singRIGHT', 'Pico NOTE LEFT0', 24, false);
					animation.addByPrefix('singRIGHTmiss', 'Pico NOTE LEFT miss', 24, false);
					animation.addByPrefix('singLEFTmiss', 'Pico Note Right Miss', 24, false);
				}

				animation.addByPrefix('singUPmiss', 'pico Up note miss', 24);
				animation.addByPrefix('singDOWNmiss', 'Pico Down Note MISS', 24);

				addOffset('idle');
				addOffset("singUP", -29, 27);
				addOffset("singRIGHT", -68, -7);
				addOffset("singLEFT", 65, 9);
				addOffset("singDOWN", 200, -70);
				addOffset("singUPmiss", -19, 67);
				addOffset("singRIGHTmiss", -60, 41);
				addOffset("singLEFTmiss", 62, 64);
				addOffset("singDOWNmiss", 210, -28);

				playAnim('idle');

				flipX = true;
             */
        }

        public static void RegisterBoyfriend(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', 'BF idle dance', 24, false);
				animation.addByPrefix('singUP', 'BF NOTE UP0', 24, false);
				animation.addByPrefix('singLEFT', 'BF NOTE LEFT0', 24, false);
				animation.addByPrefix('singRIGHT', 'BF NOTE RIGHT0', 24, false);
				animation.addByPrefix('singDOWN', 'BF NOTE DOWN0', 24, false);
				animation.addByPrefix('singUPmiss', 'BF NOTE UP MISS', 24, false);
				animation.addByPrefix('singLEFTmiss', 'BF NOTE LEFT MISS', 24, false);
				animation.addByPrefix('singRIGHTmiss', 'BF NOTE RIGHT MISS', 24, false);
				animation.addByPrefix('singDOWNmiss', 'BF NOTE DOWN MISS', 24, false);
				animation.addByPrefix('hey', 'BF HEY', 24, false);

				animation.addByPrefix('firstDeath', "BF dies", 24, false);
				animation.addByPrefix('deathLoop', "BF Dead Loop", 24, true);
				animation.addByPrefix('deathConfirm', "BF Dead confirm", 24, false);

				animation.addByPrefix('scared', 'BF idle shaking', 24);

				addOffset('idle', -5);
				addOffset("singUP", -29, 27);
				addOffset("singRIGHT", -38, -7);
				addOffset("singLEFT", 12, -6);
				addOffset("singDOWN", -10, -50);
				addOffset("singUPmiss", -29, 27);
				addOffset("singRIGHTmiss", -30, 21);
				addOffset("singLEFTmiss", 12, 24);
				addOffset("singDOWNmiss", -11, -19);
				addOffset("hey", 7, 4);
				addOffset('firstDeath', 37, 11);
				addOffset('deathLoop', 37, 5);
				addOffset('deathConfirm', 37, 69);
				addOffset('scared', -4);

				playAnim('idle');

				flipX = true;
             */
        }

        public static void RegisterBoyfriendChristmas(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', 'BF idle dance', 24, false);
				animation.addByPrefix('singUP', 'BF NOTE UP0', 24, false);
				animation.addByPrefix('singLEFT', 'BF NOTE LEFT0', 24, false);
				animation.addByPrefix('singRIGHT', 'BF NOTE RIGHT0', 24, false);
				animation.addByPrefix('singDOWN', 'BF NOTE DOWN0', 24, false);
				animation.addByPrefix('singUPmiss', 'BF NOTE UP MISS', 24, false);
				animation.addByPrefix('singLEFTmiss', 'BF NOTE LEFT MISS', 24, false);
				animation.addByPrefix('singRIGHTmiss', 'BF NOTE RIGHT MISS', 24, false);
				animation.addByPrefix('singDOWNmiss', 'BF NOTE DOWN MISS', 24, false);
				animation.addByPrefix('hey', 'BF HEY', 24, false);

				addOffset('idle', -5);
				addOffset("singUP", -29, 27);
				addOffset("singRIGHT", -38, -7);
				addOffset("singLEFT", 12, -6);
				addOffset("singDOWN", -10, -50);
				addOffset("singUPmiss", -29, 27);
				addOffset("singRIGHTmiss", -30, 21);
				addOffset("singLEFTmiss", 12, 24);
				addOffset("singDOWNmiss", -11, -19);
				addOffset("hey", 7, 4);

				playAnim('idle');

				flipX = true;
             */
        }

        public static void RegisterBoyfriendCar(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', 'BF idle dance', 24, false);
				animation.addByPrefix('singUP', 'BF NOTE UP0', 24, false);
				animation.addByPrefix('singLEFT', 'BF NOTE LEFT0', 24, false);
				animation.addByPrefix('singRIGHT', 'BF NOTE RIGHT0', 24, false);
				animation.addByPrefix('singDOWN', 'BF NOTE DOWN0', 24, false);
				animation.addByPrefix('singUPmiss', 'BF NOTE UP MISS', 24, false);
				animation.addByPrefix('singLEFTmiss', 'BF NOTE LEFT MISS', 24, false);
				animation.addByPrefix('singRIGHTmiss', 'BF NOTE RIGHT MISS', 24, false);
				animation.addByPrefix('singDOWNmiss', 'BF NOTE DOWN MISS', 24, false);

				addOffset('idle', -5);
				addOffset("singUP", -29, 27);
				addOffset("singRIGHT", -38, -7);
				addOffset("singLEFT", 12, -6);
				addOffset("singDOWN", -10, -50);
				addOffset("singUPmiss", -29, 27);
				addOffset("singRIGHTmiss", -30, 21);
				addOffset("singLEFTmiss", 12, 24);
				addOffset("singDOWNmiss", -11, -19);
				playAnim('idle');

				flipX = true;
             */
        }

        public static void RegisterBoyfriendPixel(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', 'BF IDLE', 24, false);
				animation.addByPrefix('singUP', 'BF UP NOTE', 24, false);
				animation.addByPrefix('singLEFT', 'BF LEFT NOTE', 24, false);
				animation.addByPrefix('singRIGHT', 'BF RIGHT NOTE', 24, false);
				animation.addByPrefix('singDOWN', 'BF DOWN NOTE', 24, false);
				animation.addByPrefix('singUPmiss', 'BF UP MISS', 24, false);
				animation.addByPrefix('singLEFTmiss', 'BF LEFT MISS', 24, false);
				animation.addByPrefix('singRIGHTmiss', 'BF RIGHT MISS', 24, false);
				animation.addByPrefix('singDOWNmiss', 'BF DOWN MISS', 24, false);

				addOffset('idle');
				addOffset("singUP");
				addOffset("singRIGHT");
				addOffset("singLEFT");
				addOffset("singDOWN");
				addOffset("singUPmiss");
				addOffset("singRIGHTmiss");
				addOffset("singLEFTmiss");
				addOffset("singDOWNmiss");

				setGraphicSize(Std.int(width * 6));
				updateHitbox();

				playAnim('idle');

				width -= 100;
				height -= 100;

				antialiasing = false;

				flipX = true;
             */
        }

        public static void RegisterBoyfriendPixelDead(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('singUP', "BF Dies pixel", 24, false);
				animation.addByPrefix('firstDeath', "BF Dies pixel", 24, false);
				animation.addByPrefix('deathLoop', "Retry Loop", 24, true);
				animation.addByPrefix('deathConfirm', "RETRY CONFIRM", 24, false);
				animation.play('firstDeath');

				addOffset('firstDeath');
				addOffset('deathLoop', -37);
				addOffset('deathConfirm', -37);
				playAnim('firstDeath');
				// pixel bullshit
				setGraphicSize(Std.int(width * 6));
				updateHitbox();
				antialiasing = false;
				flipX = true;
             */
        }

        public static void RegisterSenpai(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', 'Senpai Idle', 24, false);
				animation.addByPrefix('singUP', 'SENPAI UP NOTE', 24, false);
				animation.addByPrefix('singLEFT', 'SENPAI LEFT NOTE', 24, false);
				animation.addByPrefix('singRIGHT', 'SENPAI RIGHT NOTE', 24, false);
				animation.addByPrefix('singDOWN', 'SENPAI DOWN NOTE', 24, false);

				addOffset('idle');
				addOffset("singUP", 5, 37);
				addOffset("singRIGHT");
				addOffset("singLEFT", 40);
				addOffset("singDOWN", 14);

				playAnim('idle');

				setGraphicSize(Std.int(width * 6));
				updateHitbox();

				antialiasing = false;
             */
        }

        public static void RegisterSenpaiAngry(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', 'Angry Senpai Idle', 24, false);
				animation.addByPrefix('singUP', 'Angry Senpai UP NOTE', 24, false);
				animation.addByPrefix('singLEFT', 'Angry Senpai LEFT NOTE', 24, false);
				animation.addByPrefix('singRIGHT', 'Angry Senpai RIGHT NOTE', 24, false);
				animation.addByPrefix('singDOWN', 'Angry Senpai DOWN NOTE', 24, false);

				addOffset('idle');
				addOffset("singUP", 5, 37);
				addOffset("singRIGHT");
				addOffset("singLEFT", 40);
				addOffset("singDOWN", 14);
				playAnim('idle');

				setGraphicSize(Std.int(width * 6));
				updateHitbox();

				antialiasing = false;
             */
        }

        public static void RegisterSpirit(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', "idle spirit_", 24, false);
				animation.addByPrefix('singUP', "up_", 24, false);
				animation.addByPrefix('singRIGHT', "right_", 24, false);
				animation.addByPrefix('singLEFT', "left_", 24, false);
				animation.addByPrefix('singDOWN', "spirit down_", 24, false);

				addOffset('idle', -220, -280);
				addOffset('singUP', -220, -240);
				addOffset("singRIGHT", -220, -280);
				addOffset("singLEFT", -200, -280);
				addOffset("singDOWN", 170, 110);

				setGraphicSize(Std.int(width * 6));
				updateHitbox();

				playAnim('idle');

				antialiasing = false;
             */
        }

        public static void RegisterParentsChristmas(this CharacterDrawable drawable, TextureStore textures)
        {
            /*
             * animation.addByPrefix('idle', 'Parent Christmas Idle', 24, false);
				animation.addByPrefix('singUP', 'Parent Up Note Dad', 24, false);
				animation.addByPrefix('singDOWN', 'Parent Down Note Dad', 24, false);
				animation.addByPrefix('singLEFT', 'Parent Left Note Dad', 24, false);
				animation.addByPrefix('singRIGHT', 'Parent Right Note Dad', 24, false);

				animation.addByPrefix('singUP-alt', 'Parent Up Note Mom', 24, false);

				animation.addByPrefix('singDOWN-alt', 'Parent Down Note Mom', 24, false);
				animation.addByPrefix('singLEFT-alt', 'Parent Left Note Mom', 24, false);
				animation.addByPrefix('singRIGHT-alt', 'Parent Right Note Mom', 24, false);

				addOffset('idle');
				addOffset("singUP", -47, 24);
				addOffset("singRIGHT", -1, -23);
				addOffset("singLEFT", -30, 16);
				addOffset("singDOWN", -31, -29);
				addOffset("singUP-alt", -47, 24);
				addOffset("singRIGHT-alt", -1, -24);
				addOffset("singLEFT-alt", -30, 15);
				addOffset("singDOWN-alt", -30, -27);

				playAnim('idle');
             */
        }

        #endregion
    }
}