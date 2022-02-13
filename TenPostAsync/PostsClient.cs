using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TenPostAsync.Models.DTO;

namespace TenPostAsync
{
    internal class Write10Posts
    {
        HttpClient _client;
        JsonSerializerOptions _options;


        public Write10Posts()
        {
            _client = new HttpClient();
        }


        public void WritePostsToFile()
        {
            using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "\\result.txt"))
            {
                foreach (var post in Get10PostsAsync().Result)
                {
                    PostDTO postDTO = JsonSerializer.Deserialize<PostDTO>(post);
                    writer.WriteLine(postDTO.userId);
                    writer.WriteLine(postDTO.id);
                    writer.WriteLine(postDTO.title);
                    writer.WriteLine(postDTO.body + '\n');
                }
            }
        }

        private async Task<string[]> Get10PostsAsync()
        {
            var tasks = new List<Task<string>>();
            for (int i = 4; i < 14; i++)
            {
                tasks.Add(GetPostByID(i));
            }

            return await Task.WhenAll(tasks);
        }

        private Task<string> GetPostByID(int id)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Get,
                $@"https://jsonplaceholder.typicode.com/posts/{id}"
            );
            try
            {
                var response = _client.SendAsync(httpRequest).Result;
                return response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error: " + nameof(GetPostByID));
            }

            return null;
        }
    }
}
