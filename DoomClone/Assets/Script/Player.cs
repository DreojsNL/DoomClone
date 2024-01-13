using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

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
    private float damageCooldown;
    public bool isMoving;
    public GameObject[] pickUpSounds;
    private Rigidbody rb;
    public TextMeshProUGUI healthText; // Reference to the TextMeshPro component for displaying health
    public Gun shotGun;
    public Gun pistol;
    public bool hasKey = false;
    private Animator doorAnimator;



    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = maxHealth;

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (damageCooldown > 0)
        {
            damageCooldown -= Time.deltaTime;
        }
        UpdateHealthText();
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
    }

    private void LateUpdate()
    {
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
            UpdateHealthText(); // Update the health text when the player takes damage

            if (currentHealth <= 0)
            {
                Die();
            }

    }
    public void AddHealth(int healthAmount)
    {
        currentHealth += healthAmount; 
    }

        private void Die()
    {
        Debug.Log("Player died!");
        // You can add your own logic here for player death, such as game over or respawn.
    }

    // Update the TextMeshPro component with the current health value
    void UpdateHealthText()
    {
        if (healthText != null)
        {
            // Format the health value with leading zeros
            healthText.text = string.Format("{0:000}", currentHealth);
            // Alternatively, you can use string interpolation:
            // healthText.text = $"Health: {currentHealth:000}";
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealthPack") && currentHealth >= 80 && currentHealth < 100)
        {
            Instantiate(pickUpSounds[0], transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            currentHealth = 100;
        }
        else if(other.gameObject.CompareTag("HealthPack") && currentHealth < 100)
        {
            Instantiate(pickUpSounds[0], transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            AddHealth(20);
        }
        if (other.gameObject.CompareTag("HealthKit") && currentHealth >= 90 && currentHealth < 100)
        {
            Instantiate(pickUpSounds[0], transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            currentHealth = 100;
        }
        else if (other.gameObject.CompareTag("HealthKit")&& currentHealth < 100)
        {

            Instantiate(pickUpSounds[0], transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            AddHealth(10);
        }
        if (other.gameObject.CompareTag("ArmourPack"))
        {
            Instantiate(pickUpSounds[1], transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            AddHealth(20);
        }
        if (other.gameObject.CompareTag("ArmourHelmet"))
        {
            Instantiate(pickUpSounds[1], transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            AddHealth(10);
        }
        if (other.gameObject.CompareTag("SGAmmo"))
        {
            Instantiate(pickUpSounds[2], transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            shotGun.AddAmmo(10);
        }
        if (other.gameObject.CompareTag("PTAmmo"))
        {
            Instantiate(pickUpSounds[2], transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            pistol.AddAmmo(10);
        }
        if (other.gameObject.CompareTag("Key"))
        {
            Instantiate(pickUpSounds[1], transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            hasKey = true;
        }


    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Poison") && damageCooldown <= 0)
        {
            TakeDamage(10);
            damageCooldown = 1f; // Adjust the cooldown time (1.5f seconds in this example)
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            doorAnimator = collision.gameObject.GetComponent<Animator>();
            // Trigger the door animation
            doorAnimator.SetTrigger("OpenDoor");
        }
        if (collision.gameObject.CompareTag("KeyDoor"))
        {
            if(hasKey)
            {
                doorAnimator = collision.gameObject.GetComponent<Animator>();
                // Trigger the door animation
                doorAnimator.SetTrigger("OpenDoor");
            }
            
        }
    }
}
