using UnityEngine;

namespace Racer.LoadManager
{
    internal class LoadingFx : MonoBehaviour
    {
        private ParticleSystem fx;

        private void Awake()
        {
            fx = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            fx.Play();

            LoadManager.Instance.OnLoadFinished += Instance_OnLoadFinished;
        }

        private void Instance_OnLoadFinished()
        {
            fx.Stop();
        }
    }
}
