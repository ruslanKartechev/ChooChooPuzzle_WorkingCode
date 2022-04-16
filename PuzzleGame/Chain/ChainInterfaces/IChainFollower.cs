using UnityEngine;

namespace PuzzleGame
{
    public interface IChainFollower
    {
        void MoveLead(Vector2 dir);
        void MoveToFollow(SplineNode node);
  //      void MoveToFollow(SplineNode node,float percent);
   //     void ResetToFollow();
        void OnDirectionChange();
        GameObject GetGo();
    }
}
