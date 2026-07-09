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

    // Send Messages 用のシグネチャ（InputValue を引数に取る）に変更
    public void OnSteer(InputValue value)
    {
        SteerChanged?.Invoke(value.Get<float>());
    }

    public void OnAccelerate(InputValue value)
    {
        AccelerateChanged?.Invoke(value.isPressed);
    }

    public void OnReverse(InputValue value)
    {
        ReverseChanged?.Invoke(value.isPressed);
    }

    public void OnBrake(InputValue value)
    {
        BrakeChanged?.Invoke(value.isPressed);
    }

    public void OnHorn(InputValue value)
    {
        if (value.isPressed)
        {
            HornPressed?.Invoke();
        }
    }

    public void OnRetry(InputValue value)
    {
        if (value.isPressed)
        {
            RetryPressed?.Invoke();
        }
    }

    public void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            PausePressed?.Invoke();
        }
    }
}
