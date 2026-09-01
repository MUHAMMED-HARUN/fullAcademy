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
        <tr v-for="person in Persons" :key="person.id">
          <td>{{ person.firstName }}</td>
          <td>{{ person.fatherName }}</td>
          <td>{{ person.lastName }}</td>
          <td>{{ person.motherName }}</td>
          <td>{{ person.motherLastName }}</td>
          <td>{{ person.nationalNumber }}</td>
          <td>{{ person.gender }}</td>
          <td>{{ person.birthDate }}</td>
          <td>{{ person.placeOfBirth }}</td>
          <td>{{ person.livingPlace }}</td>
          <td>
            <button @click="ShowInfoPerson(person)">معلومات</button>
            <button @click="ShowUpdatePerson(person)">تحديث</button>
            <button @click="ShowDeletePerson(person)">حذف</button>
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
    Persons: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdatePerson", "ShowDeletePerson", "ShowInfoPerson"],
  methods: {
    async ShowUpdatePerson(personData) {
      this.$emit("ShowUpdatePerson", personData);
    },

    async ShowDeletePerson(personData) {
      this.$emit("ShowDeletePerson", personData);
    },
    async ShowInfoPerson(personData) {
      this.$emit("ShowInfoPerson", personData);
    },
  },
};
</script>
