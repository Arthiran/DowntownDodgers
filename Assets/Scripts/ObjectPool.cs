using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject ball;
    public Queue<GameObject> ballPool;
    public int queueSize;

    // Start is called before the first frame update
    void Start()
    {
        ballPool = new Queue<GameObject>(queueSize);

        for (int i = 0; i < queueSize; i++)
        {
            GameObject temp;
            temp = Instantiate(ball, new Vector3(0, 0, 0), Quaternion.identity);
            temp.SetActive(false);
            ballPool.Enqueue(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject SpawnBall()
    {
        GameObject curBall;

        curBall = ballPool.Dequeue();

        curBall.SetActive(true);

        return curBall;
    }

    public void DespawnBall(GameObject aBall)
    {
        aBall.SetActive(false);

        ballPool.Enqueue(aBall);
    }
}
