using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.Object
{
    public class ObjProcedureResult
    {
        public const Int32 RESULT_OK = 1; // 프로시저에서 성공은 항상 1을 리턴한다. - 이렇게 규칙을 지킬 것

        private int m_ret_cd = 0;
        private Int64 m_ret_val = 0;
        private string m_ret_msg = "";

        public int ResultCode { get { return m_ret_cd; } }
        public Int64 ResultValue { get { return m_ret_val; } }
        public string ResultMessage { get { return m_ret_msg; } }

        public ObjProcedureResult(int _ret_cd = 0, Int64 _ret_val = 0, String _ret_msg = "")
        {
            m_ret_cd = _ret_cd;
            m_ret_val = _ret_val;
            m_ret_msg = _ret_msg;
        }

        public bool setResult(DataTable _dt)
        {
            if (null != _dt && null != _dt.Rows && 0 < _dt.Rows.Count)
            {
                try
                {
                    m_ret_cd = Kons.Utility.KnUtil.parseInt32(_dt.Rows[0]["ret_cd"].ToString());
                    m_ret_val = Kons.Utility.KnUtil.parseInt64(_dt.Rows[0]["ret_val"].ToString());
                    m_ret_msg = _dt.Rows[0]["ret_msg"].ToString();
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
