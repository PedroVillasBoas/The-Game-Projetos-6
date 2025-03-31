using UnityEngine;

namespace GoodVillageGames.Game.Core.Util
{
    public static class Utillities
    {

        #region Vectors 2

        public static bool Vector2IsGreaterThan(Vector2 firstVector, Vector2 secondVector)
        {
            return firstVector.x > secondVector.x && firstVector.y > secondVector.y;
        }

        public static bool Vector2IsLessThan(Vector2 firstVector, Vector2 secondVector)
        {
            return firstVector.x < secondVector.x && firstVector.y < secondVector.y;
        }

        public static bool Vector2HorizontalIsGreaterThan(Vector2 firstVector, Vector2 secondVector)
        {
            return firstVector.x > secondVector.x;
        }

        public static bool Vector2VerticalIsGreaterThan(Vector2 firstVector, Vector2 secondVector)
        {
            return firstVector.y > secondVector.y;
        }

        public static bool Vector2IsEqualTo(Vector2 firstVector, Vector2 secondVector)
        {
            return firstVector.x == secondVector.x && firstVector.y == secondVector.y;
        }

        public static bool Vector2DifferentThanZero(Vector2 vector)
        {
            return vector != Vector2.zero;
        }

        #endregion
    }
}
