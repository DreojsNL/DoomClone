using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public int pellets = 10;
    public float spreadAngle = 20f;
    public float fireRate = 1f;
    public float shotRange = 10f;
    public Image gunImage;
    public Sprite[] gunSprites;

    private bool canShoot = true;
    public AudioSource audioSource;
    private Camera playerCamera;
    public Animator imageAnimation;

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
        audioSource.Play();
        ScrollImages();
        imageAnimation.Play("SGRecoil");
        Invoke("ResetImage", 0.4f);

        for (int i = 0; i < pellets; i++)
        {
            float currentSpread = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Vector3 direction = playerCamera.transform.forward;
            direction = Quaternion.Euler(0, currentSpread, 0) * direction;

            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, direction, out hit, shotRange))
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name);
                Debug.DrawLine(playerCamera.transform.position, hit.point, Color.red, 0.1f);
            }
            else
            {
                Debug.DrawRay(playerCamera.transform.position, direction * shotRange, Color.green, 0.1f);
            }
        }

        yield return new WaitForSeconds(1f / fireRate);
        canShoot = true;
    }
    public void ResetImage()
    {
        imageAnimation.Play("SGIdel");
        gunImage.sprite = gunSprites[0];
        currentSpriteIndex = 0;
    }
}
