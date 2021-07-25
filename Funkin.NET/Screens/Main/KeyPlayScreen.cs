using System;
using System.Collections.Generic;
using System.Linq;
using Funkin.NET.Conductor;
using Funkin.NET.Content.Elements.Composites;
using Funkin.NET.Graphics.Sprites;
using Funkin.NET.Graphics.Sprites.Characters;
using Funkin.NET.Input.Bindings;
using Funkin.NET.Songs;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Bindings;
using osuTK;

namespace Funkin.NET.Screens.Main
{
    public class KeyPlayScreen : MusicScreen, IKeyBindingHandler<UniversalAction>
    {
        // TODO: draw characters
        // TODO: register when keys are hit
        // TODO: score
        // TODO: everything else
        // TODO: single sprite for all arrows, recolor/rotate with code
        // TODO: why is there a sixth arrow "[62100,6,0]"???????

        /* Notes:
         * Start music some time later
         * Music.CurrentTime can be used to get current time in ms
         * spawn arrows some time before, like 2 sec maybe
         * when Music.CurrentTime matches arrow offset time, it should be at the arrow sprite position
         */
        
        private const int NumberOfSectionsToGenerateAhead = 8;
        private const int MusicStartOffset = 5 * 1000;
        private const int ScrollingArrowStartPos = 1000 * 15;

        public static readonly KeyAssociatedAction[] ArrowValues =
        {
            KeyAssociatedAction.Left,
            KeyAssociatedAction.Down,
            KeyAssociatedAction.Up,
            KeyAssociatedAction.Right
        };

        public override double ExpectedBpm { get; }

        public virtual Song Song { get; }

        public virtual string InstrumentalPath { get; }

        public virtual string VocalPath { get; }

        public virtual DrawableTrack VocalTrack { get; protected set; }

        public bool IsHeld { get; protected set; }

        public bool IsPressed { get; protected set; }

        protected ArrowKeyDrawable[] PlayerArrows;
        protected ArrowKeyDrawable[] OpponentArrows;
        private readonly LinkedList<ScrollingArrowDrawable[]> _notesAhead;
        private readonly IEnumerator<Section> _sectionEnumerator;
        private bool _initialized;

        public KeyPlayScreen(Song song, string instrumentalPath, string vocalPath)
        {
            Song = song;
            ExpectedBpm = song.Bpm;
            InstrumentalPath = instrumentalPath;
            VocalPath = vocalPath;
            _notesAhead = new LinkedList<ScrollingArrowDrawable[]>();
            _sectionEnumerator = song.Sections.GetEnumerator();
        }

        protected override void Update()
        {
            base.Update();

            if (!Music.IsRunning)
                MusicConductor.Offset = Time.Current;
            MusicConductor.SongPosition = Music.CurrentTime;

            if (!_initialized)
                Initialize();

            if (_notesAhead.First is null)
                return;

            // If all of the notes in the first section are dead, remove them and refill notes
            if (_notesAhead.First.Value.All(x => Time.Current >= x.LifetimeEnd))
            {
                Console.WriteLine($"remove first!!!!! {_notesAhead.First}");
                foreach (ScrollingArrowDrawable arrowDrawable in _notesAhead.First.Value) RemoveInternal(arrowDrawable);
                _notesAhead.RemoveFirst();
                FillNotes();
            }
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio)
        {
            Music = new DrawableTrack(audio.Tracks.Get(InstrumentalPath)) {Looping = true};
            VocalTrack = new DrawableTrack(audio.Tracks.Get(VocalPath)) {Looping = true};
            PlayerArrows = new ArrowKeyDrawable[ArrowValues.Length];
            OpponentArrows = new ArrowKeyDrawable[ArrowValues.Length];

            float offset = 140f;
            for (int i = 0; i < ArrowValues.Length; i++)
            {
                KeyAssociatedAction arrowKey = ArrowValues[i];
                PlayerArrows[i] = new ArrowKeyDrawable(arrowKey)
                {
                    Anchor = Anchor.Centre,
                    Position = new Vector2(offset, -200),
                    Origin = Anchor.Centre,
                    AlwaysPresent = true,
                    Alpha = 1f
                };

                offset += 75f;
            }

            offset = 140f;
            for (int i = ArrowValues.Length - 1; i >= 0; i--)
            {
                KeyAssociatedAction arrowKey = ArrowValues[i];
                OpponentArrows[i] = new ArrowKeyDrawable(arrowKey)
                {
                    Anchor = Anchor.Centre,
                    Position = new Vector2(-offset, -200),
                    Origin = Anchor.Centre,
                    AlwaysPresent = true,
                    Alpha = 1f,

                };

                offset += 75f;
            }

            foreach (ArrowKeyDrawable drawable in PlayerArrows)
                AddInternal(drawable);

            foreach (ArrowKeyDrawable drawable in OpponentArrows)
                AddInternal(drawable);

            AddInternal(new CharacterDrawable("gf", CharacterType.Girlfriend)
            {
                Position = new Vector2(0f, 300f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });

            AddInternal(new CharacterDrawable("dad", CharacterType.Opponent)
            {
                Position = new Vector2(-300f, 300f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });

            AddInternal(new CharacterDrawable("bf", CharacterType.Boyfriend)
            {
                Position = new Vector2(300f, 300f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
        }

        public virtual bool OnPressed(UniversalAction action)
        {
            if (!ArrowValues.Contains((KeyAssociatedAction) (int) action))
                return false;

            int value = (int) action;
            if (value >= PlayerArrows.Length || _notesAhead.First is null)
                return false;

            PlayerArrows[value].ArrowPressAnim.PlaybackPosition = 0;
            PlayerArrows[value].ArrowPressAnim.Show();
            PlayerArrows[value].ArrowIdleSprite.Hide();

            // if pressed on previous update frame the IsHeld should
            // be true :)
            if (IsPressed)
                IsHeld = true;

            // set IsPressed to opposite of IsHeld
            // if not held, then pressed
            // else, held
            IsPressed = !IsHeld;

            Console.WriteLine($"{IsPressed}, {IsHeld}");

            foreach (ScrollingArrowDrawable arrow in _notesAhead.SelectMany(arrowArray => arrowArray))
                arrow.Press((KeyAssociatedAction) (int) action, IsHeld);

            return true;
        }

        public virtual void OnReleased(UniversalAction action)
        {
            if (!ArrowValues.Contains((KeyAssociatedAction)(int)action))
                return;

            int value = (int) action;
            if (value >= PlayerArrows.Length || _notesAhead.First is null) 
                return;

            PlayerArrows[value].ArrowPressAnim.Hide();
            PlayerArrows[value].ArrowIdleSprite.Show();

            IsPressed = IsHeld = false;

            foreach (ScrollingArrowDrawable arrow in _notesAhead.First.Value)
                arrow.Release((KeyAssociatedAction)(int)action);
        }

        private void Initialize()
        {
            _initialized = true;

            MusicConductor.Offset = Time.Current;
            
            Scheduler.AddDelayed(() => // Start music in 5 seconds
            {
                Music.Start();
                Music.VolumeTo(1D);

                VocalTrack.Start();
                VocalTrack.VolumeTo(1D);
            }, MusicStartOffset);

            FillNotes();
        }
        
        private void FillNotes()
        {
            while (_notesAhead.Count <= NumberOfSectionsToGenerateAhead)
            {
                if (!_sectionEnumerator.MoveNext())
                    break;
                
                Section section = _sectionEnumerator.Current;

                if (section is null) 
                    continue;

                Console.WriteLine($"Position: {MusicConductor.SongPosition} - Offset: {MusicConductor.Offset}");

                ScrollingArrowDrawable[] arrows = new ScrollingArrowDrawable[section.SectionNotes.Count];
                for (int i = 0; i < section.SectionNotes.Count; i++)
                {
                    Note note = section.SectionNotes[i];
                    KeyAssociatedAction keyToUse = note.Key;

                    if ((int) keyToUse >= 4)
                        keyToUse = (KeyAssociatedAction) ((int) keyToUse - 4);


                    Vector2 notePos = section.MustHitSection
                        ? PlayerArrows[(int) keyToUse].Position
                        : OpponentArrows[(int) keyToUse].Position;
                    
                    double startOffset = MusicConductor.Offset;
                    if (!Music.IsRunning) 
                        startOffset += MusicStartOffset;

                    arrows[i] = new ScrollingArrowDrawable(note, notePos, Song.Speed, !section.MustHitSection,
                        startOffset)
                    {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Position = new Vector2(0, ScrollingArrowStartPos)
                    };
                    AddInternal(arrows[i]);
                }

                _notesAhead.AddLast(arrows);
            }
        }

        public static KeyPlayScreen GetPlayScreen(string json, string instrumental, string voices) =>
            new(Song.GetSongFromFile(json), instrumental, voices);
    }
}