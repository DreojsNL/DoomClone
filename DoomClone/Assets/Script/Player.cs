using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 10f; // Speed when sprinting
    public float rotationSpeed = 700f;
    public float maxLookUp = 80f; // Maximum angle to look up
    public float maxLookDown = 80f; // Maximum angle to look down
    private float pitch = 0f;
    public int maxHealth = 100;
    public int currentHealth;
    public bool isMoving;
    private Rigidbody rb;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = maxHealth;

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Sprinting check
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        // Calculate movement direction
        Vector3 movement = (transform.forward * vertical + transform.right * horizontal).normalized * currentSpeed;

        // Apply force to the Rigidbody
        rb.AddForce(movement);
        isMoving = movement.magnitude > 0.1f;

        // Player rotation
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Rotate the player around the Y-axis (yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera around the X-axis (pitch)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxLookUp, maxLookDown);
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player took " + damageAmount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // You can add your own logic here for player death, such as game over or respawn.
    }
}
