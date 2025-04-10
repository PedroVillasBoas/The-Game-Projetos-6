using UnityEngine;
using System.Collections;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.GameObjectEntity;


namespace GoodVillageGames.Game.Handlers
{
    public class MissileReloadHandler : MonoBehaviour, IReloadHandler
    {
        private Stats _entityStats;
        private float _currentAttackSpeed;
        private bool _isReloading;
        private Coroutine _reloadCoroutine;

        public float AttackSpeed { get => _currentAttackSpeed; set => _currentAttackSpeed = value; }
        public bool IsReloading { get => _isReloading; set => _isReloading = value; }
        public Coroutine ReloadCoroutine { get => _reloadCoroutine; set => _reloadCoroutine = value; }

        void Start()
        {
            if (transform.root.TryGetComponent<Entity>(out var statsProvider))
            {
                _entityStats = statsProvider.Stats;
                _currentAttackSpeed = _entityStats.AttackSpeed;
            }
            else
                Debug.LogError("IStatsProvider component not found on entity!", this);
        }

        public IEnumerator Reload()
        {
            if (_isReloading) yield break;

            _isReloading = true;
            yield return new WaitForSeconds(1f / _currentAttackSpeed);
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
