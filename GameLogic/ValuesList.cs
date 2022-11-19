namespace GameLogic;

public class ValuesList : IValuesList
{
    private ConcurrentDictionary<string, int> concurrentBag;

    public ValuesList()
    {
        concurrentBag = new ConcurrentDictionary<string, int>();
    }

    public void SetIfDoesNotExist(string key, int val) {
        if (!concurrentBag.ContainsKey(key)) {
            concurrentBag.TryAdd(key, val); 
        }
    }

    public void Reset(string key)
    {
        var val = concurrentBag.GetValueOrDefault(key);

        if (val > 0)
        {
            concurrentBag.Remove(key, out _);
        }
    }

    public int GetNumber(string key)
    {
        if (concurrentBag.ContainsKey(key))
        {
            return concurrentBag[key];
        }

        return default;
    }

    public bool Exist(string key)
    {
        return concurrentBag.ContainsKey(key);
    }
}