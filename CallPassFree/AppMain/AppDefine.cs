using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.AppMain
{
    class AppDefine
    {
        // accept time out nano tick
        public const long TICKS_NANO_SEC_01 = ((long)10000000 * 01);        // 01초를 나타냄
        public const long TICKS_NANO_SEC_05 = ((long)10000000 * 05);        // 05초를 나타냄
        public const long TICKS_NANO_SEC_10 = ((long)10000000 * 10);        // 10초를 나타냄
        public const long TICKS_NANO_SEC_15 = ((long)10000000 * 15);        // 15초
        public const long TICKS_NANO_SEC_20 = ((long)10000000 * 20);        // 20초
        public const long TICKS_NANO_SEC_60 = ((long)10000000 * 60);        // 60초
        public const long TICKS_NANO_SEC_90 = ((long)10000000 * 90);        // 90초
    }
}
