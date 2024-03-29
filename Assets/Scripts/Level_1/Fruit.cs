using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;
    private Rigidbody fruitRigidbody;
    public static int nbrFruit = 7, fruits_;
    private Collider fruitCollider;
    public int[] memo = new int[1000];
    public bool finishTurn = false;
    public static int nbrSliced = 0;
    public int k = 3;
    public bool finishCLick = false;
    private ParticleSystem juiceEffect;
    public static int nbrChoosenComputer = 0;
    public GameObject winUI;
    public GameObject gameoverUI;
    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceEffect = GetComponentInChildren<ParticleSystem>();
        fill_n();
    }

    private void Update()
    {
        verif();
        if (nbrFruit == 0)
        {
            winUI.SetActive(true);
        }
        nbrChoosenComputer = Math.Abs(best_choise(nbrFruit, false));
        if (finishTurn == true)
        {
            computerTurn();
            --nbrChoosenComputer;
        }
        if (nbrChoosenComputer == 0)
        {
            nbrSliced = 0;
            finishButton.isClicked = false;
        }
        if (nbrFruit == 0)
        {
            gameoverUI.SetActive(true);
        }
    }
    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        fruitCollider.enabled = false;
        whole.SetActive(false);
        sliced.SetActive(true);
        juiceEffect.Play();
        Quaternion rotation = Quaternion.LookRotation(position);
        GameObject slicedFruit = Instantiate(sliced, transform.position, rotation);
        Rigidbody[] slices = slicedFruit.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
        Destroy(slicedFruit, 3f);
        Destroy(gameObject);
    }

    void fill_n()
    {
        for (int i = 0; i < nbrFruit; ++i)
        {
            memo[i] = 0;
        }
    }

    void verif()
    {
        finishTurn = (nbrSliced == k) || (finishButton.isClicked == true);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            if (nbrSliced <= k)
            {
                ++nbrSliced;
                Slice(blade.direction, blade.transform.position, blade.sliceForce);
                //Destroy(gameObject);
                --nbrFruit;
                Debug.Log("The player removed " + this.transform.position);
                Debug.Log("remaining after playerTurn :" + nbrFruit);
            }
            else
            {
                Debug.Log("Permission Denied");
            }
        }
    }

    void computerTurn()
    {
        Debug.Log("Computer select: " + nbrChoosenComputer);
        Slice(new Vector3(5f, 2f, 0f), gameObject.transform.position, 2f);
        //Destroy(gameObject);
        --nbrFruit;
        Debug.Log("remaining after computerTurn" + nbrFruit);
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
