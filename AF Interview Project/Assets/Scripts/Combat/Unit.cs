using UnityEngine;

namespace AFSInterview.Combat
{
    public class Unit : MonoBehaviour
    {
        private UnitData UnitData { get; set; }

        public void Initialize(UnitData unitData, AffilationType affilation)
        {
            UnitData = unitData;
        }
    }
}
