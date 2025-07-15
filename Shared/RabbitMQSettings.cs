using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    static public class RabbitMQSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-queue";
        public const string Payment_StockReservedEventQueue = "payment-stock-reserved-event-queue";
        public const string Order_PaymentCompletedEventQueue = "order-payment-completed-event-queue";
        public const string Order_StockNotEventQueue = "order-stock-not-event-queue";
        public const string Order_PaymentFailedEventQueue = "order-payment-failed-event-queue";
        public const string Mail_OrderCompletedEventQueue = "mail-order-completed-event-queue";


    }
}


//  static olması Yani bu sınıftan bir örnek (instance) oluşturulmaz
// public: Genel erişim düzeyi sağlar. Diğer projeler veya sınıflar bu sınıfa erişebilir.
// const: Sabit (constant) anlamına gelir. Program çalıştığı sürece bu değişkenin değeri değiştirilemez.


// kısaca Uygulamanın her yerinde RabbitMQSettings.Stock_OrderCreatedEventQueue şeklinde ulaşabileceğin, değiştirilemez bir kuyruk adı tutar.

