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

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    float angleToPlayer;
    float scanDirection;
    float counter;

    bool isShooting;

    bool playerInRange;

    bool isRotatingLeft;

    Color colorOriginal;

    Vector3 playerDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOriginal = model.material.color;
        GameManager.instance.updateGameGoal(1);
        scanDirection = transform.rotation.eulerAngles.y;
        isRotatingLeft = true;
        counter = 2;
    }

    // Update is called once per frame
    void Update()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
        float animSpeed = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.MoveTowards(animSpeed, agentSpeed, Time.deltaTime * animSpeedTrans));

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
    }

    bool canSeePlayer()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        Debug.DrawRay(headPos.position, playerDirection);

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

        StartCoroutine(flashRed());

        agent.SetDestination(GameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
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
}
