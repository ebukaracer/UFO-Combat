using System.Collections;
using UnityEngine;

namespace Racer.LoadManager
{
    /// <summary>
    /// Smoothly Fades-in/out canvas-group alpha.
    /// See also: <see cref="LoadManager"/>.
    /// Enables a loading animation on the go.
    /// See also: <see cref="LoadTask"/>.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    internal class CanvasGroupTask : LoadTask
    {
        private CanvasGroup canvasGroup;

        private WaitForSeconds waitForSeconds;

        [Space(10)]

        [SerializeField, Range(0, 1f)]
        private float fadeDelay = .05f;


        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            waitForSeconds = new WaitForSeconds(fadeDelay);

            // Since the coroutine would iterate over ten times(..the total time it'd elapse).
            // Example: 0.05f * 10f = 0.5f, which means it'd take 0.5f seconds to complete fade-in/out.
            LoadManager.Instance.InitDelay = fadeDelay * 10f;

            // Subscription for when loading is initialized.
            LoadManager.Instance.OnLoadInit += Instance_OnLoadInit;

            // Subscription for when loading is started.
            LoadManager.Instance.OnLoadStarted += Instance_OnLoadStarted;


            if (fadeOnStart)
                StartCoroutine(FadeOutCanvasGroup());
        }
       
        private void Instance_OnLoadInit()
        {
            StartCoroutine(FadeInCanvasGroup());
        }

        private void Instance_OnLoadStarted()
        {
            EnableLoaderDefaultAnimation();
        }

        private IEnumerator FadeInCanvasGroup()
        {
            canvasGroup.alpha = 0;

            canvasGroup.blocksRaycasts = true;

            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += .1f;

                yield return waitForSeconds;
            }

            canvasGroup.alpha = 1;
        }

        private IEnumerator FadeOutCanvasGroup()
        {
            canvasGroup.alpha = 1;

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= .1f;

                yield return waitForSeconds;
            }

            canvasGroup.alpha = 0;

            canvasGroup.blocksRaycasts = false;
        }

        private void OnDestroy()
        {
            LoadManager.Instance.OnLoadStarted -= Instance_OnLoadStarted;

            LoadManager.Instance.OnLoadInit -= Instance_OnLoadInit;
        }
    }
}