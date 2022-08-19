using DG.Tweening;
using UnityEngine;

public class PlayerMovementController : PlayerMovementControllerBase
{
    private float _speed;
    private float _stamina;
    private bool _isRunning;
    private bool _movementAvailable;
    private bool _staminaIsGrowing;
    private PlayerMovementConfig _config;
    private Animator _animator;

    private Tween _StaminaGrowthEnabler;

    public PlayerMovementController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config) : base(player, signalBus)
    {
        _config = config;
        _animator = _player.Model.GetComponent<Animator>();

        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(OnStaminaChanged, this);
        _signalBus.Subscribe<SendCharacterStatesSignal>(GetCharacterStates, this);
        updateProvider.Updates.Add(UpdateStamina);

        Cursor.lockState = CursorLockMode.Locked;
        _stamina = _config.MaxStamina;
    }

    private void GetCharacterStates(SendCharacterStatesSignal signal)
    {
        _isRunning = signal.States[PlayerState.Running];
        _movementAvailable = !(signal.States[PlayerState.Jumping] || signal.States[PlayerState.Rolling]
            || signal.States[PlayerState.Collecting]);
    }

    private void OnStaminaChanged(OnStaminaChangedSignal signal)
    {
        if (signal.Stamina < _stamina)
        {
            _StaminaGrowthEnabler.Kill();
            _staminaIsGrowing = false;
            _StaminaGrowthEnabler = DOVirtual.DelayedCall(_config.StaminaRegeneratingDelay, () =>
            {
                _staminaIsGrowing = true;
            });
        }
        _stamina = signal.Stamina;
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {

        var moveDirection = new Vector3(signal.Data.Direction.x, 0, signal.Data.Direction.y);
        moveDirection = _player.transform.TransformDirection(moveDirection);

        if (!_isRunning && signal.Data.SprintAttempt && signal.Data.Direction.y >= 0 && _movementAvailable && _stamina > 10)
            StartRun();

        if (_isRunning && (signal.Data.SprintBreak || signal.Data.Direction.y < 0 || signal.Data.Direction == Vector2.zero))
            EndRun();

        if (_movementAvailable)
        {
            _player.Controller.Move(moveDirection * _speed * Time.deltaTime);
            _player.transform.Rotate(Vector3.up, signal.Data.Rotation.x);
            CalculateSpeed(signal.Data.Direction);
            if (signal.Data.Direction == Vector2.zero)
            {
                _signalBus.FireSignal(new UpdateLastSpeedSignal(0));
            }
            else
            {
                _signalBus.FireSignal(new UpdateLastSpeedSignal(signal.Data.Direction.y >= 0 ? _speed : -_speed));
            }
        }
        else CalculateSpeed(Vector2.zero);

    }

    private void CalculateSpeed(Vector2 input)
    {
        if (_isRunning)
            _speed = _config.RunningSpeed;
        else
            _speed = input.y >= 0 ? _config.WalkingSpeed : _config.WalkingBackSpeed;


        if (input == Vector2.zero)
        {
            _animator.SetInteger("Speed", 0);
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Running, false));
        }
        else
        {
            if (input.y >= 0)
            {
                _animator.SetInteger("Speed", _isRunning ? 2 : 1);
            }
            else
            {
                _animator.SetInteger("Speed", -1);
            }
        }
    }

    private void UpdateStamina()
    {
        if (_isRunning)
        {
            _stamina -= _config.StaminaConsuming * Time.deltaTime;

            if (_stamina <= 0) EndRun();
        }
        else if (_staminaIsGrowing)
        {
            _stamina += _config.StaminaRegeneration * Time.deltaTime;
            _stamina = Mathf.Clamp(_stamina, 0, _config.MaxStamina);
        }

        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina));
    }

    private void EndRun()
    {
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Running, false));
        _staminaIsGrowing = false;
        _StaminaGrowthEnabler = DOVirtual.DelayedCall(_config.StaminaRegeneratingDelay, () =>
        {
            _staminaIsGrowing = true;
        });
    }

    private void StartRun()
    {
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Running, true));
        _staminaIsGrowing = false;
        _StaminaGrowthEnabler.Kill();
    }

}
