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
        protected Vector2? StartPos;
        protected bool RegisteredAccuracyType;

        public ScrollingArrowDrawable(KeyAssociatedAction key, double targetTime, int holdTime, Vector2 targetPos,
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

            LifetimeStart = TargetTime - 5 * 1000; // Lifetime starts 5 seconds before target
            LifetimeEnd = TargetTime + 2 * 1000; // Lifetime ends 4 seconds after target
        }

        public ScrollingArrowDrawable(Note note, Vector2 targetPos, double songSpeed = 1, bool isEnemyArrow = false,
            double startOffset = 0) :
            this(note.Key, note.Offset + startOffset, note.HoldLength, targetPos, songSpeed, isEnemyArrow)
        {
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {
            string textureName = $"Arrow/{Enum.GetName(Key)!.ToLowerInvariant()}_scroll";
            ArrowSprite.Texture = textures.Get(textureName);
            AddInternal(ArrowSprite);
        }

        protected override void Update()
        {
            StartPos ??= Position;

            // TODO: fix big numbers making stuff slower
            float by = (float) (MusicConductor.SongPosition / TargetTime);
            //Console.WriteLine($"Key: {Key} - Position: {MusicConductor.SongPosition} / {TargetTime} = {by}");
            // Console.WriteLine($"Key: {Key} - {Lerp(_startPos.Value.Y, TargetPosition.Y, by)}");

            Position = new Vector2(TargetPosition.X, Lerp(StartPos.Value.Y, TargetPosition.Y, by));
            // Y = (float) (TargetPosition.Y - (MusicConductor.SongPosition - TargetTime) * 0.45 * SongSpeed);

            if (Position.Y < -250f && !IsEnemyArrow && AccuracyType is null)
                AccuracyType = IGameData.HitAccuracyType.Missed;

            // TODO: sustain note support
            if (!(Position.Y <= -200f) || !IsEnemyArrow)
                return;

            // TODO: better removal technique
            Alpha = 0f;
            HasBeenHit = true;
        }

        public virtual void Press(KeyAssociatedAction action, bool held)
        {
            if (HasBeenHit)
                return;

            // sustain notes aren't implemented yet
            // so ignore all held presses for now
            // figure out sustain notes later?
            if (action != Key || held)
                return;

            // TODO: sustain note support
            if (Position.Y is <= -250f or >= -150f || IsEnemyArrow)
                return;

            // TODO: better removal technique
            Alpha = 0f;
            HasBeenHit = true;

            IGameData.HitAccuracyType? hitType = Position.Y switch
            {
                >= -210f and <= -190f => IGameData.HitAccuracyType.Sick,
                >= -220f and <= -180f => IGameData.HitAccuracyType.Good,
                >= -245f and <= -165f => IGameData.HitAccuracyType.Bad,
                >= -250f and <= -150f => IGameData.HitAccuracyType.Shit,
                _ => null
            };

            AccuracyType = hitType;
        }

        public virtual void Release(KeyAssociatedAction action)
        {
        }

        public virtual void UpdateGameData(ref IGameData gameData)
        {
            if (RegisteredAccuracyType || !AccuracyType.HasValue || IsEnemyArrow)
                return;

            RegisteredAccuracyType = true;
            gameData.NoteHits.Add(AccuracyType.Value);
            Console.WriteLine(AccuracyType);
            gameData.AddToScore(AccuracyType.Value);
            gameData.ModifyHealth(AccuracyType.Value);
        }

        // TODO: move to own class
        public static float Lerp(float start, float end, float by) =>
            start * (1.0f - by) + end * by;

        public static Vector2 Lerp(Vector2 start, Vector2 end, float by) =>
            new(Lerp(start.X, end.X, by), Lerp(start.Y, end.Y, by));
    }
}