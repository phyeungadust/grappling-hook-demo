using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ShellBehaviour : UdonSharpBehaviour
{

    public VRCObjectPool ShellPool;
    [SerializeField]
    private ShellProperties shellProperties;
    [SerializeField]
    private ParticleSystem smoke;
    [SerializeField]
    private ParticleSystem explosion;
    [SerializeField]
    private ParticleSystem debris;
    [SerializeField]
    private DoAfterAllParticleSystemsStop doAfterAllParticleSystemsStop;
    [SerializeField]
    private ReturnShellCommand returnShellCommand;

    [SerializeField]
    private PlayerStore ownerStore;
    private LocalVRMode localVRMode;
    private InteractionMediator interactionMediator;

    private int victimID = -1;
    private bool exploding = false;
    [HideInInspector]
    public bool launched = false;

    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private Collider shellCollider;

    public void Launch()
    {
        this.LaunchBroadcast(this.victimID);
    }

    public void LaunchBroadcast(int victimID)
    {
        this.LaunchBroadcastSyncString = string.Join(
            " ",
            System.Guid.NewGuid().ToString().Substring(0, 6),
            victimID
        );
    }

    [UdonSynced, FieldChangeCallback(nameof(LaunchBroadcastSyncString))]
    private string launchBroadcastSyncString;
    public string LaunchBroadcastSyncString
    {
        get => this.launchBroadcastSyncString;
        set
        {

            this.launchBroadcastSyncString = value;
            if (this.localVRMode.IsLocal())
            {
                this.RequestSerialization();
            }
            string[] args = value.Split(' ');
            string nonce = args[0];
            int victimID = System.Int32.Parse(args[1]);

            this.LaunchLocal(victimID);

        }
    }

    public void LaunchLocal(int victimID)
    {

        this.victimID = victimID;
        this.launched = true;

        // detach from parent (player follower) so that the rocket's movement
        // is on its own, without being affected by
        // player's movements
        this.transform.parent = null;

        // smoke animation after launch
        ParticleSystem.MainModule mm = this.smoke.main;
        mm.startSpeed = -10.0f;
        ParticleSystem.EmissionModule em = this.smoke.emission;
        em.rateOverTime = 100.0f;

    }

    void OnEnable()
    {

        Debug.Log(
            $"player {this.ownerStore.playerApiSafe.Get().playerId}'s {this.gameObject.name} spawned"
        );

        this.localVRMode = this.ownerStore.localVRMode;
        this.interactionMediator = this.ownerStore.interactionMediator;

        // reattach rocket to parent (player's follower)
        // so that it follows the shooter before the rocket is launched
        this.transform.parent = this.ShellPool.gameObject.transform;

        // smoke animation before launched
        ParticleSystem.MainModule mm = this.smoke.main;
        mm.startSpeed = 0.1f;
        ParticleSystem.EmissionModule em = this.smoke.emission;
        em.rateOverTime = 10.0f;
        this.smoke.Play();

        // when first spawned, shell is not exploding nor launched
        this.exploding = false;
        this.launched = false;

        // reset to tracking pos (player's follower)
        this.transform.localPosition = Vector3.zero;
        this.transform.localEulerAngles = Vector3.zero;

        // enable mesh and collider
        this.meshRenderer.enabled = true;
        this.shellCollider.enabled = true;

    }

    void FixedUpdate()
    {

        // if shell already exploding, no need to move/ rotate it
        if (this.exploding) return;

        if (!this.launched)
        {
            // scan for new potential victim whenever shell isn't launched
            this.victimID = this.FindVictim();
        }

        Vector3 victimPos = this
            .ownerStore
            .playerStoreCollection
            .GetByID(this.victimID)
            .hitbox
            .transform
            .position;

        Vector3 shellToVictim = victimPos - this.transform.position;
        Vector3 shellToVictimNormalized = shellToVictim.normalized;
        float shellToVictimDist = shellToVictim.magnitude;

        if (shellToVictimDist > .5f)
        {

            // rotate shell to face victim

            this.transform.forward = shellToVictimNormalized;

            // this if branch is run if shellToVictimDist > .5f
            // this is to prevent shell from rapidly rotating
            // when it is very close to the victim

        }

        if (this.launched)
        {
            // shell chases victim 
            this.transform.position += 
                this.transform.forward 
                * this.shellProperties.Speed 
                * Time.fixedDeltaTime;
        }

    }

    private void OnTriggerEnter(Collider collider)
    {

        if (this.localVRMode.IsLocal())
        {

            // only check collisions of shell on shell's owner instance
            PlayerHitbox playerHitbox = collider.GetComponent<PlayerHitbox>();

            if (playerHitbox != null)
            {

                if (playerHitbox.ownerStore.localVRMode.IsLocal())
                {
                    // rocket cannot hit the shooter
                    return;
                }

                // stun victim for a few seconds
                this.interactionMediator.ShellHitUnicast(
                    playerHitbox.ownerStore.playerApiSafe.GetID(),
                    3.0f
                );

                // play explosion vfx on all game instances
                // and let the shooter despawn the shell
                this.SendCustomNetworkEvent(
                    VRC.Udon.Common.Interfaces.NetworkEventTarget.All,
                    nameof(ExplodeAndDespawn)
                );

            }

        }

    }

    public void ExplodeAndDespawn()
    {

        // disable mesh and collider before explosion takes place
        this.meshRenderer.enabled = false;
        this.shellCollider.enabled = false;

        // stop smoke trail
        this.smoke.Stop();

        // set exploding to true, so position stops updating
        this.exploding = true;

        // detach shell from shooter, if it's not already so
        // this stops the explosion animation of shell from
        // being affected by the shooter's movements
        this.transform.parent = null;

        // start explosion animation
        this.explosion.Play();
        this.debris.Play();

        if (this.localVRMode.IsLocal())
        {

            // wait until all trail and explosion animations to finish
            // before returning shell

            this.doAfterAllParticleSystemsStop.Exec(
                this.returnShellCommand.Init(this),
                this.smoke,
                this.explosion,
                this.debris
            );

        }

    }

    public void ReturnShell()
    {
        this.ShellPool.Return(this.gameObject);
        Debug.Log("shell returned");
    }

    void OnDisable()
    {
        this.launched = false;
        this.exploding = false;
    }

    private int FindVictim()
    {

        PlayerStoreCollection playerStoreCollection = this
            .ownerStore
            .playerStoreCollection;

        int playerCount = playerStoreCollection.GetCount();

        if (playerCount == 1)
        {
            // there is only 1 player, the victim is
            // the shooter themself, aka player 1
            return 1;
        }

        // temporarily set 1st non-shooter as the victim
        int start = 1;
        if (this.ownerStore.playerApiSafe.GetID() == 1)
        {
            // shooter has id 1, check from player 2 onwards.
            start = 2;
        }
        PlayerStore victimStore = playerStoreCollection.GetByID(start);

        Vector3 shooterForward = this
            .ownerStore
            .follower
            .head
            .transform
            .rotation * Vector3.forward;

        Vector3 shooterPos = this.ownerStore.hitbox.transform.position;
        Vector3 candidatePos = victimStore.hitbox.transform.position;
        Vector3 shooterToCandidate = candidatePos - shooterPos;

        bool currentBestFacing = Vector3.Dot(
            shooterForward, 
            shooterToCandidate
        ) > 0;
        float currentBestDistance = shooterToCandidate.magnitude;
        ++start;

        for (int i = start; i <= playerCount; ++i)
        {

            PlayerStore candidateStore = playerStoreCollection.GetByID(i);

            if (candidateStore != this.ownerStore)
            {

                // player being checked isn't shooter of shell
                // (so they can be potential victim)

                candidatePos = candidateStore.hitbox.transform.position;
                shooterToCandidate = candidatePos - shooterPos;

                bool tempFacing = Vector3.Dot(
                    shooterForward,
                    shooterToCandidate
                ) > 0;
                float tempDistance = shooterToCandidate.magnitude;

                if (tempFacing && !currentBestFacing)
                {
                    // player being checked is in front of shooter
                    // current best match is behind shooter
                    // update this player to be the victim and current best match
                    victimStore = candidateStore;
                    currentBestFacing = true;
                    currentBestDistance = tempDistance;
                }
                else if (tempFacing == currentBestFacing)
                {
                    // both players being checked and current best match
                    // are in front or behind the shooter
                    if (tempDistance < currentBestDistance)
                    {
                        // player being checked is closer to shooter
                        // update this player to be the victim and current best match
                        victimStore = candidateStore;
                        currentBestDistance = tempDistance;
                    }
                }

            }

        }

        return victimStore.playerApiSafe.GetID();

    }

}