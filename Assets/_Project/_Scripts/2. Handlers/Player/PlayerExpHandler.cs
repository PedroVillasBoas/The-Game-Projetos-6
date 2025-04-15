using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using System;

namespace GoodVillageGames.Game.Handlers
{
    public class PlayerExpHandler : MonoBehaviour, IVisitable
    {
        public event Action<int> OnExpCollectedEventTriggered;

        public void ExpCollected(int expAmount)
        {
            if(expAmount <= 0) return;
            
            OnExpCollectedEventTriggered?.Invoke(expAmount);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
