using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerationNeural : MonoBehaviour
{
    public GameObject iaprefab;
    public int karts=10;
    public List <IANeuralInput> iaKarts;
    public float bestCheckpoint;
    public int currentRun = 0;
    [Space(10)]
    [Header("Training Parameters")]
    public int trainingBatchSize = 4;
    public int trainingEpochs = 100;
    public float trainingRate = 1f;

   

    Noedify_Solver trainingSolver;

    const int N_threads = 8;

    public List<Hoppy_Brain.PlayerObservationDecision> trainingSet;
    public Noedify.Net net;

    void Start()
    {
        trainingSolver = Noedify.CreateSolver();
        trainingSet = new List<Hoppy_Brain.PlayerObservationDecision>();
        BuildNetwork();

        
    }

    public void StartSim()
    {
        iaKarts = new List<IANeuralInput>();

        for (int i = 0; i < karts; i++)
        {
            GameObject kart =
            Instantiate(iaprefab, transform.position, transform.rotation);
            kart.GetComponent<IANeuralInput>().genneural = this;
            iaKarts.Add( kart.GetComponent<IANeuralInput>());
            
            
        }
        ResetSim();
    }
    void BuildNetwork()
    {
        net = new Noedify.Net();

        /* Input layer */
        Noedify.Layer inputLayer = new Noedify.Layer(
            Noedify.LayerType.Input, // layer type
            2, // input size
            "antenas layer" // layer name
            );
        net.AddLayer(inputLayer);

        // Hidden layer 1 
        Noedify.Layer hiddenLayer0 = new Noedify.Layer(
            Noedify.LayerType.FullyConnected, // layer type
            150, // layer size
            Noedify.ActivationFunction.Sigmoid, // activation function
            "fully connected 1" // layer name
            );
        net.AddLayer(hiddenLayer0);

        /* Output layer */
        Noedify.Layer outputLayer = new Noedify.Layer(
            Noedify.LayerType.Output, // layer type
            2, // layer size
            Noedify.ActivationFunction.Sigmoid, // activation function
            "output layer" // layer name
            );
        net.AddLayer(outputLayer);

        net.BuildNetwork();
    }
   


    // Update is called once per frame
    void Update()
    {
       
    }


    void ResetSim()
    {
        int bestcheckpoint = -1;
        IANeuralInput bestcart = null;
        currentRun++;
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

            iaKarts[0].Reset();
        }
        
    }
    private void oldOnGUI()
    {

        if (EditorApplication.isPaused)
        {

            StartCoroutine(TrainNetwork());

            print("pause");
            int bestcheckpoint=-1;
            IANeuralInput bestcart = null;
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
                

                var cubeRenderer = bestcart.gameObject.GetComponentInChildren<Renderer>();
                cubeRenderer.material.SetColor("_Color", Color.red);
                //bestiaturnDNA = bestcart.GetDNA();
                //PlayerPrefsX.SetFloatArray("BestIA", bestiaturnDNA);
            }
        }
        
    }


    public IEnumerator TrainNetwork()
    {
        if (trainingSet != null)
        {
            if (trainingSet.Count > 0)
            {
                while (trainingSolver.trainingInProgress) { yield return null; }
                List<float[,,]> observation_inputs = new List<float[,,]>();
                List<float[]> decision_outputs = new List<float[]>();
                List<float> trainingSetWeights = new List<float>();
                for (int n = 0; n < trainingSet.Count; n++)
                {
                    observation_inputs.Add(Noedify_Utils.AddTwoSingularDims(trainingSet[n].observation));
                    decision_outputs.Add(trainingSet[n].decision);
                    trainingSetWeights.Add(trainingSet[n].weight);
                }
                trainingSolver.TrainNetwork(net, observation_inputs, decision_outputs, trainingEpochs, trainingBatchSize, trainingRate, Noedify_Solver.CostFunction.MeanSquare, Noedify_Solver.SolverMethod.MainThread, trainingSetWeights, N_threads);
            }
        }
    }
}
