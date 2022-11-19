namespace GameLogic.Interfaces;

public abstract class ValueGeneratorBase<T> : IValueGeneratorBase<T>
{
    private readonly T _min;
    private readonly T _max;
    public ValueGeneratorBase(T min, T max)
    {
        _min = min;
        _max = max;
    }

    protected T GetMin()
    {
        return _min;
    }

    protected T GetMax()
    {
        return _max;
    }

    public abstract T Get();
}
