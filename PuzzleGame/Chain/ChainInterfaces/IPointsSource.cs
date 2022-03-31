using UnityEngine;
using System.Collections.Generic;
namespace PuzzleGame
{
    public interface IPointsSource
    {
        List<Transform> GetPoints();
    }
}
