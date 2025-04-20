using TMPro;
using UnityEngine;
using GoodVillageGames.Game.Core.Pooling;
using GoodVillageGames.Game.Core.Util.Timer;

namespace GoodVillageGames.Game.Core.Util
{
    public class DamageNumbers : MonoBehaviour 
    { 
        [SerializeField] private Rigidbody2D textRb;
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private PooledObject pooledObject;
        [SerializeField] private Canvas canvas;
        [SerializeField] private float initialYVelocity = 7f;
        [SerializeField] private float initialXVelocity = 3f;
        [SerializeField] private float lifeTime = 7f;

        private CountdownTimer countdownTimer;

        void Awake()
        {
            canvas.worldCamera = Camera.main;
        }

        void OnEnable()
        {
            if (countdownTimer == null)
            {
                countdownTimer = new(lifeTime) {
                    OnTimerStop = () => OntimerFinished()
                };
                countdownTimer.Start();
            }
            else
            {
                countdownTimer.Reset(lifeTime);
                countdownTimer.Start();
            }

            transform.localScale = new(0.5f, 0.5f, 0.5f);
            textRb.linearVelocity = new(Random.Range(-initialXVelocity, initialXVelocity), initialYVelocity);
        }

        void Update()
        {
            countdownTimer.Tick(Time.deltaTime);
        }

        public void SetInfo(string amount)
        {
            damageText.SetText(amount);
        }

        void OntimerFinished()
        {
            pooledObject.ReturnToPool();
        }
    }
}
