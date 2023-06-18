#if PHOTON_UNITY_NETWORKING
using System.Collections;
using Core;
using Core.CoreManagers;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityUtils.Helpers;

namespace UnityUtils.BaseClasses
{
    public class BaseMonoPun : MonoBehaviourPunCallbacks
    {
        protected static IEnumerator ConnectPhoton()
        {
            Log("Connecting...");
            if(!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
            while (!PhotonNetwork.IsConnected)
                yield return new WaitForEndOfFrame();
            yield return WaitForConnectedToMasterServer();
            Log("Connected...");
        }
        
        protected static IEnumerator JoinLobby()
        {
            Log("Joining to Lobby");
            if(!PhotonNetwork.InLobby)
                PhotonNetwork.JoinLobby();
            yield return WaitForJoinedLobby();
            Log("Joined to Lobby");
        }
        
        public static IEnumerator JoinRoom(string roomName, RoomOptions roomOptions)
        {
            Log($"Joining to Room : {roomName} ...  ");
            yield return LeaveRoom();
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
            //JoinOrCreateRoom(roomName, roomOptions);
            
            yield return WaitForEnterRoom(roomName);
            yield return WaitForJoined();
            
            Log("Room entered " + PhotonNetwork.CurrentRoom.Name);
        }
        
        private static void JoinOrCreateRoom(string roomName, RoomOptions roomOptions)
        {
            bool roomFound = false;
            foreach (var room in NetworkManager.rooms)
            {
                if (room.Name == roomName)
                {
                    PhotonNetwork.JoinRoom(roomName);
                    roomFound = true;
                    break;
                }
            }
            if(!roomFound)
                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
        }

        public static IEnumerator JoinRoom(int id, RoomOptions roomOptions)
        {
            yield return LeaveRoom();
            var roomName = RoomName(id);
            Log($"Joining to Room : {roomName} ...  ");
            Memory.currentRoomID = id;
            yield return JoinRoom(roomName, roomOptions);
            Log("Room entered " + PhotonNetwork.CurrentRoom.Name);
        }
        
        public static IEnumerator LeaveRoom()
        {
            if (!PhotonNetwork.InRoom) yield break;
            Log("Leaving to room : " + PhotonNetwork.CurrentRoom.Name);
            PhotonNetwork.LeaveRoom();
            yield return WaitForConnectedToMasterServer();
            Log("Leaved to room : ");
        }
        
        #region Waiter Coroutine Functions

        protected static IEnumerator WaitForClientState(ClientState state)
        {
            yield return state.WaitClientState();
        }
        
        protected static IEnumerator WaitForConnectedToMasterServer()
        {
            yield return ClientState.ConnectedToMasterServer.WaitClientState();
        }
        
        protected static IEnumerator WaitForJoinedLobby()
        {
            yield return ClientState.JoinedLobby.WaitClientState();
        }
        
        protected static IEnumerator WaitForJoined()
        {
            yield return ClientState.Joined.WaitClientState();
        }
        
        protected static IEnumerator WaitForEnterRoom(string roomName)
        {
            yield return CoroutineHelper.WaitCondition(() => RoomEntered(roomName));
        }

        #endregion
        
        protected static bool RoomEntered(string room) =>
            PhotonNetwork.NetworkClientState == ClientState.Joined &&
            PhotonNetwork.CurrentRoom != null &&
            room == PhotonNetwork.CurrentRoom.Name;
        
        protected static string RoomName(int id) => "Classroom " + id;

        private static void Log(string log) => Debug.Log("Photon Network : " + log);
    }
}

#endif