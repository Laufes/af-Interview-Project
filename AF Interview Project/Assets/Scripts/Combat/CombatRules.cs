namespace AFSInterview.Combat
{
    public static class CombatRules
    {
        public static int GetResultDamage(UnitData attacker, UnitData target)
        {
            int damage = attacker.UnitStatistics.AttackDamage;
            for (int i = 0; i < attacker.UnitStatistics.BonusDamages.Count; i++)
            {
                BonusDamage bonusDamage = attacker.UnitStatistics.BonusDamages[i];
                if (target.HasAttribute(bonusDamage.AttributeType))
                {
                    damage += bonusDamage.AttackDamage;
                }
            }

            damage -= target.UnitStatistics.ArmorPoints;
            damage = System.Math.Max(damage, 1);

            return damage;
        }
    }
}
