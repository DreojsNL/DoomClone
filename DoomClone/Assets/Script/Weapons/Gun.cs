using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public int pellets = 10;
    public float spreadAngle = 20f;
    public float fireRate = 1f;
    public float shotRange = 10f;
    public float moveSpeed = 5f; // Adjust the movement speed here
    public Image gunImage;
    public Sprite[] gunSprites;
    public GameObject hitPrefab; // Assign your prefab in the inspector
    public float hitPrefabOffset = 0.01f; // Adjust the offset to prevent z-fighting
    public Animator imageAnimation;
    private bool canShoot = true;
    private bool isShooting = false; // Added variable
    public AudioSource audioSource;
    private bool isMoving;
    private Camera playerCamera;
    public Player playerScript;

    private int currentSpriteIndex = 0;
    public float scrollSpeed = 0.1f; // Adjust the scrolling speed here

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            StartCoroutine(Shoot());
        }

        // Check if the player is moving and play the SGSway animation accordingly
        if (playerScript.isMoving == true)
        {
            imageAnimation.SetBool("IsMoving", true);
        }
        else
        {
            imageAnimation.SetBool("IsMoving", false);
        }

        // Set the isShooting parameter in the animation controller
        imageAnimation.SetBool("IsShooting", isShooting);
    }

    public void ScrollImages()
    {
        // Start invoking the SwitchSprite method with a delay between switches
        InvokeRepeating("SwitchSprite", 0f, scrollSpeed * gunSprites.Length);
    }

    private void SwitchSprite()
    {
        gunImage.sprite = gunSprites[currentSpriteIndex];
        currentSpriteIndex++;

        if (currentSpriteIndex >= gunSprites.Length)
        {
            currentSpriteIndex = 0;
            CancelInvoke("SwitchSprite");
            Invoke("ResetImage", 0.2f);
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        isShooting = true; // Set isShooting to true when shooting starts
        audioSource.Play();
        ScrollImages();
        imageAnimation.Play("SGRecoil");
        Invoke("ResetImage", 0.4f);

        for (int i = 0; i < pellets; i++)
        {
            float currentSpread = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            float verticalSpread = Random.Range(-spreadAngle / 2, spreadAngle / 2);

            // Calculate direction based on spread angle for both horizontal and vertical axes
            Vector3 direction = playerCamera.transform.forward;
            direction = Quaternion.Euler(verticalSpread, currentSpread, 0) * direction;

            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, direction, out hit, shotRange))
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name);
                Debug.DrawLine(playerCamera.transform.position, hit.point, Color.red, 0.1f);

                // Offset the instantiation position to prevent z-fighting
                Vector3 hitPointWithOffset = hit.point + hit.normal * hitPrefabOffset;

                // Instantiate the prefab at the hit point with offset
                if (hitPrefab != null)
                {
                    Instantiate(hitPrefab, hitPointWithOffset, Quaternion.identity);
                }
            }
            else
            {
                Debug.DrawRay(playerCamera.transform.position, direction * shotRange, Color.green, 0.1f);
            }
        }

        yield return new WaitForSeconds(1f / fireRate);
        canShoot = true;
        isShooting = false; // Set isShooting to false when shooting is done
    }

    public void ResetImage()
    {
        imageAnimation.Play("SGIdel");
        gunImage.sprite = gunSprites[0];
        currentSpriteIndex = 0;
    }
}
