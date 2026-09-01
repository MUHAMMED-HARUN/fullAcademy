<template>
  <div>
    <PersonCard :DataReq="DataReq" />
  </div>
</template>
<script>
import PersonCard from "../Person/PersonCard.vue";
export default {
  name: "TeacherCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
  components: {
    PersonCard,
  },
};
</script>
