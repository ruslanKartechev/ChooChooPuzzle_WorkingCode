using System.Collections;
using UnityEngine;

namespace PuzzleGame
{
    [CreateAssetMenu(fileName = "ChainEffectsSettings", menuName = "ScriptableObjects/ChainEffectsSettings", order = 1)]
    public class ChainEffectsSettings : ScriptableObject
    {
        public bool ShowEffects = true;
        public SqueezeEffectsSettings SqueezeEffect;
        
    }





}