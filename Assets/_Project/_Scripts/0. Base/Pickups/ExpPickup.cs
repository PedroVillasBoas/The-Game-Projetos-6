using UnityEngine;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Pooling;
using GoodVillageGames.Game.Core.Util;

namespace GoodVillageGames.Game.Core.Pickups
{
    public class ExpPickup : MonoBehaviour, IVisitor
    {
        [SerializeField] private float _moveVelocity = 20f;
        [SerializeField] private int _expAmount = 1;

        private GameObject _target;
        private PooledObject _poolObject;
        private IDTriggerOwner owner;

        void Awake()
        {
            _poolObject = GetComponent<PooledObject>();
            owner = GetComponent<IDTriggerOwner>();
        }

        // When it goes back to the pool, it loses the target
        // prevents automatic collection of the exp when a previous EXP is spawnned a second time
        void OnDisable() => _target = null;

        void Update()
        {
            if (_target != null)
                GoToPlayer();
        }

        public void Visit<T>(T visitable) where T : Component, IVisitable
        {
            if (visitable is PlayerExpHandler handler)
            {
                handler.ExpCollected(_expAmount);
                owner.NotifySubscribers("exp-pickup");
                _poolObject.ReturnToPool();
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            collision.GetComponent<IVisitable>()?.Accept(this);
        }

        public void SetPlayer(GameObject target)
        {
            _target = target;
        }

        void GoToPlayer()
        {   
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = _target.transform.position;
            
            float step = _moveVelocity * Time.deltaTime;
            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);
        }
    }
}