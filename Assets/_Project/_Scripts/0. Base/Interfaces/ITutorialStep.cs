using System;

namespace GoodVillageGames.Game.Interfaces
{
    public interface ITutorialStep
    {
        /// <summary>
        /// Called by the manager to show this step (e.g. instantiate, animate in).
        /// </summary>
        void Enter();

        /// <summary>
        /// Fired by the step when its Continue button is clicked.
        /// </summary>
        event Action OnContinue;

        /// <summary>
        /// Called by the manager to hide and clean up this step.
        /// </summary>
        void Exit();
    }
}
