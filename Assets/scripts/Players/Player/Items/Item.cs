using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Item : UdonSharpBehaviour
{

    public virtual ItemControls GetItemControls() => null;
    public virtual GameStateControls GetGameStateControls() => null;

}