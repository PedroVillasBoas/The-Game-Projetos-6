namespace GoodVillageGames.Game.Interfaces
{
    /// <summary>
    /// Visitor Pattern
    /// </summary>
    /// <remarks>
    /// <see cref="IVisitor"/>
    /// </remarks>
    public interface IVisitable 
    {
        void Accept(IVisitor visitor);
    }
}