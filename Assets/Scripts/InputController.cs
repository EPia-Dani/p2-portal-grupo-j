using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [HideInInspector] public Vector2 move;

    [HideInInspector] public Vector2 look;

    [HideInInspector] public bool jump;

    [HideInInspector] public bool run;

    [HideInInspector] public bool button1;

    [HideInInspector] public bool button2;

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        run = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        jump = value.isPressed;
    }

    public void OnButton1(InputValue value)
    {
        button1 = !button1;
    }

    public void OnButton2(InputValue value)
    {
        button2 = !button2;
    }
}