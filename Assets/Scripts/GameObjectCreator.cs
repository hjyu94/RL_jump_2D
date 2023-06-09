using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectCreator : MonoBehaviour
{
    GameObject movingParent;
    public GameObject[] monsters;
    public Vector2[] generationPositions;

    float elapsedTime;
    float generationTime;
    int monsterIdx;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;
        generationTime = Random.Range(1f, 2.5f);
        movingParent = GameObject.Find("Moving");
        PickMonsterIdx();
    }


    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > generationTime)
        {
            var newObject = Instantiate(monsters[monsterIdx], generationPositions[monsterIdx], Quaternion.identity);
            newObject.transform.position = new Vector2(
                newObject.transform.position.x + Random.Range(-5f, 5f),
                newObject.transform.position.y
            );
            newObject.transform.SetParent(movingParent.transform);

            elapsedTime = 0f;
            generationTime = Random.Range(0.8f, 2f);
            PickMonsterIdx();
        }
    }

    void PickMonsterIdx()
    {
        monsterIdx = Random.Range(0f, 1f) > 0.7 ? 1 : 0;
    }

    public void ResetEnvironment()
    {
        Debug.Log("GameObjectCreator: reset");
        var enemies = GameObject.FindGameObjectsWithTag("MonsterRoot");
        for (int i = 0; i < enemies.Length; ++i)
        {
            Destroy(enemies[i]);
        }
    }
}
