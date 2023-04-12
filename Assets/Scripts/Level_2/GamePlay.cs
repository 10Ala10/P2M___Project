using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GamePlay : MonoBehaviour
{
    [SerializeField]
    private GameObject[] coinPrefabs;

    [SerializeField]
    private GameObject coinModel;

    [SerializeField]
    private GameObject finishButton;

    [SerializeField]
    private GameObject checkFirstPlayer;
    public GameObject gameoverUI;
    public GameObject winUI;
    public GameObject pauseMenuUI;
    public bool PlayerTurn = true;
    public bool finishButtonClicked = false;
    private bool computerPlayed = false;
    public bool destroyedMax = false;
    private bool computerTurn = false;
    public bool FinishedAttempts = false;

    public int totalNumber;

    public List<GameObject> coinInstances = new List<GameObject>();

    private int[] memo = new int[Constants.N];

    public int MASK;

    private CoinDestruction coinDestruction;

    void Start()
    {
        initVariables();
        Invoke("createCoins", Constants.TurnDelay + 3);
        createCoins();
    }

    void Update()
    {
        checkTurns();
        if (computerTurn)
        {
            Invoke("computerPlay", Constants.TurnDelay);
            computerPlayed = true;
        }
        updateFinishButtonState();
        checkGameOver();
    }

    void checkGameOver()
    {
        if (MASK == 0)
        {
            pauseMenuUI.SetActive(false);
            if (PlayerTurn)
            {
                displayGameOver();
                gameoverUI.SetActive(true);
            }
            else
            {
                displayCongratulations();
                winUI.SetActive(true);
            }

        }
    }

    public void displayGameOver()
    {
        //gameOver.SetActive(true);
        Debug.Log("GameOver You Lose");
    }

    public void displayCongratulations()
    {
        // congrats.SetActive(true);
        Debug.Log("Congratulaions You win");
    }

    public void RestartGame()
    {
    }

    public void checkTurns()
    {
        destroyedMax = coinDestruction.NbCoinsDestroyed == Constants.MaxDestruction;
        // The player has either played all attempts or has clicked the finish button
        FinishedAttempts = destroyedMax || finishButtonClicked || !coinDestruction.Exist;

        PlayerTurn = !FinishedAttempts && !computerTurn;
        computerTurn = FinishedAttempts && !computerPlayed;
    }

    void OnYesButtonClicked()
    {
        PlayerTurn = true;
        HideGameObjects();
    }

    void OnNoButtonClicked()
    {
        PlayerTurn = false;
        HideGameObjects();
    }

    private void HideGameObjects()
    {
        checkFirstPlayer.SetActive(false);
    }

    private void initVariables()
    {
        coinDestruction = GetComponent<CoinDestruction>();
        totalNumber = Constants.InitialNumber;
        MASK = Constants.MaxMask;
        for (int i = 0; i <= Constants.MaxMask; ++i)
        {
            memo[i] = -1;
        }
    }

    private int selection(int mask)
    {
        if (mask == 0)
        {
            return memo[mask] = 0;
        }
        if (memo[mask] != -1)
        {
            return memo[mask];
        }
        int i = 1, j;
        while (i <= mask)
        {
            if ((mask & i) != 0)
            {
                if (selection(mask - i) == 0)
                {
                    return memo[mask] = i;
                }
                j = i << 1;
                if (j > Constants.MaxMask)
                {
                    j = 1;
                }
                if ((mask & j) != 0)
                {
                    if (selection(mask - (i + j)) == 0)
                    {
                        return memo[mask] = i + j;
                    }
                }
                else
                {
                    i <<= 1;
                }
            }
            i <<= 1;
        }
        return 0;
    }

    private int firstActiveCoin(int mask)
    {
        int selectedMask = 1;
        while ((MASK & selectedMask) == 0)
        {
            selectedMask <<= 1;
        }
        return selectedMask;
    }

    private List<int> positionsSelectedByComputer()
    {
        List<int> positions = new List<int>();
        int selectedMask = selection(MASK);
        if (selectedMask == 0)
        {
            selectedMask = firstActiveCoin(MASK);
        }
        int i = 0;
        while ((1 << i) <= selectedMask)
        {
            if ((selectedMask & (1 << i)) != 0)
            {
                positions.Add(i);
            }
            ++i;
        }
        return positions;
    }

    private void computerPlay()
    {
        if (MASK != 0)
        {
            List<int> positions = positionsSelectedByComputer();
            foreach (var id in positions)
            {
                if (coinInstances[id].activeSelf)
                {
                    coinInstances[id].GetComponent<CoinExplosion>().Explode();
                }
            }
            coinDestruction.NbCoinsDestroyed = 0;
            computerPlayed = false;
            coinDestruction.Exist = true;
            finishButtonClicked = false;
            FinishedAttempts = false;
        }
    }

    private void updateFinishButtonState()
    {
        finishButton.GetComponent<Button>().interactable = coinDestruction.NbCoinsDestroyed != 0;
    }

    public void OnFinishButtonClicked()
    {
        finishButtonClicked = true;
    }

    public void createCoins(int n_coins = Constants.InitialNumber)
    {
        float arc = 0.8f;
        float radius = n_coins * arc / (2 * Mathf.PI);
        for (int i = 1; i <= n_coins; ++i)
        {
            float angle = 2 * Mathf.PI * i / n_coins;
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);
            Vector3 pos = transform.position + new Vector3(0, x, z);
            float angleDegree = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(angleDegree, 0, 0);
            GameObject coinClone = Instantiate(coinModel, pos, rot);

            coinClone.name = "Coin " + i;
            coinClone.SetActive(true);
            coinInstances.Add(coinClone);
        }
    }

}
