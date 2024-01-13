using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    public Animator npcAnimator;
    public float acceptableAngle = 45f;

    private GameObject player;
    private HitScanAI hs;
    private Bomber bm;
    private bool isWalking;
    public string shooting;
    public string front;
    public string back;
    public string left;
    public string right;

    void Start()
    {
        hs = GetComponent<HitScanAI>();
        bm = GetComponent<Bomber>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (hs != null)
        {
            isWalking = hs.isMoving;
        }
        else
        {
            isWalking = bm.isMoving;
        }
       
        if (player == null)
            return;

        Vector3 localPlayerDirection = transform.InverseTransformDirection(player.transform.position - transform.position);
        Quaternion rotationToPlayer = Quaternion.LookRotation(localPlayerDirection, Vector3.up);
        float angle = rotationToPlayer.eulerAngles.y;

        if (angle < 0)
            angle += 360f;

        if (Mathf.Abs(angle) < acceptableAngle || Mathf.Abs(angle - 360f) < acceptableAngle && hs != null)
        {
            if(hs != null)
            {
                if (hs.isShooting)
                {
                    SetAnimation(shooting);
                }
                else
                {
                    SetAnimation(front);
                }
            }
            else
            {
                if (bm.isExploding)
                {
                    SetAnimation(shooting);
                }
                else
                {
                    SetAnimation(front);
                }
            }
          
          
        }
        else if (Mathf.Abs(angle - 90f) < acceptableAngle )
        {
            if (bm != null && !bm.isExploding)
            {
                SetAnimation(right);
            }else if (hs != null && !hs.isShooting)
            {
                SetAnimation(right);
            }
            else
            {
                SetAnimation(shooting);
            }

        }
        else if (Mathf.Abs(angle - 180f) < acceptableAngle)
        {
            if (bm != null && !bm.isExploding)
            {
                SetAnimation(back);
            }
            else if (hs != null && !hs.isShooting)
            {
                SetAnimation(back);
            }
            else
            {
                SetAnimation(shooting);
            }
        }
        else if (Mathf.Abs(angle - 270f) < acceptableAngle)
        {
            if (bm != null && !bm.isExploding)
            {
                SetAnimation(left);
            }
            else if (hs != null && !hs.isShooting)
            {
                SetAnimation(left);
            }
            else
            {
                SetAnimation(shooting);
            }
        }
    }

  public  void SetAnimation(string animationName)
    {
        npcAnimator.SetBool("IsMoving", isWalking);
        npcAnimator.Play(animationName);
    }
}
