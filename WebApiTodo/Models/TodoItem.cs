using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class TodoItem
    {
        /// <summary>
        /// Id long
        /// </summary>
        /// <example>123</example>
        public long Id { get; set; }
        /// <summary>
        /// Name string
        /// </summary>
        /// <example>Men's basketball shoes</example>
        public string Name { get; set; }
        /// <summary>
        /// IsComplete bool
        /// </summary>
        /// <example>Men's basketball shoes</example>
        public bool IsComplete { get; set; }
        
    }
}
