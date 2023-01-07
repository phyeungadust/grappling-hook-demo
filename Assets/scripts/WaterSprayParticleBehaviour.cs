using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class WaterSprayParticleBehaviour : UdonSharpBehaviour
{

    [SerializeField]
    private WaterSprayParticleProperties properties;
    [SerializeField]
    private Transform waterSprayGunTransform; 
    [SerializeField]
    private VRCObjectPool waterSprayParticlePool;
    [SerializeField]
    private SprayedOverlayBehaviour sprayedOverlay;

    private float timeBeforeDespawn;
    private VRCPlayerApi localPlayer;
    private int ownerID;

    public void InitTransform()
    {

        // set particle position to tip of gun barrel
        this.transform.position = this
            .waterSprayGunTransform
            .position;
        // this.transform.position = this
        //     .waterSprayGunTransform
        //     .position;

        // scale particle randomly, within scale range
        float scaleFactor = Random.Range(
            this.properties.ParticleScaleMin, 
            this.properties.ParticleScaleMax
        );
        this.transform.localScale = Vector3.one * scaleFactor;

        // rotate particle randomly
        this.transform.rotation = Random.rotation;

    }

    void OnEnable()
    {

        Debug.Log("spawned " + this.gameObject.name);

        this.localPlayer = Networking.LocalPlayer;
        this.ownerID = this.properties.OwnerID;
        
        this.SendCustomEventDelayedFrames("InitTransform", 0);

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
            .waterSprayGunTransform
            .rotation * new Vector3(xOffset, yOffset, 0.0f);

        // spread-corrected shootdirection
        Vector3 shootDirection = this
            .waterSprayGunTransform
            .forward + offsetVector;
        shootDirection = shootDirection.normalized;

        // launch particle
        this.GetComponent<Rigidbody>().velocity = 
            shootDirection * this.properties.ParticleTravelSpeed;

        // set timeBeforeDespawn to predefined lifetime
        this.timeBeforeDespawn = this
            .properties
            .ParticleLifeTime;

    }

    

    void FixedUpdate()
    {
        if (this.timeBeforeDespawn > 0.0f)
        {
            this.timeBeforeDespawn -= Time.fixedDeltaTime;
        }
    }

    void Update()
    {

        // Debug.Log("particle pos: " + this.transform.position);

        if (this.localPlayer.playerId == this.ownerID)
        {
            if (this.timeBeforeDespawn <= 0.0f)
            {
                // time exceeds lifetime
                // return this particle to pool
                this
                    .waterSprayParticlePool
                    .Return(this.gameObject);
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
        this
            .GetComponent<MeshRenderer>()
            .material
            .color = particleNewColor;

        if (Input.GetKeyDown(KeyCode.Y))
        {
            this.transform.position = this
                .waterSprayGunTransform
                .position;
            this.timeBeforeDespawn = this
                .properties
                .ParticleLifeTime;
        }

    }

    void OnDisable()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("despawned " + this.gameObject.name);
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {

        // ignore if a non shooter instance 'witnesses' a particle hit
        // only the shooter is allowed to register particle hits
        if (this.localPlayer.playerId != this.ownerID) return;

        // ignore if shooter shoots themself
        if (player.playerId == this.ownerID) return;

        Networking.SetOwner(
            this.localPlayer,
            this.sprayedOverlay.gameObject
        );

        this.sprayedOverlay.SprayScreenUnicast = string.Join(
            " ",
            System.Guid.NewGuid().ToString().Substring(0, 6),
            player.playerId
        );

    }

}