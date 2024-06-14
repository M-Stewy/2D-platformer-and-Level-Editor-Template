using Unity.VisualScripting;
using UnityEngine;

public class Grapple2PointState : PlayerState
{
    public Grapple2PointState(Player player, PlayerData playerData, PlayerStateMachine playerStateMachine, string playerAnim) : base(player, playerData, playerStateMachine, playerAnim)
    {
    }
    LayerMask GrapHitted;
    Vector2 direction;
    float currentDis;
    bool missedGrap;
    bool startGrap;
    GameObject graple;
    public override void Checks()
    {
        base.Checks();
    }

    public override void Enter()
    {
        missedGrap = false;
        startGrap = false;
        
        base.Enter();
        ShootSwingPoint();
    }

    public override void Exit()
    {
        base.Exit();
        DestoryGrapPoints();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (startGrap && (currentDis > playerData.Grap2MaxDis))
        {
            if (player.rb.velocity.magnitude < playerData.Grap2MaxSpeed)
                player.rb.AddForce((graple.transform.position - player.transform.position) * playerData.Graple2Speed, ForceMode2D.Impulse);
        }
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
        if(startGrap)
        {
            player.lr.SetPosition(0, player.hand.transform.position);
            currentDis = (graple.transform.position - player.transform.position).magnitude;
        //    Debug.Log("Current Distance to point = " + currentDis);
            if (currentDis > playerData.Grap2MaxDis)
            {
                player.rb.AddForce((graple.transform.position - player.transform.position) * playerData.Graple2Speed);
            }
            if (currentDis <= playerData.Grap2MaxDis)
            {
                playerStateMachine.ChangeState(player.inAirState);
            }
        }
        
    }

    private void ShootSwingPoint()
    {
        player.AbiltySoundEffect(playerData.ShootGrappleSFX);
        direction = player.inputHandler.mouseScreenPos - player.transform.position;
        RaycastHit2D rayHit = Physics2D.Raycast(player.transform.position, direction, playerData.Grap2Dis, playerData.LaymaskGrapple2);
        if (rayHit.collider != null)
        {
            GrapHitted = Mathf.CeilToInt(Mathf.Pow(2, rayHit.collider.gameObject.layer)); //there should be a better way of doing this...
            if (GrapHitted == playerData.LaymaskGrapple2)
            {
                DestoryGrapPoints();
                missedGrap = false;
                player.rb.velocity = Vector2.zero;
                CreateGrapPoint(rayHit.point, rayHit.transform);
            }
            else
            {
                Debug.Log(GrapHitted.value + "  was not " + playerData.LaymaskGrapple2.value);
                DestoryGrapPoints();
                missedGrap = true;
            }
        }
        else
        {
            Debug.Log("??: " + rayHit.IsUnityNull());
            DestoryGrapPoints();
            missedGrap = true;
            player.AbiltySoundEffect(playerData.MissGrappleSFX);
        }

    }

    private void CreateGrapPoint(Vector2 point, Transform obj)
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
        graple.transform.SetParent(obj);

        startGrap = true;

        ConnectLineToPoint(graple);
    }

    private void ConnectLineToPoint(GameObject graple)
    {
        player.lr.enabled = true;
        player.lr.SetPosition(1, graple.transform.position);
    }


    private void DestoryGrapPoints()
    {
        if (GameObject.FindGameObjectWithTag("GraplePoint"))
            Object.Destroy(GameObject.FindGameObjectWithTag("GraplePoint"));

        player.dj.enabled = false;
        player.lr.enabled = false;

    }

}
