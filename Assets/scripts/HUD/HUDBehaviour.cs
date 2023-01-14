using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HUDBehaviour : UdonSharpBehaviour
{

    [SerializeField]
    private GameObject hud;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            this.hud.SetActive(!this.hud.activeSelf);
        }
    }

}