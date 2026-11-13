using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SpriteFrameAnimator : MonoBehaviour
{
    [Header("Frames")]
    [SerializeField] private Sprite[] frames;

    [Header("Playback")]
    [SerializeField, Min(1f)] private float framesPerSecond = 12f;
    [Tooltip("Frame từ chỉ số này trở đi sẽ lặp. Frame trước đó chỉ phát 1 lần.")]
    [SerializeField, Min(0)] private int loopStartIndex = 15;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool useUnscaledTime = false;

    [Header("Target (gán 1 trong 2)")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Image uiImage;

    private float elapsed;
    private bool isPlaying;
    private int lastShownIndex = -1;

    private void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (uiImage == null) uiImage = GetComponent<Image>();

        if (!Validate())
        {
            enabled = false;
            return;
        }

        if (playOnAwake) Play();
    }

    private void Update()
    {
        if (!isPlaying) return;

        elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        int step = (int)(elapsed * framesPerSecond);

        // Qua hết frame cuối: quay lại đầu đoạn lặp, giữ elapsed không tăng vô hạn
        if (step >= frames.Length)
        {
            float loopDuration = (frames.Length - loopStartIndex) / framesPerSecond;
            elapsed -= loopDuration;
            step = (int)(elapsed * framesPerSecond);
        }

        ShowFrame(Mathf.Clamp(step, 0, frames.Length - 1));
    }

    public void Play(bool restart = true)
    {
        if (!enabled) return;
        if (restart) elapsed = 0f;
        isPlaying = true;
        lastShownIndex = -1;
        ShowFrame(Mathf.Clamp((int)(elapsed * framesPerSecond), 0, frames.Length - 1));
    }

    public void Stop() => isPlaying = false;

    private void ShowFrame(int index)
    {
        if (index == lastShownIndex) return; // tránh gán sprite thừa mỗi frame
        lastShownIndex = index;

        if (spriteRenderer != null) spriteRenderer.sprite = frames[index];
        if (uiImage != null) uiImage.sprite = frames[index];
    }

    private bool Validate()
    {
        if (frames == null || frames.Length == 0)
        {
            Debug.LogWarning($"[{name}] Chưa gán frames cho SpriteFrameAnimator.", this);
            return false;
        }
        for (int i = 0; i < frames.Length; i++)
        {
            if (frames[i] == null)
            {
                Debug.LogWarning($"[{name}] Frame ở vị trí {i} đang trống.", this);
                return false;
            }
        }
        if (spriteRenderer == null && uiImage == null)
        {
            Debug.LogWarning($"[{name}] Cần SpriteRenderer hoặc Image để hiển thị.", this);
            return false;
        }
        if (loopStartIndex >= frames.Length)
        {
            Debug.LogWarning($"[{name}] loopStartIndex ({loopStartIndex}) phải nhỏ hơn số frame ({frames.Length}).", this);
            return false;
        }
        return true;
    }

#if UNITY_EDITOR
    // Sau khi kéo sprite vào, bấm chuột phải vào tiêu đề component để sắp xếp đúng thứ tự
    [ContextMenu("Sort Frames By Trailing Number")]
    private void SortFrames()
    {
        if (frames == null) return;
        Array.Sort(frames, (a, b) => ExtractIndex(a).CompareTo(ExtractIndex(b)));
        UnityEditor.EditorUtility.SetDirty(this);
    }

    private static int ExtractIndex(Sprite s)
    {
        if (s == null) return int.MaxValue;
        Match m = Regex.Match(s.name, @"(\d+)$");
        return m.Success ? int.Parse(m.Value) : int.MaxValue;
    }

    private void OnValidate()
    {
        if (frames != null && frames.Length > 0 && loopStartIndex >= frames.Length)
            Debug.LogWarning($"[{name}] loopStartIndex vượt quá số frame.", this);
    }
#endif
}
