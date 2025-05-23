using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.General;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Attributes;

namespace GoodVillageGames.Game.Core.GameObjectEntity
{
    /// <summary>
    /// Base Class for all GameObjects that will have interaction and movement
    /// </summary>
    public abstract class Entity : MonoBehaviour, IVisitable
    {
        [SerializeField, InlineEditor, Required] private BaseStats _baseStats;
        public Stats Stats { get; private set; }

        protected virtual void Awake()
        {
            Stats = new(new StatsMediator(), _baseStats);
        }

        public virtual void Update()
        {
            Stats.Mediator.Update(Time.deltaTime);
        }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}