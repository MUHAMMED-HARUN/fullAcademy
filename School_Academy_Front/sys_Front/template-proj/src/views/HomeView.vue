<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png" />
    <UpsertStudentComp
      @search-student="onSubmit"
      @updatemodel-value="fillStudent"
      :model-value="model"
      :person-data="this.personDTO"
      :errors="this.peronStore.errors"
    />
  </div>
</template>

<script>
// @ is an alias to /src
import { useAuthStore } from "@/store/Auth/authStore";
import UpsertStudentComp from "@/components/Person/Student/UpsertStudentComp.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";
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
        id: "",
        personID: "",
      },
      personDTO: null,
      peronStore: usePersonStore(),
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
    fillStudent(field) {
      this.model = field;
    },
    async onSubmit(type, val) {
      var personData = await this.peronStore.fetchPersonBy({
        Param: val,
        ParamType: type,
      });
      console.log("person data 43452", personData);
      if (personData.success) {
        this.personDTO = this.peronStore.selectedPerson;
        console.log("data ", this.personDTO);
      }
    },
    getPermission(e) {
      console.log("dsfghfjhgjh");
      console.log(e);
    },
  },
};
</script>
