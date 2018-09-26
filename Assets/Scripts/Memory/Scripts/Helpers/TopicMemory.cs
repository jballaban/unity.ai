using System;
using System.Collections.Generic;
using UnityEngine;

public class TopicMemory : IMemory
{
	public IMemoryID id { get; set; }

	internal TopicMemory(object o) : this((string)o) { }

	public TopicMemory(string topic)
	{
		id = new MemoryID<TopicMemory>(topic.GetHashCode());
	}
}