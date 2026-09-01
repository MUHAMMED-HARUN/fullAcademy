<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم المعلم:</div>
      <div class="item-value">{{ this.DataReq?.teacherName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم المجموعة:</div>
      <div class="item-value">{{ this.DataReq?.groupName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم المادة:</div>
      <div class="item-value">{{ this.DataReq?.subjectName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">تاريخ التسجيل:</div>
      <div class="item-value">{{ this.DataReq?.date }}</div>
    </div>
  </div>
</template>

<script>
export default {
  name: "TeachingAssignmentCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
