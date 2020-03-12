using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniqueOrderNumberGenerator.Service
{
    /// <summary>
    /// 为了方便测试，这个接口暂时放在这层，最好放在Service层
    /// </summary>
    public interface IUniqueNumberGenerator
    {
        /// <summary>
        /// 这边应该是数组注入的，通过名字来区分注入情况
        /// </summary>
        string GenerateorName { get; }

        Task<string> NewNumberAsync();
    }
}
