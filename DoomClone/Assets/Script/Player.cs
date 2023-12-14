using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 10f; // Speed when sprinting
    public float rotationSpeed = 700f;
    public float maxLookUp = 80f; // Maximum angle to look up
    public float maxLookDown = 80f; // Maximum angle to look down
    private float pitch = 0f;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Sprinting check
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        Vector3 movement = new Vector3(horizontal, 0, vertical) * currentSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Player rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the player around the Y-axis (yaw)
        Vector3 rotation = new Vector3(0, mouseX, 0) * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotation);

        // Rotate the camera around the X-axis (pitch)
        pitch -= mouseY * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -maxLookUp, maxLookDown);
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
