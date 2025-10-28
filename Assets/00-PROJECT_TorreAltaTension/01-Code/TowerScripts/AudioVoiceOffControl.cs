using UnityEngine;
using UnityEngine.Events;

public class AudioVoiceOffControl : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioSource auSource;
    [SerializeField] private GameSpeedManager _gameSpeedManager;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private int clipActual = 0;
    
    [Header("Eventos")]
    [SerializeField] private UnityEvent onAudioStart;
    [SerializeField] private UnityEvent onAudioEnd;
    
    [Header("Configuración")]
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool isForStart = false;
    [SerializeField] private bool loopAudio = false;
    
    // Variables privadas
    private bool startedPlaying = false;
    private bool hasDisabled = false;
    private float originalPitch = 1f;

    private void Awake()
    {
        if (auSource == null)
        {
            auSource = GetComponent<AudioSource>();
        }
        if (auSource != null)
        {
            originalPitch = auSource.pitch;
        }
    }
    
    private void Start()
    {
        _gameSpeedManager = GameObject.FindGameObjectWithTag("GameSpeedManager").GetComponent<GameSpeedManager>();
        if (isForStart) 
        {
            if (delay > 0)
            {
                Invoke("VoiceOffPlay", delay);
            }
            else
            {
                VoiceOffPlay();
            }
        }
        
        if (auSource != null)
        {
            auSource.loop = loopAudio;
        }
    }
    
    private void OnEnable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnVeryFastTick += FastTick;
        }
    }

    private void OnDisable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnVeryFastTick -= FastTick;
        }
        
        CancelInvoke();
    }
    
    private void FastTick()
    {
        CheckAudioStatus();
    }
    
    private void UpdateAudioPitch()
    {
        if (auSource != null && _gameSpeedManager != null)
        {
            // Si el juego está en modo velocidad aumentada, ajustar pitch
            auSource.pitch = _gameSpeedManager.IsSpeedUp ? _gameSpeedManager.CurrentSpeedFactor : originalPitch;
        }
    }
    
    private void CheckAudioStatus()
    {
        if (auSource != null && !loopAudio)
        {
            if (auSource.isPlaying) 
            {
                startedPlaying = true;
            }
            else if (startedPlaying)
            {
                OnAudioFinished();
            }
        }
    }
    
    private void OnAudioFinished()
    {
        onAudioEnd.Invoke();
        startedPlaying = false;
        //audioClips[clipActual].UnloadAudioData();
        if (audioClips != null && audioClips.Length > 0)
        {
            clipActual++;
            if (clipActual < audioClips.Length)
            {
                PlayClip(clipActual);
            }
        }
    }
    
    public void VoiceOffPlay()
    {
        if (auSource != null && !hasDisabled)
        {
            onAudioStart.Invoke();
            
            auSource.Play();
            startedPlaying = true;
            UpdateAudioPitch();
            
            Debug.Log($"{gameObject.name} está reproduciendo audio");
        }
    }
    
    public void PlayClip(int index)
    {
        if (audioClips != null && index >= 0 && index < audioClips.Length)
        {
            if (auSource != null)
            {
                clipActual = index;
                auSource.clip = audioClips[index];
                VoiceOffPlay();
            }
        }
    }
    public void PlayDelayed(float delayTime)
    {
        if (auSource != null)
        {
            Invoke("VoiceOffPlay", delayTime);
        }
    }  
    
    #region UTILIDADES COMENTADAS
    /*
     
     public void DisableAuSource()
    {
        if (auSource != null)
        {
            auSource.Stop();
            auSource.enabled = false;
        }
        hasDisabled = true;
    }
    // Reproducir clip por nombre
    public void PlayClipByName(string clipName)
    {
        if (audioClips != null)
        {
            for (int i = 0; i < audioClips.Length; i++)
            {
                if (audioClips[i] != null && audioClips[i].name == clipName)
                {
                    PlayClip(i);
                    return;
                }
            }

            Debug.LogWarning($"Clip de audio '{clipName}' no encontrado.");
        }
    }

    // Verificar si el audio está reproduciéndose
    public bool IsPlaying()
    {
        return auSource != null && auSource.isPlaying;
    }

    // Obtener duración del clip actual
    public float GetCurrentClipDuration()
    {
        if (auSource != null && auSource.clip != null)
        {
            return auSource.clip.length;
        }
        return 0f;
    }
    */
    #endregion
}