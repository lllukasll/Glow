using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void HomeClicked()
    {
        AdController.instance.ShowRewardedVideo();
    }
}
