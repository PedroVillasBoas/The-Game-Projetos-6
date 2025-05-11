using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Enums.Pooling;
using GoodVillageGames.Game.Core.Util.Timer;

namespace GoodVillageGames.Game.Core.Manager
{
    public class ItemSpawnerManager : MonoBehaviour
    {
        public static ItemSpawnerManager Instance { get; private set; }

        [Title("References")]
        [SerializeField] private Transform _playerTransform;

        [Title("Spawn Radius")]
        [SerializeField] private float _minRadius = 50f;
        [SerializeField] private float _maxRadius = 150f;

        [Title("Timing")]
        [SerializeField] private float _minTimeBetweenSpawns = 10f;
        [SerializeField] private float _startTimeSpawn = 30f;
        [SerializeField] private float _decreaseTimeSpawn = 0.5f;

        private CountdownTimer _countdownTimer;
        private readonly PoolID[] _availableItems = {
            PoolID.PickupItemDamage,
            PoolID.PickupItemSpeed,
            PoolID.PickupItemAttackSpeed
        };

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        void OnEnable()  => _countdownTimer = new(_startTimeSpawn);
        void OnDisable() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnStartRun;

        void Start()
        {
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnStartRun;
            _countdownTimer.OnTimerStop = SpawnItem;
        }

        void Update()
        {
            _countdownTimer.Tick(Time.deltaTime);
        }

        public Vector2 GetRandomPositionAroundPlayer()
        {
            if (_playerTransform == null)
            {
                Debug.LogError("Player Transform not assigned!");
                return Vector2.zero;
            }

            Vector2 dir = Random.insideUnitCircle.normalized;
            float dist = Random.Range(_minRadius, _maxRadius);
            // Spawn point
            return (Vector2)_playerTransform.position + dir * dist;
        }

        public PoolID GetRandomItemPoolID()
        {
            int index = Random.Range(0, _availableItems.Length);
            return _availableItems[index];
        }

        void SpawnItem()
        {
            var item = PoolManager.Instance.GetPooledObject(GetRandomItemPoolID());
            if (item != null)
            {
                item.transform.position = GetRandomPositionAroundPlayer();
                item.SetActive(true);
            }
            ResetSpawnTimer();
        }

        void ResetSpawnTimer()
        {
            float next = Mathf.Max(_minTimeBetweenSpawns, _startTimeSpawn - _decreaseTimeSpawn);
            _startTimeSpawn = next;
            _countdownTimer.Reset(next);
        }

        void OnStartRun(GameState gameState)
        {
            if (gameState == GameState.GameBegin)
                _countdownTimer.Start();
        }
    }
}
