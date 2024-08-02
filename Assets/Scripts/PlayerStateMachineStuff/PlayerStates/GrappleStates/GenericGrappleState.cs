using UnityEngine;

public class GenericGrappleState : PlayerState
{
    public GenericGrappleState(Player player, PlayerData playerData, PlayerStateMachine playerStateMachine, string playerAnim) : base(player, playerData, playerStateMachine, playerAnim)
    {
    }
    protected GameObject graple;
    Vector2 direction;

    protected bool missedGrap;
    protected bool startGrap;
    public override void Checks()
    {
        base.Checks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        DestoryGrapPoints();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (player.inputHandler.PressedJump)
        {
            playerStateMachine.ChangeState(player.jumpState);
        }
        if (player.inputHandler.PressedAbility2)
        {
            playerStateMachine.ChangeState(player.inAirState);
        }
        if (player.inputHandler.PressedAbility1)
        {
            playerStateMachine.ChangeState(player.useAbilityState);
        }
        if (missedGrap)
        {
            playerStateMachine.ChangeState(player.inAirState);
        }
    }

    public virtual void ShootSwingPoint(LayerMask GrapHitLay,float thisGrapDist,LayerMask thisGrapLayer, bool useDJ)
    {
        player.AbiltySoundEffect(playerData.ShootGrappleSFX);
        direction = player.inputHandler.mouseScreenPos - player.transform.position;
        RaycastHit2D rayHit = Physics2D.Raycast(player.transform.position, direction, thisGrapDist, thisGrapLayer);
        Debug.DrawRay(player.transform.position, direction * thisGrapDist, Color.yellow);
        if (rayHit.collider != null)
        {
            GrapHitLay = Mathf.CeilToInt(Mathf.Pow(2, rayHit.collider.gameObject.layer)); //there should be a better way of doing this...
            if (GrapHitLay == thisGrapLayer)
            {
                DestoryGrapPoints();
                missedGrap = false;
                CreateGrapPoint(rayHit.point, rayHit.transform, useDJ);
            }
            else
            {
                Debug.Log(GrapHitLay.value + "  was not " + thisGrapLayer.value);
                DestoryGrapPoints();
                missedGrap = true;
            }
        }
        else
        {
        //    Debug.Log(":??: " + rayHit.IsUnityNull());
            DestoryGrapPoints();
            missedGrap = true;
            player.AbiltySoundEffect(playerData.MissGrappleSFX);
        }

    }

    public virtual void CreateGrapPoint(Vector2 point, Transform parentOBJ, bool useDJ)
    {
        player.AbiltySoundEffect(playerData.HitGrappleSFX);

        graple = new GameObject("GrapplePoint");
        graple.tag = "GraplePoint";
        graple.AddComponent<SpriteRenderer>();
        graple.GetComponent<SpriteRenderer>().sprite = playerData.GrapplePointSprite;
        graple.GetComponent<SpriteRenderer>().sortingOrder = 15;
        graple.AddComponent<CircleCollider2D>();
        graple.GetComponent<CircleCollider2D>().isTrigger = true;
        graple.GetComponent<CircleCollider2D>().radius = .5f;
        graple.transform.position = point;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        graple.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        graple.transform.SetParent(parentOBJ);

        startGrap = true;
        ConnectPlayerToPoint(graple, useDJ);   
    }

    public virtual void DestoryGrapPoints()
    {
        if (GameObject.FindGameObjectWithTag("GraplePoint"))
            Object.Destroy(GameObject.FindGameObjectWithTag("GraplePoint"));
        
        player.dj.enabled = false;
        player.lr.enabled = false;
    }

    public virtual void ConnectPlayerToPoint(GameObject point, bool useDJ)
    {
        if (GameObject.FindGameObjectWithTag("GraplePoint"))
        {
            if(useDJ)
            { 
                player.dj.distance = Vector2.Distance(player.transform.position, point.transform.position);
                player.dj.enabled = true;
                player.dj.connectedAnchor = point.transform.position;
            }
            player.lr.enabled = true;
            player.lr.SetPosition(1, point.transform.position);

        }
        else
        {
            if(useDJ) player.dj.enabled = false;
            player.lr.enabled = false;
        }
    }

}
