using System;
namespace PuzzleGame
{
    public class MovesTracker
    {
        public Action onValueChange;
        public int TotalMoves = 0;
        public void Init(Action onChange)
        {
            onValueChange = onChange;
            TotalMoves = 0;
            onValueChange?.Invoke();
        }

        public void Refresh()
        {
            TotalMoves = 0;
            onValueChange?.Invoke();

        }
        public void AddCount()
        {
            TotalMoves++;
            onValueChange?.Invoke();
        }



    }
}
