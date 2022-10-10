using UnityEngine;

internal class PauseController : MonoBehaviour
{
    private bool _hasPaused;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        GameManager.OnCurrentState += GameManager_OnCurrentState;
    }

    private void GameManager_OnCurrentState(GameStates st)
    {
        if (st.Equals(GameStates.Exit))
            Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _gameManager.CurrentState == GameStates.Playing)
            SetPause();
    }

    public void SetPause()
    {
        switch (_hasPaused)
        {
            case false:
                Pause();
                break;
            case true:
                Resume();
                break;
        }
    }

    private void Pause()
    {
        _hasPaused = true;

        UIControllerGame.Instance.Pause(_hasPaused);

        Time.timeScale = 0;
    }

    private void Resume()
    {
        _hasPaused = false;

        UIControllerGame.Instance.Pause(_hasPaused);

        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}
