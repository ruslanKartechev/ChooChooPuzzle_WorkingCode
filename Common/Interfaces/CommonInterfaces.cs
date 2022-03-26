using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BomberGame
{

    public interface IBomb
    {
        void Init();
        GameObject GetGO();
        
    }
    public interface IBomber
    {
         void PlaceBomb();
         void Init();
        
    }
    public interface IBombTarget
    {
        void OnExplosion(float force);
        Transform GetTransform();
        
    }

    public interface IBreakable
    {
        void Break();

    }

    public interface IEffectsHandler
    {
        void Init();
        void Show();
        void Hide();
        void Show(float time);
        GameObject GetGo();
        void SetPosition(Vector3 pos);
        
    }

}