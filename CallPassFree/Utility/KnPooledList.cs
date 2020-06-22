using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kons.Utility
{
    /// <summary>
    /// 내부에 Pool, List를 가지고 있다. Pool <-> List 간 데이터 이동이 된다.
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    class KnPooledList<TObj>
        where TObj : new()
    {
        private int m_capacity = 0;
        private int m_created_count = 0;

        private Queue<TObj> m_pool;
        private List<TObj>  m_list;

        public KnPooledList(int _capacity)
        {
            m_capacity = _capacity;
            m_created_count = 0;

            m_pool = new Queue<TObj>(_capacity);
            m_list = new List<TObj>(_capacity);

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

        public TObj add()
        {
            TObj ret_obj = default(TObj);
            lock (m_pool)
            {
                if (0 == m_pool.Count)
                {
                    increasePool(m_capacity);
                }
                ret_obj = m_pool.Dequeue();
            }
            m_list.Add(ret_obj);
            return ret_obj;
        }

        public bool del(TObj _obj)
        {
            bool is_del = false;

            lock (m_list)
            {
                is_del = m_list.Remove(_obj);
            }

            if (is_del)
            {
                lock (m_pool)
                {
                    m_pool.Enqueue(_obj);
                }
            }

            return is_del;
        }

        public int delAll(Predicate<TObj> _match_func)
        {
            lock (m_list)
            {
                List<TObj> del_list = m_list.FindAll(_match_func);

                lock (m_pool)
                {
                    foreach (TObj item in del_list)
                    {
                        m_pool.Enqueue(item);
                    }
                }

                foreach (TObj item in del_list)
                {
                    m_list.Remove(item);
                }

                return del_list.Count;
            }
        }

        public void delAll()
        {
            lock (m_list)
            {
                lock (m_pool)
                {
                    foreach (TObj item in m_list)
                    {
                        m_pool.Enqueue(item);
                    }
                }

                m_list.Clear();
            }
        }

        public TObj find(Predicate<TObj> _match_func)
        {
            return m_list.Find(_match_func);
        }

        public List<TObj> findAll(Predicate<TObj> _match_func)
        {
            return m_list.FindAll(_match_func);
        }

        public List<TObj>.Enumerator GetEnumerator()
        {
            return m_list.GetEnumerator();
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
