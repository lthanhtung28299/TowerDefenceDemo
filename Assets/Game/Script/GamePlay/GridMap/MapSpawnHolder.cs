using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawnHolder : MonoSingleton<MapSpawnHolder>
{
    [field:SerializeField] public Transform backGroundHolder { get; private set; }
}
