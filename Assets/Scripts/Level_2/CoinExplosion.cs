using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinExplosion : MonoBehaviour{
    private CoinDestruction coinDestruction;
    private GamePlay gamePlay;
    void Start(){
        coinDestruction  = GameObject.FindWithTag("GameManager").GetComponent<CoinDestruction>();
        gamePlay  = GameObject.FindWithTag("GameManager").GetComponent<GamePlay>();
    }

    void OnMouseDown(){
        coinDestruction.ManageClick(gameObject);
   }
   public void Explode(){
        for(int x=0; x<Constants.CoinsPerAxis; ++x){
            for(int y=0; y<Constants.CoinsPerAxis; ++y){
                for(int z = 0; z<Constants.CoinsPerAxis; ++z){
                    createParticles(new Vector3(x, y, z));
                }
            }
        }
        --gamePlay.totalNumber;
        gameObject.SetActive(false);
        int position = gamePlay.coinInstances.IndexOf(gameObject);
        gamePlay.MASK -= 1<<position ;
        
   }

   private void createParticles(Vector3 coordinates){
        GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Cube);
        coin.AddComponent<AutoDestroy>();
        Renderer rd = coin.GetComponent<Renderer>() ;
        rd.material = GetComponent<Renderer>().material ;
        coin.transform.localScale = transform.localScale/Constants.CoinsPerAxis ;
        Vector3 first_coin = transform.position - transform.localScale/2 + coin.transform.localScale/2 ;
        coin.transform.position = first_coin+ Vector3.Scale(coordinates, coin.transform.localScale);

        //explosion
        Rigidbody rb = coin.AddComponent<Rigidbody>() ;
        rb.AddExplosionForce(Constants.Force, transform.position, Constants.Radius);
   }
}
