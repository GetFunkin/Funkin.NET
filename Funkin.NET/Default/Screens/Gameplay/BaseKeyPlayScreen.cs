using Funkin.NET.Common.Input;
using Funkin.NET.Common.Screens.Backgrounds;
using Funkin.NET.Intermediary.Screens.Backgrounds;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Default.Screens.Gameplay
{
    public class BaseKeyPlayScreen : MusicScreen
    {
        /* TODO + Notes:
         * draw characters
         * register when keys are hit
         * score
         * everything else
         * single sprite for all arrows, recolor/rotate with code
         *
         * Notes:
         * Start music some time later
         * Music.CurrentTime can be used to get current time in ms
         * spawn arrows some time before, like 2 sec maybe
         * when Music.CurrentTime matches arrow offset time, it should be at the arrow sprite position
         * arrows with type 4-7 are for the other character
         */

        public const int NumberOfSectionsToGenerateAhead = 8;
        public const int MusicStartOffset = 5 * 1000;
        public const int ScrollingArrowStartPos = 1000 * 15;

        public static readonly KeyAction[] ArrowValues =
        {
            KeyAction.Left,
            KeyAction.Down,
            KeyAction.Up,
            KeyAction.Right
        };

        public override IBackgroundScreen CreateBackground() => new DefaultBackgroundScreen(DefaultBackgroundType.Yellow);
    }
}