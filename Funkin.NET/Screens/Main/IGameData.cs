using System.Collections.Generic;

namespace Funkin.NET.Screens.Main
{
    public interface IGameData
    {
        public enum HitAccuracyType
        {
            // include for potential future cases
            // of people/us implementing notes that
            // are meant to be missed (i.e. funny tricky)
            Uncounted,
            Missed,
            Shit,
            Bad,
            Good,
            Sick
        }

        public int TotalMisses { get; }

        public int TotalScore { get; }

        public float Accuracy { get; }

        // range from 0 - 1
        public float Health { get; set; }

        public List<HitAccuracyType> NoteHits { get; set; }

        float GetAccuracyFromNoteHits(IEnumerable<HitAccuracyType> hits);

        void AddToScore(HitAccuracyType hit);

        void ModifyHealth(HitAccuracyType hit);
    }
}