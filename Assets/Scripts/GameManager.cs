using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ElementManager elementManager;
    [SerializeField] private GameController gameController;
    
    private void Start()
    {
        if (elementManager == null)
        {
            Debug.LogError("ElementManager is not assigned in the inspector.");
            return;
        }
        if (gameController == null)
        {
            Debug.LogError("GameController is not assigned in the inspector.");
            return;
        }
        
        elementManager.InitElementManager(gameController);
        gameController.StartCycle();
    }
}
