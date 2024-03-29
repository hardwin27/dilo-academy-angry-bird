﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird
{
    [SerializeField] public float _boostForce = 100f;
    public bool _hasBoost = false;

    public override void OnTap()
    {
        Boost();
    }

    public void Boost()
    {
        if(State == BirdState.Thrown && !_hasBoost)
        {
            Rigidbody.AddForce(Rigidbody.velocity * _boostForce);
            _hasBoost = true;
        }
    }
}
