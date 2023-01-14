using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CoinBehaviour : UdonSharpBehaviour
{

    public AudioSource coinPickUpSound;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        this.coinPickUpSound.Play();
        this
            .gameObject
            .SetActive(false);
    }
}