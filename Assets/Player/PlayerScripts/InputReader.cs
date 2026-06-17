using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public event Action<float> SteerChanged;
    public event Action<bool> AccelerateChanged;
    public event Action<bool> ReverseChanged;
    public event Action<bool> BrakeChanged;
    public event Action HornPressed;
    public event Action RetryPressed;
    public event Action PausePressed;

    public void OnSteer(InputAction.CallbackContext cont)
    {
        SteerChanged?.Invoke(cont.ReadValue<float>());
    }

    public void OnAccelerate(InputAction.CallbackContext cont)
    {
        AccelerateChanged?.Invoke(cont.ReadValueAsButton());
    }

    public void OnReverse(InputAction.CallbackContext cont)
    {
        ReverseChanged?.Invoke(cont.ReadValueAsButton());
    }

    public void OnBrake(InputAction.CallbackContext cont)
    {
        BrakeChanged?.Invoke(cont.ReadValueAsButton());
    }

    public void OnHorn(InputAction.CallbackContext cont)
    {
        if (cont.performed)
        {
            HornPressed?.Invoke();
        }
    }

    public void OnRetry(InputAction.CallbackContext cont)
    {
        if (cont.performed)
        {
            RetryPressed?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext cont)
    {
        if (cont.performed)
        {
            PausePressed?.Invoke();
        }
    }
}
