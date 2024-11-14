// SerialID: [e4a22a75-a938-4302-8b9a-d405c01db428]
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public bool IsDone { get; private set; }
    public bool IsFrozen { get; private set; }
    public bool IsBackingUp { get; private set; }
    public float BackUpTimer { get; private set; }

    public float Reward { get; private set; }

    [SerializeField] private float reverseDuration = 6.0f;
    private float ReverseDuration => reverseDuration;
    [SerializeField] private float reverseGasDuration = 4.0f;
    private float ReverseGasDuration => reverseGasDuration; 
    [SerializeField] private float reverseBrakeDuration = 2.0f;
    private float ReverseBrakeDuration => reverseBrakeDuration;

    public void SetReward(float reward) {
        Reward = reward;
    }

    public void AddReward(float reward) {
        Reward += reward;
    }

    public abstract int GetState();

    public abstract void AgentAction(double[] vectorAction, bool inReverse);
    public abstract float GetDistance();

    public abstract void AgentReset();

    public abstract void Stop();

    public abstract double[] ActionNumberToVectorAction(int ActionNumber);

    public void Done()
    {
        IsDone = true;
    }

    public void StartBackingUp() {
        BackUpTimer = 0;
        IsBackingUp = true;
    }

    public double[] UpdateBackupTimerAndGetAction(float delta) {
        BackUpTimer += delta;
        if (BackUpTimer > ReverseDuration) {
            StopBackingUp();
        }
        if (BackUpTimer < ReverseGasDuration) {
            return new double[] {0.0f, -0.3f, 0.0f};
        } else if (BackUpTimer > ReverseDuration - ReverseBrakeDuration) {
            return new double[] {0.0f, 0.0f, 1.0f};
        } else {
            return new double[] {0.0f, 0.0f, 0.0f};
        }
    }

    public void StopBackingUp() {
        IsBackingUp = false;
    }

    public void Reset()
    {
        Stop();
        AgentReset();
        IsDone = false;
        IsFrozen = false;
        SetReward(0);
    }

    public abstract List<double> GetAllObservations();
    public abstract List<double> CollectObservations();
    public abstract List<double> OriginalObservations();

    public virtual void SetAgentConfig(List<double> config) { }

    // parameter for battle
    public bool isBattle;
    public AgentExecutor agentExecutor;
    public int agentIndex;

    public abstract void GoStraight();

}
