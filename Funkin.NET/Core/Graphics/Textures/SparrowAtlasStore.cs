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
            AddExtension("png");
        }

        public Texture GetAtlas(string name)
        {
            // append expected file extension
            if (!name.EndsWith(".xml"))
                name += ".xml";

            // if we've already registered all this, no need to do it again
            if (SparrowAtlases.ContainsKey(name))
                return SparrowAtlases[name];

            using Stream stream = GetStream(name);

            List<SubTexture> subTextures = new();

            Program.ReadAsXml(stream, subTextures, out string imageName, out bool successfulRead);

            string imagePath = $"{Path.GetDirectoryName(name) ?? ""}/{imageName}";
            using Stream textureStream = GetStream(imagePath);
            SparrowAtlases[name] = Texture.FromStream(textureStream);
            TextureAtlasCache[name] = new Dictionary<string, Texture>();

            if (!successfulRead)
                throw new Exception("Attempted to read file with invalid XML data.");

            SubTextures[name] = subTextures;

            foreach (SubTexture subTexture in SubTextures[name])
            {
                subTexture.GetData(out bool _, out Rectangle _, out Rectangle _, out Rectangle crop);
                Texture croppedTexture = SparrowAtlases[name].Crop(new osuRectangleF(crop.X, crop.Y, crop.Width, crop.Height));
                TextureAtlasCache[name][subTexture.Name] = croppedTexture;
            }
            
            return SparrowAtlases[name];
        }

        public Texture GetTexture(string atlas, string textureName) =>
            TextureAtlasCache[atlas.EndsWith(".xml") ? atlas : atlas + ".xml"][textureName];
    }
}