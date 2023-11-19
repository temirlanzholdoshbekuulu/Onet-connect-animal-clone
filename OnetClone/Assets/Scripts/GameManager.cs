using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int remainedTiles = 128;
    void Start()
    {
        
    }

    void Update()
    {
        if(remainedTiles == 0)
        {
            Debug.Log("Win!");
        }
        if(Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
