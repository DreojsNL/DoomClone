using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    public Animator npcAnimator;
    public float acceptableAngle = 45f;

    private GameObject player;
    private HitScanAI hs;
    private bool isWalking;

    void Start()
    {
        hs = GetComponent<HitScanAI>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        isWalking = hs.isMoving;
        if (player == null)
            return;

        Vector3 localPlayerDirection = transform.InverseTransformDirection(player.transform.position - transform.position);
        Quaternion rotationToPlayer = Quaternion.LookRotation(localPlayerDirection, Vector3.up);
        float angle = rotationToPlayer.eulerAngles.y;

        if (angle < 0)
            angle += 360f;

        if (Mathf.Abs(angle) < acceptableAngle || Mathf.Abs(angle - 360f) < acceptableAngle)
        {
            if (hs.isShooting)
            {
                SetAnimation("JoostShooting");
            }
            else
            {
                SetAnimation("JoostFront");
            }
          
        }
        else if (Mathf.Abs(angle - 90f) < acceptableAngle)
        {
            SetAnimation("JoostRight");
        }
        else if (Mathf.Abs(angle - 180f) < acceptableAngle)
        {
            SetAnimation("Back");
        }
        else if (Mathf.Abs(angle - 270f) < acceptableAngle)
        {
            SetAnimation("JoostLeft");
        }
    }

    void SetAnimation(string animationName)
    {
        npcAnimator.SetBool("IsMoving", isWalking);
        npcAnimator.Play(animationName);
    }
}
