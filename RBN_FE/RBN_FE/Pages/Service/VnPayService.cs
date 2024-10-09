using BusinessObject.Dto.RequestDto;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RBN_FE.Pages.Service
{
    public class VnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GeneratePaymentUrl(double eventPrice, CreateBookingDto booking)
        {
            // Lấy thông tin cấu hình từ appsettings
            var vnp_TmnCode = _configuration["VNPAY:TmnCode"];
            var vnp_HashSecret = _configuration["VNPAY:HashSecret"];
            var vnp_Url = _configuration["VNPAY:BaseUrl"];
            var vnp_Returnurl = _configuration["VNPAY:PaymentBackReturnUrl"];

            // Kiểm tra eventPrice và booking không null
            if (eventPrice <= 0 || booking == null)
            {
                throw new ArgumentException("Giá sự kiện và thông tin đặt chỗ không hợp lệ.");
            }
            string transactionRef = Guid.NewGuid().ToString("N");

            // Tạo các tham số cho yêu cầu thanh toán
            var paymentParameters = new SortedDictionary<string, string>
    {
        { "vnp_Version", "2.1.0" },
        { "vnp_Command", "pay" },
        { "vnp_TmnCode", vnp_TmnCode },
        { "vnp_Amount", (eventPrice * 100).ToString("0") }, // Số tiền thanh toán (đơn vị là VND)
        { "vnp_OrderInfo", $"Thanh toán cho sự kiện: {booking.EventId}" },
        { "vnp_CurrCode", "VND" },
        { "vnp_Locale", "vn" }, // Ngôn ngữ hiển thị
        { "vnp_ReturnUrl", vnp_Returnurl }, // URL trả về sau khi thanh toán
        { "vnp_TxnRef", transactionRef }, // Mã tham chiếu của giao dịch
        { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }, // Thời gian tạo giao dịch
    };

            // Tạo chuỗi truy vấn và tính toán mã băm
            var queryString = string.Join("&", paymentParameters.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}"));
            var hashData = vnp_TmnCode + queryString;

            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(vnp_HashSecret)))
            {
                var checksum = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(hashData))).Replace("-", "").ToLower();
                return $"{vnp_Url}?{queryString}&vnp_SecureHash={checksum}";
            }
        }
    }

}
