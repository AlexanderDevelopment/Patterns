using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyScript : MonoBehaviour
{
    #region variables
    public StateMachine movementSM;
    public AttackPlayer attackPlayer;
    public Patrol patrol;
    public Death death;
    public Transform[] navMeshPoints;
    public GameObject projectile;
    public Transform projectileStartPos;
    public Transform target;
    public GameObject muzzleFlash;
    public float fireRate;
    public float health = 100f;
    public float accuracy;
    public float attackDistance = 10f;
    public AudioClip[] audioClip;
    private AimConstraint[] Aims;
    internal NavMeshAgent navMeshAgent;
    public GameObject[] squad;
    private Animator anim;
    private AudioSource audioData;
    internal bool dead;
    private bool aimsIsFull = false;
    private GameManager GM;
    internal Coroutine attackCorutine;
    internal Coroutine deathCorutine;
    #endregion

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navMeshAgent.SetDestination(navMeshPoints[Random.Range(0, navMeshPoints.Length)].transform.position);
        audioData = GetComponent<AudioSource>();
        squad = GameObject.FindGameObjectsWithTag("Player");
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementSM = new StateMachine();
        attackPlayer = new AttackPlayer(this, movementSM);
        patrol = new Patrol(this, movementSM);
        death = new Death(this, movementSM);
        movementSM.Initialize(patrol);
    }


    void Update()
    {
        movementSM.CurrentState.LogicUpdate();
    }
    #region Physics
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("grenade"))
        {
            if (!dead)
            {
                health -= 100;
                dead = true;
                GM.UnitsListDelete(gameObject);
                GM.GameProgressControl();
            }

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("projectile"))
        {
            if (!dead)
            {
                Destroy(collision.collider.gameObject);
                health -= 100;
                dead = true;
                GM.UnitsListDelete(gameObject);
                GM.GameProgressControl();
            }

        }
    }
    #endregion
    #region Methods
    public void SetNavMeshPoint()
    {
            anim.speed = 1;
            int i = Random.Range(0, navMeshPoints.Length);
            navMeshAgent.SetDestination(navMeshPoints[i].transform.position);
    }
    internal bool WayPointComplete()
    {
        if (navMeshAgent.remainingDistance < 1&&!CanAttackPlayer())
        {
            return true;
        }
        else return false;
    }
    private AudioClip RandomSound()
    {
        int i = Random.Range(0, audioClip.Length);
        AudioClip AC = audioClip[i];
        return AC;
    }
    public bool CanAttackPlayer()
    {
        var liveSquadUnit = new Vector3();
        foreach (GameObject i in squad)
        {
            if (i != null)
            {
                liveSquadUnit = i.transform.position;
            }
        }
        float dist = Vector3.Distance(transform.position, liveSquadUnit);
        if (dist <= attackDistance)
        {
            Debug.Log(movementSM.CurrentState);
            return true;
        }
        return false;

    }
    internal void AimToPlayer(bool value)
    {
        if (value)
        {

            Aims = GetComponentsInChildren<AimConstraint>();
            while (!aimsIsFull)
            {
                for (int k = 0; k < Aims.Length; k++)
                {
                    for (int j = 0; j < squad.Length; j++)
                    {
                        Aims[k].AddSource(new ConstraintSource() { sourceTransform = squad[j].transform, weight = 1 });
                    }
                }
                aimsIsFull = true;

            }
            EnableAimToPlayer(true);
        }
        else
            EnableAimToPlayer(false);
    }
    private void EnableAimToPlayer(bool value)
    {
        if (value)
        {
            foreach (AimConstraint i in Aims)
            {
                if (i != null)
                {
                    i.constraintActive = true;
                }
            }
        }
        else
        {
            foreach (AimConstraint i in Aims)
            {
                if (i != null)
                {
                    i.constraintActive = false;
                }
            }
        }
    }
    internal void SpawnMuzzleFlash()
    {
        var newMuzzleFlash = Instantiate(muzzleFlash, target.transform.position, Quaternion.identity);
        Destroy(newMuzzleFlash, 0.2f);
    }
    internal Vector3 Target()
    {
        Vector3 target = new Vector3();
        foreach (GameObject i in squad)
        {
            if (i != null)
            {
                target = i.transform.position;
            }
        }
        return target;
    }
    #endregion
    #region Corutines
    IEnumerator ShotSound()
    {
        yield return new WaitForSeconds(3f);
                audioData.clip = RandomSound(); 
                audioData.Play(); 
    }
    
    internal IEnumerator Attack()
    {
        navMeshAgent.destination = Target();
        navMeshAgent.stoppingDistance = attackDistance;
        AimToPlayer(true);
        if (CanAttackPlayer())
        {
            anim.speed = 0;
        }
        else anim.speed = 1;
        yield return new WaitForSeconds(fireRate);
        if (!dead)
        {
            StartCoroutine(ShotSound());
            GameObject newProjectile = Instantiate(projectile, projectileStartPos.position, projectileStartPos.rotation);
            newProjectile.GetComponent<Projectile>().newVector = new Vector3(Random.Range(-accuracy / 30f, accuracy / 30f), 0, 1);
            attackCorutine = null;
            SpawnMuzzleFlash();
        }
    }
    
    internal IEnumerator DeathEnemy()
    {
        anim.enabled = false;
        anim.speed = 0;
        navMeshAgent.enabled = false;
        AimToPlayer(false);
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        deathCorutine = null;
    }
    #endregion
}
