using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class WaterSprayParticleBehaviour : UdonSharpBehaviour
{

    [SerializeField]
    private WaterSprayParticleProperties properties;
    [SerializeField]
    private VRCObjectPool waterSprayParticlePool;

    [SerializeField]
    private PlayerStore ownerStore;
    private VRCPlayerApi owner = null;
    private LocalVRMode localVRMode;

    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private MeshRenderer mesh;

    private bool launched = false;
    private float timeBeforeDespawn;

    public void InitTransform()
    {

        // // set particle position to tip of gun barrel
        // this.transform.position = this
        //     .gunBarrelTip
        //     .position;
        // // this.transform.position = this
        // //     .gunBarrelTip
        // //     .position;

        // // scale particle randomly, within scale range
        // float scaleFactor = Random.Range(
        //     this.properties.ParticleScaleMin, 
        //     this.properties.ParticleScaleMax
        // );
        // this.transform.localScale = Vector3.one * scaleFactor;

        // // rotate particle randomly
        // this.transform.rotation = Random.rotation;

    }

    public void Launch(Vector3 shooterPos, Vector3 gunTip, Vector3 velocity)
    {
        this.LaunchBroadcast(shooterPos, gunTip, velocity);
    }

    public void LaunchBroadcast(Vector3 shooterPos, Vector3 gunTip, Vector3 velocity)
    {
        this.LaunchBroadcastSyncString = string.Join(
            " ",
            System.Guid.NewGuid().ToString().Substring(0, 6),
            shooterPos.x, shooterPos.y, shooterPos.z,
            gunTip.x, gunTip.y, gunTip.z,
            velocity.x, velocity.y, velocity.z
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
            string[] args = value.Split(' ');
            string nonce = args[0];
            float shooterPosX = float.Parse(args[1]);
            float shooterPosY = float.Parse(args[2]);
            float shooterPosZ = float.Parse(args[3]);
            float gunTipX = float.Parse(args[4]);
            float gunTipY = float.Parse(args[5]);
            float gunTipZ = float.Parse(args[6]);
            float velocityX = float.Parse(args[7]);
            float velocityY = float.Parse(args[8]);
            float velocityZ = float.Parse(args[9]);
            this.LaunchLocal(
                new Vector3(shooterPosX, shooterPosY, shooterPosZ),
                new Vector3(gunTipX, gunTipY, gunTipZ),
                new Vector3(velocityX, velocityY, velocityZ)
            );
        }
    }

    private void LaunchLocal(Vector3 shooterPos, Vector3 gunTip, Vector3 velocity)
    {

        if (this.owner == null)
        {
            this.owner = this.ownerStore.playerApiSafe.Get();
        }

        Vector3 newShooterPos = this.owner.GetTrackingData(
            VRCPlayerApi.TrackingDataType.Origin
        ).position;

        // corrected guntip position based on new and old shooter positions
        gunTip += newShooterPos - shooterPos;

        if (this.transform.parent != null)
        {
            // detach the particle from player follower
            // so that the particle's movements are not affected
            // by player's movements
            this.transform.parent = null;
        }

        // teleport to corrected gunTip position
        this.transform.position = gunTip;

        // enable particle mesh
        this.mesh.enabled = true;

        // give it a random rotation
        this.transform.rotation = Random.rotation;

        // actually launch the particle
        this.rb.velocity = velocity;

        Debug.Log($"velocity: {this.rb.velocity}");

        // mark as launch
        this.launched = true;

        // set timeBeforeDespawn to predefined lifetime
        this.timeBeforeDespawn = this
            .properties
            .ParticleLifeTime;

    }

    void OnEnable()
    {

        if (this.owner == null)
        {
            this.owner = this.ownerStore.playerApiSafe.Get();
        }
        this.localVRMode = this.ownerStore.localVRMode;

        if (this.localVRMode.IsLocal())
        {

            // spread offsets
            float xOffset = Random.Range(
                -this.properties.SpraySpread, 
                this.properties.SpraySpread
            );
            float yOffset = Random.Range(
                -this.properties.SpraySpread, 
                this.properties.SpraySpread
            );

            // calculation of offsetVector
            // explanation:
            // imagine there is a target at which the gun is pointing
            // imagine there is such a vector extending from the gun tip to the target at which the gun is pointing
            // imagine there is a plane normal to such vector
            // we call such plane.. the "normal plane"
            // now, originally Vector3(xOffset, yOffset, 0.0f) lies on the xy-plane globally
            // the statement below simply rotates this vector3 to lie on the "normal plane"
            // this gives the result of offsetVector
            Vector3 offsetVector = this
                .waterSprayParticlePool
                .transform
                .rotation * new Vector3(xOffset, yOffset, 0.0f);

            // spread-corrected shootdirection
            Vector3 shootDirection = this
                .waterSprayParticlePool
                .transform
                .forward + offsetVector;
            shootDirection = shootDirection.normalized;

            // calculate launchVel based on 
            // shooter's current velocity along trajectory
            Vector3 launchVel = 
                shootDirection * this.properties.ParticleTravelSpeed;
            launchVel += Vector3.Project(
                this.owner.GetVelocity(),
                this.waterSprayParticlePool.transform.forward
            );

            VRCPlayerApi.TrackingData tt = this.owner.GetTrackingData(
                VRCPlayerApi.TrackingDataType.Origin
            );

            this.Launch(
                tt.position,
                this.waterSprayParticlePool.transform.position,
                launchVel
            );

        }

    }

    void FixedUpdate()
    {

        if (!this.launched) return;

        if (this.timeBeforeDespawn > 0.0f)
        {
            this.timeBeforeDespawn -= Time.fixedDeltaTime;
        }

    }

    void Update()
    {

        if (!this.launched) return;

        if (this.localVRMode.IsLocal())
        {
            if (this.timeBeforeDespawn <= 0.0f)
            {
                // time exceeds lifetime
                // return this particle to pool
                this.waterSprayParticlePool.Return(this.gameObject);
                return;
            }
        }

        // particle color changes over time
        float timeElapsedSinceLaunched = this
            .properties
            .ParticleLifeTime - this
                .timeBeforeDespawn;
        Color particleNewColor = this
            .properties
            .ColorGradientOverLifetime
            .Evaluate(
                timeElapsedSinceLaunched / this.properties.ParticleLifeTime
            );
        this.mesh.material.color = particleNewColor;

    }

    private void OnTriggerEnter(Collider collider)
    {

        if (!this.launched) return;

        if (this.localVRMode.IsLocal())
        {

            PlayerHitbox playerHitbox = collider.GetComponent<PlayerHitbox>();

            if (playerHitbox != null)
            {

                if (playerHitbox.ownerStore.localVRMode.IsLocal())
                {
                    // spray particle cannot hit the shooter
                    Debug.Log("spray particle cannot hit the shooter");
                    return;
                }
                else
                {
                    int shooterID = this.ownerStore.playerApiSafe.GetID();
                    int victimID = playerHitbox
                        .ownerStore
                        .playerApiSafe
                        .GetID();
                    Debug.Log($"player {shooterID} shot {victimID} with {this.gameObject.name}");
                }

            }
        }
    }

    void OnDisable()
    {

        this.launched = false;
        this.rb.velocity = Vector3.zero;

        // sets mesh opacity to zero
        this.mesh.material.color = this
            .properties
            .ColorGradientOverLifetime
            .Evaluate(0.0f);

        this.mesh.enabled = false;

    }

}