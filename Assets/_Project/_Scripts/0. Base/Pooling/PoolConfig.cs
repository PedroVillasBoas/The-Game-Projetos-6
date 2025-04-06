using UnityEngine;


namespace GoodVillageGames.Game.Core.Pooling
{
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "Scriptable Objects/Pool")]
    public class PoolConfig : ScriptableObject
    {
        [SerializeField] private int _initialPoolSize = 20;
        [SerializeField] private bool _autoExpand = true;
        [SerializeField] private int _expandAmount = 5;

        public int InitialPoolSize => _initialPoolSize;
        public bool AutoExpand => _autoExpand;
        public int ExpandAmount => _expandAmount;
    }
}
