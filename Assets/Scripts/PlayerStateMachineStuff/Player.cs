using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/// <summary>
/// This is the main player controller script
/// it uses a state machine to swap between the different states of movement
/// 
/// This script initalizes all the states to be public variables that can be accessed by all the states so they can all read each other
/// This script also contains all the componets needed for the state scripts (ex. RigidBody, Collider etc.) as public variables
/// Any given state is only active once at a time
/// Every state has transitions to other states depending on the playerInputHandler script or other checks(ex. check for the ground)
/// 
/// </summary>
public class Player : MonoBehaviour
{
    private List<PlayerAbility> AllAbilities = new List<PlayerAbility>();
    private List<PlayerAbility> AviableAbilities = new List<PlayerAbility>();

    public PlayerAbility CurrentAbility;

    [HideInInspector]
    public PlayerAbility NoAbility;
    [HideInInspector]
    public PlayerAbility GrappleAbility;
    [HideInInspector]
    public PlayerAbility Grapple2Ability;
    [HideInInspector]
    public PlayerAbility GrappleInverseAbility;
    [HideInInspector]
    public PlayerAbility GrappleDynamAbility;
    [HideInInspector]
    public PlayerAbility GrappleSpringAbility;


    //These are all the state scripts that the player can switch between
    public PlayerStateMachine PSM { get; private set; }
    public PlayerGroundedState groundedState { get; private set; }
    public PlayerUseAbilityState useAbilityState { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMovingState movingState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerCrouchIdleState crouchIdleState { get; private set; }
    public PlayerCrouchMovingState crouchMoveState { get; private set; }
    public PlayerSlidingState slidingState { get; private set; }
    public PlayerSprintingState sprintingState { get; private set; }

    public PlayerInAirState inAirState { get; private set; }
    public PlayerLandedState landedState { get; private set; }
    public PlayerInAirSlideState airSlideState { get; private set; }

    public PlayerGrapplingState grapplingState { get; private set; }
    public BasicSwingGrappleState grappleBState { get; private set; }
    public Grapple2PointState grapple2State { get; private set; }
    public GrappleInvertedState inverseGrapple { get; private set; }
    public GrappleSpringState springGrapple { get; private set; }
    public GrappleDynamicSwingState dynamicGrapple { get; private set; }


    public PlayerInputHandler inputHandler { get; private set; }
    [Header("Put custom player data here")]
    [Tooltip("holds all the data of the player like speed, health, and whatever")]
    public PlayerData playerData;


    [HideInInspector]
    public Rigidbody2D rb;
    [Tooltip("This needs to be the same capsule collider as on this player object")]
    public CapsuleCollider2D cc;
    [HideInInspector]
    public DistanceJoint2D dj;
    [HideInInspector]
    public GameObject hand;
    [HideInInspector]
    public LineRenderer lr;
    [HideInInspector]
    public HingeJoint2D hj;
    [HideInInspector]
    public SpringJoint2D spr;

    [HideInInspector]
    public AudioSource audioS;
    [HideInInspector]
    public AudioSource HandAudioS;
    [HideInInspector]
    public AudioSource DamageAudioS;


    [HideInInspector]
    public Animator anim;


    [HideInInspector]
    public AudioClip CurrentClip;


    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isOnSlope;

    private int abilityIterator = 1;

    private Transform respawnPoint;

    RaycastHit2D ray;
    RaycastHit2D SlopeRay;


    private void Awake()
    {
        PSM = new PlayerStateMachine();

        groundedState = new PlayerGroundedState(this, playerData, PSM, "null");
        useAbilityState = new PlayerUseAbilityState(this, playerData, PSM, "null");

        idleState = new PlayerIdleState(this, playerData, PSM, "IsIdle");
        movingState = new PlayerMovingState(this, playerData, PSM, "IsWalking");
        jumpState = new PlayerJumpState(this, playerData, PSM, "JumpStart");
        crouchIdleState = new PlayerCrouchIdleState(this, playerData, PSM, "IsCrouchingAnim");
        crouchMoveState = new PlayerCrouchMovingState(this, playerData, PSM, "IsCrouchWalkingAnim");
        slidingState = new PlayerSlidingState(this, playerData, PSM, "IsSlidingAnim");
        sprintingState = new PlayerSprintingState(this, playerData, PSM, "IsSprinting");

        inAirState = new PlayerInAirState(this, playerData, PSM, "InAir");
        landedState = new PlayerLandedState(this, playerData, PSM, "Landed");
        airSlideState = new PlayerInAirSlideState(this, playerData, PSM, "InAirSlideAnim");
        grapplingState = new PlayerGrapplingState(this, playerData, PSM, "IsGrapplingAnim");

        grappleBState = new BasicSwingGrappleState(this, playerData, PSM, "IsGrapplingAnim");
        grapple2State = new Grapple2PointState(this,playerData, PSM, "IsGrapplingAnim"); //replace anims if you want abilties to look different
        inverseGrapple = new GrappleInvertedState(this, playerData, PSM, "IsGrapplingAnim");
        springGrapple = new GrappleSpringState(this, playerData, PSM, "IsGrappleingAnim");
        dynamicGrapple = new GrappleDynamicSwingState(this, playerData, PSM, "IsGrapplingAnim");


        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        dj = GetComponent<DistanceJoint2D>();
        hj = GetComponent<HingeJoint2D>();
        spr = GetComponent<SpringJoint2D>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();

        lr = GetComponentInChildren<LineRenderer>();
        hand = transform.GetChild(0).gameObject;
        DamageAudioS = GetComponentsInChildren<AudioSource>()[1];
        HandAudioS = GetComponentsInChildren<AudioSource>()[2];


        NoAbility = new PlayerAbility(true, false, "NoAbility", "Nothing", "Nothing");
        GrappleAbility = new PlayerAbility(false, false, "Grappling", "HoldingGrapple", "ShootGrapple");
        Grapple2Ability = new PlayerAbility(false, false, "Grapple2", "HoldingGrapple", "ShootGrapple"); //Probably changes these to have unique anims later
        GrappleInverseAbility = new PlayerAbility(false, false, "InverseGrapple", "HoldingGrapple", "ShootGrapple");
        GrappleSpringAbility = new PlayerAbility(false, false, "SpringGrapple", "HoldingGrapple","ShootGrapple");
        GrappleDynamAbility = new PlayerAbility(false, false, "DyanmicGrapple", "HoldingGrapple", "ShootGrapple");

        //foreach(PlayerAbility pa in FindObjectsByType<PlayerAbility>(0)) I feel like theres a way to make this work but I dont know how yet..
        AllAbilities.Add(NoAbility); //there is probably a better way to do this
        AllAbilities.Add(GrappleAbility);
        AllAbilities.Add(Grapple2Ability);
        AllAbilities.Add(GrappleInverseAbility);
        AllAbilities.Add(GrappleDynamAbility);
        AllAbilities.Add(GrappleSpringAbility);

        respawnPoint = transform;
    }


    private void Start()
    {
        PSM.Initialize(idleState); // this starts the state machine by setting the player to the idle state on the start of the game

        //this makes it so all abilities are aviable by default, mainly for testing purposes
        if (playerData.AllAbilitiesUnlocked) {
            foreach(PlayerAbility a in AllAbilities)
            {
                a.SetUnlocked(true);
            }
        }
        NoAbility.SetUnlocked(true);
        CurrentAbility = NoAbility;
    }

    

    private void Update()
    {
        PSM.currentState.Update(); // this is calling the base unity Update method in the current state of the state machine

        isGrounded = IsGrounded();
        isOnSlope = IsOnSlope();

        FlipPlayer();

        AbilityHandling();

        RotateHand();
    }

    private void FixedUpdate()
    {
        PSM.currentState.FixedUpdate(); // this is calling the base unity FixedUpdate method in the current state of the state machine
    }


    #region AbilityStuff
    private void AbilityHandling()
    {
        //Ability Handling ------ might want to switch to another script later not sure yet

        //this checks all the abilites and see which ones are currently unlocked then adds them to the list of aviable abilites
        foreach (var pA in AllAbilities)
        {
            if (pA.isUnlocked && !AviableAbilities.Contains(pA))
            {
                AviableAbilities.Add(pA);
            }
        }

        //each of these check change the current abilty by iterating through the list of unlocked abilities
        // until the iterator is either higher or lower than the bounds of the list of aviable abilites
        // if its higher the iterator is set to 0, if lower its set to the total number of abilities in AviableAbilities
        if (inputHandler.SwitchAbilityDown)
        {
            if (AviableAbilities.Count == 1)
                return;
            if (AviableAbilities.Count > 0)
            {
                HandAudioS.PlayOneShot(playerData.SwitchAbilitySFX);

                CurrentAbility.SetEquiped(false);
                CurrentAbility.ChangeSprite(hand.gameObject);
                if (abilityIterator == 0)
                    abilityIterator = AviableAbilities.Count;

                abilityIterator--;
                CurrentAbility = AviableAbilities[abilityIterator];

                CurrentAbility.SetEquiped(true);
                CurrentAbility.ChangeSprite(hand.gameObject);
                if (playerData.DebugAbilitySwitching)
                    Debug.Log(CurrentAbility.name);
            }
        }

        if (inputHandler.SwitchAbilityUp)
        {
            if (AviableAbilities.Count == 1)
                return;
            if (AviableAbilities.Count > 0)
            {
                HandAudioS.PlayOneShot(playerData.SwitchAbilitySFX);

                CurrentAbility.SetEquiped(false);
                CurrentAbility.ChangeSprite(hand.gameObject);
                abilityIterator++;
                if (abilityIterator == AviableAbilities.Count)
                    abilityIterator = 0;

                CurrentAbility = AviableAbilities[abilityIterator];

                CurrentAbility.SetEquiped(true);
                CurrentAbility.ChangeSprite(hand.gameObject);
                if (playerData.DebugAbilitySwitching)
                    Debug.Log(CurrentAbility.name);
            }
        }

    }

    #endregion

    #region Rotate and Ground Stuff
    private void RotateHand()
    {

        //simply rotates the gun or whatever is being held so it lines up with player's aim
        Vector3 AngleVector = inputHandler.mouseScreenPos - hand.transform.position;
        float angle = Mathf.Atan2(AngleVector.y, AngleVector.x) * Mathf.Rad2Deg;
        hand.transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
       
    }

    private void FlipPlayer() // self explanitory
    {
        if (inputHandler.moveDir.x == -1)
        {
            playerData.isFacingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
            hand.GetComponent<SpriteRenderer>().flipX = false;
            hand.GetComponent<SpriteRenderer>().flipY = false;
            hand.transform.GetChild(0).localPosition = new Vector3(1, 0, 0);
            if (hand.transform.GetChild(1))
            {
                hand.transform.GetChild(1).GetComponent<SpriteRenderer>().flipY = true;
                hand.transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = true;
                hand.transform.GetChild(1).localPosition = new Vector3(7.5f, 0, 0);
            }
                
        }
        else if (inputHandler.moveDir.x == 1)
        {
            playerData.isFacingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
            hand.GetComponent<SpriteRenderer>().flipX = true;
            hand.GetComponent<SpriteRenderer>().flipY = true;
            hand.transform.GetChild(0).localPosition = new Vector3(-1, 0, 0);
            if (hand.transform.GetChild(1))
            {
                hand.transform.GetChild(1).GetComponent<SpriteRenderer>().flipY = false;
                hand.transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = false;
                hand.transform.GetChild(1).localPosition = new Vector3(-7.5f, 0, 0);
            } 
        }
    }

    [SerializeField]
    Vector3 GroundCheckFixer = new Vector3(.3f, .2f, 0f);
    [SerializeField]
    Vector3 GroundCheckOffset = new Vector3(0, .5f, 0);

    //using a raycast box to check for the ground below
    private bool IsGrounded()
    {
        GroundCheckOffset = new Vector3(GroundCheckOffset.x, cc.bounds.size.y / 2, GroundCheckOffset.z);
        GroundCheckFixer = new Vector3(GroundCheckFixer.x, cc.size.y / 2 + .5f, GroundCheckFixer.z);
        ray = Physics2D.BoxCast(cc.bounds.center - GroundCheckOffset, cc.bounds.size - GroundCheckFixer, 0, Vector2.down, 0.1f, playerData.GroundLayer);
        return ray.collider != null;

    }

    private bool IsOnSlope()
    {
        SlopeRay = Physics2D.Raycast(cc.bounds.center - GroundCheckOffset, Vector2.down, 4.5f, playerData.GroundLayer);

        if (SlopeRay.collider != null)
        {
            float angle = Mathf.Abs(Mathf.Atan2(SlopeRay.normal.x, SlopeRay.normal.y) * Mathf.Rad2Deg);
            if (angle >= 30 || angle <= -30)
            {
                return true;
            }
        }

        return false;
    }

    //For Debugging Drawing for Grounded/Slope check
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (IsGrounded())
        {

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(cc.bounds.center - GroundCheckOffset + -transform.up * ray.distance, cc.bounds.size - GroundCheckFixer);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(cc.bounds.center - GroundCheckOffset + -transform.up * 0.1f, cc.bounds.size - GroundCheckFixer);
        }

        if (isOnSlope)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(cc.bounds.center, Vector2.down * SlopeRay.distance);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(cc.bounds.center, Vector2.down * 4.5f);
        }

    }
    #endregion

    #region AudioStuff

    bool shouldAudioStop;
    public void PlayAudioFile(AudioClip tempClip, bool loop)
    {
        shouldAudioStop = false;
        audioS.volume = 0.5f;
        audioS.pitch = 1;
        audioS.loop = loop;
        audioS.PlayOneShot(tempClip);
    }
    public void PlayAudioFile(AudioClip tempClip, bool loop, float minPitch, float maxPitch, float minVol, float maxVol)
    {
        shouldAudioStop = false;
        audioS.pitch = Random.Range(minPitch, maxPitch);
        audioS.volume = Random.Range(minVol, maxVol);
        audioS.loop = loop;
        audioS.PlayOneShot(tempClip);
    }

    public void StopAudioFile(AudioClip tempClip)
    {
        shouldAudioStop = true;
        if (audioS.isPlaying)
        {
            StartCoroutine(AudioCutOff(tempClip));
        }
        if (shouldAudioStop)
            audioS.Stop();
    }
    public void AbiltySoundEffect(AudioClip tempClip)
    {
        HandAudioS.PlayOneShot(tempClip);
    }

    IEnumerator AudioCutOff(AudioClip tempClip)
    {
        if (audioS.clip = tempClip)
        {
            for (int i = 50; i > 0; i--)
            {
                if (shouldAudioStop)
                {
                    yield return new WaitForSeconds(0.05f);
                    audioS.volume -= 0.05f;
                }
                else
                {
                    yield return null;
                    audioS.volume = 0.5f;
                }
            }
        }
        yield return null;
    }

    public void StopAllAudio(float time)
    {
        StartCoroutine(AudioPauseForSecs(time));
    }

     IEnumerator AudioPauseForSecs(float time)
    {
        audioS.mute = true;
        HandAudioS.mute = true;
        DamageAudioS.mute = true;
        yield return new WaitForSeconds(time);
        audioS.mute = false;
        HandAudioS.mute = false;
        DamageAudioS.mute = false;
        
    }

    #endregion#region Events to be Called

    #region Events to be Called

    [HideInInspector]
    public bool immuneFrames = false;
    public void recieveDamage()
    {
        if (immuneFrames) return;

        DamageAudioS.PlayOneShot(playerData.HitSFX);
        if (playerData.health - 1 != 0)
        {
            StartCoroutine(IFrames());
            playerData.health = playerData.health - 1;
        }
        else
        {
            immuneFrames = true;
        }
    }
   
    IEnumerator IFrames()
    {
        yield return new WaitForSeconds(0.0001f);
        immuneFrames = true;
        for (int i = 0; i <= playerData.immuneFrameTime; i++)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.0001f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.0001f);
        }
        immuneFrames = false;
    }
    /// <summary>
    /// removes player input and sound effects for a peroid of time
    /// will be used for when a cutscene starts / when one of the pickup popups are on screen
    /// </summary>
    /// <param name="time"></param>
    public void RemoveInputAndAudio(float time)
    {
        rb.velocity = new Vector2(0, 0);
        StopAllAudio(time);
        RemoveInput(time);
    }

    public void RemoveInput(float time)
    {
        rb.velocity = new Vector2(0, 0);
        StartCoroutine(StopAllInputs(time));
    }
    IEnumerator StopAllInputs(float time)
    {
        GetComponent<PlayerInputHandler>().setAllToZero();
        GetComponent<PlayerInputHandler>().enabled = false;
        PSM.ChangeState(idleState);
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(time);
        GetComponent<PlayerInputHandler>().enabled = true;
    }

    
    public void recieveHealth()
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().playerData.health = GameObject.FindWithTag("Player").GetComponent<Player>().playerData.health + 1;
    }

       public void HotSwapAbilty(string abilName) 
       {
        if (AllAbilities == null) return;
        if (!playerData.AllAbilitiesUnlocked) return;// right now this only works with all abilities unlocked, will probably change later to depend on unlocks

        switch (abilName)
        {
            case "Grapple1":
                CurrentAbility = GrappleAbility;
                abilityIterator = AviableAbilities.IndexOf(GrappleAbility);
                break;
            case "Grapple2":
                CurrentAbility = Grapple2Ability;
                abilityIterator = AviableAbilities.IndexOf(Grapple2Ability);
                break;
            case "AntiGrapple":
                CurrentAbility = GrappleInverseAbility;
                abilityIterator = AviableAbilities.IndexOf(GrappleInverseAbility);
                break;
            case "Spring Grapple":
                CurrentAbility = GrappleSpringAbility;
                abilityIterator = AviableAbilities.IndexOf(GrappleSpringAbility);
                break;
            case "Dynamic Grapple":
                CurrentAbility = GrappleDynamAbility;
                abilityIterator = AviableAbilities.IndexOf(GrappleDynamAbility);
                break;
            case "None":
                CurrentAbility = NoAbility;
                abilityIterator = 0;
                break;
            default:
                CurrentAbility = NoAbility;
                abilityIterator = 0;
                break;
        }

       }

        public void AbilityUnlock(string abilityName)
        {
            switch (abilityName)
            {
                case "None":
                    NoAbility.SetUnlocked(true);
                    break;
                case "Grapple":
                    GrappleAbility.SetUnlocked(true);
                    break;
                case "Graple2":
                    Grapple2Ability.SetUnlocked(true);
                    break;
                case "InvereseGrapple":
                    GrappleInverseAbility.SetUnlocked(true);
                    break;
                case "GrappleDynamAbility":
                    GrappleDynamAbility.SetUnlocked(true);
                    break;
            }
        }


        public void SetRespawnPoint(Transform spawnP)
        {
            respawnPoint = spawnP;
        }

        public void RespawnPlayerV()
        {
            gameObject.transform.position = respawnPoint.position;
            rb.velocity = new Vector2(0, 0);
            PSM.ChangeState(idleState);
        }
        public UnityAction RespawnPlayer()
        {
            gameObject.transform.position = respawnPoint.position;
            rb.velocity = new Vector2(0, 0);
            PSM.ChangeState(idleState);
            return new UnityAction(RespawnPlayerV);
        }

    #endregion
}
