using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Made by Stewy
/// 
/// Gets the inputs of the player, thats about it
/// 
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    GameObject ControllerCursor;

    public Vector2 moveDir;
    public Vector2 moveDirRaw;


    public Vector3 mouseScreenPos;
    public Vector3 _mousePos;
    public Vector3 _ControllerPos;
    private Camera _camera;



    public bool HoldingJump;
    public bool holdingCrouch;
    public bool holdingSprint;
    public bool HoldingUp;
    public bool HoldingDown;

    public bool PressedJump;
    public bool PressedCrouch;
    public bool PressedAbility1;
    public bool PressedAbility2;
    public bool HoldingAbility1;

    public bool SwitchAbilityUp;
    public bool SwitchAbilityDown;

    private void Start()
    {
        //_switchAbility = KeyCode.Joystick1Button3; //Both controllers (In theory)
       
        _camera = FindObjectOfType<Camera>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
        moveDirRaw = context.ReadValue<Vector2>();

        if (context.ReadValue<Vector2>().y > 0)
            HoldingUp = true;
        else
            HoldingUp = false;

        if(context.ReadValue<Vector2>().y < 0)
            HoldingDown = true;
        else 
            HoldingDown = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            HoldingJump = true;
            PressedJump = true;
            StopPress();
        }
        else if (context.canceled)
        {
            HoldingJump = false;
            PressedJump = false;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PressedCrouch = true;
            holdingCrouch = true;
        } 
        else if (context.canceled)
        {
            PressedCrouch= false;
            holdingCrouch = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            holdingSprint = true;
        } else if (context.canceled)
        {
            holdingSprint = false;
        }
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PressedAbility1 = true;
            StartCoroutine(StopPress());
        }
        if (context.action.WasReleasedThisFrame() )
        {
            PressedAbility1 = false;
            HoldingAbility1 = false;
        }
        if (context.action.IsPressed()) 
            HoldingAbility1 = true;
        else
            HoldingAbility1 = false;
    }
    private IEnumerator StopPress()
    {
        yield return new WaitForEndOfFrame();
        PressedAbility1 = false;
        PressedJump = false;
    }

    void Update()
    {
    
            Cursor.visible = true;
    //        ControllerCursor.SetActive(false);
    //        HoldingAbility1 = checkForKeyPress(_ability1, _ability1Controller);

            _mousePos = Input.mousePosition;
            _mousePos.z = 100;
            mouseScreenPos = _camera.ScreenToWorldPoint(_mousePos);


            if (Input.mouseScrollDelta.y == 0)
            {
                    SwitchAbilityUp = false;
                    SwitchAbilityDown = false;
            }
            else
            {
                if (Input.mouseScrollDelta.y > 0)
                    SwitchAbilityUp = true;
                if(Input.mouseScrollDelta.y < 0)
                    SwitchAbilityDown = true;
            }
        
        
    }

    
    public void setAllToZero()
    {

        /*
        HoldingJump = false;
        holdingCrouch = false;
        holdingSprint = false;
        HoldingUp = false;
        HoldingDown = false;
        PressedJump = false;
        PressedCrouch = false;
        PressedAbility1 = false;
        PressedAbility2 = false;
        HoldingAbility1 = false; 
        SwitchAbilityUp = false;
        SwitchAbilityDown = false;

        moveDir = Vector2.zero;
        moveDirRaw = Vector2.zero;
        */
    }
    

}
