using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Util
{
    public class IDTriggerOwner : MonoBehaviour
    {
        private Dictionary<string, List<Action>> actionTriggers = new();
        private Dictionary<string, List<Action<string>>> idActionTriggers = new();

        public void Subscribe(string id, Action action)
        {
            EnsureActionListExists(id);
            actionTriggers[id].Add(action);
        }

        public void SubscribeWithContext(string id, Action<string> action)
        {
            EnsureContextActionListExists(id);
            idActionTriggers[id].Add(action);
        }

        public void SubscribeTrigger(string id, BaseIDTrigger trigger)
        {
            EnsureActionListExists(id);
            actionTriggers[id].Add(trigger.Fire);
        }

        public void UnsubscribeTrigger(string id, BaseIDTrigger trigger)
        {
            Debug.Assert(actionTriggers.ContainsKey(id));
            actionTriggers[id].Remove(trigger.Fire);
        }

        public void Unsubscribe(string id, Action action)
        {
            EnsureActionListExists(id);
            actionTriggers[id].Remove(action);
        }

        public void UnsubscribeWithContext(string id, Action<string> action)
        {
            EnsureContextActionListExists(id);
            idActionTriggers[id].Remove(action);
        }

        public void NotifySubscribers(string id)
        {
            EnsureActionListExists(id);
            EnsureContextActionListExists(id);

            if (actionTriggers[id].Count == 0 && idActionTriggers[id].Count == 0)
                Debug.LogWarning($"No subscribers for ID: {id}");

            foreach (var action in actionTriggers[id]) action.Invoke();
            foreach (var contextAction in idActionTriggers[id]) contextAction.Invoke(id);
        }

        private void EnsureActionListExists(string id)
        {
            if (!actionTriggers.ContainsKey(id))
                actionTriggers.Add(id, new List<Action>());
        }

        private void EnsureContextActionListExists(string id)
        {
            if (!idActionTriggers.ContainsKey(id))
                idActionTriggers.Add(id, new List<Action<string>>());
        }
    }
}