using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [HideInInspector] private bool menuIsShown;
    [HideInInspector] private bool isVictoryScreen;

    [Header("Sounds")]
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource victorySound;
    [SerializeField] private AudioSource gameOverSound;

    [Header("Elements")]
    [SerializeField] private GameObject menuBackground;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject restartButton;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        if (menuIsShown)
        {
            Time.timeScale = 0;

            menuBackground.SetActive(true);
            restartButton.SetActive(true);

            if (isVictoryScreen)
            {
                winText.SetActive(true);
                victorySound.Play();
            }
            else
            {
                loseText.SetActive(true);
                gameOverSound.Play();
            }
        }
    }

    public void ShowGameOverScreen(bool isVictory)
    {
        menuIsShown = true;
        isVictoryScreen = isVictory;

        bgm.Stop();
    }
}
