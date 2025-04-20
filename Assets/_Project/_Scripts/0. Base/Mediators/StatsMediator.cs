using System;
using System.Collections.Generic;

namespace GoodVillageGames.Game.Core.Attributes
{
    /// <summary>
    /// This is the Broker. It sits between the Stats Class and a StatModifier
    /// </summary>
    /// <remarks>
    /// <see cref="Stats"/>
    /// <see cref="StatModifier"/>
    /// </remarks>
    public class StatsMediator
    {
        public readonly LinkedList<StatModifier> modifiers = new();

        // Perfoming a Query/Consult whenever something wants to know about a specific Stat
        public event EventHandler<Query> Queries;
        public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender,query);

        // Adding a modifier/upgrade to the List
        public void AddModifier(StatModifier modifier)
        {
            modifiers.AddLast(modifier);
            Queries += modifier.Handle;

            modifier.OnDispose += _ => {
                modifiers.Remove(modifier);
                Queries -= modifier.Handle;
            };
        }

        public void Update(float deltaTime)
        {
            // Updating all modifier with deltaTime
            var node = modifiers.First;
            while (node != null)
            {
                var modifier = node.Value;
                modifier.Update(deltaTime);
                node = node.Next;
            }

            // Disposing any that are finished
            node = modifiers.First;
            while (node != null)
            {
                var nextNode = node.Next;
                if (node.Value.MarkedForRemoval)
                {
                    node.Value.Dispose();
                }

                node = nextNode;
            }
        }
    }
}