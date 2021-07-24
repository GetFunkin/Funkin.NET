using System;
using Funkin.NET.Input.Bindings;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Graphics.Sprites
{
    public class ArrowKeyDrawable : CompositeDrawable
    {
        // Notes:
        // "x press" is for when you incorrectly press a key, or continue holding a key after key ends
        // "x confirm" is for when you correctly press a key, and continue holding while the key hasn't ended

        public ArrowKeyDrawable(KeyAssociatedAction arrowKey)
        {
            ArrowKey = arrowKey;

            ArrowIdleSprite = new Sprite
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre
            };

            ArrowPressAnim = new TextureAnimation
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                IsPlaying = true,
                Loop = false
            };

            ArrowConfirmAnim = new TextureAnimation
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                IsPlaying = true,
                Loop = false
            };
        }

        public KeyAssociatedAction ArrowKey { get; }

        public Sprite ArrowIdleSprite { get; }

        public TextureAnimation ArrowPressAnim { get; }

        public TextureAnimation ArrowConfirmAnim { get; }


        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {
            string keyName = Enum.GetName(ArrowKey)?.ToUpperInvariant() ??
                             throw new ArgumentOutOfRangeException(nameof(ArrowKey));

            // Get the arrow texture
            string arrowName = keyName;
            string textureName = "Arrow/arrow" + arrowName;
            ArrowIdleSprite.Texture = textures.Get(textureName);

            keyName = keyName.ToLowerInvariant();
            for (int i = 0; i < 4; i++)
            {
                ArrowPressAnim.AddFrame(textures.Get($"Arrow/{keyName} press{i}"));
                ArrowConfirmAnim.AddFrame(textures.Get($"Arrow/{keyName} confirm{i}"));
            }

            // Add the textures
            AddInternal(ArrowIdleSprite);
            AddInternal(ArrowPressAnim);
            AddInternal(ArrowConfirmAnim);

            ArrowPressAnim.Hide();
            ArrowConfirmAnim.Hide();
        }
    }
}