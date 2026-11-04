/**
 * useToast.js
 * ─────────────────────────────────────────────────────────────────────────────
 * Toast notifications composable — نمط المشروع القياسي.
 * حسب قواعد: 05-notifications-toast.md
 *
 * ❌ لا توجد مكتبة خارجية — composable محلي خفيف.
 * ✅ المتغير `toasts` مشترك (module-level) = كل استدعاء useToast() يرى نفس القائمة.
 *
 * الاستخدام في المكوّن:
 *   const toast = useToast()
 *   toast.success('Saved successfully', 'Your changes have been applied.')
 *   toast.error('Something went wrong', error.message)
 *
 * حاوية العرض (في AdminLayout أو App.vue):
 *   const { toasts } = useToast()
 *   // ثم v-for="t in toasts"
 * ─────────────────────────────────────────────────────────────────────────────
 */
import { ref } from "vue";

// ─── Variants ─────────────────────────────────────────────────────────────────
/**
 * @typedef {'success' | 'destructive' | 'warning' | 'info' | 'default'} ToastVariant
 */

/**
 * @typedef {Object} ToastItem
 * @property {number}        id
 * @property {ToastVariant}  variant
 * @property {string}        title
 * @property {string}        [description]
 */

// ─── الحالة المشتركة (module-level singleton) ─────────────────────────────────
/** @type {import('vue').Ref<ToastItem[]>} */
const toasts = ref([]);
let seq = 0;

// ─── الدوال الأساسية ───────────────────────────────────────────────────────────

/**
 * إضافة Toast جديد.
 * @param {Omit<ToastItem, 'id'>} item
 * @param {number} [ttl=4000] - مدة الظهور بالمللي ثانية
 */
function push(item, ttl = 4000) {
  const id = ++seq;
  toasts.value = [...toasts.value, { id, ...item }];

  // إزالة تلقائية بعد انتهاء المدة
  window.setTimeout(() => {
    toasts.value = toasts.value.filter((t) => t.id !== id);
  }, ttl);
}

/**
 * إزالة Toast بالـ id.
 * @param {number} id
 */
function dismiss(id) {
  toasts.value = toasts.value.filter((t) => t.id !== id);
}

/**
 * مسح كل الـ Toasts مرة واحدة.
 */
function dismissAll() {
  toasts.value = [];
}

// ─── Composable الرئيسي ────────────────────────────────────────────────────────

/**
 * @returns {{
 *   toasts: import('vue').Ref<ToastItem[]>,
 *   success: (title: string, description?: string) => void,
 *   error: (title: string, description?: string) => void,
 *   warning: (title: string, description?: string) => void,
 *   info: (title: string, description?: string) => void,
 *   dismiss: (id: number) => void,
 *   dismissAll: () => void,
 * }}
 */
export function useToast() {
  /**
   * ✅ نجاح
   * @param {string} title
   * @param {string} [description]
   */
  const success = (title, description) =>
    push({ variant: "success", title, description });

  /**
   * ❌ خطأ
   * @param {string} title
   * @param {string} [description]
   */
  const error = (title, description) =>
    push({ variant: "destructive", title, description });

  /**
   * ⚠️ تحذير
   * @param {string} title
   * @param {string} [description]
   */
  const warning = (title, description) =>
    push({ variant: "warning", title, description });

  /**
   * ℹ️ معلومات
   * @param {string} title
   * @param {string} [description]
   */
  const info = (title, description) =>
    push({ variant: "info", title, description });

  return {
    toasts,
    success,
    error,
    warning,
    info,
    dismiss,
    dismissAll,
  };
}
