using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    float initSpeed;

    public GameObject player;
    Animator player_anim;
    
    public Transform[] floors;

    int currentIndex;

    float elapsedTime;
    bool isPaused;

    private void Awake() {
        player_anim = player.GetComponent<Animator>();
        initSpeed = speed;
        isPaused = false;
    }

    void Update()
    {
        if (isPaused)
            return;

        elapsedTime += Time.deltaTime;
        if(elapsedTime > 3f)
        {
            elapsedTime = 0f;

            if(speed < 15)
                speed = speed * 1.2f;
            else if(speed < 20)
                speed = speed * 1.1f;
        }

        if (!player_anim.GetBool("isRunning"))
            return;

        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
        transform.position = curPos + nextPos;        

        if (floors[currentIndex].position.x < -20f)
        {
            floors[currentIndex].position = new Vector2(20f, floors[currentIndex].position.y);
            currentIndex = (currentIndex + 1) % 2;
        }
    }

    public void ResetEnvironment()
    {
        Debug.Log("Reset Environment");
        gameObject.transform.position = new Vector2(0f, 0f);
        elapsedTime = 0f;   
        currentIndex = 0;
        speed = initSpeed;
        isPaused = false;

        floors[0].position = new Vector2(0, 0);
        floors[1].position = new Vector2(20, 0);
    }

    public void pause()
    {
        isPaused = true;
    }
}
