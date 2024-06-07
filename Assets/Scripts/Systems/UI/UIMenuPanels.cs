using UnityEngine;

public class UIMenuPanels : MonoBehaviour
{
    [field: SerializeField] public MenuUI MenuUI { get; private set; }

    public void DisableAll()
    {
        MenuUI?.Disable();
    }
}
