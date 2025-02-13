using UnityEngine;

public class Lean : MonoBehaviour
{
    [SerializeField] private float leanAngle;
    [SerializeField] public float leanSpeed;

    [SerializeField] public float targetLeanAngle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            targetLeanAngle = leanAngle;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            targetLeanAngle = -leanAngle;
        }
        else
        {
            targetLeanAngle = 0;
        }


        float currentAngle = Mathf.LerpAngle(transform.localEulerAngles.z, targetLeanAngle, Time.deltaTime * leanSpeed);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentAngle);
    }
}
