using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{   
    [SerializeField] Button continueButton;
    
    bool isEnabled = false;
    
    void Start()
    {
        GameManager.OnLevelStart += EnableButton;
    }

    void EnableButton()
    {
        if (continueButton == null) return;
        continueButton.interactable = true;
        GameManager.OnLose += DisableButton;
    }

    void DisableButton()
    {
        if (continueButton == null) return;
        continueButton.interactable = false;
        GameManager.OnLose -= DisableButton;
    }
}
