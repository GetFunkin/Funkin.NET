using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Game.Screens.Gameplay
{
    // TODO: turn HitAccuracyType into an interface-based OOP design to simplify adding your own note types and stuff
    public class GameData : IGameData
    {
        // TODO: investigate accuracy calculations based on arrow hits instead of accuracy messages
        public static readonly Dictionary<IGameData.HitAccuracyType, float> AccuracyMap = new()
        {
            // uncounted included for the sake of completeness
            {IGameData.HitAccuracyType.Uncounted, 0f},
            {IGameData.HitAccuracyType.Missed, 0f},
            {IGameData.HitAccuracyType.Shit, 0.25f},
            {IGameData.HitAccuracyType.Bad, 0.5f},
            {IGameData.HitAccuracyType.Good, 0.75f},
            {IGameData.HitAccuracyType.Sick, 1f}
        };

        public static readonly Dictionary<IGameData.HitAccuracyType, int> ScoreMap = new()
        {
            // TODO: are these rewarding? fair?
            {IGameData.HitAccuracyType.Uncounted, 0},
            {IGameData.HitAccuracyType.Missed, -50},
            {IGameData.HitAccuracyType.Shit, 10},
            {IGameData.HitAccuracyType.Bad, 35},
            {IGameData.HitAccuracyType.Good, 75},
            {IGameData.HitAccuracyType.Sick, 150}
        };

        public static readonly Dictionary<IGameData.HitAccuracyType, float> HealthMap = new()
        {
            {IGameData.HitAccuracyType.Uncounted, 0f},
            {IGameData.HitAccuracyType.Missed, -0.08f},
            {IGameData.HitAccuracyType.Shit, -0.02f}, // little bit of tr
            {IGameData.HitAccuracyType.Bad, 0.01f},
            {IGameData.HitAccuracyType.Good, 0.02f},
            {IGameData.HitAccuracyType.Sick, 0.04f}
        };

        public virtual int TotalMisses => NoteHits.Count(x => x == IGameData.HitAccuracyType.Missed);

        public virtual int TotalScore { get; set; }

        public virtual float Accuracy => MathF.Round(GetAccuracyFromNoteHits(NoteHits), 2);

        // TODO: health should lower on misses and raise on hits by a constant amount. also make that changeable easily?
        public virtual float Health { get; set; }

        public virtual List<IGameData.HitAccuracyType> NoteHits { get; set; }

        public GameData()
        {
            NoteHits = new List<IGameData.HitAccuracyType>();
            Health = 0.5f;
        }

        public virtual float GetAccuracyFromNoteHits(IEnumerable<IGameData.HitAccuracyType> notes)
        {
            notes = notes.Where(x => x != IGameData.HitAccuracyType.Uncounted);

            List<IGameData.HitAccuracyType> hitAccuracyTypes = notes.ToList();
            int count = hitAccuracyTypes.Count;
            float total = hitAccuracyTypes.Sum(x => AccuracyMap[x]);

            return total / count;
        }

        public virtual void AddToScore(IGameData.HitAccuracyType hit) => TotalScore += ScoreMap[hit];

        public virtual void ModifyHealth(IGameData.HitAccuracyType hit)
        {
            Health = Math.Clamp(Health + HealthMap[hit], 0f, 1f);
        }
    }
}