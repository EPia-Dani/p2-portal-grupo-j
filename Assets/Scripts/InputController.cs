using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [HideInInspector] public Vector2 move;

    [HideInInspector] public Vector2 look;

    [HideInInspector] public bool jump;

    [HideInInspector] public bool run;

    [HideInInspector] public bool shoot;

    [HideInInspector] public bool reload;

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

    public void OnAttack(InputValue value)
    {
        shoot = !shoot;
    }

    public void OnReload(InputValue value)
    {
        reload = value.isPressed;
    }
}