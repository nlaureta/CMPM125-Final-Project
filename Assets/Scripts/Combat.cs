using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Combat : MonoBehaviour
{
    public Player1Controls gamepadControls;
    Animator punch;
    //[SerializeField] private GameObject punchHB, blockIcon;
    bool punchcd, blocking = false;
    private bool whichPlayerAttack = false;
    private bool whichPlayerBlock = false;
    float timeRemaining;

    Vector3 gamepadMove;
    public float moveSpeed = 5f;

    [SerializeField] private int playerIndex = 0; //determines what player is controlled

    private bool isButtonPressed = false;


    void Awake()
    {
        punch = GetComponentInChildren<Animator>();
        gamepadControls = new Player1Controls();
        //gamepadControls.Player1Gameplay.Punch.performed += ctx => GamepadPunch();
        //gamepadControls.Player1Gameplay.Move.performed += ctx => gamepadMove = ctx.ReadValue<Vector2>();
        //gamepadControls.Player1Gameplay.Move.canceled += ctx => gamepadMove = Vector2.zero;
    }

    void OnEnable()
    {
        gamepadControls.Player1Gameplay.Enable();
    }

    void OnDisable()
    {
        gamepadControls.Player1Gameplay.Disable();
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public void GamepadPunch()
    {
        if (!punchcd && !blocking)
        {
            //Debug.Log("punching");
            //punchHB.SetActive(true);
            punch.SetTrigger("Punch");
            //punch.Play("PlayerPunch", -1, 0f);
            punchcd = true;
            StartCoroutine(Cooldown(.5f));
        }
    }

    public void SetBlock(bool buttonPressed) // gamepad bool for block
    {
        isButtonPressed = buttonPressed;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerIndex == 0) //p1
        {
            whichPlayerAttack = Input.GetKey(KeyCode.U);
            whichPlayerBlock = Input.GetKey(KeyCode.I);
        }
        else if (playerIndex == 1) //p2
        {
            whichPlayerAttack = Input.GetKey(KeyCode.Mouse0);
            whichPlayerBlock = Input.GetKey(KeyCode.Mouse1);
        }
        if (!punchcd && !blocking && whichPlayerAttack)
        {
            //Debug.Log("punching");
            //punchHB.SetActive(true);
            punch.SetTrigger("Punch");
            //punch.Play("PlayerPunch", -1, 0f);
            punchcd = true;
            StartCoroutine(Cooldown(.5f));
        }

        if (!punchcd && whichPlayerBlock)
        {
            blocking = true;
            //blockIcon.SetActive(true);
            punch.SetBool("Block", true);
            //Debug.Log("Blocked");
        }
        else if (!punchcd && isButtonPressed) //gamepad block
        {
            blocking = true;
            //blockIcon.SetActive(true);
            punch.SetBool("Block", true);
            //Debug.Log("Blocked");
        }
        else
        {
            blocking = false;
            //blockIcon.SetActive(false);
            punch.SetBool("Block", false);
        }


        // else if (blocking && gamepadControls.Player1Gameplay.Block.ReadValue<float>() == 0)
        // {
        //     blocking = false;
        //     blockIcon.SetActive(false);
        //     punch.SetBool("blocking", false);
        //     Debug.Log("Blocked ended");
        // }

    }

    IEnumerator Cooldown(float cdtime)
    {
        timeRemaining = 0;
        while (timeRemaining < cdtime)
        {
            //Debug.Log(timeRemaining);
            timeRemaining += Time.deltaTime;
            yield return null;
        }
        punchcd = false;
        //punchHB.SetActive(false);
    }
}
