<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم الطالب:</div>
      <div class="item-value">{{ this.DataReq?.studentName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم المجموعة:</div>
      <div class="item-value">{{ this.DataReq?.groupName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">تاريخ التسجيل:</div>
      <div class="item-value">{{ this.DataReq?.date }}</div>
    </div>
  </div>
</template>

<script>
export default {
  name: "StudentEnrollmentCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
