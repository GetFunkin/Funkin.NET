using System;

namespace Funkin.NET.Core.Music.Songs.Beatmaps.Accuracy
{
    public class HitResult
    {
        #region Defaults

        public static readonly HitResult Sick = new();
        public static readonly HitResult Good = new();
        public static readonly HitResult Bad = new();
        public static readonly HitResult Shit = new();
        public static readonly HitResult Miss = new();

        #endregion

        public Action OnHit;

        public void Hit()
        {
            OnHit?.Invoke();
        }
    }
}