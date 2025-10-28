using System;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    private static TickManager _instance;
    public static TickManager Instance => _instance;

    // Diferentes eventos para distintas frecuencias de tick
    public event Action OnTick;
    public event Action OnSlowTick;
    public event Action OnFastTick;
    public event Action OnVeryFastTick;
    public event Action OnPowerFastTick;


    [SerializeField] private float _tickRate = 1f;       // 1 vez por segundo
    [SerializeField] private float _slowTickRate = 2f;    // 0.5 veces por segundo
    [SerializeField] private float _fastTickRate = 0.2f;  // 5 veces por segundo
    [SerializeField] private float _veryFastTickRate = 0.05f; // 20 veces por segundo
    [SerializeField] private float _powerFastTickRate = 0.01f; // 100 veces por segundo

    private float _tickTimer;
    private float _slowTickTimer;
    private float _fastTickTimer;
    private float _veryFastTickTimer;
    private float _powerFastTickTimer;

    private void Awake()
    {
        // Implementación del Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Actualizar los timers
        _tickTimer += Time.deltaTime;
        _slowTickTimer += Time.deltaTime;
        _fastTickTimer += Time.deltaTime;
        _veryFastTickTimer += Time.deltaTime;
        _powerFastTickTimer += Time.deltaTime;

        // Disparar eventos cuando corresponda
        if (_tickTimer >= _tickRate)
        {
            _tickTimer -= _tickRate;
            OnTick?.Invoke();
        }

        if (_slowTickTimer >= _slowTickRate)
        {
            _slowTickTimer -= _slowTickRate;
            OnSlowTick?.Invoke();
        }

        if (_fastTickTimer >= _fastTickRate)
        {
            _fastTickTimer -= _fastTickRate;
            OnFastTick?.Invoke();
        }

        if (_veryFastTickTimer >= _veryFastTickRate)
        {
            _veryFastTickTimer -= _veryFastTickRate;
            OnVeryFastTick?.Invoke();
        }
        if (_powerFastTickTimer >= _powerFastTickRate)
        {
            _powerFastTickTimer -= _powerFastTickRate;
            OnVeryFastTick?.Invoke();
        }
    }
}