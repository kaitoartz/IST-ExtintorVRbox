using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AudioVoiceOffControl : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioSource auSource;
    [SerializeField] private GameSpeedManager _gameSpeedManager;
    [Tooltip("Asigna aquí un FadeEffect que controle EXCLUSIVAMENTE el texto de los subtítulos.")]
    [SerializeField] private FadeEffect fadeEffectController;
    [SerializeField] private TextMeshProUGUI subtitleTextComponent;

    [Header("Audio Configuration")]
    [SerializeField] private List<VoiceOverDataSO> voiceOverList;
    [SerializeField] private int clipActual = 0;

    [Header("Eventos")]
    [SerializeField] private UnityEvent onAudioStart;
    [SerializeField] private UnityEvent onAudioEnd;

    [Header("Configuración")]
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool iniciarAltiro = false;
    [SerializeField] private bool loopAudio = false;
    [SerializeField] private bool autoAdvance = true;
    [Header("Debug")]
    [SerializeField] private bool debugFadeLogs = false;

    // Variables privadas
    private const float DefaultFadeDuration = -1f;
    private const float DefaultParagraphFadeDuration = 0.35f;
    private bool startedPlaying = false;
    private bool hasDisabled = false;
    private float originalPitch = 1f;
    private Coroutine paragraphCoroutine;
    private VoiceOverDataSO currentVoiceOverData;

    private struct FadeSettings
    {
        public float entryDuration;
        public float paragraphDuration;
        public float transitionDuration;
    }

    private void Awake()
    {
        // 1. AudioSource
        if (auSource == null)
        {
            auSource = GetComponent<AudioSource>();
            if (auSource == null)
            {
                Debug.LogWarning($"[{gameObject.name}] AudioSource no asignado ni encontrado. Se agregará uno automáticamente.");
                auSource = gameObject.AddComponent<AudioSource>();
            }
        }
        originalPitch = auSource.pitch;
        auSource.loop = loopAudio;

        // 2. Subtitle Text Component
        if (subtitleTextComponent == null)
        {
            subtitleTextComponent = GetComponentInChildren<TextMeshProUGUI>();
            if (subtitleTextComponent == null)
            {
                // Try finding in parent or globally if desperate, but usually children is enough
                // subtitleTextComponent = FindObjectOfType<TextMeshProUGUI>(); // Too risky
            }
        }

        // 3. Fade Effect Controller
        if (fadeEffectController == null)
        {
            fadeEffectController = GetComponentInChildren<FadeEffect>();
            // If still null, maybe it's on the same object?
            if (fadeEffectController == null) fadeEffectController = GetComponent<FadeEffect>();
        }
    }

    private void Start()
    {
        if (_gameSpeedManager == null)
        {
            GameObject speedManagerObj = GameObject.FindGameObjectWithTag("GameSpeedManager");
            if (speedManagerObj != null)
            {
                _gameSpeedManager = speedManagerObj.GetComponent<GameSpeedManager>();
            }
            else
            {
                // Try finding by type as fallback
                _gameSpeedManager = FindObjectOfType<GameSpeedManager>();

                if (_gameSpeedManager == null)
                {
                    Debug.LogWarning($"[{gameObject.name}] GameSpeedManager no encontrado. El pitch no se ajustará dinámicamente.");
                }
            }
        }

        // Initialize current data from list if needed
        if (voiceOverList != null && voiceOverList.Count > clipActual)
        {
            currentVoiceOverData = voiceOverList[clipActual];
            if (auSource != null && currentVoiceOverData != null)
            {
                auSource.clip = currentVoiceOverData.clip;
            }
        }

        if (iniciarAltiro)
        {
            if (delay > 0)
            {
                Invoke(nameof(VoiceOffPlay), delay);
            }
            else
            {
                VoiceOffPlay();
            }
        }
    }

    private void OnEnable()
    {
        // Se deja comentado mientras depuramos el evento OnAudioEnd desde Update.
        // if (TickManager.Instance != null)
        // {
        //     TickManager.Instance.OnFastTick += FastTick;
        // }
    }

    private void OnDisable()
    {
        // if (TickManager.Instance != null)
        // {
        //     TickManager.Instance.OnFastTick -= FastTick;
        // }

        CancelInvoke();
    }

    private void Update()
    {
        // Usamos Update para garantizar la verificación por frame del audio.
        FastTick();
    }

    private FadeSettings BuildFadeSettings(VoiceOverDataSO data, float requestedEntryDuration)
    {
        float paragraphDuration = data != null ? data.paragraphFadeDuration : DefaultParagraphFadeDuration;
        if (paragraphDuration <= 0f)
        {
            paragraphDuration = DefaultParagraphFadeDuration;
        }

        float transitionDuration = (data != null && data.autoTransitionToNext)
            ? data.nextVoiceOverFadeDuration
            : DefaultFadeDuration;

        return new FadeSettings
        {
            entryDuration = requestedEntryDuration,
            paragraphDuration = paragraphDuration,
            transitionDuration = transitionDuration
        };
    }

    private void ApplyFadeIn(float duration)
    {
        if (fadeEffectController == null) return;

        if (duration <= DefaultFadeDuration)
        {
            fadeEffectController.IniciarFadeIn();
            LogFade("Fade in (default duration)");
        }
        else
        {
            fadeEffectController.IniciarFadePersonalizado(0f, 1f, duration);
            LogFade($"Fade in (custom {duration:F2}s)");
        }
    }

    private void ApplyFadeOut(float duration)
    {
        if (fadeEffectController == null) return;

        if (duration <= DefaultFadeDuration)
        {
            fadeEffectController.IniciarFadeOut();
            LogFade("Fade out (default duration)");
        }
        else
        {
            fadeEffectController.IniciarFadePersonalizado(1f, 0f, duration);
            LogFade($"Fade out (custom {duration:F2}s)");
        }
    }

    private float ResolveWaitTime(float duration)
    {
        if (duration > DefaultFadeDuration)
        {
            return Mathf.Max(0f, duration);
        }

        if (fadeEffectController != null)
        {
            return Mathf.Max(0f, fadeEffectController.GetDefaultDuration());
        }

        return 0.5f;
    }

    private void LogFade(string message)
    {
        if (!debugFadeLogs) return;
        Debug.Log($"[{nameof(AudioVoiceOffControl)}] {message}");
    }

    public void VoiceOffPlay()
    {
        VoiceOffPlay(DefaultFadeDuration);
    }

    public void VoiceOffPlay(float fadeInDuration)
    {
        if (!hasDisabled)
        {
            Debug.Log("VoiceOffPlay invoked.");
            onAudioStart.Invoke();

            var fadeSettings = BuildFadeSettings(currentVoiceOverData, fadeInDuration);

            // Configurar texto y fade si corresponde
            SetupSubtitleForCurrentClip(fadeSettings);

            auSource.Play();
            startedPlaying = true;
            UpdateAudioPitch();

            Debug.Log($"{gameObject.name} está reproduciendo audio: {clipActual}");
        }
    }

    private void FastTick()
    {
        CheckAudioStatus();
    }
    private void CheckAudioStatus()
    {
        if (!loopAudio)
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
        Debug.Log("startedPlaying: " + startedPlaying + ", isPlaying: " + auSource.isPlaying);

    }

    private void OnAudioFinished()
    {
        startedPlaying = false;
        Debug.Log($"{gameObject.name} audio finished detected for clip: {clipActual}");
        StartCoroutine(TransitionToNextClip());
    }

    private IEnumerator TransitionToNextClip()
    {
        Debug.Log($"{gameObject.name} ha terminado de reproducir audio: {clipActual}");
        float extraDelay = 0f;
        float fadeDuration = DefaultFadeDuration;

        if (currentVoiceOverData != null)
        {
            extraDelay = currentVoiceOverData.postAudioDelay;
            fadeDuration = BuildFadeSettings(currentVoiceOverData, DefaultFadeDuration).transitionDuration;
        }

        // Esperar delay extra si está configurado
        if (extraDelay > 0) yield return new WaitForSeconds(extraDelay);

        {
            ApplyFadeOut(fadeDuration);
            yield return new WaitForSeconds(ResolveWaitTime(fadeDuration));
        }

        // Check for next linked SO
        bool nextFound = false;
        if (currentVoiceOverData != null && currentVoiceOverData.autoTransitionToNext && currentVoiceOverData.nextVoiceOver != null)
        {
            PlayVoiceOver(currentVoiceOverData.nextVoiceOver, fadeDuration);
            nextFound = true;
        }
        else if (voiceOverList != null && autoAdvance)
        {
            // Try next in list - use default fade as it's an unrelated clip
            int nextIndex = clipActual + 1;
            if (nextIndex < voiceOverList.Count)
            {
                PlayClip(nextIndex, DefaultFadeDuration);
                nextFound = true;
            }
        }

        if (!nextFound)
        {
            onAudioEnd.Invoke();
        }
        Debug.Log("nextFound: " + nextFound);
    }

    private void UpdateAudioPitch()
    {
        if (_gameSpeedManager != null)
        {
            // Si el juego está en modo velocidad aumentada, ajustar pitch
            auSource.pitch = _gameSpeedManager.IsSpeedUp ? _gameSpeedManager.CurrentSpeedFactor : originalPitch;
        }
    }


    private void SetupSubtitleForCurrentClip(FadeSettings fadeSettings)
    {
        if (currentVoiceOverData == null) return;

        if (paragraphCoroutine != null) StopCoroutine(paragraphCoroutine);

        // Si tenemos componente de texto y el dato actual tiene texto
        if (subtitleTextComponent != null && currentVoiceOverData.paragraphs != null && currentVoiceOverData.paragraphs.Count > 0)
        {
            if (currentVoiceOverData.paragraphs.Count == 1)
            {
                subtitleTextComponent.text = currentVoiceOverData.paragraphs[0];
                ApplyFadeIn(fadeSettings.entryDuration);
            }
            else
            {
                paragraphCoroutine = StartCoroutine(CycleParagraphs(currentVoiceOverData, fadeSettings));
            }
        }
        else
        {
            // Si no hay texto para este clip, nos aseguramos que el fade esté fuera o texto vacío
            ApplyFadeOut(DefaultFadeDuration);
        }
    }

    private IEnumerator CycleParagraphs(VoiceOverDataSO data, FadeSettings fadeSettings)
    {
        float audioDuration = data.clip != null ? data.clip.length : 5f;
        float durationPerParagraph = audioDuration / data.paragraphs.Count;

        for (int i = 0; i < data.paragraphs.Count; i++)
        {
            subtitleTextComponent.text = data.paragraphs[i];

            if (fadeEffectController != null)
            {
                if (i == 0)
                {
                    ApplyFadeIn(fadeSettings.entryDuration);
                }
                else
                {
                    ApplyFadeIn(fadeSettings.paragraphDuration);
                }
            }

            float waitTime = durationPerParagraph;

            // If not the last paragraph, we need to fade out
            if (i < data.paragraphs.Count - 1)
            {
                waitTime -= fadeSettings.paragraphDuration;
                if (waitTime < 0) waitTime = 0;

                yield return new WaitForSeconds(waitTime);

                ApplyFadeOut(fadeSettings.paragraphDuration);
                yield return new WaitForSeconds(ResolveWaitTime(fadeSettings.paragraphDuration));
            }
            else
            {
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    public void PlayClip(int index, float fadeInDuration)
    {
        if (voiceOverList != null && index >= 0 && index < voiceOverList.Count)
        {
            clipActual = index;
            PlayVoiceOver(voiceOverList[index], fadeInDuration);
        }
    }

    private void PlayVoiceOver(VoiceOverDataSO data, float fadeInDuration)
    {
        currentVoiceOverData = data;
        if (data != null)
        {
            auSource.clip = data.clip;
            VoiceOffPlay(fadeInDuration);
        }
    }


    #region UTILIDADES COMENTADAS
    /*
    public void StopAudio()
    {
        if (auSource != null)
        {
            auSource.Stop();
            startedPlaying = false;
        }
    }

    public void PauseAudio()
    {
        if (auSource != null)
        {
            auSource.Pause();
        }
    }

    public void UnPauseAudio()
    {
        if (auSource != null)
        {
            auSource.UnPause();
        }
    }
     
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