using UnityEngine;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Pickups;
using GoodVillageGames.Game.Core.Manager.UI;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PlayerExpManager : MonoBehaviour
    {
        public static PlayerExpManager Instance { get; private set; }

        [SerializeField] private CircleCollider2D _expColliderDetector;
        [SerializeField] private PlayerExpHandler _expHandler;

        private int _currentExp = 0;
        private int _maxExp = 5;
        private int _currentLevel = 1;
        private readonly int levelCap = 50;

        public int CurrentLevel { get => _currentLevel; }
        public int CurrentExp { get => _currentExp; }
        public int ExpToNextLevel { get => _maxExp; }

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

        }

        void Start() => _expHandler.OnExpCollectedEventTriggered += AddExp;
        void OnDisable() => _expHandler.OnExpCollectedEventTriggered -= AddExp;

        public void AddExp(int expAmount)
        {
            if (expAmount <= 0 || _currentLevel >= levelCap)
                return;

            _currentExp += expAmount;
            ProcessExperience();
        }

        private void ProcessExperience()
        {
            // If we have a case where a bunch of exp is gathered at once
            while (_currentExp >= _maxExp && _currentLevel < levelCap)
            {
                UIEventsManager.Instance.UpdateExpUI(1.0f);
                _currentExp -= _maxExp;
                LevelUp();
            }

            UIEventsManager.Instance.UpdateExpUI((float)_currentExp / _maxExp);
        }

        void IncreaseMaxExp()
        {
            float pct;

            // LVLs 1–10  // 10%–30%
            if (_currentLevel <= 10)
                pct = Random.Range(0.10f, 0.30f);
            // LVLs 11–25  // 3%–10%
            else if (_currentLevel <= 25)
                pct = Random.Range(0.03f, 0.10f);
            // LVLs 26–40  // 1%–3%
            else if (_currentLevel <= 40)
                pct = Random.Range(0.01f, 0.03f);
            // LVLs 41–50  // 10%–15%
            else
                pct = Random.Range(0.10f, 0.15f);

            _maxExp = Mathf.CeilToInt(_maxExp * (1 + pct));
        }

        public void LevelUp()
        {
            if (_currentLevel >= levelCap)
                return;

            _currentLevel++;
            IncreaseMaxExp();

            PlayerAudioHandler.Instance.PlayPlayerLevelUpSFX();
            GlobalEventsManager.Instance.ChangeGameState(GameState.UpgradeScreen);
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
