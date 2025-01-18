using UnityEngine;

public class cameraController : MonoBehaviour
{

    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    [SerializeField] float peekDistance = 0.5f;
    [SerializeField] KeyCode peekLeftKey = KeyCode.Q;
    [SerializeField] KeyCode peekRightKey = KeyCode.E;

    float rotX;
    Vector3 originalPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Get inputs.
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Tie the mouseY to the rotX of the camera. (look up and down)
        if(invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;

        // Clamp the camera on the x-axis.
        rotX = Mathf.Clamp( rotX, lockVertMin, lockVertMax);

        // Rotate the camera on the x-axis.
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        // Rotate the player on the y-axis. (look left and right)
        transform.parent.Rotate(Vector3.up * mouseX);

        HandlePeeking();
    }

    void HandlePeeking()
    {
        // Check if the player is peeking.
        if (Input.GetKey(peekLeftKey))
        {
            transform.localPosition = new Vector3(-peekDistance, originalPosition.y, originalPosition.z);
        }
        else if (Input.GetKey(peekRightKey))
        {
            transform.localPosition = new Vector3(peekDistance, originalPosition.y, originalPosition.z);
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }
}
