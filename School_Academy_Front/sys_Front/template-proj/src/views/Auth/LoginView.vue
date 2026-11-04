<template>
  <div
    class="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center p-4"
  >
    <div
      class="w-full max-w-4xl overflow-hidden rounded-2xl border border-gray-200 bg-white shadow-theme-lg dark:border-gray-800 dark:bg-[#1E2635] lg:grid lg:grid-cols-2"
    >
      <!-- Brand panel -->
      <div
        class="hidden bg-brand-950 p-8 text-white lg:flex lg:flex-col lg:justify-between dark:bg-white/5"
      >
        <div>
          <div
            class="flex h-10 w-10 items-center justify-center rounded-xl bg-white/10 text-lg font-bold"
          >
            S
          </div>
          <h2 class="mt-6 text-2xl font-semibold">School Academy</h2>
          <p class="mt-2 text-sm text-white/70">
            نظام إدارة أكاديمية مدرسية متكامل
          </p>
        </div>
        <p class="text-xs text-white/50">
          © {{ new Date().getFullYear() }} School Academy
        </p>
      </div>
      <div class="p-6 sm:p-8">
        <div class="mb-6 text-center">
          <h1
            class="text-xl font-semibold text-gray-800 dark:text-white/90 sm:text-2xl"
          >
            تسجيل الدخول
          </h1>
          <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">
            أدخل بياناتك للوصول إلى النظام
          </p>
        </div>

        <form class="space-y-5" novalidate @submit="onSubmit">
          <!-- البريد الإلكتروني / اسم المستخدم -->
          <div class="space-y-1.5">
            <label
              class="block text-sm font-medium text-gray-700 dark:text-gray-300"
            >
              البريد الإلكتروني <span class="text-red-500">*</span>
            </label>
            <input
              v-model="form.email"
              type="email"
              placeholder="example@domain.com"
              class="h-11 w-full rounded-[10px] border border-gray-300 bg-white px-4 text-sm text-gray-800 shadow-theme-xs outline-none transition-colors placeholder:text-gray-400 focus:border-brand-300 focus:ring-3 focus:ring-brand-500/20 dark:border-gray-700 dark:bg-gray-900 dark:text-white/90 dark:placeholder:text-gray-500"
              :aria-invalid="errors.email ? 'true' : undefined"
              @blur="touched.email = true"
            />
            <p
              v-if="errors.email"
              class="text-sm text-red-500 dark:text-red-400"
            >
              {{ errors.email }}
            </p>
          </div>

          <!-- كلمة المرور -->
          <div class="space-y-1.5">
            <label
              class="block text-sm font-medium text-gray-700 dark:text-gray-300"
            >
              كلمة المرور <span class="text-red-500">*</span>
            </label>
            <div class="relative">
              <input
                v-model="form.password"
                :type="showPassword ? 'text' : 'password'"
                placeholder="••••••••"
                class="h-11 w-full rounded-[10px] border border-gray-300 bg-white ps-4 pe-10 text-sm text-gray-800 shadow-theme-xs outline-none transition-colors placeholder:text-gray-400 focus:border-brand-300 focus:ring-3 focus:ring-brand-500/20 dark:border-gray-700 dark:bg-gray-900 dark:text-white/90 dark:placeholder:text-gray-500"
                :aria-invalid="errors.password ? 'true' : undefined"
                @blur="touched.password = true"
              />
              <button
                type="button"
                class="absolute end-2 top-1/2 -translate-y-1/2 rounded-full p-1 text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-white"
                @click="showPassword = !showPassword"
              >
                {{ showPassword ? "إخفاء" : "إظهار" }}
              </button>
            </div>
            <p
              v-if="errors.password"
              class="text-sm text-red-500 dark:text-red-400"
            >
              {{ errors.password }}
            </p>
          </div>

          <!-- خطأ عام من السيرفر -->
          <div
            v-if="generalError"
            class="rounded-xl border border-red-500 bg-red-50 p-3 text-sm text-red-600 dark:border-red-500/30 dark:bg-red-500/10 dark:text-red-400"
          >
            {{ generalError }}
          </div>

          <button
            type="submit"
            class="h-11 w-full rounded-[10px] bg-brand-500 text-sm font-medium text-white shadow-theme-xs transition-colors hover:bg-brand-600 disabled:cursor-not-allowed disabled:bg-brand-200 dark:disabled:bg-brand-800 dark:disabled:text-brand-400"
            :disabled="isLoading"
          >
            {{ isLoading ? "جاري التحميل..." : "دخول" }}
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/store/Auth/authStore";
import { useToast } from "@/Generic/useToast";

const router = useRouter();
const authStore = useAuthStore();
const toast = useToast();

const form = reactive({
  email: "",
  password: "",
});

const errors = reactive({});
const touched = reactive({});
const generalError = ref(null);
const isLoading = ref(false);
const showPassword = ref(false);

function validate() {
  const e = {};
  if (!form.email.trim()) e.email = "البريد الإلكتروني مطلوب";
  else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email.trim()))
    e.email = "صيغة البريد غير صحيحة";
  if (!form.password) e.password = "كلمة المرور مطلوبة";
  else if (form.password.length < 6)
    e.password = "كلمة المرور يجب أن لا تقل عن 6 أحرف";
  return e;
}

async function onSubmit(event) {
  event?.preventDefault?.();
  Object.keys(errors).forEach((k) => delete errors[k]);
  generalError.value = null;
  const v = validate();
  Object.assign(errors, v);
  if (Object.keys(v).length > 0) return;

  isLoading.value = true;
  try {
    const result = await authStore.login({
      email: form.email,
      password: form.password,
    });

    if (result.success) {
      toast.success("تم تسجيل الدخول بنجاح");
      router.push(router.currentRoute.value.query.redirect || "/");
    } else {
      const errs = result.errors || {};
      generalError.value =
        errs.general || errs.errors || (typeof errs === "string" ? errs : null);
      if (!generalError.value) {
        Object.assign(errors, errs);
      }
      toast.error("فشل تسجيل الدخول", generalError.value || undefined);
    }
  } finally {
    isLoading.value = false;
  }
}
</script>
