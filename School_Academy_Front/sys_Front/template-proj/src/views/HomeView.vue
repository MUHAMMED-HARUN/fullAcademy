<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png" />
    <UpsertStudentComp
      v-model="model"
      :person-data="peronStore.selectedPerson"
      :person-search-results="peronStore.personSearchResults"
      :person-search-loading="peronStore.personSearchLoading"
      :person-search-error="peronStore.personSearchError"
      :errors="studentStore.error"
      :saving="studentStore.saving"
      @search-person="onSearchPerson"
      @select-person="onSelectPerson"
      @clear-person="onClearPerson"
      @person-query-change="onPersonQueryChange"
      @submit="onSubmitStudent"
    />
  </div>
</template>

<script>
// @ is an alias to /src
import { useAuthStore } from "@/store/Auth/authStore";
import UpsertStudentComp from "@/components/Person/Student/UpsertStudentComp.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";
import { useStudentStore } from "@/store/Person/useStudentStore";
export default {
  name: "HomeView",
  components: {
    UpsertStudentComp,
  },
  data() {
    return {
      Permissions: [],
      auth: useAuthStore(),
      ResPonseErrors: null,
      HasError: false,
      model: {
        studentID: "",
        personID: "",
      },
      peronStore: usePersonStore(),
      studentStore: useStudentStore(),
    };
  },
  async mounted() {
    if (this.auth.isAuthenticated) {
      const result = await this.auth.getPermissions();
      if (result.success) {
        this.ResPonseErrors = null;
        this.Permissions = result.data;
      } else {
        this.ResPonseErrors = result.errors.general;
      }
    }
  },
  methods: {
    async onSearchPerson(criteria) {
      await this.peronStore.searchPersonCandidates(criteria);
    },
    onSelectPerson(person) {
      this.peronStore.selectPersonCandidate(person);
    },
    onClearPerson() {
      this.peronStore.clearSelectedPersonCandidate();
    },
    onPersonQueryChange() {
      this.peronStore.resetPersonCandidateSearch();
    },
    async onSubmitStudent(student) {
      const result = student.studentID
        ? await this.studentStore.updateStudent(student)
        : await this.studentStore.addStudent(student);
      if (result.success) {
        this.model = { studentID: "", personID: "" };
        this.peronStore.resetPersonCandidateSearch();
      }
    },
  },
};
</script>
