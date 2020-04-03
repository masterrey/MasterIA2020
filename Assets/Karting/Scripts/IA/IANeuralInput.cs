using KartGame.KartSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IANeuralInput : MonoBehaviour , IInput
{
    public float Acceleration
    {
        get { return m_Acceleration; }
    }
    public float Steering
    {
        get { return m_Steering; }
    }
    public bool BoostPressed
    {
        get { return m_BoostPressed; }
    }
    public bool FirePressed
    {
        get { return m_FirePressed; }
    }
    public bool HopPressed
    {
        get { return m_HopPressed; }
    }
    public bool HopHeld
    {
        get { return m_HopHeld; }
    }

   

    float m_Acceleration;
   
    float m_Steering;
    bool m_HopPressed;
    bool m_HopHeld;
    bool m_BoostPressed;
    bool m_FirePressed;
   
    public float[] iaturnDNA;
    float timeindex;
    public int checkpoint;
    public int oldCheckpoint;
    Rigidbody rdb;
    Noedify_Solver evalSolver;
    public GenerationNeural genneural;

    List<float[,,]> observation_inputs;
    List<float[]> decision_outputs;
    List<float> decision_weights;

    public class PlayerObservationDecision
    {
        public float[] observation;
        public float[] decision;
        public float weight;
    }

    PlayerObservationDecision decisionFrame;

    void Start()
    {

      
        observation_inputs = new List<float[,,]>();
        decision_outputs = new List<float[]>();
        decision_weights = new List<float>();
        evalSolver = Noedify.CreateSolver();

        // PlayerPrefs.DeleteAll();
        rdb = gameObject.GetComponent<Rigidbody>();

      
    }

  

    // Update is called once per frame
    void Update()
    {
        m_Acceleration = 1;

        if (checkpoint != oldCheckpoint)
        {
            decisionFrame = new PlayerObservationDecision();
            decisionFrame.observation = AcquireObservations();
            decisionFrame.decision = new float[2];
            float[] newRandomDecision = new float[2];
            float[] newAIDecision = new float[2];
            newAIDecision = AIDecision(decisionFrame.observation);

            print("newdecision");

            m_Steering = -newAIDecision[0] + newAIDecision[1];

            oldCheckpoint = checkpoint;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPointIA"))
        {

            checkpoint++;
        }
    }


    float[] AcquireObservations()
    {
       
        float[] sensorInputs = new float[2];

        if(Physics.Raycast(transform.position,transform.forward+transform.right,out RaycastHit hit, 10))
        {
            sensorInputs[0] = hit.distance-5;
            Debug.DrawLine(transform.position, hit.point);
        }
        else
        {
            sensorInputs[0] = 0;
        }

        if (Physics.Raycast(transform.position, transform.forward - transform.right, out RaycastHit hit2, 10))
        {
            sensorInputs[1] = hit2.distance - 5;
            Debug.DrawLine(transform.position, hit2.point);
        }
        else
        {
            sensorInputs[1] = 0;
        }




        return sensorInputs;
    }

    internal void Reset()
    {
        throw new NotImplementedException();
    }

    // Evaluate netowrk to generate decision
    float[] AIDecision(float[] observation)
    {
        evalSolver.Evaluate(genneural.net, Noedify_Utils.AddTwoSingularDims(observation), Noedify_Solver.SolverMethod.MainThread);
        return evalSolver.prediction;
    }
}
