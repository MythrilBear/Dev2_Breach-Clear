using UnityEngine;

public class Hostage : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject hostageUI; 
    
    bool isFreed = false;
    bool playerNearby = false;
    Vector3 startPosition;
    float maxRunDistance = 3f;
   
    void Start()
    {
        animator = GetComponent<Animator>();
        hostageUI.SetActive(false);
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

