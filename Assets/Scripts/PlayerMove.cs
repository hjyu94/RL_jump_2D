using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public float jumpPower;
    Animator anim;
    public GameObject gameover;
    
    float elapsedTime;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        elapsedTime = 0f;   
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
    }

    private void Update() {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > 0.5f)
        {
            anim.SetBool("isRunning", true);
        }
        // Jump
        if (Input.GetButtonDown("Jump") && anim.GetBool("isRunning"))
        {
            if (!anim.GetBool("isJumping"))
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
        }
        
        if (anim.GetBool("isRunning") && Mathf.Abs(rigid.velocity.y) > 0.2)
        {
            anim.SetBool("isJumping", true);
        }
    }

    private void FixedUpdate() 
    {
        // Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down * 2, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1);
            if (rayHit.collider != null)
            {
                anim.SetBool("isJumping", false);
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isDead", true);
            gameover.SetActive(true);
        }
    }
}
