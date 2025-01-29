using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class door : MonoBehaviour
{
    [SerializeField] GameObject Model;
   

    bool inTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Model.SetActive(false);
               GameManager.instance.buttonInteract.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.isTrigger)
        
            return;   
        

        IOpen open = other.GetComponent<IOpen>();

        if (open != null) 
        {
            inTrigger = true;
            GameManager.instance.buttonInteract.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;


        if (other.isTrigger)
            return;

        IOpen open = other.GetComponent<IOpen>();

        if (open != null)
        {
            Model.SetActive(true);
           GameManager.instance.buttonInteract.SetActive(false);
        }
    }
}
