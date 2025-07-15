using Mail.API.Services;
using MassTransit;
using Shared.Events;

namespace Mail.API.Consumer
{
    public class OrderCompletedEventConsumer : IConsumer<OrderCompletedEvent>
    {
        private readonly IMailService _mailService;
        public OrderCompletedEventConsumer(IMailService mailService)
        {
            _mailService = mailService;
        }
        public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
        {
            try
            {
                var orderEvent = context.Message;

                // E-posta konusunu belirleyelim
                var subject = "Sipariş Tamamlandı";
                var body = $"Merhaba,\n\nSiparişiniz başarıyla tamamlanmıştır.\n\nSipariş ID: {orderEvent.OrderId}\nAlıcı: {orderEvent.BuyerEmail}\nMesaj: {orderEvent.Message}";

                // Alıcıya e-posta gönder
                await _mailService.SendEmailAsync(orderEvent.BuyerEmail, subject, body);
            }
            catch (Exception ex)
            {
                // Hata işleme mekanizmalarını burada kullanabilirsiniz
                // Örneğin, hata mesajını loglayabilir veya bildirim gönderebilirsiniz
                throw new Exception("E-posta gönderme sırasında hata oluştu", ex);
            }
        }
    }
}
