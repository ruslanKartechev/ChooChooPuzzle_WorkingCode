
using UnityEngine;
namespace PuzzleGame
{
    [System.Serializable]
    public class FinishArrowLights
    {
        public Light lightSource;
        public Color targetColor;
        public bool DoResetColor = false;
        public void Init()
        {
            Deactivate();
        }
        public void Activate()
        {
            lightSource.enabled = true;

        }
        public void Deactivate()
        {
            lightSource.enabled = false;
        }

    }
}
