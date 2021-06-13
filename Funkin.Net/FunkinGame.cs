using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

[assembly: InternalsVisibleTo("Funkin.NET.Desktop")]

namespace Funkin.NET
{
    /// <summary>
    ///     Main Funkin' game. Ran by a <see cref="Host"/>.
    /// </summary>
    public class FunkinGame : Game
    {
        /// <summary>
        ///     Launched executable location.
        /// </summary>
        public static string ExecutablePath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /* Sample game code. Temporary. */
        private Box box;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(box = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(150, 150),
                Colour = Color4.Tomato
            });
        }

        protected override void Update()
        {
            base.Update();
            box.Rotation += (float)Time.Elapsed / 10;
        }
    }
}