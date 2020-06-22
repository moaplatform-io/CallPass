using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.Object
{
    public class PoolConfigPrintOutput
    {
        public ObjConfigPrintOutput[] m_list = null;

        public int COUNT { get { return (null == m_list ? 0 : m_list.Length); } }
        public bool IS_CHANGE { get; set; }

        // --------------------------------------
        //
        public PoolConfigPrintOutput(int _count)
        {
            m_list = new ObjConfigPrintOutput[_count];
            for (int i = 0; i < _count; i++)
            {
                m_list[i] = new ObjConfigPrintOutput();
            }
        }

        public ObjConfigPrintOutput getObject(int _index)
        {
            if (_index < m_list.Length)
            {
                return m_list[_index];
            }
            return null;
        }

        public void loadObjectAll()
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                m_list[i].loadFromDevice(i);
            }
        }

        public void saveObjectAll()
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                m_list[i].saveToDevice();
            }
        }

        public bool isVailedData()
        {
            Dictionary<String, ObjConfigPrintOutput> dic = new Dictionary<string, ObjConfigPrintOutput>();
            for (int i = 0; i < m_list.Length; i++)
            {
                ObjConfigPrintOutput sel_obj = m_list[i];
                if (sel_obj.IS_USE)
                {
                    String sel_key = sel_obj.m_print_port_num; // 출력포트 겹치는지 확인
                    if (dic.ContainsKey(sel_key))
                    {
                        dic.Clear();
                        return false;
                    }
                    dic.Add(sel_key, sel_obj);
                }
            }
            dic.Clear();
            return true;
        }
    }
}
