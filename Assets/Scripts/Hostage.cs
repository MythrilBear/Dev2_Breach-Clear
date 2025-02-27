using UnityEngine;
using UnityEngine.AI;

public class Hostage : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject hostageUI;
    [SerializeField] GameObject SittingProp;
    bool isFreed = false;
    bool playerNearby = false;
    bool isRegistered = false;
    Vector3 startPosition;
    float maxRunDistance = 3f;
   
    void Start()
    {
        //Debug.Log("Hostage Instance ID: " + gameObject.GetInstanceID());
        animator = GetComponent<Animator>();
        if (!isRegistered)
        {
            GameManager.instance.RegisterHostage();
            isRegistered = true;
        }

        if (GameManager.instance.remainingHostages == 1)
        {
            GameManager.instance.AddMissionObjective("Locate all and rescue all hostages!");
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
    }
    void StartRunning()
    {
        if (SittingProp != null)
        {
            Destroy(SittingProp);
        }
        
        animator.ResetTrigger("SitToStand");
        animator.SetTrigger("StandToRun");
        startPosition = transform.position;
        InvokeRepeating("MoveForward", 0f, Time.deltaTime);

    }
    
    void MoveForward()
    {
        float speedMultiplier = animator.GetFloat("SpeedMultiplier");
        float moveSpeed = 3f * speedMultiplier;
        transform.position += transform.forward * moveSpeed * Time.deltaTime; 
        if (Vector3.Distance(startPosition, transform.position) >= maxRunDistance)
        {
            StopRunning();
        }
    }
    void StopRunning()
    {
        CancelInvoke("MoveForward");
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

