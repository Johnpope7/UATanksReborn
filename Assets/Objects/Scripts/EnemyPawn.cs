//credit to Dave GameDevelopment for the tutorial on Enemy AI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


    public class EnemyPawn : MonoBehaviour
    {
        #region Variables
        [Header("Basic Nav Properties")]
        private NavMeshAgent agent;
        private Transform player;
        public LayerMask isGround, isPlayer;

        [Header("Patrolling Properties")]
        private Vector3 walkPoint;
        private bool walkPointSet;
        private float walkPointRange;

        [Header("Attacking Properties")]
        private float timeBetweenAttacks;
        private bool alreadyAttacked;

        [Header("State Properties")]
        private float sightRange, attackRange;
        private bool playerInSightRange, playerInAttackRange;

        [Header("Enemy Tank Shell")]
        public GameObject shell; //game object for the shell instantiation 
        private Rigidbody srb; //stores the shell rigid body
        [SerializeField]
        private Transform firingZone; //the spot from which the bullet comes from
        private static float shellTimeout = 1; //decides how long the bullet has till its destroyed
        [SerializeField]
        private float shotTimerDelay;
        [SerializeField]
        private float cooldownTimer;

        [Header("Enemy Tank Stats")]
        [SerializeField]
        private float shotForce = 2000f;
        [SerializeField]
        private float tankDamage = 25f;
        private float enemyHealth;
        #endregion

        #region BuiltIn Methods
        // Start is called before the first frame update
        private void Awake()
        {
            player = GameObject.Find("Tank").transform;
            agent = GetComponent<NavMeshAgent>();
        }
        void Start()
        {
            timeBetweenAttacks -= Time.deltaTime;
        }

        // Update is called once per frame
        void Update()
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patrolling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
        #endregion

        #region Custom Methods
        protected virtual void Patrolling()
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }
        protected virtual void ChasePlayer()
        {
            agent.SetDestination(player.position);
        }
        protected virtual void AttackPlayer()
        {
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!alreadyAttacked)
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

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }

        protected virtual void ResetAttack()
        {
            alreadyAttacked = false;
        }

        protected virtual void SearchWalkPoint()
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
                walkPointSet = true;
        }

        public void TakeDamage(float damage, GameObject instigator)
        {
            Debug.Log("Took Damage");
        }
        #endregion
    }


