using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AFSInterview.Combat
{
    [CreateAssetMenu(fileName = nameof(UnitData), menuName = "Combat/" + nameof(UnitData), order = 1)]
    public class UnitData : ScriptableObject
    {
        [field: SerializeField] public string UnitName { get; private set; }
        [field: SerializeField] public UnitType UnitType { get; private set; }
        [field: SerializeField] public List<AttributeType> AttributeTypes { get; private set; } = new List<AttributeType>();
        [field: SerializeField] public UnitStatistics UnitStatistics { get; private set; }
        [field: SerializeField] public Unit UnitPrefab { get; private set; }

        public bool HasAttribute(AttributeType attributeType)
        {
            return AttributeTypes.Contains(attributeType);
        }
    }
}
