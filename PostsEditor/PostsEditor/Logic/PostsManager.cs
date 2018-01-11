using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostsEditor.Logic
{
    public static class PostsManager
    {
        public static List<Post> GetPosts(string workPath)
        {
            var titles = Directory.GetFiles(workPath).Select(fp => Path.GetFileNameWithoutExtension(fp)).Distinct();
            var posts = titles.Select(t => new Post(workPath, t));
            return posts.ToList();
        }

        public static async Task<List<Post>> GetPostsAsync(string workPath)
        {
            return await Task.Run(() => { return GetPosts(workPath); });
        }
    }
}
