using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BlockBlast.UI
{
    public sealed class PlaySceneLoader : MonoBehaviour
    {
        [SerializeField] string destination = "GamePlay";
        [SerializeField] Button playButton;
        [SerializeField] GameObject loadingPanel;
        [SerializeField] Slider progressBar;
        [SerializeField] TMP_Text percentage;
        bool loading;
        AsyncOperation operation;

        void Awake()
        {
            if (loadingPanel != null) loadingPanel.SetActive(false);
            if (playButton == null || loadingPanel == null || progressBar == null || percentage == null)
                Debug.LogWarning("PlaySceneLoader: assign all loading UI references in Inspector.", this);
        }

        public void Play()
        {
            if (loading) return;
            if (!Application.CanStreamedLevelBeLoaded(destination))
            {
                Debug.LogError("Scene is missing or disabled in Build Settings: " + destination, this);
                return;
            }
            AdsManager.Instance?.LeaveMainMenu();
            loading = true;
            if (playButton != null) playButton.interactable = false;
            if (loadingPanel != null) { loadingPanel.SetActive(true); loadingPanel.transform.SetAsLastSibling(); }
            ShowProgress(0);
            StartCoroutine(Load());
        }

        IEnumerator Load()
        {
            // Render the overlay before starting the load.
            yield return null;
            try { operation = SceneManager.LoadSceneAsync(destination, LoadSceneMode.Single); }
            catch (Exception error) { Debug.LogException(error, this); }
            if (operation == null)
            {
                AdsManager.Instance?.ResumeMainMenu();
                loading = false;
                if (playButton != null) playButton.interactable = true;
                if (loadingPanel != null) loadingPanel.SetActive(false);
                yield break;
            }
            operation.allowSceneActivation = false;
            while (operation.progress < .9f)
            {
                ShowProgress(operation.progress);
                yield return null;
            }
            // 90% means ready for activation, not an already activated scene.
            ShowProgress(.9f);
            yield return null;
            operation.allowSceneActivation = true;
            while (!operation.isDone) yield return null;
            ShowProgress(1);
        }

        void ShowProgress(float value)
        {
            if (progressBar != null) progressBar.value = Mathf.Clamp01(value);
            if (percentage != null) percentage.text = Mathf.RoundToInt(value * 100) + "%";
        }

        void OnDestroy()
        {
            if (operation != null && !operation.isDone) operation.allowSceneActivation = true;
        }
    }
}

