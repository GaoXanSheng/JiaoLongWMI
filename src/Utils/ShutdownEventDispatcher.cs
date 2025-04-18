namespace JiaoLongWMI.Utils;

public class ShutdownEventDispatcher
{
	private readonly List<Action> _handlers = new();

	public void Subscribe(Action handler)
	{
		if (!_handlers.Contains(handler))
			_handlers.Add(handler);
	}

	public void Unsubscribe(Action handler)
	{
		_handlers.Remove(handler);
	}

	public void Trigger()
	{
		foreach (var handler in _handlers)
		{
			try
			{
				handler.Invoke();
			}
			catch (Exception ex)
			{
				Logger.Info($"Error: {ex.Message}");
			}
		}
	}
}

