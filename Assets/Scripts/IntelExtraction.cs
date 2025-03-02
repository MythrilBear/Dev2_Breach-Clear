using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class IntelExtraction : MonoBehaviour
{

    private float extractionTime = 3f; //required time to extract the intel
    private float currentExtractionTime = 0;
    private bool isExtracting = false;
    bool playerInRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         GameManager.instance.AddMissionObjective("Locate the enemy desktop and extract the intel!");
    }

    public void StartExtraction()
    {
        isExtracting = true;
        currentExtractionTime = 0;
    }

    // Update is called once per frame
    void Update()//similar to BombDefusing
    {
        
        
        if (playerInRange)
        {
            GameManager.instance.buttonInteract.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                //StartExtraction();
                currentExtractionTime += Time.deltaTime;
                if (currentExtractionTime >= extractionTime)
                {
                    ExtractIntel();
                    isExtracting = false;
                }
            }
            else
            {
                isExtracting = false;
            }
        }
        else
        {
            GameManager.instance.buttonInteract.SetActive(false);
        }
    }

    void ExtractIntel()
    {
        //Debug.Log("Intel collected!");
        //Destroy(gameObject);
        GameManager.instance.CompleteMissionObjective("Locate the enemy desktop and extract the intel!");
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
}
