using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class ShellShootBehaviour : UdonSharpBehaviour
{

    [SerializeField]
    private VRCObjectPool shellPool;
    private VRCPlayerApi localPlayer;

    void Start()
    {
        this.localPlayer = Networking.LocalPlayer;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            
            Networking.SetOwner(
                this.localPlayer,
                this.shellPool.gameObject
            );

            GameObject spawnedShell = this
                .shellPool
                .TryToSpawn();

            if (spawnedShell != null)
            {
                spawnedShell
                    .GetComponent<ShellBehaviour>()
                    .Init();
            }

        }

    }

}
