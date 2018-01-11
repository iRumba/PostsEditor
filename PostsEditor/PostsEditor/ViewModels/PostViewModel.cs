using Mvvm;
using PostsEditor.Helpers;
using PostsEditor.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PostsEditor.ViewModels
{
    public class PostViewModel : ViewModel
    {
        Post _post;

        public PostViewModel(Post model)
        {
            _post = model;
            Image = GetImage();
            Text = GetText();
        }

        public string Text
        {
            get
            {
                return GetText();
                //return GetValue<string>();
            }
            set
            {
                SetText(value);
                //OnPropertyChanged(nameof(Text));
                //SetValue(value);
            }
        }

        public ImageSource Image
        {
            get
            {
                return GetValue<ImageSource>();
            }

            private set
            {
                SetValue(value);
            }
        }

        string GetText()
        {
            return _post.GetText();
        }

        void SetText(string text)
        {
            _post.SetText(text);
        }

        ImageSource GetImage()
        {
            return _post.GetImage().ToImageSource();
        }

        public void Remove()
        {
            _post.Remove();
        }

        public void Remake()
        {
            _post.Remake();
        }
    }
}
