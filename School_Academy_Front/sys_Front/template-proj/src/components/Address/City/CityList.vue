<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>المدينة</th>
          <th>رمز المدينة</th>
          <th>الدولة</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="city in cities" :key="city.cityID">
          <td>{{ city.name }}</td>
          <td>{{ city.code }}</td>
          <td>{{ city.countryName }}</td>
          <td>
            <button @click="ShowUpdateCity(city)">تعديل</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "CityList",
  props: {
    cities: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateCity"],
  methods: {
    async ShowUpdateCity(cityData) {
      this.$emit("ShowUpdateCity", cityData);
    },
  },
};
</script>
