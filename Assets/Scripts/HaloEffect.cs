using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloEffect : MonoBehaviour
{
    public Vector3 speed;
    public float amplitude;
    public float frequency;

    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);

        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}
