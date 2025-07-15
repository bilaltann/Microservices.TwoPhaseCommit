using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages
{

    // stock kontrolü ile alakalı bilgiler eklenir , mesela fiyatla işimiz yok

    public class OrderItemMessage
    {
        public string ProductId { get; set; }
        public int Count { get; set; }

    }
}
