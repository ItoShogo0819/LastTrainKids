using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader _inputReader;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider _frontLeftWheel;
    [SerializeField] private WheelCollider _frontRightWheel;
    [SerializeField] private WheelCollider _rearLeftWheel;
    [SerializeField] private WheelCollider _rearRightWheel;

    [Header("Vehicle Settings")]
    [SerializeField] private float _motorTorque = 1500f;
    [SerializeField] private float _reverseTorque = 800f;
    [SerializeField] private float _brakeTorque = 3000f;
    [SerializeField] private float _maxSteerAngle = 35f;
    [SerializeField] private float _maxSpeedkmh = 60f;

    private float _steerInput;

    private bool _acceleratePressed;
    private bool _reversePressed;
    private bool _brakePressed;

    private Rigidbody _rb;

    private float Speedkmh => _rb.linearVelocity.magnitude * 3.6f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _inputReader.SteerChanged += OnSteerChanged;
        _inputReader.AccelerateChanged += OnAccelerateChanged;
        _inputReader.ReverseChanged += OnReverseChanged;
        _inputReader.BrakeChanged += OnBrakeChanged;
    }

    private void OnDisable()
    {
        _inputReader.SteerChanged -= OnSteerChanged;
        _inputReader.AccelerateChanged -= OnAccelerateChanged;
        _inputReader.ReverseChanged -= OnReverseChanged;
        _inputReader.BrakeChanged -= OnBrakeChanged;
    }

    private void FixedUpdate()
    {
        ApplySteering();
        ApplyDrive();
    }

    private void OnSteerChanged(float val)
    {
        _steerInput = val;
    }

    private void OnAccelerateChanged(bool pressed)
    {
        _acceleratePressed = pressed;
    }

    private void OnReverseChanged(bool pressed)
    {
        _reversePressed = pressed;
    }

    private void OnBrakeChanged(bool pressed)
    {
        _brakePressed = pressed;
    }

    private void ApplySteering()
    {
        float speedRatio = Mathf.InverseLerp(0f, _maxSpeedkmh, Speedkmh);
        float steerAngle = Mathf.Lerp(_maxSteerAngle, _maxSteerAngle * 0.35f, speedRatio);

        _frontLeftWheel.steerAngle = _steerInput * steerAngle;
        _frontRightWheel.steerAngle = _steerInput * steerAngle;
    }

    private void ApplyDrive()
    {
        float currentSpeed = Speedkmh;

        float appliedMotorTorque = 0f;
        float appliedBrakeTorque = 0f;

        if (_brakePressed)
        {
            appliedBrakeTorque = _brakeTorque;

        }
        else if (_acceleratePressed && currentSpeed < _maxSpeedkmh)
        {
            appliedMotorTorque = _motorTorque;
        }
        else if (_reversePressed)
        {
            appliedMotorTorque = -_reverseTorque;
        }

        _rearLeftWheel.motorTorque = appliedMotorTorque;
        _rearRightWheel.motorTorque = appliedMotorTorque;

        _frontLeftWheel.brakeTorque = appliedBrakeTorque;
        _frontRightWheel.brakeTorque = appliedBrakeTorque;
        _rearLeftWheel.brakeTorque = appliedBrakeTorque;
        _rearRightWheel.brakeTorque = appliedBrakeTorque;
    }
}
