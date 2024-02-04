using System;
using JourneyJot.Data;
using JourneyJot.Models;

namespace JourneyJot
{
    public class Seed
    {
        private readonly DataContext dataContext;

        public Seed(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.Users.Any())
            {

                var users = new List<User>()
                {
                    new User()
                    {
                        Username = "johndoe",
                        Password = "JohnPass123",
                        Email = "johndoe@email.com",
                        Posts = new List<Post>()
                        {
                            new Post()
                            {
                                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
                                Content = "Placerat duis ultricies lacus sed turpis tincidunt id aliquet risus.",
                                Comments = new List<Comment>()
                                {
                                    new Comment()
                                    { 
                                        Author = new User()
                                        {
                                            Username = "janedoe",
                                            Password = "JanePass456",
                                            Email = "janedoe@email.com",
                                        },
                                        Content = "Eu non diam phasellus vestibulum. Blandit turpis cursus in hac habitasse platea dictumst quisque."
                                    },
                                    new Comment()
                                    {
                                        Author = new User()
                                        {
                                            Username = "aliceSmith",
                                            Password = "AlicePass789",
                                            Email = "alice@email.com",
                                        },
                                        Content = "Lacinia at quis risus sed vulputate odio ut. Pulvinar neque laoreet suspendisse interdum."
                                    }
                                },
                                PostCategories = new List<PostCategory>()
                                {
                                    new PostCategory() 
                                    {
                                        Category = new Category()
                                        {
                                            Name = "Tech",
                                            Description = "Urna condimentum mattis pellentesque id. Id velit ut tortor pretium viverra suspendisse potenti nullam."
                                        },
                                    }
                                },
                                PostTags = new List<PostTag>()
                                {
                                    new PostTag() 
                                    {
                                        Tag = new Tag() { Name = "Artificial intelligence"}
                                    }
                                }
                            }
                        }
                    },
                    new User()
                    { 
                        Username = "bobJohnson",
                        Password = "BobPass2022",
                        Email = "bob@email.com",
                        Posts = new List<Post>()
                        {
                            new Post()
                            {
                                Title = "Nisl condimentum id venenatis a",
                                Content = "Sagittis nisl rhoncus mattis rhoncus. Maecenas accumsan lacus vel facilisis volutpat.",
                                Comments = new List<Comment>()
                                {
                                    new Comment()
                                    {
                                        Author = new User()
                                        {
                                            Username = "emilyDavis",
                                            Password = "EmilyPass123",
                                            Email = "emily@email.com",
                                        },
                                        Content = "Tortor condimentum lacinia quis vel eros donec."
                                    }
                                },
                                PostCategories = new List<PostCategory>()
                                {
                                    new PostCategory()
                                    {
                                        Category = new Category() 
                                        { 
                                            Name = "Sports",
                                            Description = "Pretium fusce id velit ut tortor pretium viverra. Blandit turpis cursus in hac habitasse platea dictumst quisque sagittis." 
                                        },
                                    }
                                },
                                PostTags = new List<PostTag>()
                                {
                                    new PostTag()
                                    {
                                        Tag = new Tag() { Name = "Swimming"}
                                    }
                                }
                            }
                        }
                    }
                };
                dataContext.Users.AddRange(users);
                dataContext.SaveChanges();
            }
        }
    }
}

