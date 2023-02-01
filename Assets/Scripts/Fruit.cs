using System.IO;
using UnityEngine;
using System;
public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;
    public GameObject[] fruits;
    private Rigidbody fruitRigidbody;
    public int totalFruit = 7;
    public static int nbrFruit = 7;
    private Collider fruitCollider;
    public static int i = 0;
    public int[] memo = new int[1000];
    void fill_n()
    {
        for (int i = 0; i <= memo.Length; ++i)
        {
            memo[i] = 0;
        }
    }
    private ParticleSystem juiceEffect;

    public int points = 1;

    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceEffect = GetComponentInChildren<ParticleSystem>();
        fill_n();
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        //FindObjectOfType<GameManager>().IncreaseScore(points);

        // Disable the whole fruit
        fruitCollider.enabled = false;
        whole.SetActive(false);

        // Enable the sliced fruit
        sliced.SetActive(true);
        juiceEffect.Play();

        // Rotate based on the slice angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

        // Add a force to each slice based on the blade direction
        foreach (Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
            --nbrFruit;
            Debug.Log(nbrFruit);
        }
        ///
        computerTurn();
    }

    void computerTurn()
    {
        int nbrChoosenComputer = best_choise(nbrFruit, false);
        Debug.Log(nbrChoosenComputer);
        // Vector3 direction = new Vector3(9.59f, 0f, 0f);
        // Slice(direction, direction, 25f);
    }
    int best_choise(int rest, bool player_turn)
    {
        if (rest == 1u)
        {
            return -1;
        }
        if (memo[rest] == 0)
        {
            memo[rest] = -1;
            for (int take = 1; take <= 3; ++take)
            {
                if (best_choise(rest - take, !player_turn) == -1 && rest - take > 0)
                {
                    memo[rest] = take;
                    break;
                }
            }
        }
        return memo[rest];
    }

}
