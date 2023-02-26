using System.Collections.Generic;

namespace LiteEntitySystem.Extensions;

public static class QueueExtensions
{
    public static bool TryPeek<T>(this Queue<T> queue, out T result)
    {
        result = default;
        if (queue.Count == 0)
            return false;
        result = queue.Peek();
        return true;
    }

    public static bool TryDequeue<T>(this Queue<T> queue, out T result)
    {
        result = default;
        if (queue.Count == 0)
            return false;
        result = queue.Dequeue();
        return true;
    }
}