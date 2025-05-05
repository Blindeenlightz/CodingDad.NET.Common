using System;
using System.Collections.Generic;

namespace CodingDad.NET.Common.Events
{
	public class EventAggregator
	{
		private static EventAggregator? _instance;
		private readonly Dictionary<Type, List<object>> _eventSubscribers = new();
		public static EventAggregator Instance => _instance ??= new EventAggregator();

		public void Publish<TEvent> (TEvent eventToPublish)
		{
			var eventType = typeof(TEvent);
			if (_eventSubscribers.ContainsKey(eventType))
			{
				foreach (var subscriber in _eventSubscribers [eventType])
				{
					var handler = subscriber as Action<TEvent>;
					handler?.Invoke(eventToPublish);
				}
			}
		}

		public void Subscribe<TEvent> (Action<TEvent> handler)
		{
			var eventType = typeof(TEvent);
			if (!_eventSubscribers.ContainsKey(eventType))
			{
				_eventSubscribers [eventType] = new List<object>();
			}

			_eventSubscribers [eventType].Add(handler);
		}

		public void Unsubscribe<TEvent> (Action<TEvent> handler)
		{
			var eventType = typeof(TEvent);
			if (_eventSubscribers.ContainsKey(eventType))
			{
				_eventSubscribers [eventType].Remove(handler);
			}
		}
	}
}