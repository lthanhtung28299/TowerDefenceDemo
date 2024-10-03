using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MovementCamera : MonoSingleton<MovementCamera>
{
    [field: SerializeField] public CinemachineVirtualCamera vcam { get; private set; }
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public CinemachineConfiner  camConfiner { get; private set; }

    public void SetConfiner2DCollider(Collider2D other)
    {
        camConfiner.m_BoundingShape2D = other.GetComponent<Collider2D>();
    }
}