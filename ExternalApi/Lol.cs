using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using hateekub.DTOS; 
using hateekub.Models;


[ApiController]
[Route("api/riot")]
public class RiotController : ControllerBase
{   
    private RankResult GetUnranked()
{
    return new RankResult
    {
        Tier = "UNRANKED",
        Rank = "-",
        Wins = 0,
        Losses = 0
    };
}
    private const string RIOT_API_KEY = "RGAPI-058b7120-a68d-4fb6-8d1e-cee76d53060a";


[HttpGet("Lol/{gameName}/{tagLine}")]
public async Task<IActionResult> GetAccount(string gameName, string tagLine)
{
    using var client = new HttpClient();
    client.DefaultRequestHeaders.Add("X-Riot-Token", RIOT_API_KEY);

    var accountRes = await client.GetAsync(
        $"https://asia.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}"
    );

    var accountJson = await accountRes.Content.ReadAsStringAsync();

    if (!accountRes.IsSuccessStatusCode)
        return Content(accountJson, "application/json");

    var account =
        JsonSerializer.Deserialize<RiotAccountDto>(accountJson);

    if (account == null)
        return BadRequest("Invalid account");

    var rankRes = await client.GetAsync(
        $"https://sg2.api.riotgames.com/lol/league/v4/entries/by-puuid/{account.puuid}"
    );

    var rankJson = await rankRes.Content.ReadAsStringAsync();

    if (!rankRes.IsSuccessStatusCode)
        return Content(rankJson, "application/json");

    var ranks =
        JsonSerializer.Deserialize<List<RiotRankDto>>(rankJson);

    if (ranks == null || !ranks.Any())
        return Ok(GetUnranked());

    var soloRank = ranks
        .FirstOrDefault(r => r.queueType == "RANKED_SOLO_5x5");

    if (soloRank == null)
        return Ok(GetUnranked());

    var result = new RankResult
    {
        Tier = soloRank.tier,
        Rank = soloRank.rank,
        Wins = soloRank.wins,
        Losses = soloRank.losses
    };

    return Ok(result);

}
}