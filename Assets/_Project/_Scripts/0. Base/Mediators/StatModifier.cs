using System;
using GoodVillageGames.Game.Core.Util.Timer;

namespace GoodVillageGames.Game.Core.Attributes
{
    public abstract class StatModifier : IDisposable
    {
        public bool MarkedForRemoval { get; private set; }

        public event Action<StatModifier> OnDispose = delegate { };

        readonly CountdownTimer timer;

        protected StatModifier(float duration)
        {
            if (duration <= 0) return;

            timer = new CountdownTimer(duration);
            timer.OnTimerStop += () => MarkedForRemoval = true;
            timer.Start();
        }

        public void Update(float deltaTime) => timer?.Tick(deltaTime);

        public abstract void Handle(object sender, Query query);

        public void Dispose()
        {
            OnDispose.Invoke(this);
        }
    }
}