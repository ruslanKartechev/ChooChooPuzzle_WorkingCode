using System.Collections;
using UnityEngine;

namespace PuzzleGame
{
    public interface IChainMovable
    {
        ChainPositionInfo GetChainPosition();
    }
}