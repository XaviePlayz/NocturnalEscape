using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isGameFrozen = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsGameFrozen
    {
        get { return isGameFrozen; }
    }

    public void FreezeGame()
    {
        isGameFrozen = true;
        Time.timeScale = 0f;
    }

    public void UnfreezeGame()
    {
        isGameFrozen = false;
        Time.timeScale = 1f;
    }
}
