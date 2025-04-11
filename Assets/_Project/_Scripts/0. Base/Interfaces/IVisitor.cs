using UnityEngine;

namespace GoodVillageGames.Game.Interfaces
{
    /// <summary>
    /// Visitor Pattern
    /// </summary>
    /// <remarks>
    /// <see cref="IVisitable"/>
    /// </remarks>
    public interface IVisitor 
    {
        void Visit<T>(T visitable) where T : Component, IVisitable;
    }
}