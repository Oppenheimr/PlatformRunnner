#if FRAGMASTA
using System;
using System.Linq;

using Data;

namespace UnityUtils.Extensions
{
    public static class FragmastaExtensions
    {
        public static bool TagIsBot(this string tag) => 
            FragGameData.Instance.botTags.Any(playerTag => playerTag == tag);
        
        public static bool TagIsPlayer(this string tag) => 
            FragGameData.Instance.playerTags.Any(playerTag => playerTag == tag);
        
        public static string GetTeamName(this Team team) => team switch
        {
            Team.None => "Team None",
            Team.Team1 => "Blue",
            Team.Team2 => "Red",
            Team.All => "Team All",
            _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
        };
    }
}
#endif