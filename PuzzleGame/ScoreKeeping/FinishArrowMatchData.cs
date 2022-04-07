
namespace PuzzleGame
{
    [System.Serializable]
    public class FinishArrowMatchData
    {
        public FinishArrowController Arrow;
        public ChainNumber Number;
        public FinishArrowMatchData(ChainNumber number, FinishArrowController arrow)
        {
            Number = number;
            Arrow = arrow;
        }
    }
}
