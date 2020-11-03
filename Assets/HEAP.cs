using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEAP<T> where T: IHeapItem<T>
{
    T[] items;
    int currentItemCount;


    public HEAP(int maxHeapSize)
    {
        
        items = new T[maxHeapSize];
    }

    public void add(T item)
    {
        item.HeadIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }
    public T removeFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeadIndex = 0;
        SortDown(items[0]);
        return firstItem;
        
    }
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }
    public void UpdateItem(T item)
    {
        SortUp(item);

    }
    public bool contains(T item)
    {
        return Equals(items[item.HeadIndex], item);
    }

    //parent : (n-1)/2
    // child left : 2n+1
    //child right : 2n+2
    void SortDown(T item)
    {
        while (true)
        {
            int childindexLeft = item.HeadIndex * 2 + 1;
            int childindexRight = item.HeadIndex * 2 + 2;
            int swapIndex = 0;
            if(childindexLeft < currentItemCount)
            {
                swapIndex = childindexLeft;
                if(childindexRight < currentItemCount)
                {
                    if(items[childindexLeft].CompareTo(items[childindexRight]) < 0)
                    {
                        swapIndex = childindexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
    void SortUp(T item)
    {
        int parentIndex = (item.HeadIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeadIndex - 1) / 2;

        }
    }
    void Swap(T itemA , T itemB)
    {
        items[itemA.HeadIndex] = itemB;
        items[itemB.HeadIndex] = itemA;
        int itemAIndex = itemA.HeadIndex;
        itemA.HeadIndex = itemB.HeadIndex;
        itemB.HeadIndex = itemAIndex;
    }
}
public interface IHeapItem<T> : IComparable<T>{
        int HeadIndex
    {
        get;
        set;
    }
}


