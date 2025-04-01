using UnityEngine;
using System.Collections;
using GoodVillageGames.Game.Core;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Manager;

namespace GoodVillageGames.Game.Handlers
{
    public class AttackReloadHandler : MonoBehaviour, IReloadHandler
    {
        private PlayerStatsManager _playerStats;
        private float _currentAttackSpeed;
        private Coroutine _reloadCoroutine;

        public float AttackSpeed { get => _currentAttackSpeed; set => _currentAttackSpeed = value; }
        public Coroutine ReloadCoroutine { get => _reloadCoroutine; set => _reloadCoroutine = value; }

        void Start()
        {
            if (transform.root.TryGetComponent<PlayerActions>(out var player))
            {
                _playerStats = player.PlayerStatsManager;
            }

            if (_playerStats != null)
            {
                _currentAttackSpeed = _playerStats.AttackSpeed;
            }
        }

        void Update()
        {
            // a gente pode fazer de formas diferentes
                // ou usando o attackReloadHandler com o FireHandler
                    // Mas ai a gente precisa se alguma coisa pra pegar os stats do parent, pra poder reutilizar a interface
                    // Se nao, nao faz sentido ter a interface
                // Ou a gente tem os dois, pega o component stats de StatsManager base de um parent generico
                    // E ai a gente tem um middle man pra poder fazer as chamadas ou herda dos dois ou utiliza os dois pra fazer atirar
        }

        public IEnumerator Reload()
        {
            throw new System.NotImplementedException();
        }
    }
}
