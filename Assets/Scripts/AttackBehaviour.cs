using UnityEngine;
using System.Collections;

namespace Game
{
    public class AttackBehaviour : MonoBehaviour
    {
	    // Use this for initialization
	    void Start ()
        {
	
	    }
	
	    // Update is called once per frame
	    void Update ()
        {
            //we need to get the current character class here
            Character character = new AngryMellow(); //this is just so the code isn't broken

            Attack attack;
            //change the if to whatever you need when ready
            if(true) // (eg. if attack button button was pushed) 
            {
                attack = AttackFactory.GetAttack("SomeButtonDescription", character);
            }

            int damage = attack.PerformAttack(); //Does some sequence of events to perform the attack and provide damage

          // whatever you need to do with damage calculated
	
	    }
    }   

}