﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GameStateControls : UdonSharpBehaviour
{
    public virtual void OnBeforeGameStarts() {}
    public virtual void OnGameEnds() {}
}