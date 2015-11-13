using System;
using UnityEngine;

namespace Game
{
    public class Bump : Attack
    {
        private const int BumpAttackAmount = 10;
        private const int LevelBonus = 2;

        public Bump(Character character)
        {
            strengthMean += BumpAttackAmount + (character.Level * LevelBonus);
        }


        public override int PerformAttack()
        {
            return calculateAttackPower();
        
        }
    }
}
