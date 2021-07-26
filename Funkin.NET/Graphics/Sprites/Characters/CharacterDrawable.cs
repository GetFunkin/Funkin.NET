using System.Collections.Generic;
using Funkin.NET.Conductor;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Funkin.NET.Graphics.Sprites.Characters
{
    public class CharacterDrawable : CompositeDrawable
    {
        public Dictionary<string, Vector2> AnimationOffsets;
        public Dictionary<string, TextureAnimation> Animations;
        public (string, TextureAnimation) CurrentAnimationCopy; // not a readonly but this animation is a copy
        public Vector2? UnmodifiedRelativeChildOffset;
        public CharacterType Type;
        public string Character;
        public double HoldTimer;
        protected bool DanceCycled;
        public bool PixelScaling;
        public bool Flip;

        public CharacterDrawable(string character, CharacterType type)
        {
            Character = character;
            Type = type;

            AnimationOffsets = new Dictionary<string, Vector2>();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            HitDance();
        }

        public virtual void SetAnimation(string animation)
        {
            UnmodifiedRelativeChildOffset ??= RelativeChildOffset;

            foreach (TextureAnimation tAnimation in Animations.Values)
            {
                tAnimation.GotoAndStop(0);
                tAnimation.Hide();
            }

            Animations[animation].Show();
            Animations[animation].GotoAndPlay(0);
            CurrentAnimationCopy = (animation, Animations[animation]);


            if (AnimationOffsets.ContainsKey(animation))
                RelativeChildOffset = (UnmodifiedRelativeChildOffset ?? new Vector2()) - AnimationOffsets[animation];
            else
                RelativeChildOffset = UnmodifiedRelativeChildOffset ?? new Vector2();

            if (Character != "gf")
                return;

            switch (animation)
            {
                case "singLeft":
                    DanceCycled = true;
                    break;

                case "sightRight":
                    DanceCycled = false;
                    break;

                case "singUP":
                case "singDOWN":
                    DanceCycled = !DanceCycled;
                    break;
            }
        }

        protected void HitDance()
        {
            switch (Character)
            {
                case "gf":
                case "gf-christmas":
                case "gf-car":
                case "gf-pixel":
                    if (CurrentAnimationCopy.Item1 == "hair")
                        break;

                    goto case "spooky";

                case "spooky":
                    DanceCycled = !DanceCycled;

                    SetAnimation(DanceCycled ? "danceRight" : "danceLeft");
                    break;

                default:
                    SetAnimation("idle");
                    break;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (Type != CharacterType.Boyfriend)
            {
                if (CurrentAnimationCopy.Item1.StartsWith("sing"))
                    HoldTimer += Clock.ElapsedFrameTime;

                float multiplier = 4f;

                if (Character == "dad")
                    multiplier = 6.1f;

                if (HoldTimer >= MusicConductor.StepCrochet * multiplier * 0.001D)
                {
                    HitDance();
                    HoldTimer = 0;
                }
            }

            if (Character == "gf" && CurrentAnimationCopy.Item1 == "hairFall" &&
                !Animations[CurrentAnimationCopy.Item1].IsPlaying)
                SetAnimation("danceRight");
        }

        public void AssignOffset(string animation, Vector2 offset) => AnimationOffsets[animation] = offset;

        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {
            Animations = new Dictionary<string, TextureAnimation>();
            this.LoadTextures(textures);

            if (Type == CharacterType.Boyfriend)
            {
                Flip = !Flip;

                // don't flip bf assets cos alr in right place
                if (!Character.StartsWith("bf"))
                {
                    TextureAnimation oldRight = Animations["singRIGHT"];
                    Animations["singRIGHT"] = Animations["singLEFT"];
                    Animations["singLEFT"] = oldRight;
                    
                    if (Animations.ContainsKey("singRIGHTmiss"))
                    {
                        TextureAnimation oldMiss = Animations["singRIGHTmiss"];
                        Animations["singRIGHTmiss"] = Animations["singLEFTmiss"];
                        Animations["singLEFTmiss"] = oldMiss;
                    }
                }
            }

            if (PixelScaling)
            {
                // I don't remember if enumerating through
                // the values collection here is safe
                // so I'm just taking the risk-less option
                // and accessing directly through indexes
                foreach (string key in Animations.Keys)
                    Animations[key].Scale *= FunkinCharacterAnimator.PixelZoom;
            }

            if (Flip)
                foreach (string key in Animations.Keys)
                {
                    Animations[key].Scale = new Vector2(-Animations[key].Scale.X, Animations[key].Scale.Y);
                    Animations[key].Rotation = 180f;
                }

            foreach (TextureAnimation blah in Animations.Values)
                AddInternal(blah);
        }
    }
}