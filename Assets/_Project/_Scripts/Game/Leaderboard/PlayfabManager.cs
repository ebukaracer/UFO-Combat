/* Uncomment when a playfab sdk is added to the project.

using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

internal class PlayfabManager : MonoBehaviour
{
    // Called on Start
    private void LoginUser()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, FailureCallback);
    }

    // Submit a Max-Wave 
    public void SubmitMaxWave(int wave)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "Max-Wave",
                Value = wave
            }
        }

        }, OnStatisticsUpdated, FailureCallback);
    }

    // Get the players with the top 10 Max-Waves in the game
    public void RequestLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "Max-Wave",
            StartPosition = 0,
            MaxResultsCount = 10
        },

        DisplayLeaderboard, FailureCallback);
    }

    // Retrieves/Displays top 10 Max-Waves
    private void DisplayLeaderboard(GetLeaderboardResult result)
    {
        for (int i = 0; i < result.Leaderboard.Count; i++)
        {
            Debug.Log($"{result.Leaderboard[i].PlayFabId}: {result.Leaderboard[i].StatValue}");
        }
    }

    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }


    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
    {
        Debug.Log("Successfully submitted high score");
    }

    private void OnSuccess(LoginResult result)
    {
        Debug.Log($"Success! Created: {result.NewlyCreated}, Last Seen: {result.LastLoginTime.GetValueOrDefault(System.DateTime.Now)}");
    }
}
*/