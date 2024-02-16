using DG.Tweening;
using System;
using UnityEngine;

namespace AFSInterview.Combat
{
    public class Unit : MonoBehaviour
    {
        public event Action<Unit> OnUnitHoverStarted;
        public event Action<Unit> OnUnitHoverEnded;
        public event Action<Unit> OnUnitClicked;

        public bool HadTurn { get; private set; }
        public AffilationType AffilationType { get; private set; }
        public int CurrentHealthPoints { get; private set; }
        public int CurrentCooldown { get; private set; }
        public UnitData UnitData { get; private set; }

        public bool CanUseSkill => CurrentCooldown == 0;
        public bool IsAlive => CurrentHealthPoints > 0;

        private void OnMouseEnter()
        {
            OnUnitHoverStarted?.Invoke(this);
        }

        private void OnMouseExit()
        {
            OnUnitHoverEnded?.Invoke(this);
        }

        private void OnMouseDown()
        {
            OnUnitClicked?.Invoke(this);
        }

        public void Initialize(UnitData unitData, AffilationType affilation)
        {
            UnitData = unitData;
            HadTurn = false;
            AffilationType = affilation;

            CurrentHealthPoints = UnitData.UnitStatistics.HealthPoints;
            CurrentCooldown = 0;
        }

        public void StartTurn()
        {
            CurrentCooldown = Math.Max(CurrentCooldown - 1, 0);
        }

        public void SkipTurn()
        {
            HadTurn = true;
        }

        public void RefreshTurn()
        {
            HadTurn = false;
        }

        public void AttackUnit(Unit target, Action onDealDamage, Action onAnimationCompleted)
        {
            HadTurn = true;
            CurrentCooldown = UnitData.UnitStatistics.AttackInterval;

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = target.transform.position;

            float distance = Vector3.Distance(startPosition, targetPosition);
            float t = (distance - 2) / distance;

            Vector3 destination = Vector3.LerpUnclamped(startPosition, targetPosition, t);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOJump(destination, 2f, 1, 0.5f));
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(OnDealDamage);
            sequence.AppendInterval(0.5f);
            sequence.Append(transform.DOJump(startPosition, 2f, 1, 0.5f));
            sequence.OnComplete(OnAnimationCompleted);

            void OnDealDamage()
            {
                onDealDamage?.Invoke();
            }

            void OnAnimationCompleted()
            {
                onAnimationCompleted?.Invoke();
            }
        }

        public void DealDamage(int damage)
        {
            CurrentHealthPoints -= damage;
            CurrentHealthPoints = Math.Max(CurrentHealthPoints, 0);
        }

        public void KillUnit()
        {
            transform.DOScale(0, 1f).OnComplete(OnCompleted);

            void OnCompleted()
            {
                Destroy(gameObject);
            }
        }

        public string GetDescription(int damage)
        {
            string description = string.Empty;

            description += UnitData.UnitName + "\n";
            description += $"HP: {CurrentHealthPoints}";
            if (damage > 0)
            {
                description += $"<color=red>-{damage}</color>";
            }

            description += $"/{UnitData.UnitStatistics.HealthPoints}\n";
            description += $"Damage: {UnitData.UnitStatistics.AttackDamage}\n";
            description += $"Armor: {UnitData.UnitStatistics.ArmorPoints}\n";
            description += $"Cooldown: {UnitData.UnitStatistics.AttackInterval}({CurrentCooldown})\n";
            if (UnitData.AttributeTypes.Count > 0)
            {
                description += "Attributes: ";
                for (int i = 0; i < UnitData.AttributeTypes.Count; i++)
                {
                    description += $"{UnitData.AttributeTypes[i]}";

                    if (i < UnitData.AttributeTypes.Count - 1)
                    {
                        description += ", ";
                    }
                }
                description += "\n";
            }

            if (UnitData.UnitStatistics.BonusDamages.Count > 0)
            {
                description += "Bonus damage:\n";
                for (int i = 0; i < UnitData.UnitStatistics.BonusDamages.Count; i++)
                {
                    description += $"{UnitData.UnitStatistics.BonusDamages[i].AttributeType}: {UnitData.UnitStatistics.BonusDamages[i].AttackDamage:+#;-#;0}\n";
                }
            }

            return description;
        }
    }
}
