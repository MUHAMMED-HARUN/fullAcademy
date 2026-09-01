

using System.Runtime.CompilerServices;

namespace System.Application.Common
{
    public static class clsUtil
    {
        public static Guid ToGuid( string GuidString)
        {
            if (string.IsNullOrWhiteSpace(GuidString))
                return Guid.Empty;
            Guid.TryParse(GuidString, out Guid result);
            if (result != null || Guid.Empty!=result)
            {
                return result;
            }
            return Guid.Empty;
        }
        public static string ToGuidStringPreserveCase(string guidString)
        {
            if (Guid.TryParse(guidString, out _)) // نتجاهل النتيجة المحولة ونتأكد من الصحة فقط
            {
                return guidString; // إرجاع النص كما هو (يحافظ على الكبير والصغير)
            }

            return string.Empty; // أو Guid.Empty.ToString() إذا كنت تريد نص الـ GUID الفارغ
        }
    }
}
