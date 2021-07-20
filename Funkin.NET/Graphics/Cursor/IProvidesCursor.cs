using JetBrains.Annotations;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;

namespace Funkin.NET.Graphics.Cursor
{
    /// <summary>
    ///     See: osu!'s IProvideCursor. <br />
    ///     Interface for <see cref="IDrawable"/><c>s</c> that display cursors which replace the default cursor.
    /// </summary>
    public interface IProvidesCursor : IDrawable
    {
        /// <summary>
        ///     The cursor provided by this <see cref="IDrawable"/>. <br />
        ///     May be null if the cursor should not be visible.
        /// </summary>
        [CanBeNull]
        CursorContainer Cursor { get; }

        /// <summary>
        ///     Whether the <see cref="Cursor"/> should be displayed as the singular user cursor. Temporarily hides other cursors. <br />
        ///     This value is checked every frame and may be used to control whether multiple cursors are displayed. <br />
        ///     (In osu!, this is used for things such as replays, which aren't really relevant to FNF.)
        /// </summary>
        bool ProvidingUserCursor { get; }
    }
}