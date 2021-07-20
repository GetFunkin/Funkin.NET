namespace Funkin.NET.Songs
{
    public interface INote
    {
        object[] SectionNotes { get; }
        
        int LengthInSteps { get; }
        
        int TypeOfSection { get; }
        
        bool MustHitSection { get; }
        
        double Bpm { get; }

        bool ChangeBpm { get; }
        
        bool AltAnim { get; }
    }
}