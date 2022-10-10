using UnityEngine;

namespace Racer.SoundManager
{
    /// <summary>
    /// Plays a specific sound-effect.
    /// </summary>
    internal class ButtonSfx : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;

        public void Play()
        {
            // Usage
            SoundManager.Instance.PlaySfx(clip);
        }
    }
}