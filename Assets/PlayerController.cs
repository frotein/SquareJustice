using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Vector2 movement;
    public float movementForce;
    public float jumpTime;
    public float jumpForce;
    public float spinForce;
    public float getUpForce;
    public float wallJumpForce;
    public Transform getUpPosition;
    Rigidbody2D rb;
    float startJumpTime;
    public bool onGround;
    bool jumping;
    List<Transform> corners;
    List<Transform> collisions;
    int wallJump;
    Vector2 jumpNormal;
    // Use this for initialization
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        getUpPosition = transform.GetChild(0);
        corners = new List<Transform>();
        for(int i  = 1; i < transform.childCount; i++)
        {
            corners.Add(transform.GetChild(i));
        }
    }

        
    // Update is called once per frame
    void Update()
    {
        DetectCollisions();
        movement = new Vector2(Input.GetAxis("Horizontal"), 0);

        if(Input.GetButtonDown("Jump") && onGround)
        {
            startJumpTime = Time.time;
            jumping = true;
        }
        if (Input.GetButtonUp("Jump"))
            jumping = false;

       
        if (onGround && !Input.GetButton("Turn"))
        {          
                rb.AddForceAtPosition(movement * movementForce * Time.deltaTime, transform.position.XY());
        }            
        else
        {
            // rb.AddTorque(movement.x * -Time.deltaTime * spinForce);
            float force = getUpForce;
            if (!onGround)
                force = spinForce;

            if (Input.GetButton("Turn"))
            {
                Vector2 pushPosition1 = (getUpPosition.position.XY() - transform.position.XY()) * Mathf.Abs(movement.x) + transform.position.XY();
                Vector2 pushPosition2 = transform.position + -transform.up * getUpPosition.localPosition.y * Mathf.Abs(movement.x);
                rb.AddForceAtPosition(movement.x * transform.right * force * Time.deltaTime, pushPosition1);
                rb.AddForceAtPosition(-movement.x * transform.right * force * Time.deltaTime, pushPosition2);
            }
        }

        if (jumping && Time.time < startJumpTime + jumpTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
        }

        if(wallJump != 0 && Input.GetButtonDown("Jump") && !onGround)
        {
            rb.velocity = Vector2.zero;
            Vector2 jumpVector = jumpNormal + new Vector2(0, 1.5f);
            rb.AddForce(jumpVector * wallJumpForce);
        }
    }
    void OnCollisionStay2D(Collision2D col)
    {
        Vector2 normal = Vector2.zero;
        foreach(ContactPoint2D p in col.contacts)
        {
            normal += p.normal;
        }

        normal /= col.contacts.Length;

        jumpNormal = normal;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 normal = Vector2.zero;
        foreach (ContactPoint2D p in col.contacts)
        {
            normal += p.normal;
        }

        normal /= col.contacts.Length;

        jumpNormal = normal;
    }
    void OnCollisionExit2D(Collision2D col)
    {
     /*   Vector2 pos = Vector2.zero;
        int num = 0;
        foreach (ContactPoint2D con in col.contacts)
        {
            pos += con.point;
            num++;
        }
        pos /= num;

        if (pos.y < transform.position.y)
            onGround = false;*/
    }

    void DetectCollisions()
    {
        
        collisions = new List<Transform>();
        foreach (Transform t in corners)
        {
            Collider2D col = Physics2D.OverlapPoint(t.position);

            if (col != null)
            { collisions.Add(t);  }
        }
        int below = 0;
        int left = 0;
        int right = 0;
        if (collisions.Count >= 1)
        {          
            foreach (Transform t in collisions)
            {
                if (t.position.y < transform.position.y)
                    below++;
                if (t.position.x < transform.position.x)
                    left++;
                else
                    right++;
            }
        }
        if (left > 1)
            wallJump = 1;
        else
        {
            if (right > 1)
                wallJump = -1;
            else
                wallJump = 0;
        }

        if (below >= 1 && wallJump == 0)
            onGround = true;
        else
            onGround = false;
    }
}
