using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1
{
    public class Singleton
    {
        private static Singleton _singleton = null;
        private static object _lockobj = new object();

        [Column]
        public int Id { get; set; }
        
        [Required(ErrorMessage="请输入")]
        [StringLength(50,ErrorMessage= "")]
        public string Name { get; set; }

        static Singleton()
        {
            Console.WriteLine("static Singleton");
        }

        private Singleton()
        {            

        }

        public static Singleton CreateInstance()
        {
            if (_singleton == null)
            {
                lock (_lockobj)
                {
                    if (_singleton == null)
                    {
                        Console.WriteLine("CreateInstance");

                        _singleton = new Singleton();
                    }
                    else
                    {
                        Console.WriteLine("_singleton is not null");
                    }
                }
            }

            return _singleton;
        }

    }
}
