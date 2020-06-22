using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kons.Utility
{
    /// <summary>
    /// TObj 객체는 initTObj, copyTObj 구현 해야한다. 내부적으로 복사본이 저장 및 반환 되므로 꼭 지킬 것.
    /// 내부에 Pool, List를 가지고 있다. Pool <-> List 간 데이터 이동이 된다.
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    class KnPooledQueue<TObj>
        where TObj : IKnRecycleObj<TObj>, new()
    {
        private int m_capacity = 0;
        private int m_created_count = 0;

        private Queue<TObj> m_pool;
        private Queue<TObj> m_list;

        public KnPooledQueue(int _capacity)
        {
            m_capacity = _capacity;
            m_created_count = 0;

            m_pool = new Queue<TObj>(_capacity);
            m_list = new Queue<TObj>(_capacity);

            increasePool(_capacity);
        }

        public void increasePool(int _capacity)
        {
            for (int i = 0; i < m_capacity; i++)
            {
                TObj item = new TObj();
                m_pool.Enqueue(item);
            }

            m_created_count += _capacity;
        }

        public TObj create()
        {
            TObj ret_obj = default(TObj);
            lock (m_pool)
            {
                if (0 == m_pool.Count)
                {
                    increasePool(m_capacity);
                }
                ret_obj = m_pool.Dequeue();
                ret_obj.initObj();
            }
            m_list.Enqueue(ret_obj);
            return ret_obj;
        }

        public bool pop(ref TObj _des_obj)
        {
            lock (m_list)
            {
                if (0 < m_list.Count)
                {
                    TObj item = m_list.Dequeue();       // pop from list
                    _des_obj.copyObj(item);             // copy
                    m_pool.Enqueue(item);               // return to pool
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // The number of TObj instances in the pool
        public int Count
        {
            get { return m_list.Count; }
        }

        public int Remain
        {
            get { return m_pool.Count; }
        }

        public int TotalCreated
        {
            get { return m_created_count; }
        }
    }
}
