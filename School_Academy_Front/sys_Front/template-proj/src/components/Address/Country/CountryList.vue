<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>الدولة</th>
          <th>رمز الدولة</th>
          <th>الاجرائات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="country in countries" :key="country.ID">
          <td>{{ country.Name }}</td>
          <td>{{ country.Code }}</td>
          <td>
            <button @click="ShowUpdateCountry(country)">تعديل</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "CountryList",
  props: {
    countries: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateCountry"],
  methods: {
    async ShowUpdateCountry(countryData) {
      this.$emit("ShowUpdateCountry", countryData);
    },
  },
};
</script>
