using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewVoiceOverData", menuName = "IST/VoiceOver Data")]

public class VoiceOverDataSO : ScriptableObject
{
    [Header("Audio Configuration")]
    public AudioClip clip;

    [Header("Subtitle Configuration")]
    [TextArea(3, 10)]
    public List<string> paragraphs;

    [Header("Paragraph Transition")]
    [Tooltip("Duration of the fade between paragraphs")]
    public float paragraphFadeDuration = 0.5f;

    [Header("Next Voice Over")]
    public bool autoTransitionToNext = false;
    public VoiceOverDataSO nextVoiceOver;
    public float nextVoiceOverFadeDuration = 0.5f;

    [Tooltip("Duration to wait after audio finishes before fading out text (optional extra delay)")]
    public float postAudioDelay = 0f;
}
