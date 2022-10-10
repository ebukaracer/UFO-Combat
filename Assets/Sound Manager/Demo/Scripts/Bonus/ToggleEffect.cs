using UnityEngine;
using UnityEngine.UI;

namespace Racer.SoundManager
{
    enum EffectState
    {
        Play,
        Stop,

        // Length(+1) of the enum, Ignore.
        Count
    }

    abstract class ToggleEffect : MonoBehaviour
    {
        // Placeholder for the current effect.
        // This can be replaced with a bool.
        protected int effectIndex;


        public EffectState currentState;

        // Handy if you'd save the effect's current state.
        public string saveString;
        [Space(10)]

        // Parent sprite that'd be replaced.
        public Image parentIcon;

        // Off/On Sprites that'd be in-place of parent sprite.
        public Sprite[] offOnIcons;


        /// <summary>
        /// Retrieves the save-state of the current effect.
        /// Should be called on the Start or Awake function.
        /// </summary>
        protected virtual void InitEffect()
        {
            // Replace with custom save class if present.
            effectIndex = PlayerPrefs.GetInt(saveString, 1);
        }

        /// <summary>
        /// Toggles the current effect On/Off.
        /// Should be assigned to a button, in other to achieve a toggle effect.
        /// </summary>
        public virtual void Toggle()
        {
            effectIndex++;

            // (% = '2') => for on/off
            effectIndex %= (int)EffectState.Count;

            // Replace with custom save class if present.
            PlayerPrefs.SetInt(saveString, effectIndex);
        }

        /// <summary>
        /// Plays the current effect.
        /// Override this method to add extra logic.
        /// </summary>
        protected virtual void PlayEffect()
        {
            // 1 = play, 0 = stop
            currentState = effectIndex == 1 ? EffectState.Play : EffectState.Stop;

            parentIcon.sprite = offOnIcons[effectIndex];
        }
    }
}
