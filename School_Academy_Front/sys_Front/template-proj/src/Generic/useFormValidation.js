/**
 * useFormValidation.js
 * ─────────────────────────────────────────────────────────────────────────────
 * composable موحد للتحقق من صحة الفورم — نمط المشروع القياسي.
 * حسب قواعد: 04-forms-and-validation.md
 *
 * السلوك:
 * - الخطأ يظهر بعد blur للحقل (touch) أو بعد محاولة Submit.
 * - بعد اللمس، كل تغيير يعيد التحقق فوراً.
 * - handleSubmit يمنع الإرسال إن وُجدت أخطاء، ويضبط isSubmitting.
 *
 * الاستخدام:
 *   const { values, errorOf, touch, reset, handleSubmit, isSubmitting } =
 *     useFormValidation(defaultValues, validateFn)
 *
 * ─────────────────────────────────────────────────────────────────────────────
 */
import { computed, reactive, ref } from "vue";

/**
 * @template T
 * @param {T} defaultValues - القيم الافتراضية للفورم
 * @param {(values: T) => Partial<Record<keyof T, string>>} validate - دالة التحقق
 */
export function useFormValidation(defaultValues, validate) {
  // ─── الحالة الداخلية ──────────────────────────────────────────────────────
  const values = reactive({ ...defaultValues });
  const touched = reactive({});
  const submitted = ref(false);
  const isSubmitting = ref(false);

  // ─── الأخطاء المحسوبة (تُعاد عند كل تغيير في values) ────────────────────
  const allErrors = computed(() => validate(values));

  /**
   * إرجاع رسالة الخطأ للحقل المطلوب — فقط إذا لُمس أو أُرسل الفورم.
   * @param {keyof T} field
   * @returns {string | undefined}
   */
  function errorOf(field) {
    return touched[field] || submitted.value
      ? allErrors.value[field]
      : undefined;
  }

  /**
   * تسجيل الحقل كـ "تم لمسه" (بعد blur).
   * @param {keyof T} field
   */
  function touch(field) {
    touched[field] = true;
  }

  /**
   * إعادة الفورم لحالته الافتراضية.
   */
  function reset() {
    Object.assign(values, defaultValues);
    Object.keys(touched).forEach((key) => delete touched[key]);
    submitted.value = false;
    isSubmitting.value = false;
  }

  /**
   * معالج الإرسال — يمنع الإرسال إن وجدت أخطاء.
   * @param {(data: T) => void | Promise<void>} onValid - دالة تُستدعى عند نجاح التحقق
   * @returns {(event?: Event) => Promise<void>}
   */
  function handleSubmit(onValid) {
    return async (event) => {
      event?.preventDefault?.();
      submitted.value = true;

      // توقف إذا وُجدت أخطاء
      if (Object.keys(allErrors.value).length > 0) return;

      isSubmitting.value = true;
      try {
        // نرسل نسخة من القيم (لا مرجع reactive)
        await onValid({ ...values });
      } finally {
        isSubmitting.value = false;
      }
    };
  }

  return {
    values,
    errorOf,
    touch,
    reset,
    handleSubmit,
    isSubmitting,
    // مفيد للتحقق الخارجي
    allErrors,
    submitted,
  };
}

// ─── دوال تحقق مساعدة (Helper Validators) ───────────────────────────────────

/**
 * التحقق من صيغة البريد الإلكتروني
 * @param {string} value
 * @returns {boolean}
 */
export const isEmail = (value) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);

/**
 * التحقق من أن الحقل غير فارغ
 * @param {string | null | undefined} value
 * @returns {boolean}
 */
export const isRequired = (value) =>
  value !== null && value !== undefined && String(value).trim().length > 0;

/**
 * التحقق من الحد الأدنى للطول
 * @param {string} value
 * @param {number} min
 * @returns {boolean}
 */
export const minLength = (value, min) => String(value).trim().length >= min;

/**
 * التحقق من أن القيمة رقم موجب
 * @param {string | number} value
 * @returns {boolean}
 */
export const isPositiveNumber = (value) => {
  const num = Number(value);
  return !isNaN(num) && num > 0;
};
