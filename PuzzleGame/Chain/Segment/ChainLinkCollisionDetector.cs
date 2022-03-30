using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace PuzzleGame
{

    public class CuttableDetector:MonoBehaviour, ICuttable
    {
        public Action onCut = null;
        public virtual void Cut() 
        { 
            
        }
        public virtual void Deactivate()
        {
            onCut = null;
        }
    }

    public class ChainLinkCollisionDetector: CuttableDetector
    {
        public override void Cut()
        {
            if(onCut == null) { Debug.Log("Cut Action not assigned");return; }
            onCut?.Invoke();
        }
        

    }
}
