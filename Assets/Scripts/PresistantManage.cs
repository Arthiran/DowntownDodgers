using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresistantManage : MonoBehaviour
{
    public static PresistantManage Instance
    {
        get;
        private set;
    }

    public int value;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
