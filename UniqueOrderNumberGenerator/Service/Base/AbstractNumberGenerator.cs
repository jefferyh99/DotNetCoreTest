using EasyCaching.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniqueOrderNumberGenerator.Service.Base
{
    /// <summary>
    /// 序号生成
    /// </summary>
    public abstract class AbstractNumberGenerator
    {
        private readonly IRedisCachingProvider _redisCachingProvider;

        protected AbstractNumberGenerator(IRedisCachingProvider provider)
        {
            _redisCachingProvider = provider;
        }

        /// <summary>
        /// Key过期时间
        /// </summary>
        protected abstract TimeSpan? Expiry { get; }

        /// <summary>
        /// key
        /// </summary>
        protected abstract string GetKey();

        /// <summary>
        /// Value格式化
        /// </summary>
        protected abstract string ValueFormat { get; }

        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="tenantCode"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public async Task<string> GetNumberAsync(int length)
        {
            var key = GetKey();

            if (!_redisCachingProvider.KeyExists(key))
            {
                var max = await GetMaxNumberAsync();
                _redisCachingProvider.StringSet(key, max.ToString(), Expiry);
            }

            var seq = await _redisCachingProvider.IncrByAsync(GetKey());

            return string.Format(ValueFormat, seq.ToString().PadLeft(length, '0'));
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        protected abstract Task<long> GetMaxNumberAsync();
    }
}
