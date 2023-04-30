using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyController : MonoBehaviour
{
    public NavMeshAgent agent;

    //public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //animator
    Animator animator;

    //Audio
    public AudioSource freezeImpact;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Attacking
    //public Transform target;
    private Transform target;
    public Transform shotPoint;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //Tag Changing Reference 
    public GameObject EnemyRanged;

    //Particles
    public Transform ShakesPoint1, ShakesPoint2;
    public Transform FreezePoint;
    public ParticleSystem Frozen;
    public ParticleSystem Shakes;

    private void Awake()
    {
        //player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        EnemyRanged.tag = "Alive";
    }

    private void Update()
    {
        target = GameObject.FindWithTag("Player").transform;

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        animator.SetBool("isWalking", true);
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        animator.SetBool("isWalking", true);
        //agent.SetDestination(player.position);
        agent.SetDestination(GameObject.Find("Player").transform.position);
    }

    private void AttackPlayer()
    {

        //Stop Enemy Movement
        agent.SetDestination(transform.position);
        animator.SetBool("isWalking", false);



        if (!alreadyAttacked)
        {
            //Attack code here
            transform.LookAt(target);
            Rigidbody rb = Instantiate(projectile, shotPoint.position, shotPoint.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);
            //End of Attack code

            // Starts break between attacks
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    // Ends break between attacks
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // Display sight range for testing
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    //Stun and Freeze Enemys
    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fire"))
        {
            Instantiate(Shakes, ShakesPoint1.position, ShakesPoint1.rotation);
            Instantiate(Shakes, ShakesPoint2.position, ShakesPoint2.rotation);
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            yield return new WaitForSeconds(3.5f);
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;

        }

        if (other.gameObject.CompareTag("AltFire"))
        {
            animator.speed = 0;
            freezeImpact.Play();
            this.enabled = false;
            //Instantiate(Frozen, FreezePoint.position, FreezePoint.rotation);
            EnemyRanged.tag = "Dead";
            Frozen.Play();
            // gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            //yield return new WaitForSeconds(2f);
            //gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }
}
