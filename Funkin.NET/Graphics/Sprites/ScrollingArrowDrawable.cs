using System;
using Funkin.NET.Conductor;
using Funkin.NET.Input.Bindings;
using Funkin.NET.Screens.Main;
using Funkin.NET.Songs;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Graphics.Sprites
{
    public class ScrollingArrowDrawable : CompositeDrawable
    {
        public virtual KeyAssociatedAction Key { get; }

        public virtual double TargetTime { get; }

        public virtual int HoldTime { get; }

        public virtual Vector2 TargetPosition { get; }

        public virtual double SongSpeed { get; }

        public virtual bool IsEnemyArrow { get; }

        public virtual bool HasBeenHit { get; protected set; }

        public virtual IGameData.HitAccuracyType? AccuracyType { get; protected set; }

        protected Sprite ArrowSprite { get; }
        protected Sprite HoldEndSprite { get; }
        protected Vector2? StartPos;
        protected double StartHoldTime;
        protected double LastHeldTime;
        protected bool IsHeld;
        protected bool RegisteredAccuracyType;
        
        public ScrollingArrowDrawable(KeyAssociatedAction key, double targetTime, int holdTime, Vector2 targetPos, double songSpeed, bool isEnemyArrow)
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
            LifetimeEnd = TargetTime + 2 * 1000; // Lifetime ends 4 seconds after target
        }

        public ScrollingArrowDrawable(Note note, Vector2 targetPos, double songSpeed = 1, bool isEnemyArrow = false, double startOffset = 0) :
            this(note.Key, note.Offset + startOffset, note.HoldLength, targetPos, songSpeed, isEnemyArrow)
        {
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {
            string keyName = Enum.GetName(Key)!.ToLowerInvariant();

            ArrowSprite.Texture = textures.Get($"Arrow/{keyName}_scroll");
            AddInternal(ArrowSprite);

            HoldEndSprite.Texture = textures.Get($"Arrow/{keyName}_hold_end");
            AddInternal(HoldEndSprite);

            // debug stuff lol
            // just shows some positioning text
            /*SpriteText text = new()
            {
                Font = new FontUsage("Torus-Regular", 15f),
                Alpha = 1f,
                AlwaysPresent = true
            };
            text.OnUpdate += drawable =>
            {
                ((SpriteText) drawable).Text = $"{_startPos?.Y ?? 0f}, {TargetPosition.Y}, {Position}";
                drawable.Position = ArrowSprite.Position + new Vector2(0, 40f);

                if (Position.Y <= -200f)
                    Alpha = 0f;
            };
            AddInternal(text);*/
        }

        protected override void Update()
        {
            StartPos ??= Position;

            UpdateArrowPos();
            UpdateHoldPos();

            if (Position.Y < -250f && !IsEnemyArrow && AccuracyType is null)
                AccuracyType = IGameData.HitAccuracyType.Missed;

            // TODO: sustain note support
            if (!(Position.Y <= -200f) || !IsEnemyArrow)
                return;

            // TODO: better removal technique
            Alpha = 0f;
            HasBeenHit = true;
        }

        private void UpdateArrowPos()
        {
            if (!StartPos.HasValue) return;
            // TODO: fix big numbers making stuff slower

            double songPos;
            if (IsHeld)
                songPos = TargetTime;
            else if (LastHeldTime != 0 && StartHoldTime != 0)
                songPos = MusicConductor.SongPosition - (LastHeldTime - StartHoldTime);
            else
                songPos = MusicConductor.SongPosition;
            // double songPos = MusicConductor.SongPosition - (LastHeldTime);
            // if (!IsHeld)
            //     Console.WriteLine($"{nameof(songPos)} ({songPos}) = {MusicConductor.SongPosition} - ({LastHeldTime} - {StartHoldTime})");

            if (IsHeld || LastHeldTime != 0)
                Console.WriteLine($"{nameof(songPos)} ({songPos}) = {MusicConductor.SongPosition} - ({LastHeldTime} - {StartHoldTime}) | IsHeld: {IsHeld}");
            
            // if (LastHeldTime != 0)
            //     Console.WriteLine($"{nameof(songPos)} ({songPos}) = {MusicConductor.SongPosition} - ({LastHeldTime})");
            
            float by = (float) (songPos / TargetTime);
            Position = new Vector2(TargetPosition.X, Lerp(StartPos.Value.Y, TargetPosition.Y, by));
        }

        private void UpdateHoldPos()
        {
            if (!StartPos.HasValue) return;
            if (HoldTime <= 0) return;
            
            HoldEndSprite.Show();
                
            float by = (float) (MusicConductor.SongPosition / (TargetTime + HoldTime));
            float lerpPos = Lerp(StartPos.Value.Y, TargetPosition.Y, by);
            HoldEndSprite.Position = new Vector2(0, lerpPos - Position.Y);
        }

        public virtual void Press(KeyAssociatedAction action, bool held)
        {
            if (HasBeenHit && HoldTime == 0)
                return;

            // sustain notes aren't implemented yet
            // so ignore all held presses for now
            // figure out sustain notes later?
            if (action != Key)
                return;
            
            // TODO: sustain note support
            if (Position.Y is <= -250f or >= -150f || IsEnemyArrow)
                return;

            if (!held && HoldTime > 0)
                StartHoldTime = MusicConductor.SongPosition;
            
            if (held)
            {
                Console.WriteLine($"held arrow: {Key}");
                LastHeldTime = MusicConductor.SongPosition;
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

            IGameData.HitAccuracyType? hitType = null;

            if (Position.Y is >= -210f and <= -190f)
                hitType = IGameData.HitAccuracyType.Sick;
            else if (Position.Y is >= -220f and <= -180f)
                hitType = IGameData.HitAccuracyType.Good;
            else if (Position.Y is >= -245f and <= -165f)
                hitType = IGameData.HitAccuracyType.Bad;
            else if (Position.Y is >= -250f and <= -150f)
                hitType = IGameData.HitAccuracyType.Shit;

            AccuracyType = hitType;
        }

        public virtual void Release(KeyAssociatedAction action)
        {
            IsHeld = false;
        }

        public virtual void UpdateGameData(ref IGameData gameData)
        {
            if (RegisteredAccuracyType || !AccuracyType.HasValue || IsEnemyArrow)
                return;

            RegisteredAccuracyType = true;
            gameData.NoteHits.Add(AccuracyType.Value);
            // Console.WriteLine(AccuracyType);
            gameData.AddToScore(AccuracyType.Value);
            gameData.ModifyHealth(AccuracyType.Value);
        }

        // TODO: move to own class
        public static float Lerp(float start, float end, float by) => start * (1.0f - by) + end * by;
    }
}