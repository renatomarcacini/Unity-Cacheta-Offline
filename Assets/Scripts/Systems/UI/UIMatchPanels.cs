using UnityEngine;

public class UIMatchPanels : MonoBehaviour
{
    [field: SerializeField] public GameplayUI GameplayUI { get; private set; }
    [field: SerializeField] public PauseUI PauseUI { get; private set; }
    [field: SerializeField] public MatchEndUI MatchEndUI { get; private set; }

    public void DisableAll()
    {
        GameplayUI?.Disable();
        PauseUI?.Disable();
        MatchEndUI?.Disable();
    }
}
