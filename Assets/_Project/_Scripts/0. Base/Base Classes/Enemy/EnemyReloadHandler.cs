using UnityEngine;
using System.Collections;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.Enemy.AI;

namespace GoodVillageGames.Game.Handlers
{
    public class EnemyReloadHandler : MonoBehaviour, IReloadHandler
    {
        private Stats _entityStats;
        private bool _isReloading;
        private Coroutine _reloadCoroutine;

        public float AttackSpeed { get => _entityStats.AttackSpeed; }
        public bool IsReloading { get => _isReloading; set => _isReloading = value; }
        public Coroutine ReloadCoroutine { get => _reloadCoroutine; set => _reloadCoroutine = value; }

        void Start()
        {
            if (transform.parent.parent.TryGetComponent<EnemyAI>(out var enemyProvider))
            {
                _entityStats = enemyProvider.Stats;
            }
            else
                Debug.LogError("EnemyProvider component not found on entity!", this);
        }

        public IEnumerator Reload()
        {
            if (_isReloading) yield break;

            _isReloading = true;
            yield return new WaitForSeconds(1f / _entityStats.AttackSpeed);
            _isReloading = false;
        }

        public void CancelReload()
        {
            if (_reloadCoroutine != null)
            {
                StopCoroutine(_reloadCoroutine);
                _isReloading = false;
            }
        }
    }
}
