namespace Racer.SoundManager
{
    class SoundToggle : ToggleEffect
    {
        private void Start()
        {
            InitEffect();

            PlayEffect();
        }

        public override void Toggle()
        {
            base.Toggle();

            PlayEffect();
        }

        protected override void PlayEffect()
        {
            base.PlayEffect();

            switch (currentState)
            {
                case EffectState.Play:
                    SoundManager.Instance.MuteMusic(false);
                    break;
                case EffectState.Stop:
                    SoundManager.Instance.MuteMusic(true);
                    break;
            }
        }
    }
}
