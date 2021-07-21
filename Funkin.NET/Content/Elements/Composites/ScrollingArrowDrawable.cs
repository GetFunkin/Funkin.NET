using Funkin.NET.Input.Bindings.ArrowKeys;
using Funkin.NET.Songs;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace Funkin.NET.Content.Elements.Composites
{
    public class ScrollingArrowDrawable : CompositeDrawable
    {
        public ArrowKeyAction Key { get; }
        public int TargetTime { get; }
        public int HoldTime { get; }
        public bool IsEnemyArrow { get; }
        private Sprite ArrowSprite { get; }
        private bool _shouldBeAlive = true;

        public ScrollingArrowDrawable(ArrowKeyAction key, int targetTime, int holdTime, bool isEnemyArrow)
        {
            Key = key;
            TargetTime = targetTime;
            HoldTime = holdTime;
            IsEnemyArrow = isEnemyArrow;
            ArrowSprite = new Sprite
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre
            };
        }
        
        public ScrollingArrowDrawable(Note note, bool isEnemyArrow = false) : this(note.Key, note.Offset, note.HoldLength, isEnemyArrow) { }

        public override bool RemoveWhenNotAlive => true;

        protected override bool ShouldBeAlive => _shouldBeAlive ? base.ShouldBeAlive : _shouldBeAlive;

        [BackgroundDependencyLoader]
        private void Load()
        {
            
        }
    }
}