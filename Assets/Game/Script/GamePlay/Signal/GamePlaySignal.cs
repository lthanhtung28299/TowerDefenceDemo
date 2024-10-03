using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;
using UnityEngine;

public static class GamePlaySignal
{
    public static readonly Signal<bool> ShowButtonStartWave = new Signal<bool>();
    public static readonly Signal<int> UpdateGold = new Signal<int>();
    public static readonly Signal<int> UpdateLife = new Signal<int>();
    public static readonly Signal<int> UpdateWave = new Signal<int>();
    
}
