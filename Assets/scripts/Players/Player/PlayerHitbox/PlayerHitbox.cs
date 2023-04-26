using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PlayerHitbox : UdonSharpBehaviour
{

    public PlayerStore ownerStore;
    [SerializeField]
    private Collider playerHitboxCollider;

    public Collider GetCollider() => this.playerHitboxCollider;

    [UdonSynced]
    private bool chargeAvailable = true;
    private float rechargeTimer = 10.0f;

    public void CustomStart()
    {
        if (this.ownerStore.localVRMode.IsLocal())
        {
            this.gameObject.layer = 25;
        }
        else
        {
            this.gameObject.layer = 26;
        }
    }

    public void CustomUpdate()
    {
        this.UpdateHitboxPos();
        if (!this.chargeAvailable)
        {
            // recharge when charge not available
            this.rechargeTimer += Time.deltaTime;
            if (this.rechargeTimer >= 10.0f)
            {
                this.chargeAvailable = true;
                this.ownerStore.hud.meleePanel.Ready();
                this.rechargeTimer = 10.0f;
            }
        }
    }

    private void UpdateHitboxPos()
    {

        VRCPlayerApi playerApi = this.ownerStore.playerApiSafe.Get();

        Vector3 headPos = playerApi.GetTrackingData(
            VRCPlayerApi.TrackingDataType.Head
        ).position + new Vector3(0, .5f, 0);
        Vector3 leftFootPos = playerApi.GetBonePosition(HumanBodyBones.LeftFoot);
        Vector3 rightFootPos = playerApi.GetBonePosition(HumanBodyBones.RightFoot);
        Vector3 feetPos = (leftFootPos + rightFootPos) / 2 - new Vector3(0, .5f, 0);
        Vector3 bodyCenter = (headPos + feetPos) / 2;
        this.transform.position = bodyCenter;
        this.transform.localScale = new Vector3(1, (headPos - bodyCenter).magnitude, 1);

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (this.ownerStore.localVRMode.IsLocal())
        {
            if (collider.gameObject.layer == 26)
            {
                // if player hits a NonLocalHitbox
                if (this.chargeAvailable)
                {

                    // if localPlayer has charge

                    // find and assign victimStore
                    PlayerStore victimStore = null;
                    PlayerStore[] playerStores = this.ownerStore.playerStoreCollection.GetAll();
                    int playerCount = this.ownerStore.playerStoreCollection.GetCount();
                    for (int i = 1; i <= playerCount; ++i)
                    {
                        if (playerStores[i].hitbox.GetCollider() == collider)
                        {
                            victimStore = playerStores[i];
                            break;
                        }
                    }

                    float hitterSpeedSqr = this
                        .ownerStore
                        .playerApiSafe
                        .Get()
                        .GetVelocity()
                        .sqrMagnitude;

                    float victimSpeedSqr = victimStore
                        .playerApiSafe
                        .Get()
                        .GetVelocity()
                        .sqrMagnitude;

                    if (victimStore.hitbox.chargeAvailable)
                    {

                        // if victim has charge

                        if (hitterSpeedSqr >= victimSpeedSqr)
                        {

                            // hitter speed greater than victim
                            // register melee hit

                            int score = Mathf.Clamp(
                                Mathf.CeilToInt(hitterSpeedSqr - victimSpeedSqr),
                                200,
                                800
                            );

                            this
                                .ownerStore
                                .interactionMediator
                                .MeleeHitUnicast(
                                    victimStore.playerApiSafe.GetID(),
                                    score
                                );

                            this
                                .ownerStore
                                .hud
                                .hudScoreController
                                .ChangeScoreAmount(
                                    score,
                                    $"MELEE HIT! +{score}"
                                );

                            this.ChargeUsed();

                        }
                    }
                    else
                    {

                        int score = Mathf.Clamp(
                            Mathf.CeilToInt(hitterSpeedSqr - victimSpeedSqr),
                            200,
                            800
                        );

                        this
                            .ownerStore
                            .interactionMediator
                            .MeleeHitUnicast(victimStore.playerApiSafe.GetID(), score);

                        this
                            .ownerStore
                            .hud
                            .hudScoreController
                            .ChangeScoreAmount(
                                score,
                                $"MELEE HIT! +{score}"
                            );

                        this.ChargeUsed();

                    }

                }
            }
        }
    }

    public void ChargeUsed()
    {
        this.chargeAvailable = false;
        this.ownerStore.hud.meleePanel.Cooldown();
        this.rechargeTimer = 0.0f;
        this.RequestSerialization();
    }

}