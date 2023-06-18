using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if FRAGMASTA
using Data;
#endif

namespace UnityUtils.Extensions
{
#if FRAGMASTA
    public static class UnitExtensions
    {
        public static UnitInfo GetBot(this UnitData unitData, Team team, BotType botType)
        {
            return botType switch
            {
                BotType.PatrolBot => unitData.GetPatrolBot(team),
                BotType.Boss => unitData.GetBoss(team),
                BotType.PlayerBot => unitData.GetPlayerBot(team),
                _ => throw new Exception("Invalid Spawner Type"),
            };
        }

        public static UnitInfo GetPatrolBot(this UnitData data, Team team) => team switch
        {
            Team.Team1 => data.GetPatrolBots(team).GetRandomItem(),
            Team.Team2 => data.GetPatrolBots(team).GetRandomItem(),
            _ => data.patrollers.GetRandomItem()
        };

        public static UnitInfo GetBoss(this UnitData data, Team team) => team switch
        {
            Team.Team1 => data.GetBosses(team).GetRandomItem(),
            Team.Team2 => data.GetBosses(team).GetRandomItem(),
            _ => throw new Exception("Invalid Team !")
        };

        public static UnitInfo GetPlayerBot(this UnitData data, Team team) => team switch
        {
            Team.Team1 => data.playerBots.Where(p => p.team == Team.Team1).ToList().GetRandomItem(),
            Team.Team2 => data.playerBots.Where(p => p.team == Team.Team2).ToList().GetRandomItem(),
            _ => data.playerBots.GetRandomItem()
        };

        public static List<UnitInfo> GetPatrolBots(this UnitData data, Team team) => team switch
        {
            Team.Team1 => data.patrollers.Where(p => p.team == Team.Team1).ToList(),
            Team.Team2 => data.patrollers.Where(p => p.team == Team.Team2).ToList(),
            _ => data.patrollers
        };

        public static List<UnitInfo> GetBosses(this UnitData data, Team team) => team switch
        {
            Team.Team1 => data.bosses.Where(p => p.team == Team.Team1).ToList(),
            Team.Team2 => data.bosses.Where(p => p.team == Team.Team2).ToList(),
            _ => throw new Exception("Invalid Team !")
        };

        public static List<UnitInfo> GetPlayerBots(this UnitData data, Team team) => team switch
        {
            Team.Team1 => data.playerBots.Where(p => p.team == Team.Team1).ToList(),
            Team.Team2 => data.playerBots.Where(p => p.team == Team.Team2).ToList(),
            _ => data.playerBots
        };

        public static UnitInfo GetPlayer(this UnitData data, int playerId) => data.allUnitAvatarInfos[playerId];

        public static GameObject GetPlayerPrefab(this UnitData data, int playerId) =>
            data.allUnitAvatarInfos[playerId].prefab;

        public static List<UnitInfo> GetAllUnits(this UnitData data) => new List<UnitInfo>().AddList(
            data.allUnitAvatarInfos, data.bosses, data.patrollers, data.playerBots);

        public static string[] GetAllUnitNames(this UnitData data) => data.GetAllUnits().Select(x => x.name).ToArray();
    }
#endif
}