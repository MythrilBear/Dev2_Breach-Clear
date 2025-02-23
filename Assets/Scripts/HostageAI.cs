using UnityEngine;
using UnityEngine.AI;

public class HostageAI : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject hostageUI;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform escapePoint;
    bool isFreed = false;
    bool playerNearby = false;
    bool isRegistered = false;


    void Start()
    {
        //Debug.Log("Hostage Instance ID: " + gameObject.GetInstanceID());
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (!isRegistered)
        {
            GameManager.instance.RegisterHostage();
            isRegistered = true;
        }

        if (GameManager.instance.remainingHostages == 1)
        {
            GameManager.instance.AddMissionObjective("Rescue all hostages");
        }
 
    }
    void Update()
    {
        if (playerNearby && Input.GetButtonDown("FreeHostage") && !isFreed)
        {
            isFreed = true;
            hostageUI.SetActive(false);
            animator.SetTrigger("SitToStand");
            Invoke("StartRunning", 2f);
        }
        if (isFreed)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("SpeedMultiplier", speed / agent.speed);
        }
    }
    void StartRunning()
    {
        animator.ResetTrigger("SitToStand");

        animator.SetTrigger("StandToRun");

        if (agent.isOnNavMesh)
        {
            
            agent.SetDestination(escapePoint.position);
            InvokeRepeating("CheckArrival", 0.5f, 0.5f);
        }
        else
        {
            //Debug.LogError(gameObject.name + " is NOT on the NavMesh! Fix placement.");
        }



    }
    void CheckArrival()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StopRunning();
        }
    }
   
    void StopRunning()
    {
        CancelInvoke("CheckArrival");
        Disappear();
        GameManager.instance.RescueHostage();

    }
    void Disappear()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            hostageUI.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            hostageUI.SetActive(false);
        }
    }
}

