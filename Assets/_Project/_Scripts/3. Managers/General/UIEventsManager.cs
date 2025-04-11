using System;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Manager.UI
{
    public class UIEventsManager : MonoBehaviour
    {
        public static UIEventsManager Instance { get; private set; }

        public event Action<float> PlayerUIBoostEventTriggered;
        public event Action<float> PlayerUIHealthEventTriggered;
        public event Action<float> PlayerUIMissileEventTriggered;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void UpdateBoostUI(float value)
        {
            PlayerUIBoostEventTriggered?.Invoke(value);
        }

        public void UpdateHealthUI(float value)
        {
            PlayerUIHealthEventTriggered?.Invoke(value);
        }

        public void UpdateMissileUI(float value)
        {
            PlayerUIMissileEventTriggered?.Invoke(value);
        }
    }
}