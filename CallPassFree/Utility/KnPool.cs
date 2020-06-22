using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kons.Utility
{
    class KnPool<TObj>
        where TObj : new()
    {
        private int m_capacity = 0;
        private int m_created_count = 0;

        private Queue<TObj> m_pool = null;

        // Initializes the object pool to the specified size
        //
        // The "capacity" parameter is the maximum number of 
        // TObj objects the pool can hold
        public KnPool(int _capacity)
        {
            m_capacity = _capacity;
            m_created_count = 0;

            m_pool = new Queue<TObj>(_capacity);

            increasePool(_capacity);
        }

        public void increasePool(int _capacity)
        {
            for(int i=0; i<_capacity; i++)
            {
                TObj item = new TObj();
                m_pool.Enqueue(item);
            }
            m_created_count += _capacity;
        }

        // Add a SocketAsyncEventArg instance to the pool
        //
        //The "item" parameter is the TObj instance 
        // to add to the pool
        public void push(TObj _item)
        {
            if (_item == null) { throw new ArgumentNullException("Items added to a TObj cannot be null"); }
            lock (m_pool)
            {
                m_pool.Enqueue(_item);
            }
        }

        // Removes a TObj instance from the pool
        // and returns the object removed from the pool
        public TObj pop()
        {
            lock (m_pool)
            {
                if (0 == m_pool.Count)
                {
                    increasePool(m_capacity);
                }
                return m_pool.Dequeue();
            }
        }

        // The number of TObj instances in the pool
        public int Count
        {
            get { return m_pool.Count; }
        }

        public int TotalCreated
        {
            get { return m_created_count; }
        }
    }
}
