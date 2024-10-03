using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamConfiner : MonoBehaviour
{
    [SerializeField] private Collider2D col;

    private void OnEnable()
    {
        MovementCamera.Instance.SetConfiner2DCollider(col);
    }
}
