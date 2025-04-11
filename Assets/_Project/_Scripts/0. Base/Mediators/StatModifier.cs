using System;
using GoodVillageGames.Game.Core.Util.Timer;

namespace GoodVillageGames.Game.Core.Attributes
{
    /// <summary>
    /// Base class for all Modifiers
    /// </summary>
    /// <remarks>
    /// <see cref="Timer"/> To see the base class for the <param name="CountdownTimer">
    /// </remarks>
    public abstract class StatModifier : IDisposable
    {
        public bool MarkedForRemoval { get; private set; }

        public event Action<StatModifier> OnDispose = delegate { };

        readonly CountdownTimer timer;

        protected StatModifier(float duration)
        {
            // Is a permanent modifier
            if (duration <= 0) return;

            timer = new CountdownTimer(duration);
            timer.OnTimerStop += () => MarkedForRemoval = true;
            timer.Start();
        }

        public void Update(float deltaTime) => timer?.Tick(deltaTime);

        public abstract void Handle(object sender, Query query);

        // Modifier/Upgrade is done and can be Removed com the List
        public void Dispose()
        {
            OnDispose.Invoke(this);
        }
    }
}