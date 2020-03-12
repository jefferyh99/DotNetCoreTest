using EasyCaching.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniqueOrderNumberGenerator.Service.Base;

namespace UniqueOrderNumberGenerator.Service
{
    public class ShipmentOrderUniqueNumberGenerator : AbstractNumberGenerator, IUniqueNumberGenerator
    {
        //private readonly IShipmentOrderRepository _shipmentOrderRepository;
        private const int CONCURRENCY_GAP = 100;

        public ShipmentOrderUniqueNumberGenerator(
            //IShipmentOrderRepository shipmentOrderRepository,
            IRedisCachingProvider redisCachingProvider) : base(redisCachingProvider)
        {
            //_shipmentOrderRepository = shipmentOrderRepository;
        }

        /// <summary>
        /// 过期时间(1天+1小时)
        /// </summary>
        protected override TimeSpan? Expiry => TimeSpan.FromHours(24);

        /// <summary>
        /// Value格式化
        /// </summary>
        protected override string ValueFormat => $"PM{DateTime.Now.ToString("yyyyMMdd")}" + "{0}";

        public string GenerateorName => "ShipmentOrder";

        protected override string GetKey()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var key = $"carrierservice_shipmentorder_uniqueNumber_{date}";
            return key;
        }

        protected async override Task<long> GetMaxNumberAsync()
        {
            //查询数据库中，某段时间内的最大值，这个时间要与失效时间相匹配，这个时间应该比失效时间长一点，防止重复
            //var begin = DateTime.Now.Date;
            //var end = begin.AddDays(1);
            //var specification = new MatchShipmentOrderByUpdateOnSpecification<ShipmentOrder>(begin, end);
            //var count = await _shipmentOrderRepository.CountAsync(specification);

            var databaseCount = 100;
            return databaseCount + CONCURRENCY_GAP;
        }

        public async Task<string> NewNumberAsync()
        {
            return await base.GetNumberAsync(7);
        }
    }
}
