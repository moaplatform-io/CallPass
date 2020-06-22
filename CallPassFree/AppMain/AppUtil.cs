using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kons.ShopCallpass.Model;

namespace Kons.ShopCallpass.AppMain
{
    public class ZenComboBoxItem
    {
        public string value { get; set; }
        public object key { get; set; }

        public override string ToString()
        {
            return value;
        }

        public ZenComboBoxItem()
        {
            key = null;
            value = "";
        }

        public ZenComboBoxItem(object _key, string _value)
        {
            key = _key;
            value = _value;
        }
    }

    class AppUtil
    {
        // ---------------------------------------------------------- 
        //
        static public bool isNumberString(string _value)
        {
            char[] cTemp;
            StringBuilder sb = new StringBuilder();
            cTemp = _value.Trim().ToUpper().ToCharArray();
            for (int i = 0; i < cTemp.Length; i++)
            {
                if (cTemp[i] < '0' || cTemp[i] > '9')
                {
                    return false;
                }
            }
            return true;
        }

        static public int convertToInt(string _value)
        {
            if (null == _value || 0 == _value.Trim().Length)
            {
                return 0;
            }
            return Convert.ToInt32(_value.Trim().Replace(",", ""));
        }

        // ---------------------------------------------------------- DIRECTORY
        //
        static public string getDirectoryPathFromFullName(string _full_name)
        {
            if (null == _full_name || 0 == _full_name.Trim().Length)
            {
                return "";
            }

            string[] path_array = _full_name.Split('\\');
            if (1 >= path_array.Length)
            {
                return _full_name;
            }

            StringBuilder temp_path = new StringBuilder();
            for (int i = 0; i < path_array.Length - 1; i++)
            {
                temp_path.Append(path_array[i] + "\\");
            }

            return temp_path.ToString();
        }

        // ---------------------------------------------------------- COMBO BOX
        //
        static public ZenComboBoxItem comboboxAddItem(ref ComboBox _control, string _key, string _value)
        {
            ZenComboBoxItem cb_item = new ZenComboBoxItem();
            cb_item.value = _value;
            cb_item.key = _key;
            _control.Items.Add(cb_item);

            return cb_item;
        }

        static public void comboboxSetSelItemByKey(ref ComboBox _control, string _key)
        {
            foreach (ZenComboBoxItem tmp_item in _control.Items)
            {
                if (_key.Equals(tmp_item.key))
                {
                    _control.SelectedItem = tmp_item;
                    break;
                }
            }
        }

        static public void comboboxSetSelItemByValue(ref ComboBox _control, string _value)
        {
            foreach (ZenComboBoxItem tmp_item in _control.Items)
            {
                if (_value.Equals(tmp_item.value))
                {
                    _control.SelectedItem = tmp_item;
                    break;
                }
            }
        }

        static public string comboboxGetSelKey(ref ComboBox _control)
        {
            if (_control.Items.Count <= 0)
            {
                return "";
            }

            try
            {
                ZenComboBoxItem sel_item = (ZenComboBoxItem)_control.Items[_control.SelectedIndex];
                return sel_item.key.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        static public string comboboxGetSelValue(ref ComboBox _control)
        {
            if (_control.Items.Count <= 0)
            {
                return "";
            }

            try
            {
                ZenComboBoxItem sel_item = (ZenComboBoxItem)_control.Items[_control.SelectedIndex];
                return sel_item.value;
            }
            catch (Exception)
            {
                return "";
            }
        }

        static public string getLoadFileImageFilterString()
        {
            return "Image Files(*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|JPEG Files(*.jpg;*.jpeg;*.jpe)|*.jpg;*.jpeg;*.jpe|TIFF Files(*.tif;*.tiff)|*.tif;*.tiff";
        }

        static public string getSaveFileImageFilterString()
        {
            return "Image File(*.jpg,*.png)|*.jpg;*.png";
        }

        static public string getLoadFileName(IWin32Window _owner, string _filter)
        {
            OpenFileDialog dlg_file = new OpenFileDialog();
            if (null != dlg_file)
            {
                ModelAppDevice device = new ModelAppDevice();

                dlg_file.Filter = _filter;
                dlg_file.InitialDirectory = device.getConfigLastOpenPath();
                dlg_file.Title = "파일을 선택해주세요";
                if (DialogResult.OK == dlg_file.ShowDialog(_owner))
                {
                    return dlg_file.FileName;
                }
            }
            return "";
        }

        static public string getSaveFileName(IWin32Window _owner, string _filter)
        {
            SaveFileDialog dlg_file = new SaveFileDialog();
            if (null != dlg_file)
            {
                ModelAppDevice device = new ModelAppDevice();

                dlg_file.Filter = _filter;
                dlg_file.InitialDirectory = device.getConfigLastOpenPath();
                dlg_file.Title = "저장할 이름을 입력해주세요";
                if (DialogResult.OK == dlg_file.ShowDialog(_owner))
                {
                    return dlg_file.FileName;
                }
            }
            return "";
        }

        // ---------------------------------------------------------- image
        //
        static public int getImageBinaryLength(Image _src_img)
        {
            return AppUtil.convertImageToBinary(_src_img).Length;
        }

        static public Image resizeImage(Image _src_img, int _width, int _height, bool _is_old_dispose = false)
        {
            Bitmap new_img = new Bitmap(_width, _height);
            using (Graphics g = Graphics.FromImage(new_img))
            {
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawImage(_src_img, 0, 0, _width, _height);
            }

            if (_is_old_dispose)
            {
                _src_img.Dispose();
            }

            return new_img;
        }

        public static System.Drawing.Image resizeImageScale(Image image, int Width, int Height, bool needToFill = false)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            double destX = 0;
            double destY = 0;

            double nScale = 0;
            double nScaleW = 0;
            double nScaleH = 0;

            nScaleW = ((double)Width / (double)sourceWidth);
            nScaleH = ((double)Height / (double)sourceHeight);
            if (!needToFill)
            {
                nScale = Math.Min(nScaleH, nScaleW);
            }
            else
            {
                nScale = Math.Max(nScaleH, nScaleW);
                destY = (Height - sourceHeight * nScale) / 2;
                destX = (Width - sourceWidth * nScale) / 2;
            }

            if (nScale > 1)
                nScale = 1;

            int destWidth = (int)Math.Round(sourceWidth * nScale);
            int destHeight = (int)Math.Round(sourceHeight * nScale);

            System.Drawing.Bitmap bmPhoto = null;
            try
            {
                bmPhoto = new System.Drawing.Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
                    destWidth, destX, destHeight, destY, Width, Height), ex);
            }
            using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto))
            {
                //grPhoto.InterpolationMode = _interpolationMode;
                //grPhoto.CompositingQuality = _compositingQuality;
                //grPhoto.SmoothingMode = _smoothingMode;

                Rectangle to = new System.Drawing.Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
                Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
                grPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);

                return bmPhoto;
            }
        }

        static public Image convertImageToJpeg(Image _src_img, bool _is_old_dispose = false)
        {
            if (null == _src_img)
            {
                return null;
            }

            if (ImageFormat.Jpeg.Equals(_src_img.RawFormat))
            {
                return _src_img;
            }

            // Do not use 'using' for MemoryStream when use Image.FromStream method
            // According to MSDN : You must keep the stream open for the lifetime of the Image.  https://msdn.microsoft.com/en-us/library/93z9ee4x(v=vs.110).aspx
            MemoryStream msImgData = new MemoryStream();
            _src_img.Save(msImgData, ImageFormat.Jpeg);

            if (_is_old_dispose)
            {
                _src_img.Dispose();
            }

            return Image.FromStream(msImgData);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        static public byte[] convertImageToBinary(Image _src_img)
        {
            return convertImageToBinary(_src_img, ImageFormat.Jpeg); // 기본값을 JPEG 로 한다.
        }

        static public byte[] convertImageToBinary(Image _src_img, ImageFormat _img_format)
        {
            if (null == _src_img)
            {
                return null;
            }

            byte[] ret_array = null;
            using (MemoryStream msTemp = new MemoryStream())
            {
                _src_img.Save(msTemp, _img_format); // _src_img.RawFormat 로 지정할경우 Bitmap 등의 타입이면 이 함수에서 에러난다. 이때는 Encord 지정하는 것으로 해야 정상 동작한다.
                ret_array = msTemp.ToArray();
            }
            return ret_array;
        }

        static public Image convertBinaryToImage(byte[] _src_arr)
        {
            if (null == _src_arr || 10 > _src_arr.Length)
            {
                return null;
            }

            // Do not use 'using' for MemoryStream when use Image.FromStream method
            // According to MSDN : You must keep the stream open for the lifetime of the Image.  https://msdn.microsoft.com/en-us/library/93z9ee4x(v=vs.110).aspx
            MemoryStream msArrayData = new MemoryStream(_src_arr);
            return Image.FromStream(msArrayData);
        }

        // 최대고정 넓이를 가지는 이미지 데이터를 반환한다.
        static public byte[] getImageFixedWidthBinaryData(int _max_w, int _max_h, Image _img)
        {
            if (null == _img)
            {
                return null;
            }

            // set local value
            Image new_img = (Image)_img.Clone();    // picture box 등에 할당된 원본 이미지에 영향을 안주기 위해복사본을 만든다.

            // 넓이가 RECOMMEND_W 보다 큰 경우 비율에 맞춰 사이즈를 줄인다.
            if (new_img.Width > _max_w)
            {
                int new_h = (new_img.Height * _max_w) / new_img.Width;
                new_img = AppUtil.resizeImage(new_img, _max_w, new_h, true);
            }

            // 높이가 RECOMMEND_H 보다 큰 경우 자른다 - 비율을 맞추지 않는다.
            if (new_img.Height > _max_h)
            {
                new_img = AppUtil.resizeImage(new_img, new_img.Width, _max_h, true);
            }

            // return binary
            return AppUtil.convertImageToBinary(new_img);
        }

        // ---------------------------------------------------------- DATETIME
        //
        static public string datetimeGetYMD(int _ymd)
        {
            return (_ymd / 10000).ToString() + "-" + (_ymd % 10000 / 100).ToString() + "-" + (_ymd / 100).ToString();
        }

        static public void datetimeSetDateWithYMD(ref DateTimePicker control, string _strDate)
        {
            _strDate.Trim();
            _strDate.Replace("-", "");
            if (8 != _strDate.Length)
            {
                return;
            }

            control.Value = new DateTime(int.Parse(_strDate.Substring(0, 4))
                                    , int.Parse(_strDate.Substring(4, 2))
                                    , int.Parse(_strDate.Substring(6, 2))
                                    , 0
                                    , 0
                                    , 0);
        }

        static public void datetimeSetTimeWithHMS(ref DateTimePicker control, string _strTime)
        {
            _strTime.Trim();
            _strTime.Replace(":", "");
            if (6 != _strTime.Length)
            {
                return;
            }

            control.Value = new DateTime(DateTime.Now.Year
                                    , DateTime.Now.Month
                                    , DateTime.Now.Day
                                    , int.Parse(_strTime.Substring(0, 2))
                                    , int.Parse(_strTime.Substring(2, 2))
                                    , int.Parse(_strTime.Substring(4, 2)));
        }

        static public String datetimeGetDayOfWeekString(DateTime _datetime)
        {
            string day_of_week = "";
            switch (_datetime.DayOfWeek.ToString().ToLower())
            {
                case "sunday":
                    day_of_week = "일요일";
                    break;
                case "monday":
                    day_of_week = "월요일";
                    break;
                case "tuesday":
                    day_of_week = "화요일";
                    break;
                case "wednesday":
                    day_of_week = "수요일";
                    break;
                case "thursday":
                    day_of_week = "목요일";
                    break;
                case "friday":
                    day_of_week = "금요일";
                    break;
                case "saturday":
                    day_of_week = "토요일";
                    break;
            }
            return day_of_week;
        }

        // ---------------------------------------------------------- YMD, HMS
        //
        static public string getHmsTimeFormat(string _hms)
        {
            if (null == _hms)
            {
                return "";
            }

            if (6 != _hms.Length)
            {
                return _hms;
            }

            return _hms.Substring(0, 2) + ":" + _hms.Substring(2, 2) + "." + _hms.Substring(4, 2);
        }

        static public string getYmdDateFormat(string _ymd)
        {
            if (null == _ymd)
            {
                return "";
            }

            if (8 != _ymd.Length)
            {
                return _ymd;
            }

            return _ymd.Substring(0, 4) + "-" + _ymd.Substring(4, 2) + "-" + _ymd.Substring(4, 2);
        }
    }
}
