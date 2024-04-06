using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float jump = 4.0f;
    public int doubleJump = 2;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    // holds a reference to rigidbody for access from the script
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private int numJumps;

    // Start is called before the first frame update
    void Start()
    {
        // gets its value from the gameobject rigidbody
        rb = GetComponent<Rigidbody>();
        count = 0;
        numJumps = doubleJump;

        // show score and hide win text 
        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Jump()
    {
        if (numJumps > 0)
        {
            // player is on the ground or has one jump left
            if (Mathf.Abs(rb.velocity.y) < 0.01f || numJumps == doubleJump)
            {
                rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
                numJumps--;
            }

            // player is in the air and has one jump left
            else if(numJumps == 1)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z); // Reset y velocity to avoid adding it to the second jump
                rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
                numJumps--;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    // is called by Unity when player object first touches a trigger collider other with tag PickUp
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }

    // reset jumps when player touches ground
    private void OnCollisionEnter(Collision landOnGround)
    {
        if (landOnGround.gameObject.CompareTag("Ground"))
        {
            numJumps = doubleJump;
        }
    }
}
