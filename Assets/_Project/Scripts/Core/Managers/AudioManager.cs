using UnityEngine;
using BlockBlast.Core;

namespace BlockBlast.Managers
{
    /// <summary>One persistent service with independent 2D music and one-shot SFX sources.</summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        [SerializeField] private AudioSource sfxSource, bgmSource;
        [SerializeField] private AudioClip blockPlaceClip, lineClearClip, gameOverClip;
        [SerializeField] private AudioClip buttonClip, dragClip;
        public bool SfxEnabled { get; private set; }
        public bool MusicEnabled { get; private set; }
        public float MusicVolume { get; private set; }
        public float SfxVolume { get; private set; }
#if UNITY_EDITOR
        public int PlayedSfxCount { get; private set; }
        public AudioClip LastPlayedClip { get; private set; }
#endif
        private bool subscribed;
        private void Awake()
        {
            if (Instance != null && Instance != this) { enabled = false; Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            sfxSource.playOnAwake = bgmSource.playOnAwake = false;
            sfxSource.spatialBlend = bgmSource.spatialBlend = 0;
            sfxSource.loop = false; bgmSource.loop = true;
            sfxSource.pitch = bgmSource.pitch = 1;
            ReloadPreferences();
            if (bgmSource.clip != null && MusicEnabled) bgmSource.Play();
        }
        public void ReloadPreferences()
        {
            SfxEnabled = PlayerPrefs.GetInt("UI.SFX.Enabled", 1) != 0;
            MusicEnabled = PlayerPrefs.GetInt("UI.Music.Enabled", 1) != 0;
            ApplyVolumes(PlayerPrefs.GetFloat("UI.Music", .7f), PlayerPrefs.GetFloat("UI.SFX", 1));
        }
        public void ApplyVolumes(float music, float sfx)
        {
            MusicVolume = Mathf.Clamp01(music); SfxVolume = Mathf.Clamp01(sfx);
            bgmSource.volume = MusicVolume; sfxSource.volume = SfxVolume;
            bgmSource.mute = !MusicEnabled || MusicVolume == 0;
            sfxSource.mute = !SfxEnabled || SfxVolume == 0;
            if (sfxSource.mute) sfxSource.Stop();
        }
        public void SetSfxEnabled(bool value)
        {
            SfxEnabled = value; PlayerPrefs.SetInt("UI.SFX.Enabled", value ? 1 : 0);
            ApplyVolumes(MusicVolume, SfxVolume); PlayerPrefs.Save();
        }
        public void SetMusicEnabled(bool value)
        {
            MusicEnabled = value; PlayerPrefs.SetInt("UI.Music.Enabled", value ? 1 : 0);
            ApplyVolumes(MusicVolume, SfxVolume);
            if (value && bgmSource.clip != null && !bgmSource.isPlaying) bgmSource.Play();
            PlayerPrefs.Save();
        }
        public void PlayMusic(AudioClip clip)
        {
            if (bgmSource.clip == clip)
            {
                if (clip != null && MusicEnabled && !bgmSource.isPlaying) bgmSource.Play();
                return;
            }
            bgmSource.Stop(); bgmSource.clip = clip;
            if (clip != null && MusicEnabled) bgmSource.Play();
        }
        public void StopMusic() => bgmSource.Stop();
        public void PauseMusic() => bgmSource.Pause();
        public void ResumeMusic() { if (MusicEnabled) bgmSource.UnPause(); }
        private void OnEnable()
        {
            if (Instance != this || subscribed) return;
            subscribed = true;
            GameEvents.OnShapePlaced += PlayBlockPlaceSound;
            GameEvents.OnLinesCleared += PlayLineClearSound;
            GameEvents.OnGameOver += PlayGameOverSound;
        }
        private void OnDisable()
        {
            if (!subscribed) return;
            subscribed = false;
            GameEvents.OnShapePlaced -= PlayBlockPlaceSound;
            GameEvents.OnLinesCleared -= PlayLineClearSound;
            GameEvents.OnGameOver -= PlayGameOverSound;
        }
        private void OnDestroy() { if (Instance == this) Instance = null; }
        private void PlaySfx(AudioClip clip)
        {
            if (Instance != this || !isActiveAndEnabled || clip == null || !SfxEnabled || SfxVolume <= 0) return;
            // A shared pitch would also retune already-playing one-shots; leave pitch fixed.
            sfxSource.PlayOneShot(clip);
#if UNITY_EDITOR
            PlayedSfxCount++; LastPlayedClip = clip;
#endif
        }
        public void PlayButtonSound() => PlaySfx(buttonClip);
        public void PlayDragSound() => PlaySfx(dragClip);
        private void PlayBlockPlaceSound() { BlockBlast.UI.Haptics.Pulse(); PlaySfx(blockPlaceClip); }
        private void PlayLineClearSound(int count) { if (count > 0) PlaySfx(lineClearClip); }
        private void PlayGameOverSound() => PlaySfx(gameOverClip);
        private void OnApplicationPause(bool paused) { if (paused) { sfxSource.Stop(); PlayerPrefs.Save(); } }
        private void OnApplicationQuit() => PlayerPrefs.Save();
    }
}
