using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kons.Utility
{
    public class KnFormWaitCursors : IDisposable
    {
        private Form m_owner = null;

        public KnFormWaitCursors(Form _owner)
        {
            m_owner = _owner;
            if (null != m_owner && m_owner.IsHandleCreated)
            {
                m_owner.Cursor = Cursors.WaitCursor;
            }
        }

        void IDisposable.Dispose()
        {
            if (null != m_owner && m_owner.IsHandleCreated)
            {
                m_owner.Cursor = Cursors.Default;
            }
        }
    }
}
