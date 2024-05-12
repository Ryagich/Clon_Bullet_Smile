using UnityEngine;
using UnityEngine.InputSystem;

public class ActionMapSwitcher : MonoBehaviour
{
    public void ChangeMap(string mapName)
    {
        SetActionMap(mapName);
    }
    
    private static void SetActionMap(string mapName)
    {
        var playerInput = FindObjectsByType<PlayerInput>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (playerInput.Length == 1)
        {
            playerInput[0].SwitchCurrentActionMap(mapName);
        }
    }
}
