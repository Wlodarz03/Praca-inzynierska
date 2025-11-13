using NUnit.Framework;
using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform grafika;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;
    [SerializeField] private float crouchHeight = 0.5f;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            rb.linearVelocity = Vector2.up * jumpForce;
        }

        if (isJumping && Input.GetButton("Jump"))
        {
            if (jumpTimer < jumpTime)
            {
                rb.linearVelocity = Vector2.up * jumpForce;

                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTimer = 0;
        }

        if (isGrounded && Input.GetButton("Crouch"))
        {
            grafika.localScale = new Vector3(grafika.localScale.x, crouchHeight, grafika.localScale.z);
        }

        if (isJumping && Input.GetButton("Crouch"))
        {
            grafika.localScale = new Vector3(grafika.localScale.x, 1.5f, grafika.localScale.z);
        }

        if (Input.GetButtonUp("Crouch"))
        {
            grafika.localScale = new Vector3(grafika.localScale.x, 1.5f, grafika.localScale.z);
        }
    }
}