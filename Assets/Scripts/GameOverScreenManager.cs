using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenManager : MonoBehaviour
{
    public Canvas ContinueButtonCanvas;
    public GameObject ContinueButton;
    public GameObject WatchAdButton;

    public GameObject SoundButton;
    public Sprite SoundOnImage;
    public Sprite SoundOffImage;

    private Button continueButton;
    private Button watchAdButton;
    private Button soundButton;

    private bool soundOn;

    // Start is called before the first frame update
    void Start()
    {
        continueButton = ContinueButton.GetComponent<Button>();
        watchAdButton = WatchAdButton.GetComponent<Button>();
        soundButton = SoundButton.GetComponent<Button>();

        soundOn = true;
        soundButton.image.sprite = SoundOnImage;
    }

    public void ToogleSound()
    {
        soundOn = !soundOn;

        if(soundOn)
        {
            AudioListener.volume = 1.0f;
            soundButton.image.sprite = SoundOnImage;
        }
        else
        {
            AudioListener.volume = 0.0f;
            soundButton.image.sprite = SoundOffImage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RewardManager.instance.GetGamesLeft() == 0)
        {
            if(continueButton.gameObject.activeSelf)
            {
                continueButton.gameObject.SetActive(false);
            }
            
            if(!watchAdButton.gameObject.activeSelf)
            {
                watchAdButton.gameObject.SetActive(true);
            }
            
        }
        else
        {
            watchAdButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(true);

            if (!GameManager.instance.GetContinueValue())
            {
                continueButton.interactable = false;
                ContinueButtonCanvas.GetComponent<CanvasGroup>().alpha = 0.2f;
            }
            else
            {
                continueButton.interactable = true;
                ContinueButtonCanvas.GetComponent<CanvasGroup>().alpha = 1f;
            }
        }
    }

}
