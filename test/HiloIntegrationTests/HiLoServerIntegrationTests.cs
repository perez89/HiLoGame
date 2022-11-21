namespace HiloIntegrationTests;

public class HiLoServerIntegrationTests
  { 

    [IntegrationTest]
    public async Task  Should_Make_All_Flow_Single_Player()
    {

        Console.WriteLine("Should_Make_All_Flow_Single_Player");
        var playerId = "Perez_Player_Id";

        await ANewPlayerHasEnterInTheGame(playerId);
    }

    [IntegrationTest]
    public void Should_Make_All_Flow_Multi_Players()
    {
        List<Task> TaskList = new List<Task>();

        for (int i = 0; i < 50; i++)
        {
            var playerId = Guid.NewGuid().ToString().Substring(0,5);
            var t = ANewPlayerHasEnterInTheGame(i+"##"+playerId);
         
            TaskList.Add(t);
        }

        Task.WaitAll(TaskList.ToArray());
    }

    private async Task ANewPlayerHasEnterInTheGame(string playerId)
    {
        var json = JsonConvert.SerializeObject(playerId);
        var data = new StringContent($"{json}", Encoding.UTF8, "application/json");

        var httpClient = new HttpClient();
        var httpResponseMessage = await httpClient.PostAsync("http://hilo-serverx/Session", data);

        Assert.True(httpResponseMessage.IsSuccessStatusCode);


        var token = await httpResponseMessage.Content.ReadAsStringAsync();


        var httpRequestMessage = new HttpRequestMessage(
        HttpMethod.Get,
        $"http://hilo-serverx/game/start?playerId={playerId}")
        {
            Headers =
                {
                    { HeaderNames.Authorization, $"Bearer {token}" }
                }
        };

        httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        Assert.True(httpResponseMessage.IsSuccessStatusCode);


        var contentString =
            await httpResponseMessage.Content.ReadAsStringAsync();

        var startDto = JsonConvert.DeserializeObject<StartDto>(contentString);

        Assert.NotNull(startDto);

        var r = new Random();
        var guess = r.Next(1, 100);

        while (true)
        {
            var resp = await Play(startDto.GameId, startDto.PlayerId, token, guess, httpClient);

            if (resp.HasFinish)
            {
                //You have successfully found the mystery number
                Assert.StartsWith("You have successfully found the mystery number", resp.Response);
                Console.WriteLine("player= " + playerId + " | Final response= " + resp.Response);
                break;
            }

            if (resp.Response.StartsWith("The mystery number is less than your guess", StringComparison.OrdinalIgnoreCase))
            {

                guess = (guess / 2) + r.Next(0, 2);
            }
            else
            {
                guess = guess + r.Next(0,3);
              
            }

            if (guess < 1)
                guess = 1;

            if (guess > 100)
                guess = 100;
        }
    }

    private async Task<PlayDto> Play(string gameId, string playerId, string token, int guess, HttpClient httpClient)
    {

        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"http://hilo-serverx/game/guess?gameId={gameId}&playerId={playerId}&guess={guess}")
        {
            Headers =
                {
                    { HeaderNames.Authorization, $"Bearer {token}" }
                }
        };

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        Assert.True(httpResponseMessage.IsSuccessStatusCode);          

        var contentString =
            await httpResponseMessage.Content.ReadAsStringAsync();

        Assert.IsType<PlayDto>(JsonConvert.DeserializeObject<PlayDto>(contentString));

        var playDto = JsonConvert.DeserializeObject<PlayDto>(contentString);

        Assert.NotNull(playDto);

        return playDto;
    }

}