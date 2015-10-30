using UnityEngine;

namespace Game
{
    public class AttackFactory
    {
        public static Attack GetAttack(string keyPress, Character character)
        {
            if(keyPress == "Button B")
            {
                return new Squash(character);               
            }

            if(keyPress == "Button C")
            {
                return new Punch(character);
            }

            if (keyPress == "Button D")
            {
                return new Bump(character);
            }

            return null;
        }
    }
}
