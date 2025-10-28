using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FadeManager : MonoBehaviour
{
    public string fadeInputKey;
    public UnityEvent fadeInputEvent;
    
    private InputAction _inputAction;
    private Keyboard _keyboard;
    
    private void Awake()
    {
        // Configurar el Input Action
        _inputAction = new InputAction(binding: "<Keyboard>/" + fadeInputKey);
        _inputAction.performed += ctx => OnFadeInput();
        _keyboard = InputSystem.GetDevice<Keyboard>();
    }
    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    private void OnDestroy()
    {
        _inputAction.Dispose();
    }

    public void OnFadeInput()
    {
        Debug.Log($"Tecla presionada: {fadeInputKey}");
        fadeInputEvent.Invoke();
    }

    private void OnValidate()
    {
        if (Application.isPlaying && _inputAction != null)
        {
            _inputAction.Disable();
            _inputAction.ApplyBindingOverride("<Keyboard>/" + fadeInputKey);
            _inputAction.Enable();
        }
    }
}
