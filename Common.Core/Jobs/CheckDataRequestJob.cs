using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Jobs
{
    public class CheckDataRequestJob
    {
        public void Execute(string urlCheck)
        {
            Console.WriteLine($"Haciendo Peticion para checkeo de datos API externa... {urlCheck}");
        }
    }
}
