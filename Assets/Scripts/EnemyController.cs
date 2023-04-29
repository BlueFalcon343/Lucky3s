using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;

    //public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //animator
    Animator animator;

    //Audio
    public AudioSource freezeHit;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //States
    public float sightRange;
    public bool playerInSightRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //Tag Changing Reference 
    public GameObject Enemy;

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
        Enemy.tag = "Alive";
    }

    private void Update()
    {
        //Checks for player in sight range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange) Patroling();
        if (playerInSightRange) ChasePlayer();
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
        //agent.SetDestination(player.position);
        agent.SetDestination(GameObject.Find("Player").transform.position);
    }

    private void OnCollisionStay(Collision other)
    {
        if (!alreadyAttacked)
        {
            //Attack code here
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            animator.SetBool("isAttacking", true);
            if (player != null)
            {
                player.ChangeHealth(-1);
            }
            //End of Attack code

            // Starts break between attacks
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    // Ends break between attacks
    private void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
        alreadyAttacked = false;
    }

    // Display sight range for testing
    private void OnDrawGizmosSelected()
    {
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
            Instantiate(Frozen, FreezePoint.position, FreezePoint.rotation);
            freezeHit.Play();
            this.enabled = false;
            Enemy.tag = "Dead";
            // gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            //yield return new WaitForSeconds(2f);
            //gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }
}