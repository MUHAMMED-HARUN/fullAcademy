<template>
  <div class="auth-container">
    <h2>إنشاء حساب جديد</h2>

    <form @submit.prevent="handleRegister">
      <div class="form-group">
        <label>اسم المستخدم:</label>
        <input v-model="form.userName" type="text" />
        <span v-if="errors.userName" class="error">{{ errors.userName }}</span>
      </div>

      <div class="form-group">
        <label>البريد الإلكتروني:</label>
        <input v-model="form.email" type="email" />
        <span v-if="errors.email" class="error">{{ errors.email }}</span>
      </div>

      <div class="form-group">
        <label>كلمة المرور:</label>
        <input v-model="form.password" type="password" />
        <span v-if="errors.password" class="error">{{ errors.password }}</span>
      </div>

      <div class="form-group">
        <label>تأكيد كلمة المرور:</label>
        <input v-model="form.confirmPassword" type="password" />
        <span v-if="errors.confirmPassword" class="error">{{
          errors.confirmPassword
        }}</span>
      </div>

      <div class="form-group">
        <label>الأدور</label>
        <select v-on:change="setUserType" v-model="form.roleID">
          <option disabled>اختر دورا</option>
          <option v-for="role in Roles" :value="role.ID" :key="role.ID">
            {{ role.Name }}
          </option>
        </select>
        <span v-if="errors.roleID" class="error">{{
          errors.confirmPassword
        }}</span>
      </div>

      <div class="form-filter">
        <label>نوع البحث</label>
        <label>البحث عن طريق رقم الهوية:</label>
        <input v-model="searchData.searchValue" type="text" />
        <button @click="searchEntity">بحث</button>
      </div>

      <div class="person-card">
        <PersonCard
          v-if="searchData.showCard && form.userType == 1"
          :-data-req="PersonStore.selectedPerson"
        />
        <StudentCard
          v-if="searchData.showCard && form.userType == 2"
          :-data-req="StudentStore.selectedStudent"
        />
        <TeacherCard
          v-if="searchData.showCard && form.userType == 0"
          :-data-req="TeacherCard.selectedTeacher"
        />
      </div>
      <div v-if="errors.general" class="alert-error">
        {{ errors.general }}
      </div>

      <button type="submit" :disabled="isLoading">
        {{ isLoading ? "جاري التسجيل..." : "إنشاء الحساب" }}
      </button>
    </form>
  </div>
</template>

<script setup>
import { onMounted, reactive, ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/store/Auth/authStore";
import { usePersonStore } from "@/store/Person/usePersonStore";
import { useTeacherStore } from "@/store/Person/useTeacherStore";
import { useStudentStore } from "@/store/Person/useStudentStore";
import StudentCard from "@/components/Person/Student/StudentCard.vue";
import PersonCard from "@/components/Person/Person/PersonCard.vue";
import TeacherCard from "@/components/Person/Teacher/TeacherCard.vue";

const router = useRouter();
const authStore = useAuthStore();
const PersonStore = usePersonStore();
const TeacherStore = useTeacherStore();
const StudentStore = useStudentStore();

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

const errors = ref({});
const Roles = ref([]);
const isLoading = ref(false);
const handleRegister = async () => {
  errors.value = {};
  isLoading.value = true;

  const result = await authStore.register(form);
  isLoading.value = false;

  if (result.success) {
    router.push("/dashboard");
  } else {
    errors.value = result.errors;
  }
};
onMounted(async () => {
  await fetchRole();
});
const fetchRole = async () => {
  const result = await authStore.getRoles();
  if (result.success) Roles.value = result.data;
  console.log(Roles);
};
const setUserType = async () => {
  const role = Roles.value.find((r) => r.ID === form.roleID);
  if (role == undefined) {
    return;
  }
  console.log("role", role);
  if (role.Name === "Admin") form.userType = 1;
  else if (role.Name === "Teacher") form.userType = 0;
  else form.userType = 2;
};
const searchEntity = async () => {
  const role = Roles.value.find((r) => r.ID === form.roleID);
  if (role == undefined) {
    return;
  }
  if (form.userType == 1) {
    // user
    form.userType = 1;
    var res = await PersonStore.fetchPersonBy({
      Param: searchData.searchValue,
      ParamType: 2,
    });
    if (res.success) {
      form.entityID = PersonStore.selectedPerson.id;
      searchData.showCard = true;
      return;
    }
  } else if (form.userType == 0) {
    //teacher
    var rees = await TeacherStore.fetchTeacherBy({
      Param: searchData.searchValue,
      ParamType: 3,
    });
    if (rees.success) {
      form.entityID = TeacherStore.selectedTeacher.TeacherID;
      searchData.showCard = true;
      return;
    }
  } else {
    var result = await StudentStore.fetchStudentBy({
      Param: searchData.searchValue,
      ParamType: 3,
    });
    if (result.success) {
      form.entityID = StudentStore.selectedStudent.studentID;
      searchData.showCard = true;
      return;
    }
  }
  searchData.showCard = false;
};
</script>
