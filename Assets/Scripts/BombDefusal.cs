using UnityEngine;

public class BombDefusal : MonoBehaviour
{
    public float defusalTime = 3f;
    private float currentDefusalTime = 0;
    private bool isDefusing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.AddMissionObjective("Defuse Bomb");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        { //Press 'B' to start defusing
            isDefusing = true;
            currentDefusalTime = 0;
        }
        if (isDefusing)
        {
            if (Input.GetKey(KeyCode.B))
            { //Hold 'B' to continue defusing
                currentDefusalTime += Time.deltaTime;
                if (currentDefusalTime >= defusalTime)
                {
                    DefuseBomb();
                    isDefusing = false;
                }
            }
            else
            {
                isDefusing = false; //Release 'B' to stop defusing
            }
        }
    }

    public void DefuseBomb()
    {
        Debug.Log("Bomb has been defused!");
        Destroy(gameObject);
        GameManager.instance.CompleteMissionObjective("Defuse Bomb");
    }
}
