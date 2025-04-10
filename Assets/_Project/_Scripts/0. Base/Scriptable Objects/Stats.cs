using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.General
{
    public class Stats : ScriptableObject
    {
        [Title("Base Info")]
        public string Name = "Name";

        [Title("Visuals Prefab")]
        public GameObject EntityPrefab;

        [Title("Base Stats")]
        public int MaxHealth = 100;
        public float MaxSpeed = 20f;

        [Title("Basic Attack")]
        public int AttackDamage = 10;
        public float AttackSpeed = 1f;

        [Title("Movement")]
        public float Acceleration = 10f;
    }
}
