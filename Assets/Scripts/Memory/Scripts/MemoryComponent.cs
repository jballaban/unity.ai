using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemoryComponent : MonoBehaviour
{
    Dictionary<IMemoryID, IMemory> memoryById = new Dictionary<IMemoryID, IMemory>();
    Dictionary<Type, HashSet<IMemory>> memoryByType = new Dictionary<Type, HashSet<IMemory>>();
    Dictionary<IMemoryID, Dictionary<Type, HashSet<IMemory>>> associationById = new Dictionary<IMemoryID, Dictionary<Type, HashSet<IMemory>>>();

    public IEnumerable<T> RecallNear<T>(Vector3 position, float radius) where T : class, ILocationMemory
    {
        if (!memoryByType.ContainsKey(typeof(T)))
            return new HashSet<T>();
        return memoryByType[typeof(T)].Cast<T>().Where(x => x.Intersects(position, radius));
    }

    public IEnumerable<T> RecallAll<T>() where T : IMemory
    {
        if (!memoryByType.ContainsKey(typeof(T)))
            return new HashSet<T>();
        return memoryByType[typeof(T)].Cast<T>();
    }

    public T Recall<T>(object o) where T : IMemory
    {
        return Recall<T>(((T)Activator.CreateInstance(typeof(T), o)).id);
    }

    public T Recall<T>(IMemoryID id) where T : IMemory
    {
        if (!memoryById.ContainsKey(id)) return default(T);
        return (T)memoryById[id];
    }

    public T Remember<T>(object o) where T : IMemory
    {
        return (T)Remember((IMemory)Activator.CreateInstance(typeof(T), o));
    }

    public IMemory Remember(IMemory memory)
    {
        memoryById[memory.id] = memory;
        // FYI for now we are only indexing by type itself and not its ancestors even though technicall your can RECALL by ancestor.
        if (!memoryByType.ContainsKey(memory.GetType()))
            memoryByType[memory.GetType()] = new HashSet<IMemory>() { memory };
        else
            memoryByType[memory.GetType()].Add(memory);
        return memory;
    }

    public bool IsKnown(IMemoryID id)
    {
        return memoryById.ContainsKey(id);
    }

    public IEnumerable<T> GetAssociations<T>(IMemory memory) where T : IMemory
    {
        if (!associationById.ContainsKey(memory.id) || !associationById[memory.id].ContainsKey(typeof(T)))
            return new HashSet<T>();
        return associationById[memory.id][typeof(T)].Cast<T>();
    }

    public void Disassociate(IMemory a, IMemory b)
    {
        disassociate(a, b);
        disassociate(b, a);
    }

    public void Associate(params IMemory[] memories)
    {
        for (int a = 0; a < memories.Length - 1; a++)
        {
            if (!IsKnown(memories[a].id))
                throw new Exception("Cannot associate an unknown memory: " + memories[a].id);
            for (int b = a + 1; b < memories.Length; b++)
            {
                associate(memories[a], memories[b]);
                associate(memories[b], memories[a]);
            }
        }
    }

    void disassociate(IMemory a, IMemory b)
    {
        if (associationById.ContainsKey(a.id) && associationById[a.id].ContainsKey(b.GetType()))
            associationById[a.id][b.GetType()].Remove(b);
    }

    void associate(IMemory a, IMemory b)
    {
        if (!associationById.ContainsKey(a.id))
            associationById[a.id] = new Dictionary<Type, HashSet<IMemory>>() { { b.GetType(), new HashSet<IMemory>() { b } } };
        else if (!associationById[a.id].ContainsKey(b.GetType()))
            associationById[a.id][b.GetType()] = new HashSet<IMemory>() { b };
        else
            associationById[a.id][b.GetType()].Add(b);
    }
}