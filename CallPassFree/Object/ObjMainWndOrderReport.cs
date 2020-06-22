using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.Object
{
    public class ObjMainWndOrderReport
    {
        private int[] m_state_count = new int[(int)ObjOrder.STATE_TYPE.ORDER_STATE_END];
        public int m_order_total_count;

        public int m_customer_pay_cash_count;
        public int m_customer_pay_card_count;
        public int m_customer_pay_step_wait;
        public int m_customer_pay_step_ok;
        //public long m_customer_cost_sum;

        // ----------------------------------------------------------------------------------------
        //
        public void initObj()
        {
            for (int i = 0; i < m_state_count.Length; i++)
            {
                m_state_count[i] = 0;
            }
            m_order_total_count = 0;
        }

        public void setOrderCount(int _state_cd, int _count)
        {
            if (_state_cd >= m_state_count.Length)
            {
                return;
            }
            m_state_count[_state_cd] = _count;
        }

        public int getOrderCount(int _state_cd)
        {
            if (_state_cd >= m_state_count.Length)
            {
                return 0;
            }
            return m_state_count[_state_cd];
        }
    }
}
