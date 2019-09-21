using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryButton : MonoBehaviour
{

    public GameManager GameManager;
    
    public void RetryClicked()
    {
        GameManager.Retry();
    }
    
}
