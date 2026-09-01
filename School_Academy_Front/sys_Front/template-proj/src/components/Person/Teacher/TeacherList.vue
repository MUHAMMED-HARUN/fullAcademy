<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>الاسم</th>
          <th>الاب</th>
          <th>الكنية</th>
          <th>الام</th>
          <th>كنية الام</th>
          <th>رقم الهوية</th>
          <th>الجنس</th>
          <th>العمر</th>
          <th>مكان الولادة</th>
          <th>مكان الاقامة</th>
          <th>الاجرائات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="teacher in Teachers" :key="teacher.id">
          <td>{{ teacher.firstName }}</td>
          <td>{{ teacher.fatherName }}</td>
          <td>{{ teacher.lastName }}</td>
          <td>{{ teacher.motherName }}</td>
          <td>{{ teacher.motherLastName }}</td>
          <td>{{ teacher.nationalNumber }}</td>
          <td>{{ teacher.gender }}</td>
          <td>{{ teacher.birthDate }}</td>
          <td>{{ teacher.placeOfBirth }}</td>
          <td>{{ teacher.livingPlace }}</td>
          <td>
            <button @click="ShowInfoTeacher(teacher)">معلومات</button>
            <button @click="ShowUpdateTeacher(teacher)">تحديث</button>
            <button @click="ShowDeleteTeacher(teacher)">حذف</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "PersonList",
  props: {
    Teachers: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateTeacher", "ShowDeleteTeacher", "ShowInfoTeacher"],
  methods: {
    async ShowUpdateTeacher(Data) {
      this.$emit("ShowUpdateTeacher", Data);
    },

    async ShowDeleteTeacher(Data) {
      this.$emit("ShowDeleteTeacher", Data);
    },
    async ShowInfoTeacher(Data) {
      this.$emit("ShowInfoTeacher", Data);
    },
  },
};
</script>
