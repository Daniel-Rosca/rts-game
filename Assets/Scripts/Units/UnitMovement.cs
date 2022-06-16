using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Targeter targeter = null;

    [SerializeField] private float chaseRange = 10f;


    #region Server
    
    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;

    }

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }



    [Command]
    public void CmdMove(Vector3 position)
    {
        ServerMove(position);
    }
    [ServerCallback]
    private void Update()
    {
        Targetable target = targeter.GetTarget();
        
        if(target != null)
        {
            if ((target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange)//a minor optimisation. we could use a trick to check if smth is in range
            {
                agent.SetDestination(target.transform.position);
            }
            else if (agent.hasPath)
            {
                agent.ResetPath();
            }


            return;
        }

        if (!agent.hasPath) { return; }
        
        if (agent.remainingDistance > agent.stoppingDistance) { return; }

        agent.ResetPath();
    }


    [Server]
    private void ServerHandleGameOver()
    {
        agent.ResetPath();
    }


    [Server]
    public void ServerMove(Vector3 position)
    {
        targeter.ClearTarget();

        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }
    #endregion

}
