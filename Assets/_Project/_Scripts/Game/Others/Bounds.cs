using Racer.Utilities;
using UnityEngine;

internal class Bounds : MonoBehaviour
{
    // Can be re-factored to use arrays instead.
    [SerializeField] private BoxCollider2D left, right, top;

    // Snap collider-s to screen edges, irrespective of the screen's dimension.
    private void Start()
    {
        left.transform.position = new Vector3(-ScreenPos.x - left.size.x / 2, 0, 0);
        right.transform.position = new Vector3(ScreenPos.x + left.size.x / 2, 0, 0);
        top.transform.position = new Vector3(0, ScreenPos.y + top.size.y / 2, 0);

        GameManager.OnCurrentState += GameManager_OnCurrentState;
    }

    private void GameManager_OnCurrentState(GameStates gs)
    {
        if (!gs.Equals(GameStates.Playing)) return;

        left.isTrigger = false;
        right.isTrigger = false;
        top.isTrigger = false;
    }

    private static Vector2 ScreenPos => Utility.ScreenDimension;

    private void OnDestroy() => GameManager.OnCurrentState -= GameManager_OnCurrentState;
}
