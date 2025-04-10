using UnityEngine;
namespace GoodVillageGames.Game.Interfaces
{
    public interface IVisitable 
    {
        void Accept(IVisitor visitor);
    }
}