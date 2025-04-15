using UnityEngine;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Util.Timer;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Manager
{
    public class ItemSpawnerManager : MonoBehaviour
    {
        public static ItemSpawnerManager Instance { get; private set; }

        [SerializeField] private SpriteRenderer _worldBounds;
        [SerializeField] private float _minTimeBetweenSpawns = 30f;
        [SerializeField] private float _startTimeSpawn = 300f;
        [SerializeField] private float _decreaseTimeSpawn = 5f;

        private CountdownTimer _countdownTimer;

        private readonly PoolID[] _availableItems = { PoolID.PickupItemDamage, PoolID.PickupItemSpeed, PoolID.PickupItemAttackSpeed };


        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void OnEnable()
        {
            _countdownTimer = new(_startTimeSpawn);
        }

        void OnDisable()
        {
            GlobalEventsManager.Instance.StartRunEventTriggered -= OnStartRun;
        }

        void Start()
        {
            GlobalEventsManager.Instance.StartRunEventTriggered += OnStartRun;
            _countdownTimer.OnTimerStop = () => SpawnItem();
        }

        void Update()
        {
            _countdownTimer.Tick(Time.deltaTime);
        }

        public Vector2 GetRandomPositionWithinSprite()
        {
            if (_worldBounds == null)
            {
                Debug.LogError("SpriteRenderer was not found!");
                return Vector2.zero;
            }

            Bounds bounds = _worldBounds.bounds;

            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);

            return new Vector2(randomX, randomY);
        }

        public PoolID GetRandomItemPoolID()
        {
            int index = Random.Range(0, _availableItems.Length);
            return _availableItems[index];
        }

        void SpawnItem()
        {
            GameObject item = PoolManager.Instance.GetPooledObject(GetRandomItemPoolID());

            if (item != null)
            {
                Vector3 spawnPos = GetRandomPositionWithinSprite();
                item.transform.position = spawnPos;
                item.SetActive(true);
            }

            ResetSpawnTimer();
        }

        void ResetSpawnTimer()
        {
            if (_startTimeSpawn - _decreaseTimeSpawn >= _minTimeBetweenSpawns)
            {
                _startTimeSpawn -= _decreaseTimeSpawn;
                _countdownTimer.Reset(_startTimeSpawn);
            }
            else
            {
                _countdownTimer.Reset(_minTimeBetweenSpawns);
            }
        }

        private void OnStartRun()
        {
            _countdownTimer.Start();
        }
    }
}
