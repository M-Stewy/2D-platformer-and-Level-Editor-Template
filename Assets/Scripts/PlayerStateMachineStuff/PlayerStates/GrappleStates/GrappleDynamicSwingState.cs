using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Windows;

public class GrappleDynamicSwingState : GenericGrappleState
{
    public GrappleDynamicSwingState(Player player, PlayerData playerData, PlayerStateMachine playerStateMachine, string playerAnim) : base(player, playerData, playerStateMachine, playerAnim)
    {
    }

    float xInput;


    protected List<Rigidbody2D> allJoints = new List<Rigidbody2D>();
    protected GameObject hingePoint;

    public override void Checks()
    {
        base.Checks();
        xInput = player.inputHandler.moveDir.x;
    }

    public override void Enter()
    {
        base.Enter();
        ShootSwingPoint(playerData.DynamGrappleDis, playerData.LaymaskGrapple, false);
        player.rb.gravityScale = playerData.DynamGrappePGrav;
        player.rb.drag = playerData.DynamGrappPDrag;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.rb.AddForce(new Vector2(xInput * player.playerData.DynamGrappleSpeed, 0));
    }
    public override void Update()
    {
        base.Update();

    }

    public override void ShootSwingPoint(bool Custom = true)
    {
        Debug.Log("Entered Dynamic Swing State");
        DestoryGrapPoints();
    }
    public override void ShootSwingPoint(float thisGrapDist, LayerMask thisGrapLayer, bool useDJ)
    {
        base.ShootSwingPoint(thisGrapDist, thisGrapLayer, useDJ);
        base.stopGrap = true;

    }

    public override void ConnectPlayerToPoint(GameObject point, bool useDJ)
    {
        CreateRopePoints();
    }


    public override void CreateGrapPoint(Vector2 point, Transform parentOBJ, bool useDJ)
    {
        base.CreateGrapPoint(point, parentOBJ, useDJ);
        Debug.Log("this doesnt get called does it?");
    }

    public override void DestoryGrapPoints()
    {
        base.DestoryGrapPoints();
        DestoryRopePoints();
    }


    void CreateRopePoints()
    {
        Rigidbody2D Hrb;
        HingeJoint2D Hj;
        CircleCollider2D Hcc;
        Vector3 VStore;
        Vector3 playerDir = (graple.transform.position - player.transform.position).normalized;
        float PtHjDis = Vector3.Distance(graple.transform.position, player.transform.position);
        int PHDist = Mathf.CeilToInt(PtHjDis)*2;
        for(int i = 0; i < PHDist; i++)
        {

            hingePoint = new GameObject("Hinge" + i);
            Hrb = hingePoint.AddComponent<Rigidbody2D>();
            Hj = hingePoint.AddComponent<HingeJoint2D>();
            Hcc = hingePoint.AddComponent<CircleCollider2D>();
            Hrb.mass = playerData.DynamGrappleRopeMass;
            Hrb.gravityScale = playerData.DynamGrappleRopeGrav;
            Hrb.drag = playerData.DynamGrappleRopeDrag;
            SpriteRenderer renderer = hingePoint.AddComponent<SpriteRenderer>();
            renderer.sprite = playerData.GrapplePointSprite;
            if (i == 0)
            {
                hingePoint.transform.position = graple.transform.position;
            }
            else
            {
                hingePoint.transform.position = allJoints[i-1].transform.position - 0.5f * playerDir;
                Hj.connectedBody = allJoints[i - 1];
            }
            
            allJoints.Add( Hrb );
        }
        VStore = player.rb.velocity;
        Debug.Log(VStore);
        player.hj.enabled = true;
        player.hj.connectedBody = allJoints[PHDist-1];
        player.rb.AddForce(VStore,ForceMode2D.Impulse);
        //player.rb.velocity = VStore;    
        Debug.Log(player.rb.velocity);
    }

    void DestoryRopePoints()
    {
        foreach(Rigidbody2D hp in allJoints)
        {
            Object.Destroy( hp.gameObject );
        }
        allJoints.Clear();
    }

}
