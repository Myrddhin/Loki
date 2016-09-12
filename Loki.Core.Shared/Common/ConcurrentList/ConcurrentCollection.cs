using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Loki.Common
{
    public class ConcurrentCollection<T> : IEnumerable<IListNode<T>>
    {
        private class NodeIterator : IEnumerator<IListNode<T>>
        {
            private readonly ConcurrentCollection<T> parentList;

            private readonly long currentVersion;

            private ConcurrentListNode<T> currentNode;

            public IListNode<T> Current => this.currentNode;

            public void Dispose()
            {
                lock (parentList.syncLock)
                {
                    if (currentNode != null)
                    {
                        currentNode.Read = false;
                    }
                }

                parentList.StopIterating();
            }

            object IEnumerator.Current => this.Current;

            public bool MoveNext()
            {
                lock (parentList.syncLock)
                {
                    var node = this.currentNode == null ? this.parentList.firstNode : this.currentNode.Next;

                    while (node != null && (node.Version > currentVersion || node.Deleted))
                    {
                        node = node.Next;
                    }

                    if (node != null)
                    {
                        node.Read = true;
                    }

                    if (currentNode != null)
                    {
                        currentNode.Read = false;
                    }

                    currentNode = node;
                }

                return currentNode != null;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public NodeIterator(ConcurrentCollection<T> list, long version)
            {
                this.currentVersion = version;
                this.parentList = list;
            }
        }

        private readonly object syncLock = new object();

        private readonly ConcurrentQueue<ConcurrentListNode<T>> itemsToRemove;

        private long currentVersion;

        private ConcurrentListNode<T> firstNode;

        private ConcurrentListNode<T> lastNode;

        private int iteratorCount;

        public bool IsEmpty => this.firstNode == null;

        public ConcurrentCollection()
        {
            itemsToRemove = new ConcurrentQueue<ConcurrentListNode<T>>();
        }

        public void Add(T value)
        {
            lock (syncLock)
            {
                currentVersion++;
                ConcurrentListNode<T> nodeToAdd = new ConcurrentListNode<T>(value, currentVersion);

                if (this.lastNode == null)
                {
                    this.lastNode = nodeToAdd;

                    // visible for read
                    this.firstNode = nodeToAdd;
                }
                else
                {
                    var previousLast = this.lastNode;
                    nodeToAdd.Previous = previousLast;
                    this.lastNode = nodeToAdd;

                    // visible for read
                    previousLast.Next = nodeToAdd;
                }
            }
        }

        private void PurgeItems()
        {
            ConcurrentListNode<T> nodeBuffer;
            ConcurrentListNode<T> nodeToRemove = null;

            if (!this.itemsToRemove.TryPeek(out nodeBuffer))
            {
                return;
            }

            while (this.itemsToRemove.Count > 0 && nodeBuffer != nodeToRemove)
            {
                if (this.itemsToRemove.TryDequeue(out nodeToRemove))
                {
                    if (nodeToRemove.Read)
                    {
                        // is read, try later
                        this.itemsToRemove.Enqueue(nodeToRemove);
                    }
                    else
                    {
                        this.InternalRemove(nodeToRemove);
                    }
                }

                this.itemsToRemove.TryPeek(out nodeToRemove);
            }
        }

        private void StopIterating()
        {
            iteratorCount--;

            if (iteratorCount == 0)
            {
                PurgeItems();
            }
        }

        public void Remove(IListNode<T> nodeToRemove)
        {
            ConcurrentListNode<T> buffer = nodeToRemove as ConcurrentListNode<T>;
            if (buffer == null)
            {
                return;
            }

            buffer.Deleted = true;
            if (iteratorCount == 0)
            {
                this.InternalRemove(buffer);
                PurgeItems();
            }
            else
            {
                itemsToRemove.Enqueue(buffer);
            }
        }

        private void InternalRemove(ConcurrentListNode<T> nodeToRemove)
        {
            lock (syncLock)
            {
                if (nodeToRemove.Read)
                {
                    // is read ; try later
                    itemsToRemove.Enqueue(nodeToRemove);
                    return;
                }

                var previousNode = nodeToRemove.Previous;
                var nextNode = nodeToRemove.Next;

                if (previousNode != null)
                {
                    // remove from read if in the list
                    previousNode.Next = nextNode;
                }

                if (nodeToRemove == this.firstNode)
                {
                    // remove from read if first
                    this.firstNode = nextNode;
                }

                if (nextNode != null)
                {
                    nextNode.Previous = previousNode;
                }

                if (nodeToRemove == this.lastNode)
                {
                    this.lastNode = previousNode;
                }

                nodeToRemove.Next = null;
                nodeToRemove.Previous = null;
            }
        }

        public IEnumerator<IListNode<T>> GetEnumerator()
        {
            iteratorCount++;
            return new NodeIterator(this, this.currentVersion);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}