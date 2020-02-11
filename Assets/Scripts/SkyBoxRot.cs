﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxRot : MonoBehaviour
{
    public float skyboxSpeed;

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxSpeed);
    }
}