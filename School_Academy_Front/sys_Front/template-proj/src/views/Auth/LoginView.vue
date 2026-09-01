<template>
  <div class="auth-container">
    <h2>تسجيل الدخول</h2>

    <form @submit.prevent="handleLogin">
      <!-- البريد الإلكتروني -->
      <div class="form-group">
        <label>البريد الإلكتروني:</label>
        <input
          v-model="form.email"
          type="email"
          placeholder="example@domain.com"
        />
        <ValidationError :messages="errors.email" />
      </div>

      <!-- كلمة المرور -->
      <div class="form-group">
        <label>كلمة المرور:</label>
        <input v-model="form.password" type="password" />
        <ValidationError :messages="errors.password" />
      </div>

      <!-- خطأ عام من السيرفر -->
      <div v-if="errors.general" class="alert-error">
        {{ errors.general }}
      </div>

      <button type="submit" :disabled="isLoading">
        {{ isLoading ? "جاري التحميل..." : "دخول" }}
      </button>
    </form>
  </div>
</template>

<script setup>
import { reactive, ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/store/Auth/authStore";
import ValidationError from "@/components/general/ValidationError.vue";

const router = useRouter();
const authStore = useAuthStore();

const form = reactive({
  userName: "",
  password: "",
});

const errors = ref({});
const isLoading = ref(false);

const handleLogin = async () => {
  errors.value = {};
  isLoading.value = true;

  const result = await authStore.login(form);
  isLoading.value = false;

  if (result.success) {
    router.push("/dashboard"); // التوجيه بعد النجاح
  } else {
    errors.value = result.errors; // ربط الأخطاء القادمة من TResult
  }
};
</script>

<style scoped>
.form-group {
  margin-bottom: 1rem;
}
.error {
  color: red;
  font-size: 0.85rem;
  display: block;
}
.alert-error {
  background: #ffe6e6;
  color: red;
  padding: 0.5rem;
  border-radius: 4px;
  margin-bottom: 1rem;
}
</style>
