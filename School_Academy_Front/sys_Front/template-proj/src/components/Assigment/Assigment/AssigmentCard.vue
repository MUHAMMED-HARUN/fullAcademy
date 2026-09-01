<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم الطالب:</div>
      <div class="item-value">{{ this.DataReq?.studentName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">نطاق الامتحان:</div>
      <div class="item-value">{{ this.DataReq?.assigmentScopeName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">حالة الامتحان:</div>
      <div class="item-value">{{ this.DataReq?.assignmentState }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">تاريخ تنفيذ الامتحان:</div>
      <div class="item-value">{{ this.DataReq?.scheduledDate }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">نتيحة الامتحان:</div>
      <div class="item-value">{{ this.DataReq?.isPassed }}</div>
    </div>
  </div>
</template>
<script>
export default {
  name: "AssigmentCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
