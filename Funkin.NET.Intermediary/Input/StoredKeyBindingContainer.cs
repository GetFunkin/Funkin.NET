using System.Collections.Generic;
using System.IO;
using Funkin.NET.Intermediary.Json;
using Newtonsoft.Json;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Intermediary.Input
{
    /// <summary>
    ///     Abstract <see cref="KeyBindingContainer{T}"/> which saves bindings to a <c>JSON</c> file. Capable of serialization and deserialization.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class StoredKeyBindingContainer<T> : KeyBindingContainer<T> where T : struct
    {
        // storage instance used for saving
        public Storage Storage { get; protected set; }

        public virtual string Location => Storage.GetFullPath(Path.Combine("KeyBindings", GetType().Name + ".json"));

        /// <summary>
        ///     Indicates that a file is to be created. Only <see langword="true"/> if <see cref="Location"/> points to an empty path.
        /// </summary>
        protected bool ExpectedFileToBeCreated { get; }

        protected StoredKeyBindingContainer(Storage storage, 
            SimultaneousBindingMode simultaneousMode = SimultaneousBindingMode.None,
            KeyCombinationMatchingMode matchingMode = KeyCombinationMatchingMode.Any) : 
            base(simultaneousMode, matchingMode)
        {
            Storage = storage;

            if (!File.Exists(Location))
                ExpectedFileToBeCreated = true;
        }

        public sealed override IEnumerable<IKeyBinding> DefaultKeyBindings
        {
            get
            {
                // No file, return fall-backs.
                // This means we're retrieving what are essentially the actual defaults.
                if (ExpectedFileToBeCreated)
                    return FallbackKeyBindings;

                // Deserialize the existing JSON file.
                JsonSerializerSettings options = new()
                {
                    Formatting = Formatting.Indented,
                    // Use a custom JSON converter
                    Converters = {new KeyBindingConverter<T>()}
                };

                // Deserialize into a collection if IKeyBindings
                return JsonConvert.DeserializeObject<IEnumerable<IKeyBinding>>(File.ReadAllText(Location), options)!;
            }
        }

        /// <summary>
        ///     Real default key bindings, only used if there's no pre-existing file to deserialize. <br />
        ///     This is not checked if there is a deserializable file.
        /// </summary>
        public abstract IEnumerable<IKeyBinding> FallbackKeyBindings { get; }

        /// <summary>
        ///     Trigger an update to the key binding settings which will write <paramref name="bindings"/> to a file at <see cref="Location"/>.
        /// </summary>
        /// <param name="bindings"></param>
        public virtual void UpdateKeyBindings(IEnumerable<IKeyBinding> bindings)
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                Converters = {new KeyBindingConverter<T>()}
            };

            // Ensure the directory exists.
            Directory.CreateDirectory(Path.GetDirectoryName(Location) ?? "");

            // Serialize to a JSON.
            File.WriteAllText(Location, JsonSerializer.Serialize(bindings, options));

            // Reload our mappings.
            ReloadMappings();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (!ExpectedFileToBeCreated)
                return;

            // Save to a file if it doesn't already exist.
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                Converters = {new KeyBindingConverter<T>()}
            };

            Directory.CreateDirectory(Path.GetDirectoryName(Location) ?? "");
            File.WriteAllText(Location, JsonSerializer.Serialize(DefaultKeyBindings, options));
        }
    }
}