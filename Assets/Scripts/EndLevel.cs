﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField]
    private ThrowableController throwable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        throwable.Reset();
    }

}
