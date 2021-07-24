namespace Funkin.NET.Graphics.UserInterface
{
    /// <summary>
    ///     See: osu!'s SeekLimitedSearchTextBox. <br />
    ///     A <see cref="SearchTextBox"/> which does not handle left/right arrow keys for seeking.
    /// </summary>
    public class SeekLimitedSearchTextBox : SearchTextBox
    {
        public override bool HandleLeftRightArrows => false;
    }
}