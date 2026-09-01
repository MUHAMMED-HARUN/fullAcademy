<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الطالب</th>
          <th>اسم الشعبة</th>
          <th>من</th>
          <th>الى</th>
          <th>التاريخ</th>
          <th>مستوى التقييم</th>
          <th>الاجرائات</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="dailyEvaluation in DailyEvaluations"
          :key="dailyEvaluation.dailyEvaluationID"
        >
          <td>{{ dailyEvaluation.studentName }}</td>
          <td>{{ dailyEvaluation.groupName }}</td>
          <td>{{ dailyEvaluation.from }}</td>
          <td>{{ dailyEvaluation.to }}</td>
          <td>{{ dailyEvaluation.date }}</td>
          <td>{{ dailyEvaluation.levelType }}</td>
          <td>
            <button @click="ShowInfoDailyEvaluation(dailyEvaluation)">
              معلومات
            </button>
            <button @click="ShowUpdateDailyEvaluation(dailyEvaluation)">
              تحديث
            </button>
            <button @click="ShowDeleteDailyEvaluation(dailyEvaluation)">
              حذف
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "DailyEvaluationList",
  props: {
    DailyEvaluations: {
      type: Array,
      required: true,
    },
  },
  emits: [
    "ShowUpdateDailyEvaluation",
    "ShowDeleteDailyEvaluation",
    "ShowInfoDailyEvaluation",
  ],
  methods: {
    async ShowUpdateDailyEvaluation(Data) {
      this.$emit("ShowUpdateDailyEvaluation", Data);
    },

    async ShowDeleteDailyEvaluation(Data) {
      this.$emit("ShowDeleteDailyEvaluation", Data);
    },
    async ShowInfoDailyEvaluation(Data) {
      this.$emit("ShowInfoDailyEvaluation", Data);
    },
  },
};
</script>
