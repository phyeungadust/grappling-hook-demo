using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class ItemPickUpBox : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStoreCollection playerStoreCollection;
    [SerializeField]
    private Animator boxBang;
    [SerializeField]
    private GameObject boxObj;
    [SerializeField]
    private Collider boxCollider;
    [SerializeField]
    private float defaultRespawnTime = 5.0f;
    private float respawnTime;
    private bool crashed = false;
    private bool crashing = false;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal)
        {

            Debug.Log("hello");

            // play box crash animation
            this.SendCustomNetworkEvent(
                VRC.Udon.Common.Interfaces.NetworkEventTarget.All,
                nameof(PlayBangAndDespawn)
            );
            // give whomever touches the itempickupbox a random item
            this
                .playerStoreCollection
                .GetByID(player.playerId)
                .itemManager
                .EquipRandomItem();

        }
    }

    public void PlayBangAndDespawn()
    {
        this.boxBang.Play("Bang", -1);

        // stops other players from triggering item gen
        this.boxCollider.enabled = false;

        this.crashed = false;
        this.crashing = true;
    }

    void FixedUpdate()
    {

        if (this.crashed)
        {
            // box crashed

            // countdown respawn time
            this.respawnTime -= Time.fixedDeltaTime;

            if (this.respawnTime <= 0.0f)
            {
                // respawn box
                this.boxObj.SetActive(true);
                // exit crashed state
                this.crashed = false;
                // enter idle state

                // enable collider so that a player can pick up an item
                this.boxCollider.enabled = true;
            }

        }
        else if (this.crashing)
        {

            // box crashing

            if (this.boxBang.GetCurrentAnimatorStateInfo(0).IsName("Bang"))
            {

                Debug.Log($"normalizedTime: {this.boxBang.GetCurrentAnimatorStateInfo(0).normalizedTime}");

                if (this.boxBang.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {

                    Debug.Log("passed 1.0f");
                    Debug.Log("passed 1.0f.");
                    Debug.Log("passed 1.0f");
                    Debug.Log("passed 1.0f.");
                    Debug.Log("passed 1.0f");
                    Debug.Log("passed 1.0f.");
                    Debug.Log("passed 1.0f");
                    Debug.Log("passed 1.0f.");

                    // box crashed

                    // deactivate boxObj to hide meshes
                    this.boxObj.SetActive(false);

                    // reset animator to entry state
                    this.boxBang.Rebind();
                    this.boxBang.Update(0.0f);

                    // exit crashing state
                    this.crashing = false;

                    // enter crashed state
                    this.crashed = true;
                    // start timer for respawning
                    this.respawnTime = this.defaultRespawnTime;

                }

            }

        }

    }

}