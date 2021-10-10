using System;
using Funkin.NET.Common.Input;
using Funkin.NET.Core.Music.Songs;
using Funkin.NET.Core.Music.Songs.Legacy;
using Funkin.NET.Default.Screens.Gameplay;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Default.Graphics.Composites.Gameplay
{
    /// <summary>
    ///     Scrolling arrows for the key screen.
    /// </summary>
    public class ScrollingArrowDrawable : CompositeDrawable
    {
        public virtual KeyAction Key { get; }

        public virtual double TargetTime { get; }

        public virtual int HoldTime { get; }

        public virtual Vector2 TargetPosition { get; }

        public virtual double SongSpeed { get; }

        public virtual bool IsEnemyArrow { get; }

        public virtual bool HasBeenHit { get; protected set; }

        protected Sprite ArrowSprite { get; }
        protected Sprite HoldEndSprite { get; }
        protected Vector2? StartPos;
        protected double StartHoldTime;
        protected double LastHeldTime;
        protected bool IsHeld;
        protected bool RegisteredAccuracyType;

        [Resolved]
        protected FunkinGame Game { get; private set; }

        public ScrollingArrowDrawable(KeyAction key, double targetTime, int holdTime, Vector2 targetPos,
            double songSpeed, bool isEnemyArrow)
        {
            Key = key;
            TargetTime = targetTime;
            HoldTime = holdTime;
            TargetPosition = targetPos;
            SongSpeed = songSpeed;
            IsEnemyArrow = isEnemyArrow;
            ArrowSprite = new Sprite
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre
            };
            HoldEndSprite = new Sprite
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Alpha = 0
            };

            LifetimeStart = TargetTime - 5 * 1000; // Lifetime starts 5 seconds before target
            LifetimeEnd = TargetTime + (2 * 1000 + holdTime); // Lifetime ends 2 seconds + hold length after target
        }

        public ScrollingArrowDrawable(LegacyNote note, Vector2 targetPos, double songSpeed = 1, bool isEnemyArrow = false,
            double startOffset = 0) :
            this(note.Key, note.Offset + startOffset, note.HoldLength, targetPos, songSpeed, isEnemyArrow)
        {
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {
            string keyName = Enum.GetName(Key)!.ToLowerInvariant();

            HoldEndSprite.Texture = textures.Get($"Arrow/{keyName}_hold_end");
            AddInternal(HoldEndSprite);

            ArrowSprite.Texture = textures.Get($"Arrow/{keyName}_scroll");
            AddInternal(ArrowSprite);
        }

        protected override void Update()
        {
            StartPos ??= Position;

            UpdateArrowPos();
            UpdateHoldPos();

            // TODO: sustain note support
            if (!(Position.Y <= -200f) || !IsEnemyArrow)
                return;

            // TODO: better removal technique
            Alpha = 0f;
            HasBeenHit = true;
        }

        private void UpdateArrowPos()
        {
            if (!StartPos.HasValue) 
                return;
            // BUG: fix big numbers making stuff slower
            // maybe reset clock when changing to BaseKeyPlayScreen ?

            double songPos;
            if (IsHeld && Game.Conductor.CurrentSongPosition > TargetTime)
                songPos = TargetTime;
            else if (LastHeldTime != 0 && StartHoldTime != 0)
                songPos = Game.Conductor.CurrentSongPosition - (LastHeldTime - StartHoldTime);
            else
                songPos = Game.Conductor.CurrentSongPosition;

            if (IsHeld || LastHeldTime != 0)
                Console.WriteLine($"{nameof(songPos)} ({songPos}) = {Game.Conductor.CurrentSongPosition} - ({LastHeldTime} - {StartHoldTime}) | IsHeld: {IsHeld}");

            float by = (float) (songPos / TargetTime);
            Position = new Vector2(TargetPosition.X, Lerp(StartPos.Value.Y, TargetPosition.Y, by));
        }

        private void UpdateHoldPos()
        {
            if (!StartPos.HasValue) return;
            if (HoldTime <= 0) return;
            
            HoldEndSprite.Show();
                
            float by = (float) (Game.Conductor.CurrentSongPosition / (TargetTime + HoldTime));
            float lerpPos = Lerp(StartPos.Value.Y, TargetPosition.Y, by);
            HoldEndSprite.Position = new Vector2(0, lerpPos - Position.Y);
        }

        public virtual void Press(KeyAction action, bool held)
        {
            if (HasBeenHit && HoldTime == 0)
                return;

            if (action != Key)
                return;

            if (HoldTime <= 0 && (Position.Y is <= -250f or >= -150f || IsEnemyArrow))
                return;

            if (HoldTime > 0 && (Position.Y >= -150f || Game.Conductor.CurrentSongPosition > TargetTime + HoldTime || IsEnemyArrow)) {
                if (Game.Conductor.CurrentSongPosition > TargetTime + HoldTime && IsHeld) {
                    // Do some hit accuracy calculation
                    // Maybe use TargetTime and StartHoldTime?
                    
                    // Remove note
                    Alpha = 0f;
                }
                IsHeld = false;
                return;
            }

            // Set StartHoldTime when song time is near target time
            if (HoldTime > 0 && Math.Abs(Game.Conductor.CurrentSongPosition - TargetTime) < 25)
                StartHoldTime = Game.Conductor.CurrentSongPosition;

            if (HoldTime > 0)
            {
                LastHeldTime = Game.Conductor.CurrentSongPosition;
                IsHeld = true;

                // TODO: some kind of score for held notes maybe
                return;
            }

            Console.WriteLine("5");
            HasBeenHit = true;
            if (HoldTime > 0) 
                return;
            
            Console.WriteLine("6");
            // TODO: better removal technique
            Alpha = 0f;
        }

        public virtual void Release(KeyAction action)
        {
            IsHeld = false;
        }

        // TODO: move to own class
        public static float Lerp(float start, float end, float by) =>
            start * (1.0f - by) + end * by;

        public static Vector2 Lerp(Vector2 start, Vector2 end, float by) =>
            new(Lerp(start.X, end.X, by), Lerp(start.Y, end.Y, by));
    }
}