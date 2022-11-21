namespace GameLogic.Interfaces;

public interface IValueGeneratorBase<T>
{
    public abstract T Get();
}