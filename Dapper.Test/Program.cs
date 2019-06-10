using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dapper.Test
{
    class Program
    {
        //Dapper的扩展

        /// <summary>Main class for Dapper.SimpleCRUD extensions</summary>
        /// <summary>Main class for Dapper.SimpleCRUD extensions</summary>

        static void Main(string[] args)
        {
            //Install - Package Microsoft.Extensions.Configuration
            //Install - Package Microsoft.Extensions.Configuration.Json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory().Replace(@"\bin\Debug\netcoreapp2.2", ""))
                .AddJsonFile("appsettings.json", false, true);

            var config = builder.Build();


            var connectionString = config["Localdb:ConnectionString"];
            //test_insert(connectionString);
            //test_mult_insert(connectionString);

            //test_select_one(connectionString);
            //test_select_list(connectionString);
            //test_insert_content_with_comment(connectionString);

            //test_select_content_with_comment(connectionString);
            test_select_content_with_comment_join(connectionString);

            Console.ReadKey();
        }


        /// <summary>
        /// 测试插入单条数据
        /// </summary>
        static void test_insert(string connectionString)
        {
            var content = new Content
            {
                title = "标题1",
                content = "内容1",

            };
            using (var conn = new SqlConnection(connectionString))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
                VALUES   (@title,@content,@status,@add_time,@modify_time);select @@IDENTITY";

                //插入成功数据的数量
                var result = conn.Execute(sql_insert, content);

                //获取刚插入的自增Id
                //var result1 = conn.ExecuteScalar(sql_insert, content);//select @@IDENTITY

                Console.WriteLine($"test_insert：插入了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试插入多条数据
        /// </summary>
        static void test_mult_insert(string connectionString)
        {
            List<Content> contents = new List<Content>() {
                new Content
                {
                    title = "批量插入标题1",
                    content = "批量插入内容1",

                },
                new Content
                {
                    title = "批量插入标题2",
                    content = "批量插入内容2",

                },
            };

            using (var conn = new SqlConnection(connectionString))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_insert：插入了{result}条数据！");
            }
        }

        /// <summary>
        /// 查询单条指定的数据
        /// </summary>
        static void test_select_one(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql_insert = @"select * from [dbo].[content] where id=@id";
                var result = conn.QueryFirstOrDefault<Content>(sql_insert, new { id = 1 });
                Console.WriteLine($"test_select_one：查到的数据为：{JsonConvert.SerializeObject(result)}");
            }
        }

        /// <summary>
        /// 查询多条指定的数据
        /// </summary>
        static void test_select_list(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql_insert = @"select * from [dbo].[content] where id in @ids";
                var result = conn.Query<Content>(sql_insert, new { ids = new int[] { 1, 2 } });
                Console.WriteLine($"test_select_one：查到的数据为：{ JsonConvert.SerializeObject(result)}");
            }
        }

        static void test_insert_content_with_comment(string connectionString)
        {
            var contentWithCommnet = new ContentWithComment
            {
                title = "标题11",
                content = "内容11",
                comments = new List<Comment>()
                {
                    new Comment()
                    {
                        content = "CommentContent"
                    }
                }

            };

            //使用transation與con
            using (var conn = new SqlConnection(connectionString))
            {
                string sql_insert = @"INSERT INTO[Content]
                    (title, [content], status, add_time, modify_time)
                VALUES(@title, @content, @status, @add_time, @modify_time);select @@IDENTITY;";

                string sql_insert2 = @"INSERT INTO[Comment]
                    (content_id, [content])
                VALUES(@content_id, @content);";

                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var contentId = Convert.ToInt32(conn.ExecuteScalar(sql_insert, contentWithCommnet, tran));

                        foreach (var comment in contentWithCommnet.comments)
                        {
                            var currentComment = comment;
                            currentComment.content_id = contentId;
                            conn.Execute(sql_insert2, currentComment, tran);
                        }

                        tran.Commit();
                        Console.WriteLine("成功");

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("失败：" + e.ToString());
                        tran.Rollback();
                    }
                }
            }
        }


        static void test_select_content_with_comment(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql_insert = @"select * from content where id=@id;
select * from comment where content_id=@id;";
                using (var result = conn.QueryMultiple(sql_insert, new { id = 5 }))
                {
                    var content = result.ReadFirstOrDefault<ContentWithComment>();
                    content.comments = result.Read<Comment>();
                    Console.WriteLine($"test_select_content_with_comment:内容5的评论数量{content.comments.Count()}");
                }

            }
        }


        static void test_select_content_with_comment_join(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql_insert = @"select con.*,com.*
from content con inner join comment com on con.id = com.content_id
where con.id = @id";

                var result = conn.Query<ContentWithComment, Comment, ContentWithComment>(sql_insert,
                    (contentWithComment, comment) =>
                    {
                        if (contentWithComment.comments == null)
                        {
                            contentWithComment.comments = new List<Comment>();
                        }

                        contentWithComment.comments.AsList().Add(comment);
                        return contentWithComment;
                    },new{id= 23 });

                result = result;

            }
        }


    }

    #region Class
    public class Content
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 状态 1正常 0删除
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime add_time { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modify_time { get; set; }
    }
    public class Comment
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public int content_id { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; } = DateTime.Now;
    }

    public class ContentWithComment
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 状态 1正常 0删除
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime add_time { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modify_time { get; set; }
        /// <summary>
        /// 文章评论
        /// </summary>
        public IEnumerable<Comment> comments { get; set; }
    }
    #endregion



}
