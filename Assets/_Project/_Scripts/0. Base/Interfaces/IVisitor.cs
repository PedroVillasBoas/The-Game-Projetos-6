using UnityEngine;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IVisitor 
    {
        void Visit<T>(T visitable) where T : Component, IVisitable;
    }
}