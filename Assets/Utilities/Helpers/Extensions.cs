using System.Collections;
using UnityEngine;

namespace Racer.Utilities
{
    public static class Extensions
    {
        public static IEnumerator ShakePosition(this Transform myTransform, float duration, float magnitude)
        {
            // Vector3 originalPosition = myTransform.localPosition; // Issue for non-static cameras
            Vector3 originalPosition = new(0, 0, myTransform.position.z); // Only for static cameras

            var elapsed = 0f;

            while (elapsed < duration)
            {
                var x = Random.Range(-1f, 1f) * magnitude;
                var y = Random.Range(-1f, 1f) * magnitude;

                myTransform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

                elapsed += Time.deltaTime;

                yield return 0;
            }
            myTransform.position = originalPosition;
        }

        public static void ToggleActive(this GameObject gameObject, bool state)
        {
            if (state)
            {
                if (!gameObject.activeInHierarchy)
                    gameObject.SetActive(true);
            }
            else
            {
                if (gameObject.activeInHierarchy)
                    gameObject.SetActive(false);
            }
        }
    }
}