using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameDivisor : MonoBehaviour
{
    [SerializeField] private Text consoleText;
    [SerializeField] private InputField inputField;
    public int n = 100;

    private bool isPlayerTurn;

    void Start()
    {
        consoleText.text = "Game started! Your turn first.";
        isPlayerTurn = true;
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (n > 1)
        {
            yield return isPlayerTurn ? StartCoroutine(PlayerTurn()) : StartCoroutine(ComputerTurn());
        }

        consoleText.text = isPlayerTurn ? "Game over. You lose." : "Game over. You win!";
    }

    private IEnumerator PlayerTurn()
    {
        consoleText.text = "Your turn. Choose a number x between 1 and " + (n - 1) + ".\nNote: x must verify that " + n + " % x == 0";
        PrintDivisors();

        bool isDone = false;

        // Wait for the player to finish entering the number
        inputField.ActivateInputField();
        inputField.text = "";
        inputField.onEndEdit.AddListener((value) =>
        {
            isDone = true;
        });

        yield return new WaitUntil(() => isDone);

        int x = GetPlayerInput();

        while (x == 0 || x == n || n % x != 0)
        {
            consoleText.text = "You can't choose " + x + ". Please choose another number.";

            isDone = false;
            inputField.ActivateInputField();
            inputField.text = "";
            inputField.onEndEdit.AddListener((value) =>
            {
                isDone = true;
            });

            yield return new WaitUntil(() => isDone);
            x = GetPlayerInput();
        }

        n = n - x;
        consoleText.text = "New n = " + n;
        isPlayerTurn = false;
    }

    private IEnumerator ComputerTurn()
    {
        int x = chooseNumber();
        consoleText.text = "Computer chooses " + x;
        yield return new WaitForSeconds(1.5f);

        n = n - x;
        consoleText.text += "\nNew n = " + n;
        isPlayerTurn = true;
    }

    private void PrintDivisors()
    {
        List<int> divisors = new List<int>();

        for (int i = 1; i * i <= n; i++)
        {
            if (n % i == 0)
            {
                divisors.Add(i);

                if (i != n / i && i != 1)
                {
                    divisors.Add(n / i);
                }
            }
        }

        consoleText.text += "\n" + string.Join(" ", divisors);
    }

    private int GetPlayerInput()
    {
        int x = 0;
        int.TryParse(inputField.text.Trim(), out x);
        inputField.text = "";
        return x;
    }


    private int chooseNumber()
    {
        int ans = 1;

        for (int i = 1; i < n; ++i)
        {
            if (n % i == 0 && ((n - i) & 1) == 1)
            {
                ans = i;
            }
        }

        return ans;
    }
}
