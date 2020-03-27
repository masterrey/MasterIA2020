using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerationAdmin : MonoBehaviour
{
    public GameObject iaprefab;
    public int karts=10;
    public IAInput[] iaKarts;
    public float[] bestiaturnDNA;
    void Start()
    {
        if (PlayerPrefs.HasKey("BestIA"))
        {
            bestiaturnDNA = PlayerPrefsX.GetFloatArray("BestIA");
            print("tem save");
        }

           iaKarts = new IAInput[karts];
        for (int i = 0; i < karts; i++)
        {
            GameObject kart=
            Instantiate(iaprefab, transform.position, transform.rotation);
            iaKarts[i] = kart.GetComponent<IAInput>();
            if (bestiaturnDNA.Length>0)
            {
                print("colocando o melhor");
                iaKarts[i].SetDNA(bestiaturnDNA);
            }
            else
            {
                
                iaKarts[i].CreateDNA();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnGUI()
    {
        if (EditorApplication.isPaused)
        {
            print("pause");
            int bestcheckpoint=-1;
            IAInput bestcart = null;
            for (int i = 0; i < karts; i++)
            {

                if (bestcheckpoint < iaKarts[i].checkpoint)
                {
                    bestcheckpoint = iaKarts[i].checkpoint;
                    bestcart = iaKarts[i];
                }


            }
            if (bestcheckpoint > 0)
            {
                print("salvando melhor");
                bestiaturnDNA = bestcart.GetDNA();
                PlayerPrefsX.SetFloatArray("BestIA", bestiaturnDNA);
            }
        }
        
    }

}
