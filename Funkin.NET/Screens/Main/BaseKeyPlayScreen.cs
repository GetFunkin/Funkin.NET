﻿using System;
using System.Collections.Generic;
using System.Linq;
using Funkin.NET.Conductor;
using Funkin.NET.Graphics.Sprites;
using Funkin.NET.Graphics.Sprites.Characters;
using Funkin.NET.Input.Bindings;
using Funkin.NET.Songs;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osuTK;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Screens.Main
{
    public class BaseKeyPlayScreen : MusicScreen, IKeyBindingHandler<UniversalAction>
    {
        /* TODO:
         * draw characters
         * register when keys are hit
         * score
         * everything else
         * single sprite for all arrows, recolor/rotate with code
         * why is there a sixth arrow "[62100,6,0]"???????
         *
         * Notes:
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

        public virtual DrawableTrack VocalTrack { get; protected set; }

        public virtual string InstrumentalPath { get; }

        public virtual string VocalPath { get; }

        public virtual bool[] IsHeld { get; protected set; }

        public virtual bool[] IsPressed { get; protected set; }

        public IGameData GameData;

        protected ArrowKeyDrawable[] PlayerArrows;
        protected ArrowKeyDrawable[] OpponentArrows;
        protected bool Initialized;
        protected readonly LinkedList<ScrollingArrowDrawable[]> NotesAhead;
        protected readonly IEnumerator<Section> SectionEnumerators;

        public BaseKeyPlayScreen(Song song, string instrumentalPath, string vocalPath)
        {
            Song = song;
            ExpectedBpm = song.Bpm;
            InstrumentalPath = instrumentalPath;
            VocalPath = vocalPath;
            NotesAhead = new LinkedList<ScrollingArrowDrawable[]>();
            SectionEnumerators = song.Sections.GetEnumerator();
            GameData = new GameData();
        }

        protected override void Update()
        {
            base.Update();

            if (!Music.IsRunning)
                MusicConductor.Offset = Time.Current;

            MusicConductor.SongPosition = Music.CurrentTime;

            if (!Initialized)
            {
                Initialize();
                Initialized = true;
            }

            if (NotesAhead.First is null)
                return;

            foreach (ScrollingArrowDrawable arrow in NotesAhead.SelectMany(arrowArray => arrowArray))
                arrow.UpdateGameData(ref GameData);

            // If all of the notes in the first section are dead, remove them and refill notes
            if (!NotesAhead.First.Value.All(x => Time.Current >= x.LifetimeEnd))
                return;

            Console.WriteLine($"remove first!!!!! {NotesAhead.First}");
            foreach (ScrollingArrowDrawable arrowDrawable in NotesAhead.First.Value) RemoveInternal(arrowDrawable);
            NotesAhead.RemoveFirst();
            FillNotes();
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio)
        {
            // property init
            Music = new DrawableTrack(audio.Tracks.Get(InstrumentalPath)) {Looping = true};
            VocalTrack = new DrawableTrack(audio.Tracks.Get(VocalPath)) {Looping = true};
            PlayerArrows = new ArrowKeyDrawable[ArrowValues.Length];
            OpponentArrows = new ArrowKeyDrawable[ArrowValues.Length];
            IsHeld = new bool[ArrowValues.Length];
            IsPressed = new bool[ArrowValues.Length];

            for (int i = 0; i < ArrowValues.Length; i++)
            {
                IsHeld[i] = false;
                IsPressed[i] = false;
            }

            // initialize arrows
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

                AddInternal(PlayerArrows[i]);

                offset += 75f;
            }

            // loop backwards for opponent arrows because they
            // go the other direction lol
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
                    Alpha = 1f

                };

                AddInternal(OpponentArrows[i]);

                offset += 75f;
            }

            // TEMPORARY
            TextFlowContainer displayText = new()
            {
                Anchor = Anchor.TopLeft,
                Padding = new MarginPadding
                {
                    Top = 20f,
                    Left = 20f
                },
                Alpha = 1f,
                AlwaysPresent = true,
                Width = 400f
            };

            displayText.OnUpdate += drawable =>
            {
                if (drawable is not TextFlowContainer flow)
                    return;

                static void AddFont(SpriteText text) => text.Font = new FontUsage("Torus-Regular", 35f);

                flow.Text = "";
                flow.AddParagraph($"Accuracy: {GameData.Accuracy}" +
                                  $"\nHealth: {GameData.Health}" +
                                  $"\nMisses: {GameData.TotalMisses}" +
                                  $"\nTotal Score:{GameData.TotalScore}" +
                                  $"\nHit count: {GameData.NoteHits.Count}", AddFont);
            };

            AddInternal(displayText);

#if DEBUG
            // TESTING:
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
#endif
        }

        public virtual bool OnPressed(UniversalAction action)
        {
            if (!ArrowValues.Contains((KeyAssociatedAction) (int) action))
                return false;

            int value = (int) action;
            if (value >= PlayerArrows.Length || NotesAhead.First is null)
                return false;

            PlayerArrows[value].ArrowPressAnim.PlaybackPosition = 0;
            PlayerArrows[value].ArrowPressAnim.Show();
            PlayerArrows[value].ArrowIdleSprite.Hide();

            // if pressed on previous update frame the
            // IsHeld should be true :)
            if (IsPressed[value])
                IsHeld[value] = true;

            // set IsPressed to opposite of IsHeld
            // if not held, then pressed
            // else, held
            IsPressed[value] = !IsHeld[value];

            Console.WriteLine($"{IsPressed[value]}, {IsHeld[value]}");

            foreach (ScrollingArrowDrawable arrow in NotesAhead.SelectMany(arrowArray => arrowArray))
                arrow.Press((KeyAssociatedAction) (int) action, IsHeld[value]);

            return true;
        }

        public virtual void OnReleased(UniversalAction action)
        {
            if (!ArrowValues.Contains((KeyAssociatedAction) (int) action))
                return;

            int value = (int) action;
            if (value >= PlayerArrows.Length || NotesAhead.First is null)
                return;

            PlayerArrows[value].ArrowPressAnim.Hide();
            PlayerArrows[value].ArrowIdleSprite.Show();

            IsPressed[value] = false;
            IsHeld[value] = false;

            foreach (ScrollingArrowDrawable arrow in NotesAhead.SelectMany(arrowArray => arrowArray))
                arrow.Release((KeyAssociatedAction) (int) action);
        }

        protected virtual void Initialize()
        {
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

        protected virtual void FillNotes()
        {
            while (NotesAhead.Count <= NumberOfSectionsToGenerateAhead)
            {
                if (!SectionEnumerators.MoveNext())
                    break;

                Section section = SectionEnumerators.Current;

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

                    arrows[i] = new ScrollingArrowDrawable(note, notePos, Song.Speed, !section.MustHitSection, startOffset)
                    {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Position = new Vector2(0, ScrollingArrowStartPos)
                    };

                    AddInternal(arrows[i]);
                }

                NotesAhead.AddLast(arrows);
            }
        }

        public static BaseKeyPlayScreen GetPlayScreen(string json, string instrumental, string voices) =>
            new(Song.GetSongFromFile(json), instrumental, voices);
    }
}