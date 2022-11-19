namespace GameLogic;

public class IterationsList : IIterationsList
{
    private ConcurrentDictionary<string, List<Iteration>> concurrentBag;

    public IterationsList()
    {
        concurrentBag = new ConcurrentDictionary<string, List<Iteration>>();
    }

    public void Add(string gameId, string playerId, int guess, int mysteryNumber)
    {

        var key = GetKey(gameId, playerId);

        var iteration = new Iteration(gameId, playerId, guess, mysteryNumber);

        List<Iteration> list;
        if (concurrentBag.TryGetValue(key, out list))
        {
            list.Add(iteration);
            concurrentBag.TryUpdate(key, list, null);

            return;
        }

        list = new List<Iteration>
        {
            iteration
        };

        concurrentBag.TryAdd(key, list);
        
    }

    public int Count(string gameId, string playerId)
    {
        var key = GetKey(gameId, playerId);

        var listIterations = concurrentBag.GetValueOrDefault(key);

        if (listIterations == null || !listIterations.Any())
            return 0;

        return listIterations.Count;
    }

    private static string GetKey(string gameId, string playerId)
    {
        return $"{gameId}-{playerId}";
    }
}