
namespace PuzzleGame
{
    [System.Serializable]
    public class FinishViewMatchData
    {
        public FinishHeadView Arrow;
        public ChainNumber Number;
        public FinishViewMatchData(ChainNumber number, FinishHeadView arrow)
        {
            Number = number;
            Arrow = arrow;
        }
    }
}
