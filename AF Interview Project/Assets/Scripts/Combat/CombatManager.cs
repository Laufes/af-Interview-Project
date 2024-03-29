using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AFSInterview.Combat
{
    public class CombatManager : MonoBehaviour
    {
        [field: SerializeField] private CombatData CombatData { get; set; }
        [field: SerializeField] private Transform Player1ArmyTransform { get; set; }
        [field: SerializeField] private Transform Player2ArmyTransform { get; set; }
        [field: Header("UI")]
        [field: SerializeField] private Indicator Indicator { get; set; }
        [field: SerializeField] public Tooltip Tooltip { get; set; }

        private List<Unit> AllUnits { get; set; } = new List<Unit>();

        private Unit CurrentUnit { get; set; }
        private bool IsCombat { get; set; }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            AllUnits.Clear();
            SpawnArmy(CombatData.Player1Army, Player1ArmyTransform, AffilationType.Player1);
            SpawnArmy(CombatData.Player2Army, Player2ArmyTransform, AffilationType.Player2);

            AllUnits = AllUnits.OrderBy(x => Random.value).ToList();
            IsCombat = true;

            if (AllUnits.Count == 0)
            {
                Debug.LogWarning($"No units in armies");
                return;
            }

            Indicator.Initialize();
            Indicator.Hide();

            CheckConditions();
            StartNextTurn();
        }

        private void SpawnArmy(List<UnitData> armyList, Transform parent, AffilationType affilation)
        {
            for (int i = 0; i < armyList.Count; i++)
            {
                UnitData unitData = armyList[i];

                Vector3 position = parent.position + Vector3.forward * 2.5f * i;

                Unit unit = Instantiate(unitData.UnitPrefab, position, new Quaternion(), parent);
                unit.Initialize(unitData, affilation);
                unit.OnUnitClicked += Unit_OnUnitClicked;

                unit.OnUnitHoverStarted += Unit_OnUnitHoverStarted;
                unit.OnUnitHoverEnded += Unit_OnUnitHoverEnded;

                AllUnits.Add(unit);
            }
        }

        private void StartNextTurn()
        {
            if (!IsCombat)
            {
                return;
            }

            CurrentUnit = AllUnits.FirstOrDefault(x => x.HadTurn == false);

            if (!CurrentUnit)
            {
                AllUnits.ForEach(x => x.RefreshTurn());
                StartNextTurn();
                return;
            }

            CurrentUnit.StartTurn();

            if (!CurrentUnit.CanUseSkill)
            {
                CurrentUnit.SkipTurn();
                StartNextTurn();
                return;
            }

            Indicator.Show(CurrentUnit.transform.position);
        }

        private void CheckConditions()
        {
            List<AffilationType> currentPlayers = new List<AffilationType>();

            for (int i = 0; i < AllUnits.Count; i++)
            {
                Unit unit = AllUnits[i];
                if (!currentPlayers.Contains(unit.AffilationType))
                {
                    currentPlayers.Add(unit.AffilationType);
                }
            }

            if (currentPlayers.Count <= 1)
            {
                IsCombat = false;
                Debug.Log($"Battle Ended");
            }
        }

        private void Unit_OnUnitHoverEnded(Unit unit)
        {
            Tooltip.HideText();
        }

        private void Unit_OnUnitHoverStarted(Unit unit)
        {
            int damage = 0;

            if (CurrentUnit.AffilationType != unit.AffilationType)
            {
                damage = CombatRules.GetResultDamage(CurrentUnit.UnitData, unit.UnitData);
            }

            Tooltip.ShowTooltip(unit.transform.position, unit.GetDescription(damage));
        }

        private void Unit_OnUnitClicked(Unit target)
        {
            if (CurrentUnit == null || CurrentUnit.HadTurn)
            {
                return;
            }

            if (CurrentUnit.AffilationType == target.AffilationType)
            {
                return;
            }

            Indicator.Hide();
            Tooltip.HideText();

            int damage = CombatRules.GetResultDamage(CurrentUnit.UnitData, target.UnitData);
            CurrentUnit.AttackUnit(target, DealDamage, AnimationCompleted);

            void DealDamage()
            {
                target.DealDamage(damage);

                if (!target.IsAlive)
                {
                    AllUnits.Remove(target);
                    target.KillUnit();
                    CheckConditions();
                }
            }

            void AnimationCompleted()
            {
                StartNextTurn();
            }
        }
    }
}
