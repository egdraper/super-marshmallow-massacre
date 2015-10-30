using UnityEngine;

namespace Game
{
    public class AttackFactory
    {
        public static Attack GetAttack(string keyPress)
        {
            if(keyPress == "Button B")
            {
                return new Squash();               
            }

            if(keyPress == "Button C")
            {
                return new Punch();
            }

            if (keyPress == "Button D")
            {
                return new Bump();
            }

            return null;
        }
    }
}
