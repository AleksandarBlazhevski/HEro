using System;
using System.Collections.Generic;

public class MarbleBag<T>
{
    private readonly List<T> fullListing;
    private readonly List<T> current = new List<T>();

    public MarbleBag(IEnumerable<T> contents)
    {
        if(contents == null)
        {
            throw new ArgumentNullException(nameof(contents));
        }

        fullListing = new List<T>(contents);
    }

    public void Reset()
    {
        current.Clear();
        current.AddRange(fullListing);

        for(int i = current.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T swap = current[i];
            current[i] = current[j];
            current[j] = swap;
        }
    }

    public T Next()
    {
        if(fullListing.Count == 0)
        {
            throw new InvalidOperationException("Marble bag is empty.");
        }

        if(current.Count == 0)
        {
            Reset();
        }

        int last = current.Count - 1;
        T result = current[last];
        current.RemoveAt(last);

        return result;
    }
}

