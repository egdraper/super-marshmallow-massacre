using System;
using UnityEngine;

namespace Game
{
    public class Squash: Attack
    {
        private const int SquashAttackAmount = 50;
        private const int LevelBonus = 2;

        public Squash(Character character)
        {
            strengthMean += SquashAttackAmount + (character.Level * LevelBonus);
        }


        public override int PerformAttack()
        {
            return calculateAttackPower();
            throw new NotImplementedException();
        }


    }
}
