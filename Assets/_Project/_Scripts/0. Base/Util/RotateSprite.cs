using DG.Tweening;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Util
{
    public class RotateSprite : MonoBehaviour 
    {
        [Tooltip("Rotation speed in degrees per second. Positive = clockwise, Negative = counter-clockwise")]
        [SerializeField] private float _rotationSpeed = 90f;

        private void Update()
        {
            transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
        }
    }
}
