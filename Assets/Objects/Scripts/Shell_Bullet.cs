using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletSpace
{
    public class Shell_Bullet : MonoBehaviour
    {
        #region Variables
        public GameObject instigator; //stores the object that fires this bullet

        private float shellDamage; //the damage value of the bullet



        #endregion

        #region BuiltIn Method
        private void OnTriggerEnter(Collider _other)
        {
            if (_other.CompareTag("Player") || _other.CompareTag("Enemy"))
            {
                /* _other.GetComponent<Tank_Controller>().TakeDamage(damage, instigator);*/
            }
        }

        #endregion

        #region CustomMethods

        #region GettersAndSetters
        public float GetShellDamage //the getter for the damage
        {
            get { return shellDamage; }
        }
        public float SetShellDamage(float dmg) //the setter of the damage
        {
            shellDamage = dmg;
            return shellDamage;
        }

        #endregion

        #endregion
    }
}
