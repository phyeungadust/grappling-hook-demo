﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HUD : UdonSharpBehaviour
{
    public HUDStatusPopUpBehaviour popup;
    public SprayedOverlayBehaviour sprayHUD;
    public GameStartCountDown gameStartCountDown;
    public GameEndSequence gameEndSequence;
    public HUDScoreController hudScoreController;
    public MeleePanel meleePanel;
}