using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MLPlayer : Agent
{
    Rigidbody2D rigid;
    public float jumpPower;
    Animator anim;
    
    public GameObject background;
    Background backgroundScript;

    public GameObject creator;
    GameObjectCreator creatorScript;

    float elapsedTime;

    public override void Initialize()
    {
        rigid = GetComponent<Rigidbody2D>();
        // elapsedTime = 0f;   
        anim = GetComponent<Animator>();
        // anim.SetBool("isRunning", false);
        // anim.SetBool("isJumping", false);
        // anim.SetBool("isDead", false);

        backgroundScript = background.GetComponent<Background>();
        creatorScript = creator.GetComponent<GameObjectCreator>();
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var action = actionBuffers.DiscreteActions[0];
        // var actionZ = 2f * Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
        // var actionX = 2f * Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);

        // if ((gameObject.transform.rotation.z < 0.25f && actionZ > 0f) ||
        //     (gameObject.transform.rotation.z > -0.25f && actionZ < 0f))
        // {
        //     gameObject.transform.Rotate(new Vector3(0, 0, 1), actionZ);
        // }

        // if ((gameObject.transform.rotation.x < 0.25f && actionX > 0f) ||
        //     (gameObject.transform.rotation.x > -0.25f && actionX < 0f))
        // {
        //     gameObject.transform.Rotate(new Vector3(1, 0, 0), actionX);
        // }
        // if ((ball.transform.position.y - gameObject.transform.position.y) < -2f ||
        //     Mathf.Abs(ball.transform.position.x - gameObject.transform.position.x) > 3f ||
        //     Mathf.Abs(ball.transform.position.z - gameObject.transform.position.z) > 3f)
        // {
        //     SetReward(-1f);
        //     EndEpisode();
        // }
        // else
        // {
        //     SetReward(0.1f);
        // }
    }

    public override void OnEpisodeBegin()
    {
        // Reset everything
        Debug.Log("Episode Begin");
        ResetEnvironment();
        backgroundScript.ResetEnvironment();
        creatorScript.ResetEnvironment();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if(Input.GetButtonDown("Jump"))
        {
            discreteActionsOut[0] = 0;
        }
        else
        {
            discreteActionsOut[0] = 1;
        }
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

        if (gameObject.transform.position.y < -5f)
        {
            EndEpisode();
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
        if(collision.gameObject.tag == "WallReward")
        {
            Debug.Log("+0.1 reward");
            AddReward(0.1f);
        }

        if(collision.gameObject.tag == "Enemy")
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isDead", true);
            Debug.Log("-1 reward");
            Invoke("EndDeath", 2f);
            backgroundScript.pause();
            AddReward(-1.0f);
        }
    }

    void ResetEnvironment()
    {
        gameObject.transform.position = new Vector2(-8f, -3f);
        elapsedTime = 0f;   
        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isDead", false);
    }


    void EndDeath()
    {
        Debug.Log("Restart!");
        EndEpisode();
    }
}
