using System;
using System.Collections.Generic;
using Funkin.NET.Conductor;
using Funkin.NET.Content.Elements.Composites;
using Funkin.NET.Input.Bindings.ArrowKeys;
using Funkin.NET.Songs;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Bindings;
using osuTK;

namespace Funkin.NET.Screens
{
    public class SimpleKeyScreen : MusicScreen, IKeyBindingHandler<ArrowKeyAction>
    {
        // TODO: handle input
        // TODO: draw characters
        // TODO: draw enemy keys
        // TODO: draw all song keys
        // TODO: single sprite for all arrows, recolor/rotate with code

        /* Notes:
         * Start music some time later
         * Music.CurrentTime can be used to get current time in ms
         * spawn arrows some time before, like 2 sec maybe
         * when Music.CurrentTime matches arrow offset time, it should be at the arrow sprite position
         */

        private const int NumberOfSectionsToGenerateAhead = 4;
        private const int MusicStartOffset = 5 * 1000;

        public override double ExpectedBpm { get; }

        public Song Song { get; }

        private ArrowKeyDrawable[] _arrows;
        private bool _initialized;
        private LinkedList<ScrollingArrowDrawable[]> _notesAhead;
        private IEnumerator<Section> _sectionEnumerator;

        public SimpleKeyScreen(Song song)
        {
            Song = song;
            ExpectedBpm = Song.Bpm;
            _notesAhead = new LinkedList<ScrollingArrowDrawable[]>();
            _sectionEnumerator = Song.Sections.GetEnumerator();
        }

        protected override void LoadComplete()
        {
            AddInternal(new ArrowKeyBindingContainer(Game));
        }

        protected override void Update()
        {
            base.Update();

            if (!Music.IsRunning)
                MusicConductor.Offset = Time.Current;

            MusicConductor.SongPosition = Music.CurrentTime;
            Console.WriteLine(MusicConductor.SongPosition);

            if (!_initialized) Initialize();
            
            // TODO: if arrow drawables are ded, spawn new ones
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio)
        {
            Music = new DrawableTrack(audio.Tracks.Get(@"Songs/Bopeebo/Bopeebo_Inst.ogg"));
            Music.Stop();
            Music.Looping = false;

            Scheduler.AddDelayed(() =>
            {
                Music.Start();
                Music.VolumeTo(1D);
            }, MusicStartOffset); // Start music in 5 seconds

            ArrowKeyAction[] arrowValues = Enum.GetValues<ArrowKeyAction>();
            _arrows = new ArrowKeyDrawable[arrowValues.Length];

            int offset = 20;

            for (int i = 0; i < arrowValues.Length; i++)
            {
                ArrowKeyAction arrowKey = arrowValues[i];
                _arrows[i] = new ArrowKeyDrawable(arrowKey)
                {
                    Anchor = Anchor.Centre,
                    Position = new Vector2(offset, -200),
                    Origin = Anchor.Centre,
                    AlwaysPresent = true,
                    Alpha = 1f
                };

                offset += 170;
            }
        }

        public bool OnPressed(ArrowKeyAction action)
        {
            int value = (int) action;
            if (value >= _arrows.Length) return false;

            _arrows[value].ArrowPressAnim.PlaybackPosition = 0;
            _arrows[value].ArrowPressAnim.Show();

            return true;
        }

        public void OnReleased(ArrowKeyAction action)
        {
            int value = (int) action;
            if (value >= _arrows.Length) return;

            _arrows[value].ArrowPressAnim.Hide();
        }
        
        private void Initialize()
        {
            _initialized = true;

            foreach (ArrowKeyDrawable drawable in _arrows)
                AddInternal(drawable);
            
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
                    arrows[i] = new ScrollingArrowDrawable(note, GetStaticNotePosition(note.Key), Song.Speed, !section.MustHitSection, MusicConductor.SongPosition + MusicStartOffset)
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

        private Vector2 GetStaticNotePosition(ArrowKeyAction key)
        {
            int v = (int) key;
            return _arrows[v].Position;
        }
    }
}