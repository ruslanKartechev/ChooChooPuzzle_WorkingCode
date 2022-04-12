
namespace PuzzleGame
{
    [System.Serializable]
    public class FinishViewMatchData
    {
        public FinishViewController Arrow;
        public ChainNumber Number;
        public FinishViewMatchData(ChainNumber number, FinishViewController arrow)
        {
            Number = number;
            Arrow = arrow;
        }
    }
}
