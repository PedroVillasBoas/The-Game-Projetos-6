using GoodVillageGames.Game.Core.Pooling;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Interfaces;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Pickups
{
    public class ExpPickup : MonoBehaviour, IVisitor
    {
        [SerializeField] private float _moveVelocity = 20f;
        [SerializeField] private int _expAmount = 1;

        private GameObject _target;
        private PooledObject _poolObject;

        void Awake() => _poolObject = GetComponent<PooledObject>();

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
                //_poolObject.ReturnToPool();
                Destroy(gameObject);
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