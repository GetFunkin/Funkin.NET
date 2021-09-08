using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Core.Input.Bindings
{
    public abstract class StoredKeyBindingContainer<T> : KeyBindingContainer<T> where T : struct
    {
        public Storage Storage { get; protected set; }

        public virtual string Location => Storage.GetFullPath(Path.Combine("KeyBindings", GetType().Name + ".json"));

        private readonly bool ExpectedFileToBeCreated;

        protected StoredKeyBindingContainer(Storage storage,
            SimultaneousBindingMode simultaneousMode = SimultaneousBindingMode.None,
            KeyCombinationMatchingMode matchingMode = KeyCombinationMatchingMode.Any) : base(simultaneousMode,
            matchingMode)
        {
            Storage = storage;

            if (!File.Exists(Location))
                ExpectedFileToBeCreated = true;
        }

        public sealed override IEnumerable<IKeyBinding> DefaultKeyBindings
        {
            get
            {
                if (ExpectedFileToBeCreated)
                    return FallbackKeyBindings;

                JsonSerializerOptions options = new()
                {
                    WriteIndented = true,
                    Converters = {new KeyBindingConverter<T>()}
                };

                return JsonSerializer.Deserialize<IEnumerable<IKeyBinding>>(File.ReadAllText(Location), options);
            }
        }

        public abstract IEnumerable<IKeyBinding> FallbackKeyBindings { get; }

        public virtual void UpdateKeyBindings(IEnumerable<IKeyBinding> bindings)
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                Converters = {new KeyBindingConverter<T>()}
            };

            Directory.CreateDirectory(Path.GetDirectoryName(Location) ?? "");
            File.WriteAllText(Location, JsonSerializer.Serialize(bindings, options));

            ReloadMappings();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (!ExpectedFileToBeCreated)
                return;

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