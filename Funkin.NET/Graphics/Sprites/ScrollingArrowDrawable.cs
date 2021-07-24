using System;
using Funkin.NET.Conductor;
using Funkin.NET.Input.Bindings;
using Funkin.NET.Songs;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Funkin.NET.Content.Elements.Composites
{
    public class ScrollingArrowDrawable : CompositeDrawable
    {
        public UniversalAction Key { get; }
        public double TargetTime { get; }
        public int HoldTime { get; }
        public Vector2 TargetPosition { get; }
        public double SongSpeed { get; }
        public bool IsEnemyArrow { get; }
        [Resolved] private TextureStore Textures { get; set; }
        private Sprite ArrowSprite { get; }
        private Vector2? _startPos;

        public ScrollingArrowDrawable(UniversalAction key, double targetTime, int holdTime, Vector2 targetPos, double songSpeed,
            bool isEnemyArrow)
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
            this(note.Key, note.Offset + startOffset, note.HoldLength, targetPos, songSpeed, isEnemyArrow) { }

        [BackgroundDependencyLoader]
        private void Load()
        {
            string textureName = $"Arrow/{Enum.GetName(Key)!.ToLowerInvariant()}_scroll";
            ArrowSprite.Texture = Textures.Get(textureName);
            AddInternal(ArrowSprite);
        }

        protected override void Update()
        {
            if (!_startPos.HasValue)
                _startPos = Position;
            
            // TODO: fix big numbers making stuff slower
            float by = (float) (MusicConductor.SongPosition / TargetTime);
            //Console.WriteLine($"Key: {Key} - Position: {MusicConductor.SongPosition} / {TargetTime} = {by}");
            // Console.WriteLine($"Key: {Key} - {Lerp(_startPos.Value.Y, TargetPosition.Y, by)}");

            Position = new Vector2(TargetPosition.X, Lerp(_startPos.Value.Y, TargetPosition.Y, by));
            // Y = (float) (TargetPosition.Y - (MusicConductor.SongPosition - TargetTime) * 0.45 * SongSpeed);
        }

        private static float Lerp(float start, float end, float by) => (start * (1.0f - by)) + (end * by);
        private static Vector2 Lerp(Vector2 start, Vector2 end, float by) => new(Lerp(start.X, end.X, by), Lerp(start.Y, end.Y, by));
    }
}