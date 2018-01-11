using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostsEditor.Logic
{
    public class Post
    {
        string[] ImageExtensions = { "png", "jpg", "jpeg", "gif", "bmp" };

        string PostTitle { get; }

        string WorkPath { get; }

        public Post(string workPath, string postTitle)
        {
            WorkPath = workPath;
            PostTitle = postTitle;
        }

        public string GetText()
        {
            var path = GetTextFilePath();
            if (File.Exists(path))
                return File.ReadAllText(GetTextFilePath());
            return null;
        }

        public void SetText(string text)
        {
            var path = GetTextFilePath();
            if (!string.IsNullOrEmpty(text))
                File.WriteAllText(path, text);
            else if (File.Exists(path))
                File.Delete(path);
        }

        public Image GetImage()
        {
            return Image.FromFile(GetImageFilePath());
        }

        public void SetImage(Image image)
        {
            RemoveOldImage();
            image.Save(GetImageFilePath(image));
        }

        public void SetImage(string imagePath)
        {
            RemoveOldImage();
            var ext = Path.GetExtension(imagePath);
            File.Copy(imagePath, GetImageFilePath(ext), true);
        }

        public void Remove()
        {
            foreach (var file in GetAllFiles())
                File.Delete(file);
        }

        public void Remake()
        {
            foreach(var file in GetAllFiles())
            {
                File.Replace(file, Path.Combine(WorkPath, "Remake", Path.GetFileName(file)), null);
            }
        }

        //public void SetImage(Stream imageStream)
        //{
        //    var bm = new Bitmap(imageStream);
        //    bm.Save(GetImageFilePath(bm));
        //}

        string GetTextFilePath()
        {
            return Path.Combine(WorkPath, $"{PostTitle}.txt");
        }

        string GetImageFilePath(Image image)
        {
            var ext = image.RawFormat.ToString();
            return Path.Combine(WorkPath, $"{PostTitle}.{ext}");
        }

        string GetImageFilePath()
        {
            return GetExistingImages().FirstOrDefault();
        }

        string GetImageFilePath(string extension)
        {
            return Path.Combine(WorkPath, $"{PostTitle}.{extension}");
        }

        void RemoveOldImage()
        {
            foreach (var path in GetExistingImages())
                File.Delete(path);
        }

        IEnumerable<string> GetExistingImages()
        {
            foreach (var ext in ImageExtensions)
            {
                var path = GetImageFilePath(ext);
                if (File.Exists(path))
                    yield return path;
            }
        }

        IEnumerable<string> GetAllFiles()
        {
            return Directory.GetFiles(WorkPath, $"{PostTitle}.*");
        }
    }
}
