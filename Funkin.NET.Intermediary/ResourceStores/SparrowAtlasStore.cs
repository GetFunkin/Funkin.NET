using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Funkin.NET.Resources;
using GetFunkin.AdobeNecromancer;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osuRect = osu.Framework.Graphics.Primitives.RectangleF;

namespace Funkin.NET.Intermediary.ResourceStores
{
    /// <summary>
    ///     ResourceStore designed for caching Textures loaded from a SparrowAtlas.
    /// </summary>
    public class SparrowAtlasStore : ResourceStore<byte[]>
    {
        protected readonly Dictionary<string, Texture> SparrowAtlases = new();
        protected readonly Dictionary<string, List<SubTexture>> SubTextures = new();
        protected readonly Dictionary<string, Dictionary<string, Texture>> TextureAtlasCache = new();

        public SparrowAtlasStore(IResourceStore<byte[]>? store = null) : base(store)
        {
            // We only care about XML files for the sparrow atlas provided by Adobe.
            // png texture assets are retrieved from paths stored in the XML.
            AddExtension("xml");
        }

        /// <sumary>
        ///     Retrieve a texture atlas used for retrieving atlas sub-texture frames.
        /// </summary>
        public Texture GetAtlas(string name)
        {
            // Append the expected file extension for standardization.
            if (!name.EndsWith(".xml"))
                name += ".xml";

            // If we've already registered all this, no need to do it again.
            // Return the cached value.
            if (SparrowAtlases.ContainsKey(name))
                return SparrowAtlases[name];

            // Open a usable stream to the XML file.
            // Disposed of at the end of the method.
            using Stream stream = GetStream(name);

            List<SubTexture> subTextures = new();

            // Generate a mapping of sub-textures from the XML file.
            // Uses the stream to get the XML file, populates subTextures.
            // We also retrieve the imageName.
            // TODO: SpriteSheetPacker support.
            Necromancer.ReadAsXml(stream, subTextures, out string imageName, out bool successfulRead);

            // Upon an unsuccessful read attempt: panick.
            if (!successfulRead)
                throw new Exception("Attempted to read file with invalid XML data.");

            // Find the associated image path, provided by the XML file.
            string imagePath = PathHelper.SanitizeForResources($"{Path.GetDirectoryName(name) ?? ""}/{imageName}");

            // Open a stream to the image.
            using Stream textureStream = GetStream(imagePath);

            // Cache the image in a dictionary.
            SparrowAtlases[name] = Texture.FromStream(textureStream);

            // Upscale by 2, unsure of why this is required to properly render textures.
            // TODO: Figure out why - is this normal/okay??
            SparrowAtlases[name].ScaleAdjust = 2f;

            TextureAtlasCache[name] = new Dictionary<string, Texture>();

            // Store sub-texture mappings.
            SubTextures[name] = subTextures;

            // Crop textures based on sub-textures, store them and ensure we don't produce any duplicates.
            // This helps greatly with performance because this format is gatbage.
            Dictionary<Rectangle, Texture> cropCache = new();

            // Iterate through sub-textures.
            foreach (SubTexture subTexture in SubTextures[name])
            {
                // Retrieve crop data, we don't care about anything else.
                subTexture.GetData(out bool _, out Rectangle _, out Rectangle _, out Rectangle crop);

                // Store if not present (based on rectangle cropping!).
                // Based on rectangle cropping to ensure we don't have duplicate frames.
                if (!cropCache.ContainsKey(crop))
                {
                    cropCache[crop] = SparrowAtlases[name].Crop(
                        new osuRect(
                            crop.X,
                            crop.Y,
                            crop.Width,
                            crop.Height
                        )
                    );

                    // See earlier ScaleAdjust line.
                    cropCache[crop].ScaleAdjust = 2f;
                }

                // Update values with all cached cropping assets.
                TextureAtlasCache[name][subTexture.Name] = cropCache[crop];
            }

            // Return the atlas.
            return SparrowAtlases[name];
        }

        /// <summary>
        ///     Core method for atlas retrieval. Does not handle frames.
        /// </summary>
        public Texture GetTexture(string atlas, string textureName)
        {
            atlas = atlas.EndsWith(".xml") ? atlas : atlas + ".xml";

            if (TextureAtlasCache.ContainsKey(atlas))
                return TextureAtlasCache[atlas][textureName];

            GetAtlas(atlas);

            return TextureAtlasCache[atlas][textureName];
        }

        /// <summary>
        ///     Get a sub-texture with a given frame from the specified atlas.
        /// </summary>
        public Texture GetTexture(string atlas, string textureName, int frame) =>
            GetTexture(atlas, textureName + Atlas.FrameAsString(frame));
    }
}