using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    // Start is called before the first frame update
    public string playerName;
    public int score;

    private static Recorder Instence;
    private void Awake()
    {
        if(Instence != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instence = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
