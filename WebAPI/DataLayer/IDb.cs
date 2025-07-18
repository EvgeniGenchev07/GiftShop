﻿namespace DataLayer;

public interface IDb<T, K>
{
    Task Create(T item);
    Task<T> Read(K key, bool useNavigationalProperties = false, bool isReadOnly = false);
    Task<List<T>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false);
    Task Update(T item, bool useNavigationalProperties = false);
    Task Delete(K key);
}
