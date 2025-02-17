using UnityEditor;
using UnityEngine;

public class IntelExtraction : MonoBehaviour
{

    private float extractionTime = 5f; //required time to extract the intel
    private float currentExtractionTime = 0;
    private bool isExtracting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         GameManager.instance.AddMissionObjective("Extract Intel");
    }

    public void StartExtraction()
    {
        isExtracting = true;
        currentExtractionTime = 0;
    }

    // Update is called once per frame
    void Update()//similar to BombDefusing
    {
        if (isExtracting)
        {
            if (Input.GetKey(KeyCode.E))
            {
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
    }

    void ExtractIntel()
    {
        Debug.Log("Intel collected!");
        Destroy(gameObject);
        GameManager.instance.CompleteMissionObjective("Extract Intel");
    }
}
