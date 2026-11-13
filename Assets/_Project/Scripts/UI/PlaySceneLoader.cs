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
        [SerializeField] BlockBlast.Menu.MenuPresentation menuPresentation;
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
            if (loading || (playButton != null && !playButton.interactable)) return;
            if (!Application.CanStreamedLevelBeLoaded(destination))
            {
                Debug.LogError("Scene is missing or disabled in Build Settings: " + destination, this);
                return;
            }
            AdsManager.Instance?.LeaveMainMenu();
            loading = true;
            if (playButton != null) playButton.interactable = false;
            ShowProgress(0);
            StartCoroutine(Load());
        }

        IEnumerator Load()
        {
            if (menuPresentation != null) yield return menuPresentation.PlayReaction();
            if (loadingPanel != null) { loadingPanel.SetActive(true); loadingPanel.transform.SetAsLastSibling(); }
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
                if (menuPresentation != null) menuPresentation.Restore();
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

