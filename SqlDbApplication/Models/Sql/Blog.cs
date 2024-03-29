﻿//using System.Collections.Generic;

//namespace SqlDbApplication.Models.Sql
//{
//    public class Blog
//    {
//        public int Id { get; set; } // Primary key
//        public string Name { get; set; }

//        public IList<Post> Posts { get; } = new List<Post>(); // Collection navigation
//        public BlogAssets Assets { get; set; } // Reference navigation
//    }

//    public class BlogAssets
//    {
//        public int Id { get; set; } // Primary key
//        public byte[] Banner { get; set; }

//        public int? BlogId { get; set; } // Foreign key
//        public Blog Blog { get; set; } // Reference navigation
//    }

//    public class Post
//    {
//        public int Id { get; set; } // Primary key
//        public string Title { get; set; }
//        public string Content { get; set; }

//        public int? BlogId { get; set; } // Foreign key
//        public Blog Blog { get; set; } // Reference navigation

//        public IList<Tag> Tags { get; } = new List<Tag>(); // Skip collection navigation
//    }

//    public class Tag
//    {
//        public int Id { get; set; } // Primary key
//        public string Text { get; set; }

//        public IList<Post> Posts { get; } = new List<Post>(); // Skip collection navigation
//    }
//}
