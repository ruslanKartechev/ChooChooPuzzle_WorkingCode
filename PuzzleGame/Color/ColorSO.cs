
using UnityEngine;
namespace PuzzleGame
{
    [CreateAssetMenu(fileName = "ColorSO", menuName = "SO/ColorSO", order = 1)]
    public class ColorSO : ScriptableObject
    {
        [SerializeField] private Color _color = Color.white;
        [SerializeField]  private ParticleSystem.MinMaxGradient _colorOverLifetime;
        public Color GetColor()
        {
            return _color;
        }

        public ParticleSystem.MinMaxGradient GetLiftimeColor()
        {
            return _colorOverLifetime;
        }
    }
}