using System;
using UnityEngine;

namespace AFSInterview.Combat
{
    [Serializable]
    public class BonusDamage
    {
        [field: SerializeField] public AttributeType AttributeType { get; set; }
        [field: SerializeField] public int AttackDamage { get; set; }
    }
}
