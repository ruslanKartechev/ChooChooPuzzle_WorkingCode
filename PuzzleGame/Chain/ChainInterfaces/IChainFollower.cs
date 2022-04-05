using UnityEngine;

namespace PuzzleGame
{
    public interface IChainFollower
    {
        void MoveLead(Vector2 dir);
        void MoveToFollow(SplineNode node);
        void OnDirectionChange();
        GameObject GetGo();
    }
}
