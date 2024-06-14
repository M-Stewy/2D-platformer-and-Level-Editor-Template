﻿using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Made by Stewy 
///     Edited by: 
///     
///  This Sript holds all the information about the player
///     such as their move speeds,  gravity values, current ability, health etc.
///  Because it is a scriptableObject it can be easily modified by other scripts without needing to
///     get refernces to the gameObject with the script and then the componet of the script itself
///  
/// </summary>

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Player Data")]
public class PlayerData : ScriptableObject
{
    [HideInInspector]
    public bool isFacingRight;

    //This is the ground layer Defined in the inspector
    [Tooltip("Ground Layer(any layers you want the player to be able to jump on)")]
    public LayerMask GroundLayer;

    [Space(5)]
    [Header("Health Stuff")]
    public float health;
    public float maxHealth = 1;
    public float immuneFrameTime = 160;

    [Space(5)]
    [Header("Move Speed")]
    public float baseMoveSpeed = 30f;
    public float SprintSpeed = 60f;
    public float CrouchSpeed = 20f;
    public float SlideSpeedBoost = 5f;
    

    [Space]
    [Header("Jump stuff")]
    public float JumpPower = 25f;
    public float JumpTime = 75f;
    public int TotalJumps = 1;
    public float CyoteTime = 10;

    [Space(5)]
    [Header("Drag Values")]

    public float GroundDrag = 1f;
    public float SlopeDrag = 0.0001f;
    public float AirDrag = 0.75f;
    public float SlideDrag = 0.001f;
    public float IdleDrag = 5f;
    public float SlowFallDrag = 3;

    [Space]
    [Header("Gravity Values")]
    public float GroundGravity = 1f;
    public float SlopeGravity = 3f;
    public float AirGravity = 5f;
    public float SlowFallGravity;

    [Space(5)]

    [Header("Ability Things and Stuff Yea")]
    [Space]

    public bool AllAbilitiesUnlocked;
    public bool DebugAbilitySwitching;

    [Space]
    [Header("Grapple Stuff")]
    public Sprite GrapplePointSprite;
    [Tooltip("Uncheck Ground if you dont want player to be able to grapple to the ground")]
    public LayerMask LaymaskGrapple;
    public float GrappleReelSpeed = 5f; //This gets weird at higher values need to firgure out why
    public float GrappleSwingSpeed = 20f;
    public float GrappleDistance = 25f;
    [Space]

    [Header("Gun Stuff")]
    [Tooltip("Bullet the player shoots out of the gun")]
    public GameObject playerBullet;
    public float gunForce;
    public int MaxShots = 7;
    public int AmmoLeft;
    public float BulletForce = 50;

    [Space]
    [Header("Grapple Type 2")]
    public LayerMask LaymaskGrapple2;
    public float Graple2Speed;
    public float Grap2Dis;
    public float Grap2MaxDis;
    public float Grap2MaxSpeed;


    [Space(16)]
    [Header("AudioFiles")]

    public int AudioCutoffTime;

    public AudioClip JumpSFX;
    public AudioClip WalkingSFX;
    public AudioClip SlideSFX;
    public AudioClip LandedSFX;
    public AudioClip HitSFX;
    public AudioClip CrouchSFX;
    public AudioClip CrouchWalkSFX;
    public AudioClip AirWooshSFX;

    public AudioClip SwitchAbilitySFX;
    public AudioClip UmbrellaSFX;
    public AudioClip UmbrellaCloseSFX;
    public AudioClip ShootGunSFX;
    public AudioClip EmptyGunSFX;
    public AudioClip ShootGrappleSFX;
    public AudioClip MissGrappleSFX;
    public AudioClip HitGrappleSFX;



    //[HideInInspector] 
    [Space]
    [Header("DO NOT CHANGE THESE FROM DEFAULT, \n" +
        "           (unless we change player size)")]
    public Vector2 CrouchOffset = new Vector2(-0.27f, -0.6f);
    //[HideInInspector]
    public Vector2 CrouchSize = new Vector2(0.85f, 0.65f);
   // [HideInInspector]
    public Vector2 NormalOffset = new Vector2(-0.27f, -0.35f);
    // [HideInInspector]
    public Vector2 NormalSize = new Vector2(0.85f, 1.31f);



    [Header("Debug Stuff")]
    public bool ShowEnterStateInConsole;
}
