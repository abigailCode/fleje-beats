using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class HardwareRig : MonoBehaviour, INetworkRunnerCallbacks
{
    public Transform playerTransform;

    public Transform headTransform;

    public Transform leftHandTransform;

    public Transform rightHandTransform;

    void Start()
    {
        NetworkManager.Instance.Runner.AddCallbacks(this);
    }


    #region INetworkRunnerCallbacks
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
        RigState xrRigState = new RigState();

        xrRigState.HeadsetPosition = headTransform.position;
        xrRigState.HeadsetRotation = headTransform.rotation;

        xrRigState.PlayerPosition = playerTransform.position;
        xrRigState.PlayerRotation = playerTransform.rotation;

        xrRigState.LeftHandPosition = leftHandTransform.position;
        xrRigState.LeftHandRotation = leftHandTransform.rotation;

        xrRigState.RightHandPosition = rightHandTransform.position;
        xrRigState.RightHandRotation = rightHandTransform.rotation;

        input.Set(xrRigState);
    }
    #endregion

    #region Unused INetworkRunnerCallbacks
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {

    }

    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner)
    {

    }

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }



    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {

    }

    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {

    }

    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
    {

    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }
    #endregion

}

public struct RigState : INetworkInput
{
    public Vector3 PlayerPosition;
    public Quaternion PlayerRotation;

    public Vector3 HeadsetPosition;
    public Quaternion HeadsetRotation;

    public Vector3 LeftHandPosition;
    public Quaternion LeftHandRotation;

    public Vector3 RightHandPosition;
    public Quaternion RightHandRotation;
}