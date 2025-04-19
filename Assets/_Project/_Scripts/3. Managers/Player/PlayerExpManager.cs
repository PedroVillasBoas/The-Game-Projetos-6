using UnityEngine;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Pickups;
using GoodVillageGames.Game.Core.Manager.UI;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PlayerExpManager : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D _expColliderDetector;
        [SerializeField] private PlayerExpHandler _expHandler;

        [SerializeField] private float _expMultiplier = 1.5f;
        private int _currentExp = 0;
        private int _maxExp = 5;
        private int _currentLevel = 1;

        public int CurrentLevel { get => _currentLevel; }

        void Start()
        {
            _expHandler.OnExpCollectedEventTriggered += AddExp;
        }

        public void AddExp(int expAmount)
        {
            if(expAmount <= 0)
                return;

            _currentExp += expAmount;
            ProcessExperience();
        }

        private void ProcessExperience()
        {
            // If we have a case where a bunch of exp is gathered at once
            while(_currentExp >= _maxExp)
            {
                UIEventsManager.Instance.UpdateExpUI(1.0f);
                _currentExp -= _maxExp;
                LevelUp();
            }

            UIEventsManager.Instance.UpdateExpUI((float)_currentExp / _maxExp);
        }

        void IncreaseMaxExp()
        {
            _maxExp = Mathf.RoundToInt(_maxExp * _expMultiplier);
        }

        public void LevelUp()
        {
            _currentLevel++;
            IncreaseMaxExp();
            GlobalEventsManager.Instance.ChangeGameState(Enums.Enums.GameState.UpgradeScreen);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<ExpPickup>(out var expPickup))
            {
                expPickup.SetPlayer(gameObject);
            }
        }
    }
}
