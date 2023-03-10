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
    public static List<Vector3> allPosfruit = new List<Vector3>(){
        new Vector3(0,0,0),
        new Vector3(-10.49f,0,0),
        new Vector3(0,4f,0),
        new Vector3(0,8.5f,0),
        new Vector3(0f,6.5f,0f),
        new Vector3(0,-4f,0f),
        new Vector3(-10f,-4f,0f),
    };
    public List<GameObject> allFruit;
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
        Quaternion rotation = Quaternion.LookRotation(position);
        GameObject slicedFruit = Instantiate(sliced, transform.position, rotation);
        Rigidbody[] slices = slicedFruit.GetComponentsInChildren<Rigidbody>();
        //Add a force to each slice based on the blade direction
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
                allPosfruit.Remove(this.transform.position);
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
        Slice(new Vector3(0f, 0f, 0f), gameObject.transform.position, 25f);
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
