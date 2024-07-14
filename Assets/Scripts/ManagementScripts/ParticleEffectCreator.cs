using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectCreator : MonoBehaviour
{
    private static ParticleEffectCreator singleton;
    public void Awake()
    {
        singleton = this;
    }
}
