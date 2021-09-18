using System;
using System.Collections.Generic;
using System.Linq;
using Funkin.NET.Common.Input;
using Funkin.NET.Common.Screens.Backgrounds;
using Funkin.NET.Core.Music.Songs;
using Funkin.NET.Game.Graphics.Composites.Characters;
using Funkin.NET.Game.Graphics.Composites.Gameplay;
using Funkin.NET.Intermediary.Screens.Backgrounds;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Game.Screens.Gameplay
{
    public class BaseKeyPlayScreen : MusicScreen, IKeyBindingHandler<UniversalAction>
    {
        /* TODO:
         * draw characters
         * register when keys are hit
         * score
         * everything else
         * single sprite for all arrows, recolor/rotate with code
         *
         * Notes:
         * Start music some time later
         * Music.CurrentTime can be used to get current time in ms
         * spawn arrows some time before, like 2 sec maybe
         * when Music.CurrentTime matches arrow offset time, it should be at the arrow sprite position
         * arrows with type 4-7 are for the other character
         */

        public const int NumberOfSectionsToGenerateAhead = 8;
        public const int MusicStartOffset = 5 * 1000;
        public const int ScrollingArrowStartPos = 1000 * 15;

        public static readonly KeyAction[] ArrowValues =
        {
            KeyAction.Left,
            KeyAction.Down,
            KeyAction.Up,
            KeyAction.Right
        };

        public virtual Song Song { get; }

        public virtual DrawableTrack VocalTrack { get; protected set; }

        public virtual string InstrumentalPath { get; }

        public virtual string VocalPath { get; }

        public virtual bool[] IsHeld { get; protected set; }

        public virtual bool[] IsPressed { get; protected set; }

        public IGameData GameData = new GameData();

        protected ArrowKeyDrawable[] PlayerArrows;
        protected ArrowKeyDrawable[] OpponentArrows;
        protected CharacterDrawable[] Characters;
        protected bool Initialized;
        protected readonly LinkedList<ScrollingArrowDrawable[]> NotesAhead = new();
        protected readonly IEnumerator<Section> SectionEnumerators;
        protected Sprite HealthBarBackground;
        protected Box StaticRedBox;
        protected Box DynamicGreenBox;
        protected HealthIconDrawable PlayerIcon;
        protected HealthIconDrawable OpponentIcon;

        public BaseKeyPlayScreen(Song song, string instrumentalPath, string vocalPath)
        {
            Song = song;
            MusicBpm.Value = song.Bpm;
            InstrumentalPath = instrumentalPath;
            VocalPath = vocalPath;
            SectionEnumerators = song.Sections.GetEnumerator();
            //Position = new Vector2(100, 100);
        }

        protected override void Update()
        {
            base.Update();

            if (!Music.IsRunning)
                Conductor.Offset = Time.Current;

            Conductor.CurrentSongPosition = Music.CurrentTime;

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

            foreach (ScrollingArrowDrawable arrowDrawable in NotesAhead.First.Value) RemoveInternal(arrowDrawable);
            NotesAhead.RemoveFirst();
            FillNotes();
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio, TextureStore textures)
        {
            // property init
            Music = new DrawableTrack(audio.Tracks.Get(InstrumentalPath)) {Looping = true};
            VocalTrack = new DrawableTrack(audio.Tracks.Get(VocalPath)) {Looping = true};
            PlayerArrows = new ArrowKeyDrawable[ArrowValues.Length];
            OpponentArrows = new ArrowKeyDrawable[ArrowValues.Length];
            Characters = new CharacterDrawable[3];
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
                KeyAction arrowKey = ArrowValues[i];
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
                KeyAction arrowKey = ArrowValues[i];
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
                flow.AddParagraph($"Accuracy: {GameData.Accuracy}%" +
                                  $"\nHealth: {GameData.Health}" +
                                  $"\nMisses: {GameData.TotalMisses}" +
                                  $"\nTotal Score:{GameData.TotalScore}" +
                                  $"\nHit count: {GameData.NoteHits.Count}", AddFont);
            };

            AddInternal(displayText);

            /*// TESTING:
            Characters[0] = new CharacterDrawable("gf", CharacterType.Girlfriend)
            {
                Position = new Vector2(0f, 200f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            Characters[1] = new CharacterDrawable("dad", CharacterType.Opponent)
            {
                Position = new Vector2(-300f, 200f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            Characters[2] = new CharacterDrawable("bf", CharacterType.Boyfriend)
            {
                Position = new Vector2(300f, 200f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            foreach (CharacterDrawable drawable in Characters)
                AddInternal(drawable);*/

            HealthBarBackground = new Sprite
            {
                Texture = textures.Get("Shared/healthBar"),
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.Centre,
                Position = new Vector2(-0f, -50f),
                Scale = new Vector2(2f)
            };

            AddInternal(HealthBarBackground);

            StaticRedBox = new Box
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.Centre,
                Position = new Vector2(0f, -50f),
                Colour = Colour4.Red
            };

            StaticRedBox.OnUpdate += drawable =>
            {
                drawable.Width = HealthBarBackground.Texture.Width - 8f;
                drawable.Height = HealthBarBackground.Texture.Height - 8f;
            };

            AddInternal(StaticRedBox);

            DynamicGreenBox = new Box
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.CentreRight,
                Position = new Vector2(0f, -50f),
                Colour = Colour4.Lime,
                AlwaysPresent = true
            };

            DynamicGreenBox.OnUpdate += drawable =>
            {
                drawable.Width = HealthBarBackground.Texture.Width / 2f - 4f;
                drawable.Height = HealthBarBackground.Texture.Height - 8f;
                drawable.Position = new Vector2(HealthBarBackground.Texture.Width / 2f - 4f, drawable.Position.Y);
                drawable.Scale = new Vector2(ScrollingArrowDrawable.Lerp(drawable.Scale.X, GameData.Health * 2f, 0.01f),
                    1f);
            };

            AddInternal(DynamicGreenBox);

            PlayerIcon = new HealthIconDrawable("bf", true)
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.CentreRight,
                Position = new Vector2(0f, -50f)
            };

            PlayerIcon.OnUpdate += drawable =>
            {
                drawable.Position =
                    new Vector2(DynamicGreenBox.Position.X - DynamicGreenBox.DrawWidth * DynamicGreenBox.Scale.X + 25f,
                        drawable.Position.Y);

                ((HealthIconDrawable) drawable).Set(GameData.Health <= 0.2f
                    ? HealthIconDrawable.Type.Dead
                    : HealthIconDrawable.Type.Alive);
            };

            AddInternal(PlayerIcon);

            OpponentIcon = new HealthIconDrawable("dad")
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.CentreRight,
                Position = new Vector2(0f, -50f)
            };

            OpponentIcon.OnUpdate += drawable =>
            {
                drawable.Position =
                    new Vector2(DynamicGreenBox.Position.X - DynamicGreenBox.DrawWidth * DynamicGreenBox.Scale.X - 25f,
                        drawable.Position.Y);

                ((HealthIconDrawable) drawable).Set(GameData.Health >= 0.8f
                    ? HealthIconDrawable.Type.Dead
                    : HealthIconDrawable.Type.Alive);
            };

            AddInternal(OpponentIcon);
        }

        protected override void BeatHit()
        {
            base.BeatHit();

            Vector2 oldPlayerScale = PlayerIcon.Scale;
            PlayerIcon.ScaleTo(PlayerIcon.Scale * 1.25f, 100D);

            Scheduler.AddDelayed(() => { PlayerIcon.ScaleTo(oldPlayerScale, 500D); }, 100D);

            Vector2 oldOpponentScale = OpponentIcon.Scale;
            OpponentIcon.ScaleTo(OpponentIcon.Scale * 1.25f, 100D);

            Scheduler.AddDelayed(() => { OpponentIcon.ScaleTo(oldOpponentScale, 500D); }, 100D);
        }

        public virtual bool OnPressed(UniversalAction action)
        {
            if (!ArrowValues.Contains((KeyAction) (int) action))
                return false;

            int value = (int) action;
            if (value >= PlayerArrows.Length || NotesAhead.First is null)
                return false;

            // if pressed on previous update frame the
            // IsHeld should be true :)
            if (IsPressed[value])
                IsHeld[value] = true;

            // set IsPressed to opposite of IsHeld
            // if not held, then pressed
            // else, held
            IsPressed[value] = !IsHeld[value];

            if (!IsHeld[value])
            {
                PlayerArrows[value].ArrowPressAnim.PlaybackPosition = 0;
                PlayerArrows[value].ArrowPressAnim.Show();
                PlayerArrows[value].ArrowIdleSprite.Hide();
            }

            Console.WriteLine($"{IsPressed[value]}, {IsHeld[value]}");

            foreach (ScrollingArrowDrawable arrow in NotesAhead.SelectMany(arrowArray => arrowArray))
                arrow.Press((KeyAction) (int) action, IsHeld[value]);

            return true;
        }

        public virtual void OnReleased(UniversalAction action)
        {
            if (!ArrowValues.Contains((KeyAction) (int) action))
                return;

            int value = (int) action;
            if (value >= PlayerArrows.Length || NotesAhead.First is null)
                return;

            PlayerArrows[value].ArrowPressAnim.Hide();
            PlayerArrows[value].ArrowIdleSprite.Show();

            IsPressed[value] = false;
            IsHeld[value] = false;

            foreach (ScrollingArrowDrawable arrow in NotesAhead.SelectMany(arrowArray => arrowArray))
                arrow.Release((KeyAction) (int) action);
        }

        protected virtual void Initialize()
        {
            Conductor.Offset = Time.Current;

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

                Console.WriteLine($"Position: {Conductor.CurrentSongPosition} - Offset: {Conductor.Offset}");

                ScrollingArrowDrawable[] arrows = new ScrollingArrowDrawable[section.SectionNotes.Count];
                for (int i = 0; i < section.SectionNotes.Count; i++)
                {
                    Note note = section.SectionNotes[i];
                    int keyToUse = (int) note.Key;

                    if (keyToUse >= 4)
                        keyToUse -= 4;

                    bool mustHitSection = (section.MustHitSection && (int) note.Key < 4) ||
                                          (!section.MustHitSection && (int) note.Key >= 4);

                    Vector2 notePos = mustHitSection ? PlayerArrows[keyToUse].Position : OpponentArrows[keyToUse].Position;

                    double startOffset = Conductor.Offset;
                    if (!Music.IsRunning)
                        startOffset += MusicStartOffset;

                    note.Offset += startOffset;
                    note.Key = (KeyAction)keyToUse;

                    arrows[i] = new ScrollingArrowDrawable(note, notePos, Song.Speed, !mustHitSection)
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

        public override IBackgroundScreen CreateBackground() => new DefaultBackgroundScreen(DefaultBackgroundType.Yellow);
    }
}