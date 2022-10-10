using UnityEngine;

namespace Racer.LoadManager
{
    /// <summary>
    /// Simulates loading circle.
    /// Automatically starts simulating when the containing gameobject is active.
    /// Containing gameobject must be inactive for this to work appropriately.
    /// </summary>
    internal class LoadingCircle : MonoBehaviour
    {
        private RectTransform mainIcon;

        private Vector3 iconAngle;

        private float startTime;

        [SerializeField] private float timeStep = 0.03f;

        [SerializeField] private float oneStepAngle = 30;



        private void Awake()
        {
            mainIcon = GetComponent<RectTransform>();

            startTime = Time.time;
        }

        private void Start()
        {
            LoadManager.Instance.OnLoadFinished += Instance_OnLoadFinished;
        }


        private void Update()
        {
            // TODO: Find a smoother way to interpolate the rotation.
            if (!(Time.time - startTime >= timeStep)) return;

            iconAngle = mainIcon.localEulerAngles;

            iconAngle.z += oneStepAngle;

            mainIcon.localEulerAngles = iconAngle;

            startTime = Time.time;
        }

        private void Instance_OnLoadFinished()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            startTime = 0;
        }
    }
}