using System.Collections.Generic;
using UnityEngine;

namespace AFSInterview.Combat
{
    [CreateAssetMenu(fileName = nameof(CombatData), menuName = "Combat/" + nameof(CombatData), order = 1)]
    public class CombatData : ScriptableObject
    {
        [field: SerializeField] public List<UnitData> Player1Army { get; private set; }
        [field: SerializeField] public List<UnitData> Player2Army { get; private set; }
    }
}
