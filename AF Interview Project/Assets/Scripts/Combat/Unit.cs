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

        /// <summary>
        /// Returns true if unit is killed
        /// </summary>
        public bool DealDamage(int damage)
        {
            CurrentHealthPoints -= damage;
            CurrentHealthPoints = Math.Max(CurrentHealthPoints, 0);

            if (CurrentHealthPoints < 0)
            {
                transform.DOScale(0, 1f);

                return true;
            }

            return false;

        }
    }
}
