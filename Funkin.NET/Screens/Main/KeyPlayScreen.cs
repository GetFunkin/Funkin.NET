using System;
using System.Collections.Generic;
using System.Linq;
using Funkin.NET.Conductor;
using Funkin.NET.Content.Elements.Composites;
using Funkin.NET.Graphics.Sprites;
using Funkin.NET.Input.Bindings;
using Funkin.NET.Songs;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Bindings;
using osuTK;

namespace Funkin.NET.Screens.Main
{
    public class KeyPlayScreen : MusicScreen, IKeyBindingHandler<UniversalAction>
    {
        // TODO: draw characters
        // TODO: draw keys (player & opponent)
        // TODO: register when keys are hit
        // TODO: score
        // TODO: everything else
        // TODO: single sprite for all arrows, recolor/rotate with code

        /* Notes:
         * Start music some time later
         * Music.CurrentTime can be used to get current time in ms
         * spawn arrows some time before, like 2 sec maybe
         * when Music.CurrentTime matches arrow offset time, it should be at the arrow sprite position
         */
        
        private const int NumberOfSectionsToGenerateAhead = 4;
        private const int MusicStartOffset = 5 * 1000;

        public static readonly UniversalAction[] ArrowValues =
        {
            UniversalAction.Left,
            UniversalAction.Down,
            UniversalAction.Up,
            UniversalAction.Right
        };

        public override double ExpectedBpm { get; }

        public virtual Song Song { get; }

        public virtual string InstrumentalPath { get; }

        public virtual string VocalPath { get; }

        public virtual DrawableTrack VocalTrack { get; protected set; }

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
            
            // TODO: if arrow drawables are ded, spawn new ones
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
                UniversalAction arrowKey = ArrowValues[i];
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
                UniversalAction arrowKey = ArrowValues[i];
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
        }

        public virtual bool OnPressed(UniversalAction action)
        {
            if (!ArrowValues.Contains(action))
                return false;

            int value = (int) action;
            if (value >= PlayerArrows.Length) return false;

            PlayerArrows[value].ArrowPressAnim.PlaybackPosition = 0;
            PlayerArrows[value].ArrowPressAnim.Show();
            PlayerArrows[value].ArrowIdleSprite.Hide();

            return true;
        }

        public virtual void OnReleased(UniversalAction action)
        {
            if (!ArrowValues.Contains(action))
                return;

            int value = (int) action;
            if (value >= PlayerArrows.Length) return;

            PlayerArrows[value].ArrowPressAnim.Hide();
            PlayerArrows[value].ArrowIdleSprite.Show();
        }

        private void Initialize()
        {
            _initialized = true;
            
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
            while (_notesAhead.Count != NumberOfSectionsToGenerateAhead)
            {
                if (!_sectionEnumerator.MoveNext()) break;
                
                Section section = _sectionEnumerator.Current;
                if (section is null) continue;

                ScrollingArrowDrawable[] arrows = new ScrollingArrowDrawable[section.SectionNotes.Count];
                for (int i = 0; i < section.SectionNotes.Count; i++)
                {
                    Note note = section.SectionNotes[i];
                    Vector2 notePos = PlayerArrows[(int) note.Key].Position;
                    
                    arrows[i] = new ScrollingArrowDrawable(note, notePos, Song.Speed, !section.MustHitSection, MusicConductor.SongPosition + MusicStartOffset)
                    {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Position = new Vector2(0, 10000)
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