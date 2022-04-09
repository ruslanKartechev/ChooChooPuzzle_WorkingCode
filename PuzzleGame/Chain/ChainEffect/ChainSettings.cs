using System.Collections;
using UnityEngine;

namespace PuzzleGame
{
    [CreateAssetMenu(fileName = "ChainSettings", menuName = "ScriptableObjects/ChainSettings", order = 1)]
    public class ChainSettings : ScriptableObject
    {
        public FollowerSettings followerSettings;
        [Space(15)]
        public ChainEffectsSettings ChainEffects;

    }





}