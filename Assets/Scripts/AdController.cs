using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;

public class AdController : MonoBehaviour
{
    public static AdController instance;

    private string store_id = "3305731";
    private string rewarder_video_ad = "rewardedVideo";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if(Monetization.isSupported)
        {
            Monetization.Initialize(store_id, true);
        }
    }

    public void ShowRewardedVideo()
    {
        StartCoroutine(WaitForAd());
    }

    private IEnumerator WaitForAd()
    {
        while(!Monetization.IsReady(rewarder_video_ad))
        {
            yield return null;
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(rewarder_video_ad) as ShowAdPlacementContent;

        if (ad != null)
        {
            ad.Show(AdFinished);
        }
    }

    private void AdFinished(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Reward the player");
            RewardManager.instance.AddGames(5);
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("The player skipped the video - DO NOT REWARD!");
        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }
}
