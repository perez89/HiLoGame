namespace GameLogic;

public class ValueGeneratorInt : ValueGeneratorBase<int>
{
    private const int MIN = 1;
    private const int MAX = int.MaxValue-1;

    public ValueGeneratorInt(int min, int max): base(min, max)
    {
        if (min < MIN) {
            throw new ArgumentException($"The min value cannot be less than {MIN}");
        }

        if (max > MAX) {
            throw new ArgumentException($"The max value cannot be greater than {MAX}");
        }
    }

    public override int Get()
    {
        var r = new Random();
        return r.Next(GetMin(), GetMax());
    }
}
