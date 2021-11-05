using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;

namespace Funkin.NET.Graphics.Cursor
{
    public interface ICursorProvider : IDrawable
    {
        CursorContainer? Cursor { get; }

        bool ProvidingUserCursor { get; }
    }
}