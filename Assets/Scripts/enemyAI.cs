using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPosition, headPos;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDist;
    [SerializeField] bool roaming;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    //enemies moving between points - MG
    [SerializeField] float patrolSpeed;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] bool patrol;

    Coroutine cor;

    Vector3 soundPos;

    float angleToPlayer;
    float stoppingDistOrig;
    float scanDirection;
    float counter;

    bool isShooting;

    bool playerInRange;

    bool isRotatingLeft;

    bool isRoaming;

    Color colorOriginal;

    Vector3 playerDirection;

    Vector3 startingPos;

    Coroutine co;
    internal object enemyType;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOriginal = model.material.color;
        //GameManager.instance.updateGameGoal(1);
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
        scanDirection = transform.rotation.eulerAngles.y;
        isRotatingLeft = true;
        counter = 2;

        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
        float animSpeed = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.MoveTowards(animSpeed, agentSpeed, Time.deltaTime * animSpeedTrans));

        if (roaming != false)
        {
            if (playerInRange && !canSeePlayer())
            {
                if (!isRoaming && agent.remainingDistance < 0.01f)
                {
                    co = StartCoroutine(roam());
                }
            }
            else if (!playerInRange)
            {
                if (!isRoaming && agent.remainingDistance < 0.01f)
                {
                    co = StartCoroutine(roam());
                }
            }
        }

        if (playerInRange && canSeePlayer())
        {
            scanDirection = transform.rotation.eulerAngles.y;
        }
        else { lookForTargets(); }



        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            isRotatingLeft = !isRotatingLeft;
            counter = 2;
        }

        // Method for the enemy patrolling - MG
        if (patrol != false && !isRoaming)
        {
            cor = StartCoroutine(patrolling());
        }

    }

    IEnumerator roam()
    {
        isRoaming = true;
        yield return new WaitForSeconds(roamPauseTime);
        agent.stoppingDistance = 0;

        Vector3 randomPos = Random.insideUnitSphere * roamDist;
        randomPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);

        isRoaming = false;
    }

    bool canSeePlayer()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        //Debug.DrawRay(headPos.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    void lookForTargets()
    {

        if (isRotatingLeft)
        {
            lookLeft();
        }
        else
        {
            lookRight();
        }
    }

    void lookLeft()
    {
        transform.Rotate(0, -45 * Time.deltaTime, 0);
    }

    void lookRight()
    {
        transform.Rotate(0, 45 * Time.deltaTime, 0);
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDirection.x, 0, playerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        agent.SetDestination(GameManager.instance.player.transform.position);
        if (co != null)
        {
            StopCoroutine(co);
            isRoaming = false;
        }

        StartCoroutine(flashRed());

        //agent.SetDestination(GameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            //GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOriginal;
    }

    IEnumerator shoot()
    {

        isShooting = true;
        Instantiate(bullet, shootPosition.position, Quaternion.LookRotation(playerDirection.normalized));

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator patrolling()
    {
        bool atPoint = true;
        isRoaming = true;
        agent.stoppingDistance = 0;

        while (patrol)
        {
            if (!playerInRange && agent.remainingDistance < 2f)
            {
                yield return new WaitForSeconds(roamPauseTime);

                if (atPoint)
                {
                    agent.SetDestination(endPoint.position);
                }
                else
                {
                    agent.SetDestination(startPoint.position);
                }

                atPoint = !atPoint;
            }

            yield return null;
        }

        isRoaming = false;
    }

    public void investigate(Vector3 soundPos, float radius)
    {
      //  Debug.Log("Enemy investigating sound at: " + soundPos);

        if (co != null) StopCoroutine(co);
        if (cor != null) StopCoroutine(cor);

        agent.isStopped = false;
        isRoaming = true;

        Vector3 randomPos = soundPos + Random.insideUnitSphere * radius;

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas))
        {
           // Debug.LogWarning("No valid NavMesh position found, using sound source.");
            hit.position = soundPos;
        }

        agent.SetDestination(hit.position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(InvestigateArea());
        }
    }


    IEnumerator InvestigateArea()
    {
        //Debug.Log("Investigating area...");

        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

       // Debug.Log("Reached investigation point, pausing...");
        yield return new WaitForSeconds(3f);

        if (!canSeePlayer())
        {
            isRoaming = false;

            if (roaming)
            {
                co = StartCoroutine(roam());
            }
            else if (patrol)
            {
                cor = StartCoroutine(patrolling());
            }
        }
    }
}