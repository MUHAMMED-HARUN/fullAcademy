<template>
  <div
    class="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center p-4"
  >
    <div class="w-full max-w-2xl">
      <div
        class="rounded-2xl border border-gray-200 bg-white p-6 sm:p-8 shadow-theme-lg dark:border-gray-800 dark:bg-[#1E2635]"
      >
        <div class="mb-6 text-center">
          <h1
            class="text-xl font-semibold text-gray-800 dark:text-white/90 sm:text-2xl"
          >
            إنشاء حساب جديد
          </h1>
        </div>

        <form class="space-y-5" novalidate @submit="handleRegister">
          <div class="grid grid-cols-1 gap-5 sm:grid-cols-2">
            <!-- اسم المستخدم -->
            <div class="space-y-1.5">
              <label
                class="block text-sm font-medium text-gray-700 dark:text-gray-300"
              >
                اسم المستخدم <span class="text-red-500">*</span>
              </label>
              <input
                v-model="form.userName"
                type="text"
                class="h-11 w-full rounded-[10px] border border-gray-300 bg-white px-4 text-sm text-gray-800 shadow-theme-xs outline-none transition-colors placeholder:text-gray-400 focus:border-brand-300 focus:ring-3 focus:ring-brand-500/20 dark:border-gray-700 dark:bg-gray-900 dark:text-white/90"
                :aria-invalid="errors.userName ? 'true' : undefined"
              />
              <p
                v-if="errors.userName"
                class="text-sm text-red-500 dark:text-red-400"
              >
                {{ errors.userName }}
              </p>
            </div>

            <!-- البريد الإلكتروني -->
            <div class="space-y-1.5">
              <label
                class="block text-sm font-medium text-gray-700 dark:text-gray-300"
              >
                البريد الإلكتروني <span class="text-red-500">*</span>
              </label>
              <input
                v-model="form.email"
                type="email"
                class="h-11 w-full rounded-[10px] border border-gray-300 bg-white px-4 text-sm text-gray-800 shadow-theme-xs outline-none transition-colors placeholder:text-gray-400 focus:border-brand-300 focus:ring-3 focus:ring-brand-500/20 dark:border-gray-700 dark:bg-gray-900 dark:text-white/90"
                :aria-invalid="errors.email ? 'true' : undefined"
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
              <input
                v-model="form.password"
                type="password"
                class="h-11 w-full rounded-[10px] border border-gray-300 bg-white px-4 text-sm text-gray-800 shadow-theme-xs outline-none transition-colors placeholder:text-gray-400 focus:border-brand-300 focus:ring-3 focus:ring-brand-500/20 dark:border-gray-700 dark:bg-gray-900 dark:text-white/90"
                :aria-invalid="errors.password ? 'true' : undefined"
              />
              <p
                v-if="errors.password"
                class="text-sm text-red-500 dark:text-red-400"
              >
                {{ errors.password }}
              </p>
            </div>

            <!-- تأكيد كلمة المرور -->
            <div class="space-y-1.5">
              <label
                class="block text-sm font-medium text-gray-700 dark:text-gray-300"
              >
                تأكيد كلمة المرور <span class="text-red-500">*</span>
              </label>
              <input
                v-model="form.confirmPassword"
                type="password"
                class="h-11 w-full rounded-[10px] border border-gray-300 bg-white px-4 text-sm text-gray-800 shadow-theme-xs outline-none transition-colors placeholder:text-gray-400 focus:border-brand-300 focus:ring-3 focus:ring-brand-500/20 dark:border-gray-700 dark:bg-gray-900 dark:text-white/90"
                :aria-invalid="errors.confirmPassword ? 'true' : undefined"
              />
              <p
                v-if="errors.confirmPassword"
                class="text-sm text-red-500 dark:text-red-400"
              >
                {{ errors.confirmPassword }}
              </p>
            </div>
          </div>

          <!-- الدور -->
          <div class="space-y-1.5">
            <label
              class="block text-sm font-medium text-gray-700 dark:text-gray-300"
            >
              الدور <span class="text-red-500">*</span>
            </label>
            <select
              v-model="form.roleID"
              class="h-11 w-full rounded-[10px] border border-gray-300 bg-white px-4 text-sm text-gray-800 shadow-theme-xs outline-none transition-colors focus:border-brand-300 focus:ring-3 focus:ring-brand-500/20 dark:border-gray-700 dark:bg-gray-900 dark:text-white/90"
              @change="setUserType"
            >
              <option disabled value="">اختر دورا</option>
              <option v-for="role in Roles" :value="role.id" :key="role.id">
                {{ role.name }}
              </option>
            </select>
            <p
              v-if="errors.roleID"
              class="text-sm text-red-500 dark:text-red-400"
            >
              {{ errors.roleID }}
            </p>
          </div>

          <!-- البحث عن كيان -->
          <div
            class="rounded-xl border border-gray-200 bg-gray-50 p-4 dark:border-gray-800 dark:bg-white/5"
          >
            <label
              class="mb-2 block text-sm font-medium text-gray-700 dark:text-gray-300"
            >
              البحث عن طريق رقم الهوية
            </label>
            <div class="flex gap-2">
              <input
                v-model="searchData.searchValue"
                type="text"
                class="h-11 flex-1 rounded-[10px] border border-gray-300 bg-white px-4 text-sm text-gray-800 shadow-theme-xs outline-none transition-colors focus:border-brand-300 focus:ring-3 focus:ring-brand-500/20 dark:border-gray-700 dark:bg-gray-900 dark:text-white/90"
              />
              <button
                type="button"
                class="h-11 rounded-[10px] bg-brand-500 px-4 text-sm font-medium text-white shadow-theme-xs transition-colors hover:bg-brand-600"
                :disabled="isSearching || !searchData.searchValue.trim()"
                @click="searchEntity"
              >
                {{ isSearching ? "جارٍ البحث..." : "بحث" }}
              </button>
            </div>
            <p
              v-if="errors.entityID"
              class="mt-2 text-sm text-red-500"
              role="alert"
            >
              {{ errors.entityID }}
            </p>
          </div>

          <!-- بطاقة الكيان -->
          <div
            v-if="searchData.showCard"
            class="rounded-xl border border-gray-200 bg-white p-4 dark:border-gray-800 dark:bg-white/3"
          >
            <PersonCard
              v-if="form.userType == 1"
              :data-req="PersonStore.selectedPerson"
            />
            <StudentCard
              v-if="form.userType == 2"
              :data-req="StudentStore.selectedStudent"
            />
            <TeacherCard
              v-if="form.userType == 0"
              :data-req="TeacherStore.selectedTeacher"
            />
          </div>

          <!-- خطأ عام -->
          <div
            v-if="errors.general"
            class="rounded-xl border border-red-500 bg-red-50 p-3 text-sm text-red-600 dark:border-red-500/30 dark:bg-red-500/10 dark:text-red-400"
          >
            {{ errors.general }}
          </div>

          <button
            type="submit"
            class="h-11 w-full rounded-[10px] bg-brand-500 text-sm font-medium text-white shadow-theme-xs transition-colors hover:bg-brand-600 disabled:cursor-not-allowed disabled:bg-brand-200 dark:disabled:bg-brand-800 dark:disabled:text-brand-400"
            :disabled="isLoading"
          >
            {{ isLoading ? "جاري التسجيل..." : "إنشاء الحساب" }}
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, reactive, ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/store/Auth/authStore";
import { usePersonStore } from "@/store/Person/usePersonStore";
import { useTeacherStore } from "@/store/Person/useTeacherStore";
import { useStudentStore } from "@/store/Person/useStudentStore";
import { useToast } from "@/Generic/useToast";
import StudentCard from "@/components/Person/Student/StudentCard.vue";
import PersonCard from "@/components/Person/Person/PersonCard.vue";
import TeacherCard from "@/components/Person/Teacher/TeacherCard.vue";

const router = useRouter();
const authStore = useAuthStore();
const PersonStore = usePersonStore();
const TeacherStore = useTeacherStore();
const StudentStore = useStudentStore();
const toast = useToast();

const searchData = reactive({
  searchValue: "",
  showCard: false,
});

const form = reactive({
  userName: "",
  email: "",
  password: "",
  confirmPassword: "",
  roleID: "",
  userType: 1,
  entityID: null,
});

const errors = reactive({});
const Roles = ref([]);
const isLoading = ref(false);
const isSearching = ref(false);

const handleRegister = async () => {
  Object.keys(errors).forEach((k) => delete errors[k]);
  isLoading.value = true;
  try {
    const result = await authStore.register(form);
    if (result.success) {
      toast.success("تم إنشاء الحساب بنجاح");
      router.push("/");
    } else {
      const errs = result.errors || {};
      if (typeof errs === "string") {
        errors.general = errs;
      } else {
        Object.assign(errors, errs);
      }
      toast.error("فشل إنشاء الحساب", errors.general || undefined);
    }
  } finally {
    isLoading.value = false;
  }
};

onMounted(async () => {
  await fetchRole();
});

const fetchRole = async () => {
  const result = await authStore.getRoles();
  if (result.success) Roles.value = result.data;
};

const setUserType = async () => {
  const role = Roles.value.find((item) => item.id === form.roleID);
  if (!role) return;
  if (role.name === "Admin") form.userType = 1;
  else if (role.name === "Teacher") form.userType = 0;
  else form.userType = 2;
  clearEntitySelection();
};

function clearEntitySelection() {
  form.entityID = null;
  searchData.showCard = false;
  PersonStore.selectedPerson = null;
  StudentStore.selectedStudent = null;
  TeacherStore.selectedTeacher = null;
}

const searchEntity = async () => {
  Object.keys(errors).forEach((key) => {
    if (key === "entityID") delete errors[key];
  });
  clearEntitySelection();
  if (!form.roleID || !searchData.searchValue.trim()) {
    errors.entityID = "اختر الدور وأدخل رقم الهوية للبحث";
    return;
  }

  isSearching.value = true;
  try {
    let result;
    if (form.userType === 1) {
      result = await PersonStore.fetchPersonBy({
        Param: searchData.searchValue.trim(),
        ParamType: 2,
      });
      if (result.success)
        form.entityID = PersonStore.selectedPerson?.id || null;
    } else if (form.userType === 0) {
      result = await TeacherStore.fetchTeacherBy({
        Param: searchData.searchValue.trim(),
        ParamType: 3,
      });
      if (result.success)
        form.entityID = TeacherStore.selectedTeacher?.teacherID || null;
    } else {
      result = await StudentStore.fetchStudentBy({
        Param: searchData.searchValue.trim(),
        ParamType: 3,
      });
      if (result.success)
        form.entityID = StudentStore.selectedStudent?.studentID || null;
    }

    if (result?.success && form.entityID) {
      searchData.showCard = true;
    } else {
      form.entityID = null;
      errors.entityID = "لم يتم العثور على كيان نشط مطابق";
    }
  } finally {
    isSearching.value = false;
  }
};
</script>
