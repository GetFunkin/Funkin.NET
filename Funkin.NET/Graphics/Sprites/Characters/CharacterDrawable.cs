﻿using System.Collections.Generic;
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
                tAnimation.Loop = false; // never loop kthxbai
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
            Animations = this.LoadTextures(textures);
        }
    }

    public enum CharacterType
    {
        Boyfriend,
        Opponent,
        Girlfriend
    }
}