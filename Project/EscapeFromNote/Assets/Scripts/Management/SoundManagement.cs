using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagement : Manager<SoundManagement> {
    private AudioClip bgm_audio;
    private AudioClip false_audio;
    private AudioSource audioSource;

    private GameManagement.GameState currentState;
    private GameManagement.GameState previousState;

    public void SetCurrentState(GameManagement.GameState state) { this.currentState = state; }

    protected override void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        bgm_audio = Resources.Load("Raw/BGM_Audio") as AudioClip;
        false_audio = Resources.Load("Raw/False_Audio") as AudioClip;
        currentState = GameManagement.GameState.INIT;
        previousState = GameManagement.GameState.NULL;
        audioSource.loop = true;
        StartCoroutine(CheckState());
    }
    private void OnBackToTitle()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
    private void OnGameOver()
    {
        audioSource.clip = false_audio;
        audioSource.Play();
    }
    private void OnPlay()
    {
        
    }
    private void OnInitPlay()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
    private void OnTitle()
    {
        if (audioSource.clip != bgm_audio)
        {
            audioSource.clip = bgm_audio;
            audioSource.Play();
        }
    }

    private IEnumerator CheckState()
    {
        do
        {
            if (previousState != currentState)
            {
                previousState = currentState;
                switch (currentState)
                {
                    case GameManagement.GameState.TITLE:
                        OnTitle();
                        break;
                    case GameManagement.GameState.INIT_PLAY:
                        OnInitPlay();
                        break;
                    case GameManagement.GameState.PLAY:
                        OnPlay();
                        break;
                    case GameManagement.GameState.GAMEOVER:
                        OnGameOver();
                        break;
                    case GameManagement.GameState.BACK_TO_TITLE:
                        OnBackToTitle();
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        } while (true);
    }


}
