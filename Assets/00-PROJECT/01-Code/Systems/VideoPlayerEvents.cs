using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

namespace IST.Systems
{
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoPlayerEvents : MonoBehaviour
    {
        private VideoPlayer _videoPlayer;

        [Header("Video Events")]
        [Tooltip("Event invoked when the video is prepared and ready to play.")]
        public UnityEvent OnVideoPrepared;

        [Tooltip("Event invoked when the video starts playing.")]
        public UnityEvent OnVideoStarted;

        [Tooltip("Event invoked when the video finishes playing (reaches the end).")]
        public UnityEvent OnVideoFinished;

        private void Awake()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
        }

        private void OnEnable()
        {
            if (_videoPlayer != null)
            {
                _videoPlayer.prepareCompleted += HandlePrepareCompleted;
                _videoPlayer.started += HandleStarted;
                _videoPlayer.loopPointReached += HandleLoopPointReached;
            }
        }

        private void OnDisable()
        {
            if (_videoPlayer != null)
            {
                _videoPlayer.prepareCompleted -= HandlePrepareCompleted;
                _videoPlayer.started -= HandleStarted;
                _videoPlayer.loopPointReached -= HandleLoopPointReached;
            }
        }

        private void HandlePrepareCompleted(VideoPlayer source)
        {
            OnVideoPrepared?.Invoke();
        }

        private void HandleStarted(VideoPlayer source)
        {
            OnVideoStarted?.Invoke();
        }

        private void HandleLoopPointReached(VideoPlayer source)
        {
            OnVideoFinished?.Invoke();
        }
    }
}
