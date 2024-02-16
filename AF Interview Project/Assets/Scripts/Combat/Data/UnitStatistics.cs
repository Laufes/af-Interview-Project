using System;
using System.Collections.Generic;
using UnityEngine;

namespace AFSInterview.Combat
{
    [Serializable]
    public class UnitStatistics
    {
        [field: SerializeField] public int HealthPoints { get; set; }
        [field: SerializeField] public int ArmorPoints { get; set; }
        [field: SerializeField] public int AttackInterval { get; set; }
        [field: SerializeField] public int AttackDamage { get; set; }
        [field: SerializeField] public List<BonusDamage> BonusDamages { get; set; } = new List<BonusDamage>();
    }
}
