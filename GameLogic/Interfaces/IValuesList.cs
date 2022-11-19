namespace GameLogic.Interfaces;

public interface IValuesList
{
    public bool Exist(string key);
    public void SetIfDoesNotExist(string key, int val);
    public void Reset(string key);
    public int GetNumber(string key);
}