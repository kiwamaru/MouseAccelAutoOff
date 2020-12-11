using System.Windows.Media.Imaging;

namespace MouseAccelAutoOffMonitor.Models
{
    public class BitmapConverter
    {
        public static BitmapSource ConvBitmapSource(System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null) return null;

            // MemoryStreamを利用した変換処理
            using (var ms = new System.IO.MemoryStream())
            {
                // MemoryStreamに書き出す
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                // MemoryStreamをシーク
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                // MemoryStreamからBitmapFrameを作成
                // (BitmapFrameはBitmapSourceを継承しているのでそのまま渡せばOK)
                BitmapSource bitmapSource =
                    BitmapFrame.Create(
                        ms,
                        BitmapCreateOptions.None,
                        BitmapCacheOption.OnLoad
                    );

                return bitmapSource;
            }
        }
    }
}