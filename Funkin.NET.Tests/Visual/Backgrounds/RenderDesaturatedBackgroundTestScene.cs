using Funkin.NET.Backgrounds;
using Funkin.NET.Intermediary.Screens;
using Funkin.NET.Resources;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osuTK;

namespace Funkin.NET.Tests.Visual.Backgrounds
{
    public class RenderDesaturatedBackgroundTestScene : FunkinTestScene
    {
        private DefaultScreenStack Stack;

        [Test]
        public void PushBackgrounds()
        {
            AddStep("push desaturated background", () =>
            {
                //if (Game.ScreenStack is not DefaultScreenStack defStack)
                //    throw new Exception("Malformed ScreenStack type.");

                //defStack.PushBackground(new SimpleBackgroundScreen(Textures.Backgrounds.DesaturatedMenu));
                
                if (Stack is null) 
                    LoadComponentAsync(Stack = new DefaultScreenStack(), Add);

                Stack.PushBackground(new SimpleBackgroundScreen(Textures.Backgrounds.DesaturatedMenu));
            });

            AddStep("push gradient background", () =>
            {
                SimpleBackgroundScreen screen = new(Textures.Backgrounds.DesaturatedMenu);

                screen.OnUpdate += drawable =>
                {
                    drawable.Colour = ColourInfo.GradientHorizontal(Colour4.Red, Colour4.Yellow);
                };

                if (Stack is null)
                    LoadComponentAsync(Stack = new DefaultScreenStack(), Add);

                Stack.PushBackground(screen);
            });

            float progress = 1f;

            AddStep("push flowing gradient background", () =>
            {
                SimpleBackgroundScreen screen = new(Textures.Backgrounds.DesaturatedMenu);

                screen.OnUpdate += drawable =>
                {
                    progress -= 0.01f;

                    if (progress < 0f)
                        progress = 1f;

                    ColourInfo gradient = ColourInfo.GradientVertical(Colour4.Yellow, Colour4.Red);
                    gradient.TopRight = gradient.TopLeft = gradient.Interpolate(new Vector2(progress)) * gradient.TopRight;

                    drawable.Colour = gradient;
                };

                if (Stack is null)
                    LoadComponentAsync(Stack = new DefaultScreenStack(), Add);

                Stack.PushBackground(screen);
            });
        }
    }
}