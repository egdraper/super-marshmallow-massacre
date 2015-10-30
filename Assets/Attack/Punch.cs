using System;
using UnityEngine;

namespace Game
{
    public class Punch : Attack
    {
        private const int PunchAttackAmount = 25;
        private const int LevelBonus = 2;

        public Punch(Character character)
        {
            strengthMean += PunchAttackAmount + (character.Level * LevelBonus);
        }


        public override int PerformAttack()
        {
            return calculateAttackPower();
            throw new NotImplementedException();
        }
    }
}
