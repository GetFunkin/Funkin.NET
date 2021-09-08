using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using GetFunkin.AdobeNecromancer;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osuRectangleF = osu.Framework.Graphics.Primitives.RectangleF;

namespace Funkin.NET.Core.Graphics.Textures
{
    /// <summary>
    ///     ResourceStore designed for caching Textures loaded from a SparrowAtlas.
    /// </summary>
    public class SparrowAtlasStore : ResourceStore<byte[]>
    {
        protected readonly Dictionary<string, Texture> SparrowAtlases = new();
        protected readonly Dictionary<string, List<SubTexture>> SubTextures = new();
        protected readonly Dictionary<string, Dictionary<string, Texture>> TextureAtlasCache = new();

        public SparrowAtlasStore(IResourceStore<byte[]> store = null) : base(store)
        {
            AddExtension("xml");
            // AddExtension("png");
        }

        public Texture GetAtlas(string name)
        {
            // append expected file extension
            if (!name.EndsWith(".xml"))
                name += ".xml";

            // if we've already registered all this, no need to do it again
            if (SparrowAtlases.ContainsKey(name))
                return SparrowAtlases[name];

            // open a stream to the xml file
            using Stream stream = GetStream(name);

            List<SubTexture> subTextures = new();

            // generate mapping of sub-textures from the xml file
            Program.ReadAsXml(stream, subTextures, out string imageName, out bool successfulRead);

            // find the associated image path
            string imagePath = $"{Path.GetDirectoryName(name) ?? ""}/{imageName}";

            // open a stream to the image
            using Stream textureStream = GetStream(imagePath);

            // cache the image in memory
            SparrowAtlases[name] = Texture.FromStream(textureStream);

            TextureAtlasCache[name] = new Dictionary<string, Texture>();

            if (!successfulRead)
                throw new Exception("Attempted to read file with invalid XML data.");

            // cache sub-texture data
            SubTextures[name] = subTextures;

            // crop textures based on sub-textures, cache them and make sure we don't create any duplicates
            Dictionary<Rectangle, Texture> cropCache = new();

            foreach (SubTexture subTexture in SubTextures[name])
            {
                subTexture.GetData(out bool _, out Rectangle _, out Rectangle _, out Rectangle crop);

                // add to cache if not already present
                if (!cropCache.ContainsKey(crop))
                    cropCache[crop] = SparrowAtlases[name].Crop(new osuRectangleF(crop.X, crop.Y, crop.Width, crop.Height));

                TextureAtlasCache[name][subTexture.Name] = cropCache[crop];
            }
            
            // return previously-found texture
            return SparrowAtlases[name];
        }

        // return a texture from the atlas
        public Texture GetTexture(string atlas, string textureName) =>
            TextureAtlasCache[atlas.EndsWith(".xml") ? atlas : atlas + ".xml"][textureName];
    }
}