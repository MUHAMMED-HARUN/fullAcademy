import api from "@/api/axios";
import TResult from "./TResult";
export default class clsUtil {
  static async FeachRequset(path, apiType, payload) {
    try {
      let response;
      if (enApiType.get == apiType) {
        if (payload) response = await api.get(path, payload);
        else {
          response = await api.get(path);
        }
      } else if (enApiType.post == apiType) {
        if (payload) response = await api.post(path, payload);
        else response = await api.post(path);
      } else if (enApiType.put == apiType) {
        if (payload) response = await api.put(path, payload);
        else response = await api.put(path);
      } else if (enApiType.delete == apiType) {
        if (payload) response = await api.delete(path, { data: payload });
        else response = await api.delete(path);
      }
      const { Value, IsSuccess, errors } = response.data;
      return new TResult(Value, IsSuccess, errors ? errors : {});
    } catch (err) {
      // فشل الطلب (Status 4xx, 5xx)
      console.log("feachr error details:", err);

      // 1. التحقق من وجود استجابة من السيرفر
      if (err.response && err.response.data) {
        // 2. استخراج الأخطاء بناءً على هيكل الـ Backend
        // الـ Backend يرسل: { errors: ... }
        const serverData = err.response.data;

        // حاول الوصول لـ errors أولاً (كما في كودك)، ثم Error كبديل
        const serverErrors =
          serverData.errors || serverData.Error || serverData;

        // إرجاع النتيجة مع الأخطاء
        return new TResult(null, false, serverErrors);
      }

      // 3. خطأ في الشبكة أو السيرفر غير متاح
      return new TResult(null, false, {
        errors: "تعذر الاتصال بالخادم أو خطأ في الشبكة",
      });
    }
  }
}
const enApiType = Object.freeze({ post: 1, get: 2, delete: 3, put: 4 });
export { enApiType };
