using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDestruction : MonoBehaviour{

    public int NbCoinsDestroyed = 0;
    public bool Exist = true;

    private static int LastPosition = -1;
    private int position;
    private GamePlay gamePlay;

    void Start(){
        gamePlay = GetComponent<GamePlay>();
    }

    public void ManageClick(GameObject clickedCoin){
        if(gamePlay.PlayerTurn){
            position = gamePlay.coinInstances.IndexOf(clickedCoin);
            bool adjacency = verifyAdjacency();
            if(adjacency){
                destroy(clickedCoin);
                updateAllVariables();  

            } else{
                Debug.Log("Please choose adjacent diamonds !");
            }
        }
    }

    private void updateAllVariables(){
        ++NbCoinsDestroyed; // nb coins destroyed by player
        Exist = adjacencyExist();
        gamePlay.checkTurns();
        LastPosition = (gamePlay.FinishedAttempts) ? -1 : position;
    }

    private void destroy(GameObject coin){
        coin.GetComponent<CoinExplosion>().Explode();
    }

    private bool adjacencyExist(){
        int beforePosition =( position  + Constants.N -1)% Constants.N ;
        int afterPosition = (position +1 )% Constants.N ;
        return ((1<<beforePosition & gamePlay.MASK )!=0)|| ((1<<afterPosition & gamePlay.MASK) != 0) ;
    }

    public void OnFinishButtonClicked(){
        LastPosition = -1 ; 
    }

    private bool verifyAdjacency(){
        if(LastPosition == -1){
            return true;
        }
        int dx = Mathf.Abs(position-LastPosition);
        return (dx == 1) || (dx == (Constants.InitialNumber-1)); 
    }

}
