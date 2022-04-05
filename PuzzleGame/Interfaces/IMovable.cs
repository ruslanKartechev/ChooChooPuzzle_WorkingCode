using UnityEngine;
namespace PuzzleGame
{
    public interface IMovable
    {
        void OnMoveStart();
        void OnMoveEnd();
        void TakeInput(Vector2 input);
    }

}
