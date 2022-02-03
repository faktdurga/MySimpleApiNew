using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySimpleApi.Model
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsGluntenFree { get; set; }
    }
}
