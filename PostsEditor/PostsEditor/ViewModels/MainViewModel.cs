using Microsoft.Win32;
using Mvvm;
using PostsEditor.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostsEditor.ViewModels
{
    public class MainViewModel : ViewModel
    {
        List<Post> _posts;
        int CurrentPostRealIndex
        {
            get { return CurrentPostIndex - 1; }
        }

        public RelayCommand OpenDirectoryCommand { get; }
        public RelayCommand RemakeCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand NextCommand { get; }
        public RelayCommand PrevCommand { get; }

        public string Directory
        {
            get { return GetValue<string>(); }
            private set
            {
                SetValue(value);
            }
        }

        [Dependencies(nameof(CurrentPost))]
        public int CurrentPostIndex
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int PostsCount
        {
            get
            {
                if (_posts == null)
                    return 0;
                return _posts.Count;
            }
        }

        public PostViewModel CurrentPost
        {
            get
            {
                if (PostsCount == 0)
                    return null;
                return new PostViewModel(_posts[CurrentPostRealIndex]);
            }
        }

        public MainViewModel()
        {
            OpenDirectoryCommand = new RelayCommand(OpenDirectory);
            RemoveCommand = new RelayCommand(Remove, CanRemove);
            RemakeCommand = new RelayCommand(Remake, CanRemake);
            NextCommand = new RelayCommand(SelectNext, CanSelectNext);
            PrevCommand = new RelayCommand(SelectPrev, CanSelectPrev);
        }

        async void OpenDirectory(object parameter)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = Directory;
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Directory = dialog.SelectedPath;
                    await OnDirectoryChanged();
                } 
            }
        }

        async Task OnDirectoryChanged()
        {
            CurrentPostIndex = 0;
            _posts = await PostsManager.GetPostsAsync(Directory);
            OnPropertyChanged(nameof(PostsCount));
            if (_posts.Count > 0)
                CurrentPostIndex = 1;
                
            OnPropertyChanged(nameof(CurrentPost));
        }

        void Remake(object parameter)
        {
            CurrentPost.Remake();
            RemovePost();
        }

        bool CanRemake(object parameter)
        {
            return CurrentPost != null;
        }

        void Remove(object parameter)
        {
            CurrentPost.Remove();
            RemovePost();
        }

        bool CanRemove(object parameter)
        {
            return CurrentPost != null;
        }

        void RemovePost()
        {
            _posts.RemoveAt(CurrentPostRealIndex);
            if (CurrentPostIndex == _posts.Count)
                CurrentPostIndex--;
            OnPropertyChanged(nameof(PostsCount));
        }

        void SelectNext(object parameter)
        {
            CurrentPostIndex++;
        }

        bool CanSelectNext(object parameter)
        {
            return CurrentPostIndex < PostsCount;
        }

        void SelectPrev(object parameter)
        {
            CurrentPostIndex--;
        }

        bool CanSelectPrev(object parameter)
        {
            return CurrentPostRealIndex > 0;
        }
    }
}
