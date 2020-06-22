using Kons.ShopCallpass.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.AppMain
{
    public class AppDoc
    {
        // 주문목록
        private Dictionary<String, ObjOrder> m_dic_order = new Dictionary<string, ObjOrder>();

        // 일회성 설정값
        public int m_is_use_save_order_input = 0;
        public int m_is_use_send_order_input = 0;

        // ----------------------------------------------------------
        //
        public String setObjOrder(ObjOrder _order)
        {
            if (null == _order || null == _order.m_order_num || 0 == _order.m_order_num.Length)
            {
                return null;
            }

            if (m_dic_order.ContainsKey(_order.m_order_num))
            {
                m_dic_order[_order.m_order_num] = _order;
            }
            else
            {
                m_dic_order.Add(_order.m_order_num, _order);
            }

            return _order.m_order_num;
        }

        public ObjOrder getObjOrder(String _order_num)
        {
            if (null == _order_num || 0 == _order_num.Length)
            {
                return null;
            }

            if (m_dic_order.ContainsKey(_order_num))
            {
                return m_dic_order[_order_num];
            }

            return null;
        }
    }
}
