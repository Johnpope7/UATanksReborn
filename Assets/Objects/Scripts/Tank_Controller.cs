//credits to Indie Pixel for the tutorial on this script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletSpace;


namespace TankSpace
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Tank_Input))]
    public class Tank_Controller : MonoBehaviour
    {
        #region Variables
        [Header("Movement Properties")]
        public float tankSpeed = 15f; //tanks movement speed
        public float tankRotationSpeed = 100f; //tanks turning speed

        [Header("Turret Properties")]
        public Transform turretTransform; //transform for the tank turret
        public float turretLagSpeed = 7f; //speed at which the turret lags behind the mouse position

        [Header("Reticle Properties")]
        public Transform reticleTransform; //location of the reticle

        private Rigidbody trb; //stores the rigid bod required, presumably the tank
        private Tank_Input input; //stores the input for the tanks
        private Vector3 finalTurretFacing; //the final direction the turret is facing

        [Header("Tank Shell")]
        public GameObject shell; //game object for the shell instantiation 
        private Rigidbody srb; //stores the shell rigid body
        [SerializeField]
        private Transform firingZone; //the spot from which the bullet comes from
        private static float shellTimeout = 1; //decides how long the bullet has till its destroyed
        [SerializeField]
        private float shotTimerDelay;
        [SerializeField]
        private float cooldownTimer;

        [Header("Tank Stats")]
        [SerializeField]
        private float shotForce = 2000f;
        [SerializeField]
        private float tankDamage = 25f;
        private float playerHealth;

        #endregion

        #region Builtin Methods
        // Start is called before the first frame update
        void Start()
        {
            trb = GetComponent<Rigidbody>(); //gets the rigid body from the object its attached to.
            input = GetComponent<Tank_Input>();//gets the Tank_Inputs script on the object its attached to.
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (trb && input)
            {
                HandleMovement();
                HandleTurret();
                HandleReticle();
                cooldownTimer -= Time.deltaTime;

            }
        }
        #endregion

        #region Custom Methods
        protected virtual void HandleMovement()
        {
            //moves the tank forward and backwards
            Vector3 desiredPosition = transform.position + (transform.forward * input.ForwardInput * tankSpeed * Time.deltaTime);
            trb.MovePosition(desiredPosition);

            //Rotates the tank
            Quaternion desiredRotation = transform.rotation * Quaternion.Euler(Vector3.up * (tankRotationSpeed * input.RotationInput * Time.deltaTime));
            trb.MoveRotation(desiredRotation);
        }

        protected virtual void HandleTurret() //handles the rotation of the turret head for the tank
        {
            if (turretTransform)
            {
                Vector3 turretFace = input.p_ReticlePosition - turretTransform.position;
                turretFace.y = 0f;
                finalTurretFacing = Vector3.Lerp(finalTurretFacing, turretFace, Time.deltaTime * turretLagSpeed);
                turretTransform.rotation = Quaternion.LookRotation(finalTurretFacing);

            }
        }

        protected virtual void HandleReticle()  //makes the reticle follow the mouse
        {
            reticleTransform.position = input.p_ReticlePosition;
        }


        public void Shoot(float _shotforce)
        {
            if (cooldownTimer <= 0)
            {
                //create the vector 3 variable that is equal to our firing zones forward vector multiplied by shot force
                Vector3 shotDir = firingZone.forward * shotForce;
                //spawn the bullet
                GameObject shellInstance = Instantiate(shell, firingZone.position, firingZone.rotation);
                //change bullet tag
                shellInstance.gameObject.layer = gameObject.layer;
                //get the instigator
                shellInstance.GetComponent<Shell_Bullet>().instigator = gameObject;
                //get the shellDamage variable
                shellInstance.GetComponent<Shell_Bullet>().SetShellDamage(tankDamage);
                //get the shell rigid body to apply force
                srb = shellInstance.GetComponent<Rigidbody>();
                //apply the shotforce variable to the rigid body to make the bullet move
                srb.AddForce(shotDir);
                //destroy the bullet after a desired time
                Destroy(shellInstance, shellTimeout);
                cooldownTimer = shotTimerDelay;
            }
        }

        public void TakeDamage(float damage, GameObject instigator)
        {
            Debug.Log("Took Damage");
        }

        #region GettersAndSetters
        public float GetTankDamage //the getter for the tank damage variable
        {
            get { return tankDamage; }
        }
        public float SetTankDamage(float dmg) //the setter of the tank damage variable
        {
            tankDamage = dmg;
            return tankDamage;
        }

        public float GetShotForce
        {
            get { return shotForce; }
        }

        public float SetShotForce(float force)
        {
            shotForce = force;
            return shotForce;
        }

        #endregion

        #endregion

    }
}
