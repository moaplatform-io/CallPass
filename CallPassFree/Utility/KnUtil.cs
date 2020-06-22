using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Kons.Utility
{
    class KnUtil
    {
        static public int getStringBufferLength(char[] _buffer)
        {
            int iLen = System.Array.IndexOf(_buffer, '\0');

            return iLen == -1 ? _buffer.Length : iLen;
        }

        static public void insertListBoxAtLast(ListBox listBox, int maxShowCount, string msg)
        {
            if (null == listBox)
            {
                return;
            }

            if (maxShowCount < listBox.Items.Count)
            {

                for (int i = 0; i < listBox.Items.Count - maxShowCount; i++)
                {
                    listBox.Items.RemoveAt(0);
                }
            }

            listBox.Items.Add(msg);
        }

        static public void insertListBoxAtFirst(ListBox listBox, int maxShowCount, string msg)
        {
            if (null == listBox)
            {
                return;
            }

            if (maxShowCount < listBox.Items.Count)
            {

                for (int i = 0; i < listBox.Items.Count - maxShowCount; i++)
                {
                    listBox.Items.RemoveAt(listBox.Items.Count - 1);
                }
            }

            listBox.Items.Insert(0, msg);
        }

        // byte[] -> double
        static public double doubleFromByte(byte[] _buffer, int _offset)
        {
            return (((_buffer[_offset++] & 0xff))
                | ((_buffer[_offset++] & 0xff) << 8)
                | ((_buffer[_offset++] & 0xff) << 16)
                | ((_buffer[_offset++] & 0xff) << 24)
                | ((_buffer[_offset++] & 0xff) << 32)
                | ((_buffer[_offset++] & 0xff) << 40)
                | ((_buffer[_offset++] & 0xff) << 48)
                | ((_buffer[_offset++] & 0xff) << 56));
        }

        // byte[] -> double, return offset
        static public double doubleFromByte(byte[] _buffer, int[] _offset)
        {
            double value = (double)(((_buffer[_offset[0]++] & 0xff))
                                  | ((_buffer[_offset[0]++] & 0xff) << 8)
                                  | ((_buffer[_offset[0]++] & 0xff) << 16)
                                  | ((_buffer[_offset[0]++] & 0xff) << 24)
                                  | ((_buffer[_offset[0]++] & 0xff) << 32)
                                  | ((_buffer[_offset[0]++] & 0xff) << 40)
                                  | ((_buffer[_offset[0]++] & 0xff) << 48)
                                  | ((_buffer[_offset[0]++] & 0xff) << 56));
            return value;
        }

        // byte[] -> int
        static public Int32 intFromByte(byte[] _buffer, int offset)
        {
            return (((_buffer[offset++]) & 0x000000ff)
                  | ((_buffer[offset++] << 8) & 0x0000ff00)
                  | ((_buffer[offset++] << 16) & 0x00ff0000)
                  | (Int32)((_buffer[offset++] << 24) & 0xff000000));
        }

        // byte[] -> int, return offset
        static public int intFromByte(byte[] _buffer, int[] _offset)
        {
            int value = (int)(((_buffer[_offset[0]++]) & 0x000000ff)
                            | ((_buffer[_offset[0]++] << 8) & 0x0000ff00)
                            | ((_buffer[_offset[0]++] << 16) & 0x00ff0000)
                            | (Int32)((_buffer[_offset[0]++] << 24) & 0xff000000));
            return value;
        }

        // byte[] -> short
        static public short shortFromByte(byte[] _buffer, int offset)
        {
            return (short)((_buffer[offset++] & 0xff)
                         | (_buffer[offset++] & 0xff) << 8);
        }

        // byte[] -> short, return offset
        static public short shortFromByte(byte[] _buffer, int[] _offset)
        {
            short value = (short)((_buffer[_offset[0]++] & 0xff)
                                | (_buffer[_offset[0]++] & 0xff) << 8);
            return value;
        }

        // byte[] -> String
        static public String byteFromString(byte[] _buffer)
        {
            String msg = "";
            try
            {
                msg = System.Text.Encoding.UTF8.GetString(_buffer);
            }
            catch (Exception)
            {
            }
            return msg;
        }

        // byte[] -> String
        static public String byteFromString(byte[] _buffer, int _offset, int length)
        {
            String msg = "";
            try
            {
                msg = System.Text.Encoding.UTF8.GetString(_buffer, _offset, length);
            }
            catch (Exception)
            {
            }
            return msg;
        }

        // byte[] -> String
        static public String byteFromString(byte[] _buffer, int _offset, int length, int[] _ref_offset)
        {
            String msg = "";
            try
            {
                msg = System.Text.Encoding.UTF8.GetString(_buffer, _offset, length);
            }
            catch (Exception)
            {
            }
            _ref_offset[0] = _offset + length;
            return msg;
        }

        // double -> byte[], return offset
        static public void setByteWithDouble(byte[] _buffer, int[] _offset, int value)
        {
            _buffer[_offset[0]++] = (byte)(value & 0xFF);
            _buffer[_offset[0]++] = (byte)((value >> 8) & 0xFF);
            _buffer[_offset[0]++] = (byte)((value >> 16) & 0xFF);
            _buffer[_offset[0]++] = (byte)((value >> 24) & 0xFF);
            _buffer[_offset[0]++] = (byte)((value >> 32) & 0xFF);
            _buffer[_offset[0]++] = (byte)((value >> 40) & 0xFF);
            _buffer[_offset[0]++] = (byte)((value >> 48) & 0xFF);
            _buffer[_offset[0]++] = (byte)((value >> 56) & 0xFF);
        }

        // int -> byte[], return offset
        static public void setByteWithInt(byte[] _buffer, int[] _offset, int value)
        {
            _buffer[_offset[0]++] = (byte)(value & 0xFF);
            _buffer[_offset[0]++] = (byte)((value & 0xFF00) >> 8);
            _buffer[_offset[0]++] = (byte)((value & 0xFF0000) >> 16);
            _buffer[_offset[0]++] = (byte)((value & 0xFF000000) >> 24);
        }

        // short -> byte[], return offset
        static public void setByteWithShort(byte[] _buffer, int[] _offset, short value)
        {
            _buffer[_offset[0]++] = (byte)(value & 0xFF);
            _buffer[_offset[0]++] = (byte)((value & 0xFF00) >> 8);
        }

        // My Phone Number
        static public String formatTelNumber(String _tel_num)
        {
            if (null == _tel_num)
            {
                return "";
            }

            int nLen = _tel_num.Length;
            if (nLen == 8)
            {
                String tmpTel1 = _tel_num.Substring(0, 4);
                String tmpTel2 = _tel_num.Substring(4, 4);
                return String.Format("{0}{1}", tmpTel1, tmpTel2);
            }
            else if (nLen == 9)
            {
                if (_tel_num.Substring(0, 2) == "02")
                {
                    String tmpTel1 = _tel_num.Substring(0, 2);
                    String tmpTel2 = _tel_num.Substring(2, 3);
                    String tmpTel3 = _tel_num.Substring(5, 4);
                    return String.Format("{0}{1}{2}", tmpTel1, tmpTel2, tmpTel3);
                }
            }
            else if (nLen == 10)
            {
                if (_tel_num.Substring(0, 2) == "02")
                {
                    String tmpTel1 = _tel_num.Substring(0, 2);
                    String tmpTel2 = _tel_num.Substring(2, 4);
                    String tmpTel3 = _tel_num.Substring(6, 4);
                    return String.Format("{0}{1}{2}", tmpTel1, tmpTel2, tmpTel3);
                }
                else
                {
                    String tmpTel1 = _tel_num.Substring(0, 3);
                    String tmpTel2 = _tel_num.Substring(3, 3);
                    String tmpTel3 = _tel_num.Substring(6, 4);
                    return String.Format("{0}{1}{2}", tmpTel1, tmpTel2, tmpTel3);
                }
            }
            else if (nLen == 11)
            {
                String str = _tel_num.Substring(0, 3);
                bool bType = str.Equals("050");
                String Tel1 = bType ? _tel_num.Substring(0, 4) : _tel_num.Substring(0, 3);
                String Tel2 = bType ? _tel_num.Substring(4, 3) : _tel_num.Substring(3, 4);
                String Tel3 = bType ? _tel_num.Substring(7, 4) : _tel_num.Substring(7, 4);
                return String.Format("{0}{1}{2}", Tel1, Tel2, Tel3);
            }
            else if (nLen == 12)
            {
                String Tel1 = _tel_num.Substring(0, 4);
                String Tel2 = _tel_num.Substring(4, 4);
                String Tel3 = _tel_num.Substring(8, 4);
                return String.Format("{0}{1}{2}", Tel1, Tel2, Tel3);
            }

            return _tel_num;
        }

        /*
        int won = 123456890;
        Console.WriteLine(string.Format("{0:n0}", won));
        Console.WriteLine(string.Format("{0}", won.ToString("n0"))); ;
        Console.WriteLine(string.Format("{0:#,##0}", won));
        Console.WriteLine(string.Format("{0}", won.ToString("#,##0")));
        */
        static public String formatMoney(int _money)
        {
            return String.Format("{0:n0}", _money);
        }

        static public String formatSocialNo(string _num)
        {
            if (13 != _num.Length)
            {
                return _num;
            }
            return (_num.Substring(0, 6) + "-" + _num.Substring(6, 7));
        }

        /// <summary>
        /// '121 Km', '252 원' 등 띄워쓰기를 기준으로 하여 숫자 이외의 값을 포함한 경우 사용한다.
        /// parseInt32() 함수의 속도(?)를 매번 희생하지 않기 위해 합치지 않고 별도로 분리 했음.
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        static public Int32 formatInt32(string _value)
        {
            if (null == _value || "" == _value)
            {
                return 0;
            }

            _value.Trim();
            int chop_idx = _value.IndexOf(" ");
            if (0 < chop_idx)
            {
                _value = _value.Substring(0, chop_idx);
            }

            return parseInt32(_value);
        }

        /// <summary>
        /// '121 Km', '252 원' 등 띄워쓰기를 기준으로 하여 숫자 이외의 값을 포함한 경우 사용한다. 덧붙여셔 , 를 제거해 준다.
        /// parseInt32() 함수의 속도(?)를 매번 희생하지 않기 위해 합치지 않고 별도로 분리 했음.
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        static public Int32 formatInt32FromMoneyFormat(string _value)
        {
            if (null == _value || "" == _value)
            {
                return 0;
            }

            _value.Trim();
            int chop_idx = _value.IndexOf(" ");
            if (0 < chop_idx)
            {
                _value = _value.Substring(0, chop_idx);
            }
            _value = _value.Replace(",", "");

            return parseInt32(_value);
        }

        static public String formatHideTelNo(object _num)
        {
            if (null == _num)
            {
                return "";
            }
            return KnUtil.formatHideTelNo(_num.ToString());
        }

        static public String formatHideTelNo(string _num)
        {
            if (null == _num)
            {
                return "";
            }
            if (5 > _num.Length)
            {
                return _num;
            }
            return ("***-****-" + _num.Substring(_num.Length - 4, 4));
        }

        // ----------------------------------------------------------
        //
        static public Int64 parseInt64(string _value)
        {
            if (null == _value || "" == _value)
            {
                return 0;
            }

            try
            {
                return Int64.Parse(_value);
            }
            catch { }

            return 0;
        }

        /// <summary>
        /// 단순 변환. formatInt32() 와 비교됨.
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        static public Int32 parseInt32(string _value)
        {
            if (null == _value || "" == _value)
            {
                return 0;
            }

            try
            {
                return Int32.Parse(_value);
            }
            catch { }

            return 0;
        }

        static public float parsefloat(string _value)
        {
            if (null == _value || "" == _value)
            {
                return 0;
            }

            try
            {
                return float.Parse(_value);
            }
            catch { }

            return 0;
        }

        static public Double parseDouble(string _value)
        {
            if (null == _value || "" == _value)
            {
                return 0;
            }

            try
            {
                return Double.Parse(_value);
            }
            catch { }

            return 0;
        }

        static public String parseNormalString(string _value)
        {
            if (null == _value || "" == _value)
            {
                return "";
            }

            try
            {
                return _value.Replace("-", "").Trim();
            }
            catch { }

            return _value;
        }

        static public DateTime parseDateTime(string _value)
        {
            if (null == _value || "" == _value)
            {
                return DateTime.MinValue;
            }

            try
            {
                return DateTime.Parse(_value);
            }
            catch { }

            return DateTime.MinValue;
        }

        static public byte[] parseSqlBinary(object _value)
        {
            if (null == _value || DBNull.Value == _value)
            {
                return null;
            }
            return (byte[])_value;
        }

        static public DateTime getDateTimeFromYMD(Int32 _ymd)
        {
            if (0 == _ymd)
            {
                return DateTime.MinValue;
            }

            try
            {
                return new DateTime((_ymd / 10000), (_ymd % 10000 / 100), (_ymd % 100));
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        static public DateTime getDateTimeFromYMDHM(Int32 _ymd, Int32 _hm)
        {
            if (0 == _ymd)
            {
                return DateTime.MinValue;
            }

            try
            {
                return new DateTime((_ymd / 10000), (_ymd % 10000 / 100), (_ymd % 100), (_hm / 100), (_hm % 100), 0);
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        static public DateTime getDateTimeFromYMDHMS(Int32 _ymd, Int32 _hms)
        {
            if (0 == _ymd)
            {
                return DateTime.MinValue;
            }

            try
            {
                return new DateTime((_ymd / 10000), (_ymd % 10000 / 100), (_ymd % 100), (_hms / 10000), (_hms % 10000 / 100), (_hms % 100));
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }

        }

        static public Int32 getIntDateYMD(DateTime _datetime)
        {
            if (DateTime.MinValue == _datetime)
            {
                return 0;
            }
            return (_datetime.Year * 10000) + (_datetime.Month * 100) + _datetime.Day;
        }

        static public bool isEmptyString(String _string)
        {
            if (null == _string)
            {
                return true;
            }

            if (0 == _string.Trim().Length)
            {
                return true;
            }

            return false;
        }

        static public bool isDigit(string _string)
        {
            if (_string == null) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(_string, "^\\d+$");
        }

        static public bool isInt(string _string)
        {
            if (_string == null) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(_string, @"^[+-]?\d*$");
        }

        static public bool isDouble(string _string)
        {
            if (_string == null) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(_string, @"^[+-]?\d*(\.?\d*)$");
        }

        // ----------------------------------------------------------
        //
        // 해당컬럼을 있는지 확인해서 없으면 추가한다.
        static public void insureTableColumn(ref DataTable _table, String _column_name, Type _type)
        {
            if (!_table.Columns.Contains(_column_name))
            {
                _table.Columns.Add(_column_name, _type);
            }
        }

        static public String getDataRowString(DataRow _dr, String _column_name)
        {
            if (_dr.Table.Columns.Contains(_column_name))
            {
                return _dr[_column_name].ToString();
            }
            return "";
        }

        static public bool setDataRowData(DataRow _dr, String _column_name, object _data)
        {
            if (_dr.Table.Columns.Contains(_column_name))
            {
                _dr[_column_name] = _data;
                return true;
            }
            return false;
        }

        static public void delayExecFuntion(Control _owner, Delegate _action, int _delay_ms)
        {
            Task.Factory.StartNew(() =>
            {
                if (0 < _delay_ms)
                {
                    Thread.Sleep(_delay_ms);
                }
                _owner.Invoke(_action);
            });
        }

        static public void setDlgLocationMousePosition(Point _location, Form _form)
        {
            _form.StartPosition = FormStartPosition.Manual;

            if (null != _form && _form.Visible)
            {
                _form.Location = _location;
            }
        }

        static public void setDlgLocationParentCenter(Form _parnet, Form _form)
        {
            _form.StartPosition = FormStartPosition.Manual;

            if (null != _parnet && _parnet.Visible)
            {
                //_form.StartPosition = FormStartPosition.CenterParent;
                _form.Location = new System.Drawing.Point(((_parnet.Width - _form.Width) / 2) + _parnet.Location.X, ((_parnet.Height - _form.Height) / 2) + _parnet.Location.Y);
            }
            else
            {
                //_form.StartPosition = FormStartPosition.Manual;

                Screen[] screens = Screen.AllScreens;
                System.Drawing.Size form_size = _form.Size;
                System.Drawing.Point center_point;

                foreach (Screen scrn in screens)
                {
                    if (scrn.Primary)
                    {
                        center_point = new System.Drawing.Point((scrn.WorkingArea.Width - _form.Width) / 2, (scrn.WorkingArea.Height - _form.Height) / 2);
                        _form.Location = center_point;

                        break;
                    }
                }
            }
        }

        static public String getMyAppPath()
        {
            // 실행파일 경로 얻기
            string my_app_full_path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            // 경로 및 파일명 만들기
            int temp_len = my_app_full_path.Length;
            int temp_idx = my_app_full_path.LastIndexOf('\\');

            // 경로
            string my_app_path = my_app_full_path.Substring(0, temp_idx);                                   // 순수 경로 이름 얻기

            return my_app_path;
        }

        static public String getMyAppName()
        {
            // 실행파일 경로 얻기
            string my_app_full_path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            // 경로 및 파일명 만들기
            int temp_len = my_app_full_path.Length;
            int temp_idx = my_app_full_path.LastIndexOf('\\');

            // 파일명
            string my_app_name = my_app_full_path.Substring(temp_idx + 1, temp_len - temp_idx - 1);         // 실행 파일 이름 얻기
            if (-1 != my_app_name.LastIndexOf('.'))
            {
                my_app_name = my_app_name.Substring(0, my_app_name.LastIndexOf('.'));                       // 확장자 지우기
            }

            return my_app_name;
        }

        static public string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        static public string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        /// <summary>
        /// 맨앞에 0x를 제거한 값을 넘겨야 한다.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        static public byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
