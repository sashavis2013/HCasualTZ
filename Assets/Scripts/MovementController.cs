using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{



    private float runSpeed=0f;
    public Joystick Joystick;
    public float JumpForce = 2.0f;
    public float DestroyRadius = 7f;

    public LayerMask LayerWithProps;

    public ParticleSystem JumpDownAnimation;
    public AudioClip JumpDownSound;

    private Animator animator;
    private bool wasJumpDown = false;
    private Collider[] destructionColliders;
    private bool cooldown = false;

    [SerializeField] private bool isGrounded;

    private Rigidbody rb;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        runSpeed = Mathf.Abs(Joystick.Vertical)> Mathf.Abs(Joystick.Horizontal) ? Joystick.Vertical : Joystick.Horizontal;
        if (Joystick.Horizontal != 0 || Joystick.Vertical != 0)
        {
            animator.SetFloat("Speed", Mathf.Clamp(Mathf.Abs(runSpeed), 0.4f, 0.8f));
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    void FixedUpdate()
    {
        if (Joystick.Horizontal != 0 || Joystick.Vertical != 0)
        {
            transform.eulerAngles = new Vector3(0, Mathf.Atan2(Joystick.Horizontal, Joystick.Vertical) * 180 / Mathf.PI, 0);
        }

    }

    void OnCollisionEnter(Collision col)
    {
        isGrounded = true;
        animator.ResetTrigger("JumpDown");
        if (wasJumpDown)
        {
            DestroyNearItems();
        }
        wasJumpDown = false;

    }

    private void ResetCooldown()
    {
        cooldown = false;
    }

    private void DestroyNearItems()
    {
        JumpDownAnimation.gameObject.transform.position = transform.position;
        JumpDownAnimation.Play();
        AudioSource.PlayClipAtPoint(JumpDownSound, transform.position);

        destructionColliders = Physics.OverlapSphere(transform.position, DestroyRadius, LayerWithProps);

        foreach (Collider col in destructionColliders)
        {
            col.gameObject.GetComponent<PropDeleteScript>().Destroy();
        }
    }

    public void DoJump()
    {
        if(!isGrounded||cooldown) return;
        animator.ResetTrigger("JumpDown");
        animator.SetTrigger("Jump");
        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        isGrounded = false;
        Invoke(nameof(ResetCooldown), 1.0f);
        cooldown = true;

    }

    public void DoJumpDown()
    {
        if (isGrounded) return;
        {
            animator.SetTrigger("JumpDown");
            rb.AddForce(Vector3.down * JumpForce, ForceMode.Impulse);
            animator.ResetTrigger("Jump");
            wasJumpDown = true;
        }
    }

    

}
