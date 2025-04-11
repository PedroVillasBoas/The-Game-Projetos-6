using System;
using System.Collections.Generic;

namespace GoodVillageGames.Game.Core.Attributes
{
    public class StatsMediator
    {
        readonly LinkedList<StatModifier> modifiers = new();

        public event EventHandler<Query> Queries;
        public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender,query);

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

            // Dispose any that are finished
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