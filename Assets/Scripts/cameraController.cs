using UnityEngine;

public class cameraController : MonoBehaviour
{
    public static cameraController instance;

    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    

    float rotX;
    Vector3 originalPosition;
    public Vector3 currentRecoil;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        transform.localRotation = Quaternion.Euler(rotX + currentRecoil.y, currentRecoil.x, 0);

        // Rotate the player on the y-axis. (look left and right)
        transform.parent.Rotate(Vector3.up * mouseX);

        
    }
    
    
}
