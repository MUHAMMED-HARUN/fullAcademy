<template>
  <section class="space-y-4" dir="rtl" aria-label="إنشاء حساب للمستخدم">
    <header>
      <h2 class="text-xl font-bold text-[var(--primary-color)]">
        إنشاء حساب للمستخدم
      </h2>
      <p class="mt-1 text-sm text-[var(--text-muted)]">
        الحساب مرتبط بالشخص المحدد، والدور يحدد صلاحياته في النظام.
      </p>
    </header>

    <form class="space-y-4" novalidate @submit.prevent="submit">
      <fieldset
        class="m-0 grid grid-cols-1 gap-4 border-0 p-0 sm:grid-cols-2"
        :disabled="props.saving"
      >
        <div class="form-group">
          <label for="auth-user-name">اسم المستخدم:</label>
          <input
            id="auth-user-name"
            v-model="formData.userName"
            class="form-control"
            autocomplete="username"
            :aria-invalid="!!props.errors?.userName"
          />
          <ValidationError :messages="props.errors?.userName" />
        </div>

        <div class="form-group">
          <label for="auth-user-email">البريد الإلكتروني:</label>
          <input
            id="auth-user-email"
            v-model="formData.email"
            class="form-control"
            type="email"
            autocomplete="email"
            :aria-invalid="!!props.errors?.email"
          />
          <ValidationError :messages="props.errors?.email" />
        </div>

        <div class="form-group">
          <label for="auth-user-password">كلمة المرور:</label>
          <input
            id="auth-user-password"
            v-model="formData.password"
            class="form-control"
            type="password"
            autocomplete="new-password"
            :aria-invalid="!!props.errors?.password"
          />
          <ValidationError :messages="props.errors?.password" />
        </div>

        <div class="form-group">
          <label for="auth-user-confirm-password">تأكيد كلمة المرور:</label>
          <input
            id="auth-user-confirm-password"
            v-model="formData.confirmPassword"
            class="form-control"
            type="password"
            autocomplete="new-password"
            :aria-invalid="!!props.errors?.confirmPassword"
          />
          <ValidationError :messages="props.errors?.confirmPassword" />
        </div>

        <div class="form-group sm:col-span-2">
          <label for="auth-user-role">الدور:</label>
          <select
            id="auth-user-role"
            v-model="formData.roleID"
            class="form-select"
            :aria-invalid="!!props.errors?.roleID"
          >
            <option value="" disabled>اختر دوراً</option>
            <option v-for="role in props.roles" :key="role.id" :value="role.id">
              {{ role.name }}
            </option>
          </select>
          <ValidationError :messages="props.errors?.roleID" />
        </div>
      </fieldset>

      <p v-if="generalError" class="alert-error" role="alert">
        {{ generalError }}
      </p>

      <footer
        class="flex justify-end border-t border-[var(--border-color)] pt-4"
      >
        <button
          type="submit"
          class="btn btn-primary"
          :disabled="props.saving"
          :aria-busy="props.saving"
        >
          {{ props.saving ? "جارٍ إنشاء الحساب..." : "إنشاء الحساب" }}
        </button>
      </footer>
    </form>
  </section>
</template>

<script setup>
import { computed, reactive, watch } from "vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  errors: { type: [Object, String, Array], default: () => ({}) },
  roles: { type: Array, default: () => [] },
  entityId: { type: String, default: "" },
  userType: { type: Number, default: 1 },
  saving: { type: Boolean, default: false },
});

const emit = defineEmits(["onSubmit"]);
const formData = reactive({
  userName: "",
  email: "",
  password: "",
  confirmPassword: "",
  roleID: "",
  userType: props.userType,
  entityID: props.entityId,
});

const flattenMessages = (value) => {
  if (typeof value === "string") return [value];
  if (Array.isArray(value)) return value.flatMap(flattenMessages);
  if (value && typeof value === "object") {
    return Object.values(value).flatMap(flattenMessages);
  }
  return [];
};

const generalError = computed(() => {
  const errors = props.errors;
  if (typeof errors === "string" || Array.isArray(errors))
    return flattenMessages(errors).join("، ");
  if (!errors || typeof errors !== "object") return "";
  return flattenMessages(errors.general || errors.errors).join("، ");
});

watch(
  () => [props.entityId, props.userType],
  ([entityID, userType]) => {
    formData.entityID = entityID || "";
    formData.userType = userType;
  },
  { immediate: true }
);

function submit() {
  if (props.saving) return;
  emit("onSubmit", { ...formData });
}
</script>
