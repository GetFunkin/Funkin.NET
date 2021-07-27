using osu.Framework.Graphics;

namespace Funkin.NET.Core.Graphics.Containers
{
    /// <summary>
    ///     See: osu!'s UserTrackingScrollContainer.
    /// </summary>
    public class UserTrackingScrollContainer : UserTrackingScrollContainer<Drawable>
    {
        public UserTrackingScrollContainer()
        {
        }

        public UserTrackingScrollContainer(Direction direction)
            : base(direction)
        {
        }
    }

    /// <summary>
    ///     See: osu!'s UserTrackingScrollContainer&lt;T&gt;.
    /// </summary>
    public class UserTrackingScrollContainer<T> : FunkinScrollContainer<T>
        where T : Drawable
    {
        /// <summary>
        /// Whether the last scroll event was user triggered, directly on the scroll container.
        /// </summary>
        public bool UserScrolling { get; private set; }

        public void CancelUserScroll() => UserScrolling = false;

        public UserTrackingScrollContainer()
        {
        }

        public UserTrackingScrollContainer(Direction direction)
            : base(direction)
        {
        }

        protected override void OnUserScroll(float value, bool animated = true, double? distanceDecay = default)
        {
            UserScrolling = true;
            base.OnUserScroll(value, animated, distanceDecay);
        }

        public new void ScrollTo(float value, bool animated = true, double? distanceDecay = null)
        {
            UserScrolling = false;
            base.ScrollTo(value, animated, distanceDecay);
        }

        public new void ScrollToEnd(bool animated = true, bool allowDuringDrag = false)
        {
            UserScrolling = false;
            base.ScrollToEnd(animated, allowDuringDrag);
        }
    }
}