namespace GameLogic;

public class EngineList : IEngineList
{
    private readonly IValuesList _valuesList;
    private readonly IValueGeneratorBase<int> _valueGenerator;

    public EngineList(IValuesList valuesList, IValueGeneratorBase<int> valueGenerator)
	{
		_valuesList = valuesList;
        _valueGenerator = valueGenerator;
    }    

	public int GetValueFromPlayerGame(string playerId) {

        var value = 0;
        if (_valuesList.Exist(playerId))
        {
            value = _valuesList.GetNumber(playerId);
        }

        if (value > 0)
            return value;

        value = _valueGenerator.Get();

        _valuesList.SetIfDoesNotExist(playerId, value);

        return value;
    }

    public void ResetPlayerGame(string playerId)
    {
        _valuesList.Reset(playerId);
    }

    public void StartPlayerGame(string playerId)
    {
        _valuesList.Reset(playerId);

        var value = _valueGenerator.Get();

        _valuesList.SetIfDoesNotExist(playerId, value);
    }
}