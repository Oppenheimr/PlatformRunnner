using System;
using System.Collections;
using Object = UnityEngine.Object;

#if FRAGMASTA
using InGame;
#endif

#if PHOTON_UNITY_NETWORKING
using Photon.Realtime;
using Photon.Pun;
#endif

namespace UnityUtils.Helpers
{
#if PHOTON_UNITY_NETWORKING
    public static class PhotonHelper
    {
        public static string LocalName
        {
      
            get
            {
#if FRAGMASTA && MFPS
                if (bl_PhotonNetwork.LocalPlayer != null && OfflineManager.IsOffline)
                    return PhotonNetwork.LocalPlayer != null ? PhotonNetwork.LocalPlayer.NickName : "None";
                return OfflineManager.Instance.playerReferences.name;
#endif
                return PhotonNetwork.LocalPlayer != null ? PhotonNetwork.LocalPlayer.NickName : "None";
            }
        }

        public static PhotonView GetViewByActorNumber(int actorNumber)
        {
            var views = Object.FindObjectsOfType<PhotonView>();
            foreach (var view in views)
                if (actorNumber == view.ControllerActorNr)
                    return view;
            throw new Exception($@"Can not find actor number! 
Number of found objects : {views.Length}
View ID : {actorNumber}");
        }
        
        public static IEnumerator WaitClientState(this ClientState state)
        {
            yield return CoroutineHelper.WaitCondition(() => PhotonNetwork.NetworkClientState == state);
        }
    }
#endif
}