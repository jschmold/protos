using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static Engine.LangHelpers;
using Engine.Exceptions;

namespace Engine.Types {
    /// <summary>
    /// Full encapsulation for a List<typeparamref name="T"/>, but errors when adding is attempted past a certain limit.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CappedList<T> {
        private List<T> Contents {
            get; set;
        } = new List<T>( );
        public uint Limit {
            get; set;
        }
        public CappedList(uint lim) {
            Limit = lim;
            Contents = new List<T>( );
        }
        public CappedList(IEnumerable<T> collection, uint lim) : this(lim) => AddRange(collection);

        public T this[int key] {
            get => Contents[key];
            set => Contents[key] = value;
        }
        public int Count => Contents.Count;

        public bool LimitReached => Contents.Count == Limit;
        public bool UnderLimit => Contents.Count < Limit;
        public bool CanHold(uint more) => Contents.Count + more <= Limit;
        public bool CanHold(int more) => Contents.Count + more <= Limit;

        public void Add(T obj) => Perform(UnderLimit,
            new LimitMetException( ), () => Contents.Add(obj));
        public void AddRange(IEnumerable<T> obj) => Perform(CanHold(obj.Count( )),
            new LimitMetException( ), () => Contents.AddRange(obj));
        public IEnumerable<T> AsReadOnly() => Contents.AsReadOnly( );
        public int BinarySearch(T obj) => Contents.BinarySearch(obj);
        public void Clear() => Contents.Clear( );
        public bool Contains(T item) => Contents.Contains(item);
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => Contents.ConvertAll(converter);
        public void CopyTo(T[] arr) => Contents.CopyTo(arr);
        public override bool Equals(object obj) => Contents.Equals(obj);
        public bool Exists(Predicate<T> match) => Contents.Exists(match);
        public T Find(Predicate<T> match) => Contents.Find(match);
        public List<T> FindAll(Predicate<T> match) => Contents.FindAll(match);
        public int FindLast(Predicate<T> match) => Contents.FindIndex(match);
        public int FindLastIndex(Predicate<T> match) => Contents.FindLastIndex(match);
        public void ForEach(Action<T> action) => Contents.ForEach(action);
        public List<T>.Enumerator GetEnumerator() => Contents.GetEnumerator( );
        public override int GetHashCode() => Contents.GetHashCode( );
        public List<T> GetRange(int index, int count) => Contents.GetRange(index, count);
        public int IndexOf(T item) => Contents.IndexOf(item);
        public void Insert(int index, T item) => Perform(UnderLimit,
            new LimitMetException( ), () => Contents.Insert(index, item));
        public void InsertRange(int index, IEnumerable<T> collection) => Perform(CanHold(collection.Count( )),
            new LimitMetException( ), () => Contents.InsertRange(index, collection));
        public int LastIndexOf(T item) => Contents.LastIndexOf(item);
        public int LastIndexOf(T item, int index) => Contents.LastIndexOf(item, index);
        public int LastIndexOf(T item, int index, int count) => Contents.LastIndexOf(item, index, count);
        public bool Remove(T item) => Contents.Remove(item);
        public int RemoveAll (Predicate<T> match) => Contents.RemoveAll(match);
        public void RemoveAt(int index) => Contents.RemoveAt(index);
        public void RemoveRange(int index, int count) => Contents.RemoveRange(index, count);
        public void Reverse() => Contents.Reverse( );
        public void Sort() => Contents.Sort( );
        public void Sort(Comparison<T> comparison) => Contents.Sort(comparison);
        public void Sort(IComparer<T> comparer) => Contents.Sort(comparer);
        public void Sort(int index, int count, IComparer<T> comparer) => Contents.Sort(index, count, comparer);
        public T[] ToArray() => Contents.ToArray( );
        public void TrimExcess() => Contents.TrimExcess( );
        public bool TrueForAll(Predicate<T> match) => Contents.TrueForAll(match);
    }
}
