using UnityEngine;

namespace GoodVillageGames.Game.General.UI.Animations
{
    public class ActivateGameObjectOnAnimationEvent : MonoBehaviour
    {
        [SerializeField] private GameObject objectToToggle;

        public void ActivateObject() => objectToToggle.SetActive(true);
        public void DeactivateObject() => objectToToggle.SetActive(false);
    }
}
