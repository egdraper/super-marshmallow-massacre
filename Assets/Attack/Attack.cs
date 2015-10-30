using System;

namespace Game
{
    public abstract class Attack
    {
        protected int strengthMean;

        protected int calculateAttackPower()
        {
            throw new NotImplementedException();
        }

        public abstract int PerformAttack();

        
        
    }
}
    