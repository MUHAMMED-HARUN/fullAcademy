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
        <tr v-for="student in Students" :key="student.id">
          <td>{{ student.firstName }}</td>
          <td>{{ student.fatherName }}</td>
          <td>{{ student.lastName }}</td>
          <td>{{ student.motherName }}</td>
          <td>{{ student.motherLastName }}</td>
          <td>{{ student.nationalNumber }}</td>
          <td>{{ student.gender }}</td>
          <td>{{ student.birthDate }}</td>
          <td>{{ student.placeOfBirth }}</td>
          <td>{{ student.livingPlace }}</td>
          <td>
            <button @click="ShowInfoStudent(student)">معلومات</button>
            <button @click="ShowUpdateStudent(student)">تحديث</button>
            <button @click="ShowDeleteStudent(student)">حذف</button>
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
    Students: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateStudent", "ShowDeleteStudent", "ShowInfoStudent"],
  methods: {
    async ShowUpdateStudent(Data) {
      this.$emit("ShowUpdateStudent", Data);
    },

    async ShowDeleteStudent(Data) {
      this.$emit("ShowDeleteStudent", Data);
    },
    async ShowInfoStudent(Data) {
      this.$emit("ShowInfoStudent", Data);
    },
  },
};
</script>
