using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calypso
{
    public delegate void ConfirmAction();

    internal class Util
    {
        public static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static DialogResult ShowInfoDialog(string message)
        {
            return MessageBox.Show(
                message,
                "Notice",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);
        }
        public static DialogResult ShowConfirmDialog(string message)
        {
            return MessageBox.Show(
                message,
                "Confirm",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);
        }



        public static string[] GetAllImageFilepaths(string path)
        {
           return System.IO.Directory.GetFiles(path, "*.*")
                               .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                           f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                           f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                           f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                           f.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                               .ToArray();
        }

        public static void Save<T>(T obj, string filePath)
        {
            string json = JsonSerializer.Serialize(obj);
            File.WriteAllText(filePath, json);
        }

        public static bool TryLoad<T>(string filePath, out T result)
        {
            result = default;
            try
            {
                string json = File.ReadAllText(filePath);
                result = JsonSerializer.Deserialize<T>(json);
                return result != null;
            }
            catch
            {
                return false;
            }
        }


        public static Image CreateThumbnail(string imagePath, int thumbnailHeight)
        {
            using Image fullImage = Image.FromFile(imagePath);
            int originalWidth = fullImage.Width;
            int originalHeight = fullImage.Height;

            int newHeight = thumbnailHeight;
            int newWidth = (int)(originalWidth * (newHeight / (float)originalHeight));

            return fullImage.GetThumbnailImage(newWidth, newHeight, () => false, IntPtr.Zero);
        }

        public static ImageFormat GetImageFormatFromExtension(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();

            return ext switch
            {
                ".jpg" or ".jpeg" => ImageFormat.Jpeg,
                ".png" => ImageFormat.Png,
                ".bmp" => ImageFormat.Bmp,
                ".gif" => ImageFormat.Gif,
                ".tiff" => ImageFormat.Tiff,
                _ => ImageFormat.Png // default fallback
            };
        }
    }
}
