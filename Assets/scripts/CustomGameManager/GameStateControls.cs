﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GameStateControls : UdonSharpBehaviour
{
    public virtual void OnBeforeGameStarts() {}
    public virtual void OnAfterGameStarts() {}
    public virtual void OnBeforeGameEnds() {}
    public virtual void OnAfterGameEnds() {}
}