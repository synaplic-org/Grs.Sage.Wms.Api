using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public class EmplacementResponse
    {
        public int ErpID { get; set; }
        public int WareHouseId { get; set; }
        public string ErpCode { get; set; }
        public string Name { get; set; }
    }
}
