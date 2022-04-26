using UnityEngine;
namespace PuzzleGame
{
    public interface IFinish
    {
        ChainNumber GetNumber();
        void OnReached();
        void OnCompleted();
        void OnWrong();
        Vector3 GetFinishPoint();
    }
}
