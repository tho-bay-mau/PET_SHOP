using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.ViewModel;

namespace ThoBayMau_ASM.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl (HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
