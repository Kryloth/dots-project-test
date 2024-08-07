using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerInputSystem : SystemBase
{
    private PlayerInputActions _playerInputActions;
    
    private Vector2 _movementDirection;
    private Vector2 _aimDirection;

    protected override void OnStartRunning()
    {
        if (!SystemAPI.TryGetSingleton(out PlayerInputComponent playerInputComponent))
        {
            EntityManager.CreateEntity(typeof(PlayerInputComponent));
        }
        
        InitializeInputEvents();
    }

    protected override void OnStopRunning()
    {
        TerminateInputEvents();
    }

    protected override void OnUpdate()
    {
        SystemAPI.SetSingleton(new PlayerInputComponent()
        {
            MovementDirection =  _movementDirection,
            AimDirection = _aimDirection
        });
    }

    private void InitializeInputEvents()
    {
        if (_playerInputActions == null)
            _playerInputActions = new();
        
        _playerInputActions.Enable();

        _playerInputActions.Gameplay.Movement.performed += OnMovementPerformed;
        _playerInputActions.Gameplay.Movement.canceled += OnMovementCanceled;

        _playerInputActions.Gameplay.Aim.performed += OnAttackPerformed;
        _playerInputActions.Gameplay.Aim.canceled += OnAttackCanceled;
    }

    private void TerminateInputEvents()
    {
        _playerInputActions.Disable();
        _playerInputActions.Gameplay.Movement.performed -= OnMovementPerformed;
        _playerInputActions.Gameplay.Movement.canceled -= OnMovementCanceled;

        _playerInputActions.Gameplay.Aim.performed -= OnAttackPerformed;
        _playerInputActions.Gameplay.Aim.canceled -= OnAttackCanceled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        _movementDirection = context.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        _movementDirection = Vector2.zero;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        _aimDirection = context.ReadValue<Vector2>();
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        _aimDirection = Vector2.zero;
    }
}
