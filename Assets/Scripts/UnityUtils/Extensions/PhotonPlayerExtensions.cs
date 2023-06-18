using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if FRAGMASTA
using Data;
using InGame;
using InGame.GameLogic;
using UI.UnitUI;
#endif
#if PHOTON_UNITY_NETWORKING
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityUtils.Helpers;
#endif
#if MFPS
using Inherited.Inherited_Lovatto.InGame;
using Inherited.Inherited_Lovatto.InGame.Player;
#endif

#if INVECTOR_BASIC || INVECTOR_MELEE || INVECTOR_SHOOTER || INVECTOR_AI_TEMPLATE
#endif

namespace UnityUtils.Extensions
{
#if PHOTON_UNITY_NETWORKING
    public static class PhotonPlayerExtensions
    {

        #region FRAGMASTA

#if FRAGMASTA
        public static bool IsSecondBestKiller(this Player target, Team team)
        {
            var betterKiller = 0;
            foreach (var player in GameManager.Ins.connectedPlayerList)
            {
                if (team != Team.All && player.GetPlayerTeam() != team) continue;
                if (Equals(target, player)) continue;
                if (player.Combo() > target.Combo())
                    betterKiller++;
            }
            return betterKiller == 1;
        }

        public static bool IsBestKiller(this Player target, Team team) =>
            GameManager.Ins.connectedPlayerList.Where(player => team == Team.All || player.GetPlayerTeam() == team)
                .Where(player => !Equals(target, player)).All(player => player.Combo() <= target.Combo());
        
        public static void SetMaster(this Player player, PlayerMasterType masterType)
        {
            var controller = (PFirstPersonController)player.GetReferences().firstPersonController;

            if (masterType == PlayerMasterType.DefaultPlayer)
                controller.SetDefaultAttributes();
            else
                controller.AttributesMultiplication(player.Combo(), masterType);

            if(player.IsLocal)
                UnitUIManager.Instance.SetupLocal();
            if(player.isTeamMate())
                UnitUIManager.Instance.SetupTeamMate(player);
        }
        
        public static PlayerMasterType GetMasterTypeFromManager(this Player player)
        {
            if (!FragmastaGameMode.IsActive) return PlayerMasterType.DefaultPlayer;
            
            if (player.NickName == FragmastaGameMode.Ins.Fragmasta.NickName)
                return PlayerMasterType.Fragmasta;
            
            if (player.NickName == FragmastaGameMode.Ins.Riotmasta.NickName)
                return PlayerMasterType.Riotmasta;
            
            return PlayerMasterType.DefaultPlayer;
        }
        
        public static bool IsFrag(this Player player) => player.IsBestKiller(Team.All);
        
        /// <summary>
        /// using PUN's implementation of Hashtable
        /// </summary>
        public static void UpdateCombo(this Player p, int combo) => p.SetCustomProperties(new Hashtable
        {
            [PlayerPropertiesKeys.ComboKey] = combo
        }); // this locally sets the score and will sync it in-game asap.


        /// <summary>
        /// 
        /// </summary>
        public static void UpdateKill(this Player p)
        {
            int current = p.GetKills();
            current = current + 1;

            Hashtable score = new Hashtable(); // using PUN's implementation of Hashtable
            score[PropertiesKeys.KillsKey] = current;

            p.SetCustomProperties(score); // this locally sets the score and will sync it in-game asap.
            p.UpdateCombo(p.Combo() + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void UpdateDeaths(this Player p)
        {
            int current = p.GetDeaths();
            current = current + 1;

            Hashtable score = new Hashtable(); // using PUN's implementation of Hashtable
            score[PropertiesKeys.DeathsKey] = current;

            p.SetCustomProperties(score); // this locally sets the score and will sync it in-game asap.
            p.UpdateCombo(0);
        }
#endif

        #endregion
        
        #region MFPS

#if MFPS
        public static MFPSPlayer GetMPlayer(this Player player) =>
            GameManager.Ins.AllActors.FirstOrDefault(a => a.ActorNumber == player.ActorNumber);
        
        public static List<MFPSPlayer> GetTeamMates(this Player player) =>
            GameManager.Ins.OthersActorsInScene.Where(o => o.Team == player.GetPlayerTeam()).ToList();
        
        public static PlayerReferences GetReferences(this Player player)
        {
            if (OfflineManager.IsOffline)
                return OfflineManager.Instance.playerReferences;
            
            var mPlayer = player.GetMPlayer();
            return mPlayer != null ? mPlayer.ActorView.GetComponent<PlayerReferences>() : player.FindReferences();
        }

        private static PlayerReferences FindReferences(this Player player)
        {
            var allViews = Object.FindObjectsOfType<PhotonView>();
            var playerViews = allViews.Where(p => p.ControllerActorNr == player.ActorNumber).ToList();
            PlayerReferences references = null;

            foreach (var view in playerViews.Where(view => view.TryGetComponent(out references)))
                break;
            return references;
        }
        
        public static PhotonView GetPhotonView(this Player player)
        {
            var references = player.GetReferences();

            return references == null ? null : references.photonView;
        }
        
        public static int Combo(this Player p)
            => p.GetProperties<int>(PlayerPropertiesKeys.ComboKey);
#endif

        #endregion

        public static PhotonView GetView(this Player p) => PhotonHelper.GetViewByActorNumber(p.ActorNumber);

        public static T GetProperties<T>(this Player p, string key)
        {
            if (p.CustomProperties.ContainsKey(key))
                return (T)p.CustomProperties[key];
            throw new Exception("Properties is invalid...");
        }
        
        public static bool TryGetProperties<T>(this Player p, out T property, string key)
        {
            property = default;
            var contain = p.CustomProperties.ContainsKey(key);
            if (contain)
                property = (T)p.CustomProperties[key];
            return contain;
        }
    }
#endif
}