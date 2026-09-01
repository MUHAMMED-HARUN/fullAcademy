<template>
  <router-link v-if="canVisable(-1)" to="/">Home</router-link> |
  <router-link v-if="canVisable(-1)" to="/register">register</router-link>
  <router-link v-if="canVisable(1)" to="/roles">Roles</router-link>
  <router-link v-if="canVisable(1)" to="/country">Country</router-link>
  <router-link v-if="canVisable(1)" to="/city">City</router-link>
  <router-link v-if="canVisable(1)" to="/district">District</router-link>
  <router-link v-if="canVisable(1)" to="/neighborhood"
    >Neighborhood</router-link
  >
  <router-link v-if="canVisable(1)" to="/persons">Person</router-link>
  <router-link v-if="canVisable(1)" to="/students">Student</router-link>
  <router-link v-if="canVisable(1)" to="/teachers">Teacher</router-link>
  <router-link v-if="canVisable(1)" to="/institute">Institute</router-link>
  <router-link v-if="canVisable(1)" to="/program">program</router-link>
  <router-link v-if="canVisable(1)" to="/class">class</router-link>
  <router-link v-if="canVisable(1)" to="/instituteClass"
    >instituteClass</router-link
  >
  <router-link v-if="canVisable(1)" to="/group">group</router-link>
  <router-link v-if="canVisable(-1)" to="/studentEnrollment"
    >studentEnrollment</router-link
  >
  <router-link v-if="canVisable(-1)" to="/teachingAssignment"
    >TeachingAssignmentStore</router-link
  >
  <router-link v-if="canVisable(1)" to="/session">session</router-link>
  <router-link v-if="canVisable(1)" to="/stage">Stage</router-link>
  <router-link v-if="canVisable(1)" to="/stageinfo">Stage info</router-link>
  <router-link v-if="canVisable(-1)" to="/dailyEvaluation"
    >Daily Evaluation</router-link
  >
  <router-link v-if="canVisable(-1)" to="/studentStage"
    >Student Stage</router-link
  >

  <router-link v-if="canVisable(1)" to="/assignmentScope"
    >Assignment Scope</router-link
  >
  <router-link v-if="canVisable(1)" to="/assignmentScopeInfo"
    >Assignment Scope info</router-link
  >
  <router-link v-if="canVisable(1)" to="/assignmentNomination"
    >Assignment Nomination</router-link
  >
  <router-link v-if="canVisable(1)" to="/assignment">Assignment</router-link>
  <router-link v-if="canVisable(1)" to="/result">Result</router-link>
</template>
<script>
import { useAuthStore } from "@/store/Auth/authStore";
export default {
  name: "linksComp",
  data() {
    return {
      authStore: useAuthStore(),
    };
  },

  methods: {
    canVisable(roleType) {
      // -1 means all role
      if (roleType == -1) return true;
      if (roleType == 1)
        //Admin
        return this.authStore.UserRole == "Admin";
      else if (roleType == 0)
        //Teacher
        return this.authStore.UserRole == "Teacher";
      else return false;
    },
  },
};
</script>
