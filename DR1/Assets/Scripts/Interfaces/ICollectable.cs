using System;
public interface ICollectable<T> 
{
    event Action<T> Collect;
}
