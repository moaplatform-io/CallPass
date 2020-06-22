using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.Utility
{
    public class KnNotify
    {
        // ---------------------------------------------------------- notify basic function
        //
        public delegate void MyNotifyHandler(object _sender, object _what, object _obj);

        public event MyNotifyHandler Notify;
        public object NotifySender = null;

        protected void sendMyNotify(object _what, object _obj)
        {
            if (null != Notify)
            {
                Notify(NotifySender, _what, _obj);
            }
        }

        protected void sendMyNotify(object _sender, object _what, object _obj)
        {
            if (null != Notify)
            {
                Notify(_sender, _what, _obj);
            }
        }
    }
}
