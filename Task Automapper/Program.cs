using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using TaskAutomapper.data;
using TaskAutomapper.Mapping;
using AutoMapper.QueryableExtensions;

namespace TaskAutomapper
{
    class Program
    {
        static void Main()
        {
            AutoMapperConfig.Config();

            if (File.Exists("MyDateBase.db"))
            {
                File.Delete("MyDateBase.db");
            }


            using (var db = new SqliteContext("MyDateBase.db"))
            {

                var position = new Position()
                {
                    Description = "Best Programmer"
                };

                db.Positions.Add(position);
                db.SaveChanges();

               var user = new User()
               {
                   Name = "Kos",
                   Age = 23,
                   PositionId = position.Id
               };
                db.Users.Add(user);
                db.SaveChanges();
            }

            using (var db = new SqliteContext("MyDateBase.db"))
            {
                var userUpdates = db.Users.ProjectTo<UserUpdate>().ToList();

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine(string.Join(Environment.NewLine, userUpdates));
            }

            Console.ReadKey();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }
    }

    public class Position
    {
        public Position()
        {
            Users = new HashSet<User>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
    public class UserUpdate : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public void CreateMappings(IMapperConfiguration configuration) => configuration.CreateMap<User, UserUpdate>()
            .ForMember(m => m.Description, opt => opt.MapFrom(u => u.Position.Description));

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Age: {Age}, Description: {Description}";
        }
    }
}
