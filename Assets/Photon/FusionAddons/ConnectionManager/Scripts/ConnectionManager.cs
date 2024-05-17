using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Fusion.Addons.ConnectionManagerAddon
{
    /**
     * 
     * Handles:
     * - connection launch (either with room name or matchmaking session properties)
     * - user representation spawn on connection
     **/
    public class ConnectionManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        public GameObject leftHand, rightHand, headset;
        [System.Flags]
        public enum ConnectionCriterias
        {
            RoomName = 1,
            SessionProperties = 2
        }

        [System.Serializable]
        public struct StringSessionProperty
        {
            public string propertyName;
            public string value;
        }
        [Header("Room configuration")]
        public GameMode gameMode = GameMode.Host;
        public string roomName = "Room1";
        public bool connectOnStart = true;
        [Tooltip("Set it to 0 to use the DefaultPlayers value, from the Global NetworkProjectConfig (simulation section)")]
        public int playerCount = 2;

        [Header("Room selection criteria")]
        public ConnectionCriterias connectionCriterias = ConnectionCriterias.RoomName;
        [Tooltip("If connectionCriterias include SessionProperties, additionalSessionProperties (editable in the inspector) will be added to sessionProperties")]
        public List<StringSessionProperty> additionalSessionProperties = new List<StringSessionProperty>();
        public Dictionary<string, SessionProperty> sessionProperties;

        [Header("Fusion settings")]
        [Tooltip("Fusion runner. Automatically created if not set")]
        public NetworkRunner runner;
        public INetworkSceneManager sceneManager;

        [Header("Local user spawner")]
        public NetworkObject userPrefab;

        [Header("Event")]
        public UnityEvent onWillConnect = new UnityEvent();

        [Header("Info")]
        public List<StringSessionProperty> actualSessionProperties = new List<StringSessionProperty>();

        // Dictionary of spawned user prefabs, to store them on the server for host topology, and destroy them on disconnection (for shared topology, use Network Objects's "Destroy When State Authority Leaves" option)
        private Dictionary<PlayerRef, NetworkObject> _spawnedUsers = new Dictionary<PlayerRef, NetworkObject>();

        bool ShouldConnectWithRoomName => (connectionCriterias & ConnectionManager.ConnectionCriterias.RoomName) != 0;
        bool ShouldConnectWithSessionProperties => (connectionCriterias & ConnectionManager.ConnectionCriterias.SessionProperties) != 0;

        private void Awake()
        {
            // Check if a runner exist on the same game object
            if (runner == null) runner = GetComponent<NetworkRunner>();

            // Create the Fusion runner and let it know that we will be providing user input
            if (runner == null) runner = gameObject.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;
        }

        private async void Start()
        {
            // Launch the connection at start
            if (connectOnStart) await Connect();
        }

        Dictionary<string, SessionProperty> AllConnectionSessionProperties
        {
            get
            {
                var propDict = new Dictionary<string, SessionProperty>();
                actualSessionProperties = new List<StringSessionProperty>();
                if (sessionProperties != null)
                {
                    foreach (var prop in sessionProperties)
                    {
                        propDict.Add(prop.Key, prop.Value);
                        actualSessionProperties.Add(new StringSessionProperty { propertyName = prop.Key, value = prop.Value });
                    }
                }
                if (additionalSessionProperties != null)
                {
                    foreach (var additionalProperty in additionalSessionProperties)
                    {
                        propDict[additionalProperty.propertyName] = additionalProperty.value;
                        actualSessionProperties.Add(additionalProperty);
                    }

                }
                return propDict;
            }
        }

        public virtual NetworkSceneInfo CurrentSceneInfo()
        {
            var activeScene = SceneManager.GetActiveScene();
            SceneRef sceneRef = default;

            if (activeScene.buildIndex < 0 || activeScene.buildIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError("Current scene is not part of the build settings");
            }
            else
            {
                sceneRef = SceneRef.FromIndex(activeScene.buildIndex);
            }

            var sceneInfo = new NetworkSceneInfo();
            if (sceneRef.IsValid)
            {
                sceneInfo.AddSceneRef(sceneRef, LoadSceneMode.Single);
            }
            return sceneInfo;
        }

        public async Task Connect()
        {
            // Create the scene manager if it does not exist
            if (sceneManager == null) sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
            if (onWillConnect != null) onWillConnect.Invoke();

            // Start or join (depends on gamemode) a session with a specific name
            var args = new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                Scene = CurrentSceneInfo(),
                SceneManager = sceneManager
            };

            // Connexion criteria (note: actual project code contains alternative options)
            args.SessionName = roomName;

            await runner.StartGame(args);
        }

        #region Player spawn
        public void OnPlayerJoinedSharedMode(NetworkRunner runner, PlayerRef player)
        {
            if (player == runner.LocalPlayer && userPrefab != null)
            {
                // Spawn the user prefab for the local user
                NetworkObject networkPlayerObject = runner.Spawn(userPrefab, position: transform.position, rotation: transform.rotation, player, (runner, obj) => {
                });
            }
        }

        public void OnPlayerJoinedHostMode(NetworkRunner runner, PlayerRef player)
        {
            // The user's prefab has to be spawned by the host
            if (runner.IsServer && userPrefab != null)
            {
                Debug.Log($"OnPlayerJoined. PlayerId: {player.PlayerId}");
                // We make sure to give the input authority to the connecting player for their user's object
                NetworkObject networkPlayerObject = runner.Spawn(userPrefab, position: transform.position, rotation: transform.rotation, inputAuthority: player, (runner, obj) => {
                });

                // Keep track of the player avatars so we can remove it when they disconnect
                _spawnedUsers.Add(player, networkPlayerObject);
            }
        }

        // Despawn the user object upon disconnection
        public void OnPlayerLeftHostMode(NetworkRunner runner, PlayerRef player)
        {
            // Find and remove the players avatar (only the host would have stored the spawned game object)
            if (_spawnedUsers.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedUsers.Remove(player);
            }
        }

        #endregion

        #region INetworkRunnerCallbacks
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.Topology == Topologies.ClientServer)
            {
                OnPlayerJoinedHostMode(runner, player);
            }
            else
            {
                OnPlayerJoinedSharedMode(runner, player);
            }
        }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
            if (runner.Topology == Topologies.ClientServer)
            {
                OnPlayerLeftHostMode(runner, player);
            }
        }
        #endregion

        #region INetworkRunnerCallbacks (debug log only)
        public void OnConnectedToServer(NetworkRunner runner) {
            Debug.Log("OnConnectedToServer");

        }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.Log("Shutdown: " + shutdownReason);
        }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) {
            Debug.Log("OnDisconnectedFromServer: " + reason);
        }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {
            Debug.Log("OnConnectFailed: " + reason);
        }
        #endregion

        #region INetworkRunnerCallbacks 

        public struct RigInput : INetworkInput
        {
            public Vector3 playAreaPosition;
            public Quaternion playAreaRotation;
            public Vector3 leftHandPosition;
            public Quaternion leftHandRotation;
            public Vector3 rightHandPosition;
            public Quaternion rightHandRotation;
            public Vector3 headsetPosition;
            public Quaternion headsetRotation;
            public HandCommand leftHandCommand;
            public HandCommand rightHandCommand;

        }
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            RigInput rigInput = new RigInput();
            rigInput.playAreaPosition = transform.position;
            rigInput.playAreaRotation = transform.rotation;

            rigInput.leftHandPosition = leftHand.transform.position;
            rigInput.leftHandRotation = leftHand.transform.rotation;
            rigInput.rightHandPosition = rightHand.transform.position;
            rigInput.rightHandRotation = rightHand.transform.rotation;
            rigInput.headsetPosition = headset.transform.position;
            rigInput.headsetRotation = headset.transform.rotation;

            input.Set(rigInput);
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            throw new NotImplementedException();
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            throw new NotImplementedException();
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            throw new NotImplementedException();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            throw new NotImplementedException();
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            throw new NotImplementedException();
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            throw new NotImplementedException();
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            throw new NotImplementedException();
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
            throw new NotImplementedException();
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            throw new NotImplementedException();
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            throw new NotImplementedException();
        }

        /*
        public override void Render()
        {
            base.Render();
            if (IsLocalNetworkRig)
            {
                // Extrapolate for local user:
                // we want to have the visual at the good position as soon as possible, so we force the visuals to follow the most fresh hardware positions
                // To update the visual object, and not the actual networked position, we move the interpolation targets
                transform.position = hardwareRig.transform.position;
                transform.rotation = hardwareRig.transform.rotation;
                leftHand.transform.position = hardwareRig.leftHand.transform.position;
                leftHand.transform.rotation = hardwareRig.leftHand.transform.rotation;
                rightHand.transform.position = hardwareRig.rightHand.transform.position;
                rightHand.transform.rotation = hardwareRig.rightHand.transform.rotation;
                headset.transform.position = hardwareRig.headset.transform.position;
                headset.transform.rotation = hardwareRig.headset.transform.rotation;
            }
        }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        #endregion
    }
        */
    }


    public class NetworkHeadset : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkTransform networkTransform;
        private void Awake()
        {
            if (networkTransform == null) networkTransform = GetComponent<NetworkTransform>();
        }
    }

    // Structure representing the inputs driving a hand pose
    [System.Serializable]
    public struct HandCommand : INetworkStruct
    {
        public float thumbTouchedCommand;
        public float indexTouchedCommand;
        public float gripCommand;
        public float triggerCommand;
        // Optionnal commands
        public int poseCommand;
        public float pinchCommand;// Can be computed from triggerCommand by default
    }

    public interface IHandRepresentation
    {
        public void SetHandCommand(HandCommand command);
        public GameObject gameObject { get; }
        public void SetHandColor(Color color);
        public void SetHandMaterial(Material material);
        public void DisplayMesh(bool shouldDisplay);
        public bool IsMeshDisplayed { get; }
        public Material SharedHandMaterial { get; }
    }
}
#endregion