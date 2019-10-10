using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraScript : MonoBehaviour
{
    private Camera EditorCamera;

    private float moveSpeed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        EditorCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("ArrowsVertical") != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Input.GetAxisRaw("ArrowsVertical") * moveSpeed);
        }
        if (Input.GetAxisRaw("ArrowsHorizontal") != 0)
        {
            transform.position = new Vector3(transform.position.x + Input.GetAxisRaw("ArrowsHorizontal") * moveSpeed, transform.position.y, transform.position.z);
        }

        if ((Input.GetAxisRaw("Mouse ScrollWheel") != 0))
        {
            if (EditorCamera.orthographicSize >= 0.5f && EditorCamera.orthographicSize <= 50)
            {
                EditorCamera.orthographicSize -= Input.GetAxisRaw("Mouse ScrollWheel");
            }
            else
            {
                if (EditorCamera.orthographicSize < 0.5f)
                {
                    EditorCamera.orthographicSize = 0.5f;
                }
                else if (EditorCamera.orthographicSize > 50)
                {
                    EditorCamera.orthographicSize = 50;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            moveSpeed += 0.2f;

            if (moveSpeed == 0.8f)
            {
                moveSpeed = 0.2f;
            }
        }
    }
}
