﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour {

	Agent testAgent;
	private List<Agent> agents;
	public float currentAgentFitness;
	public float bestFitness;
	private float currentTimer;
	private int checkPointsHit;

	public NNet neuralNet;

	public GA genAlg;
	public int checkpoints;

	private Vector3 defaultpos;
	private Quaternion defaultrot;

	hit hit;
	// Use this for initialization
	void Start () {

		genAlg = new GA ();
		int totalWeights = 5 * 8 + 8 * 2 + 8 + 2;
		genAlg.GenerateNewPopulation (15, totalWeights);
		currentAgentFitness = 0.0f;
		bestFitness = 0.0f;

		neuralNet = new NNet ();
		Genome genome = genAlg.GetNextGenome ();
		neuralNet.FromGenome (ref genome, 5, 8, 2);
		genAlg.SetThisGenome (genome);

		testAgent = gameObject.GetComponent<Agent>();
		testAgent.Attach (neuralNet);

		hit = gameObject.GetComponent<hit> ();
		checkpoints = hit.checkpoints;
		defaultpos = hit.init_pos;
		defaultrot = hit.init_rotation;
	}

	// Update is called once per frame
	void Update () {
		if (testAgent.hasFailed) {
			if(genAlg.GetCurrentGenomeIndex() == 15-1){
				EvolveGenomes();
				return;
			}
			NextTestSubject();
		}
		currentAgentFitness += testAgent.dist;
		if (currentAgentFitness > bestFitness) {
			bestFitness = currentAgentFitness;
		}
	}

	public void NextTestSubject(){
		genAlg.SetGenomeFitness (currentAgentFitness, genAlg.GetCurrentGenomeIndex ());
		currentAgentFitness = 0.0f;
		Genome genome = genAlg.GetNextGenome ();

		neuralNet.FromGenome (ref genome, 5, 8, 2);

		testAgent.Attach (neuralNet);
		testAgent.ClearFailure ();

		//reset the checkpoints

	}

	public void BreedNewPopulation(){
		genAlg.ClearPopulation ();
		int totalweights = 5 * 8 + 8 * 2 + 8 + 2;
		genAlg.GenerateNewPopulation (15, totalweights);
	}

	public void EvolveGenomes(){
		genAlg.BreedPopulation ();
		NextTestSubject ();
	}

	public int GetCurrentMemberOfPopulation(){
		return genAlg.GetCurrentGenomeIndex ();
	}

	public void PrintStats(){
		//to be implemented
	}

}
