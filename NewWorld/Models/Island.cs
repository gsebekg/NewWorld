using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWorld.Models
{
    public class Island
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Place { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Glinianka { get; set; }
        public int Zelazo { get; set; }
        public bool Ziemniaki { get; set; }
        public bool Zboze { get; set; }
        public bool Chmiel { get; set; }
        public bool Papryka { get; set; }

        public virtual UserGameProperty Property { get; set; }
        public virtual Resources Resources { get; set; }




        public static double Distance(int x1, int x2, int y1, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
        public static double Distance(Island islandA, Island islandB)
        {
            return Island.Distance(islandA.X, islandB.X, islandA.Y, islandB.Y);
        }
    }

}