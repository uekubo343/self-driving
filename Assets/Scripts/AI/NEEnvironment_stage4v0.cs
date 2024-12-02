// SerialID: [d9c2059b-6dd6-4071-8f77-d635fae0e462]
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class NEEnvironment_stage4default : Environment
{
    [Header("Settings"), SerializeField] private int totalPopulation = 100;
    private int TotalPopulation { get { return totalPopulation; } }

    [SerializeField] private int tournamentSelection = 85;
    private int TournamentSelection { get { return tournamentSelection; } }

    [SerializeField] private int eliteSelection = 4;
    private int EliteSelection { get { return eliteSelection; } }

    [SerializeField] private int inputSize = 7;
    private int InputSize { get { return inputSize; } }

    [SerializeField] private int hiddenSize = 8;
    private int HiddenSize { get { return hiddenSize; } }

    [SerializeField] private int hiddenLayers = 1;
    private int HiddenLayers { get { return hiddenLayers; } }

    [SerializeField] private int outputSize = 4;
    private int OutputSize { get { return outputSize; } }

    [SerializeField] private int nAgents = 4;
    private int NAgents { get { return nAgents; } }

    [Header("Agent Prefab"), SerializeField] private GameObject gObject = null;
    private GameObject GObject => gObject;

    [Header("UI References"), SerializeField] private Text populationText = null;
    private Text PopulationText { get { return populationText; } }

    private float GenBestRecord { get; set; }

    private float SumReward { get; set; }
    private float AvgReward { get; set; }

    private List<NNBrain> Brains { get; set; } = new List<NNBrain>();
    private List<GameObject> GObjects { get; } = new List<GameObject>();
    private List<Agent> Agents { get; } = new List<Agent>();
    private int Generation { get; set; }

    private float BestRecord { get; set; }

    private List<AgentPair> AgentsSet { get; } = new List<AgentPair>();
    private Queue<NNBrain> CurrentBrains { get; set; }

    private List<Obstacle> Obstacles { get; } = new List<Obstacle>();

    void Start() {
        for(int i = 0; i < TotalPopulation; i++) {
            Brains.Add(new NNBrain(InputSize, HiddenSize, HiddenLayers, OutputSize));
        }

        for(int i = 0; i < NAgents; i++) {
            var obj = Instantiate(GObject);
            obj.SetActive(true);
            GObjects.Add(obj);
            Agents.Add(obj.GetComponent<Agent>());
        }
        BestRecord = -9999;
        SetStartAgents();
        Obstacles.AddRange(FindObjectsOfType<Obstacle>());
    }

    void SetStartAgents() {
        CurrentBrains = new Queue<NNBrain>(Brains);
        AgentsSet.Clear();
        var size = Math.Min(NAgents, TotalPopulation);
        for(var i = 0; i < size; i++) {
            AgentsSet.Add(new AgentPair {
                agent = Agents[i],
                brain = CurrentBrains.Dequeue()
            });
        }
    }

    void FixedUpdate() {
        foreach(var pair in AgentsSet.Where(p => !p.agent.IsDone)) {
            AgentUpdate(pair.agent, pair.brain);
        }

        AgentsSet.RemoveAll(p => {
            if(p.agent.IsDone) {
                p.agent.Stop();
                p.agent.gameObject.SetActive(false);
                float r = p.agent.Reward;
                BestRecord = Mathf.Max(r, BestRecord);
                GenBestRecord = Mathf.Max(r, GenBestRecord);
                p.brain.Reward = r;
                SumReward += r;
            }
            return p.agent.IsDone;
        });

        if(CurrentBrains.Count == 0 && AgentsSet.Count == 0) {
            SetNextGeneration();
        }
        else {
            SetNextAgents();
        }
    }

    private void AgentUpdate(Agent a, NNBrain b) {
        var observation = a.GetAllObservations();
        var action = b.GetAction(observation);
        a.AgentAction(action, false);
    }

    private void SetNextAgents() {
        int size = Math.Min(NAgents - AgentsSet.Count, CurrentBrains.Count);
        for(var i = 0; i < size; i++) {
            var nextAgent = Agents.First(a => a.IsDone);
            var nextBrain = CurrentBrains.Dequeue();
            nextAgent.Reset();
            AgentsSet.Add(new AgentPair {
                agent = nextAgent,
                brain = nextBrain
            });
        }
        UpdateText();
    }

    private void SetNextGeneration() {
        AvgReward = SumReward / TotalPopulation;
        GenPopulation();
        SumReward = 0;
        GenBestRecord = -9999;
        Agents.ForEach(a => a.Reset());
        SetStartAgents();
        UpdateText();
        Obstacles.ForEach(o => o.SetRandomPosition());
    }

    private static int CompareBrains(Brain a, Brain b) {
        if(a.Reward > b.Reward) return -1;
        if(b.Reward > a.Reward) return 1;
        return 0;
    }

    private void GenPopulation() {
        var children = new List<NNBrain>();
        var bestBrains = Brains.ToList();

        // Elite selection
        bestBrains.Sort(CompareBrains);
        if(EliteSelection > 0) {
            children.AddRange(bestBrains.Take(EliteSelection));
        }

#if UNITY_EDITOR
        var path = string.Format("Assets/LearningData/NE/{0}.json", EditorSceneManager.GetActiveScene().name);
        bestBrains[0].Save(path);
#endif

        while(children.Count < TotalPopulation) {
            var tournamentMembers = Brains.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(tournamentSelection).ToList();
            tournamentMembers.Sort(CompareBrains);
            children.Add(tournamentMembers[0].Mutate(Generation));
            children.Add(tournamentMembers[1].Mutate(Generation));
        }
        Brains = children;
        Generation++;
    }

    private void UpdateText() {
        PopulationText.text = "Population: " + (TotalPopulation - CurrentBrains.Count) + "/" + TotalPopulation
            + "\nGeneration: " + (Generation + 1)
            + "\nBest Record: " + BestRecord
            + "\nBest this gen: " + GenBestRecord
            + "\nAverage: " + AvgReward;
    }

    private struct AgentPair
    {
        public NNBrain brain;
        public Agent agent;
    }
}
