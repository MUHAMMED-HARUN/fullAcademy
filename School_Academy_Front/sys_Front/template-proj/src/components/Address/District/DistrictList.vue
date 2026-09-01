<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>الحارة</th>
          <th>المدينة</th>
          <th>الدولة</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="district in districts" :key="district.id">
          <td>{{ district.name }}</td>
          <td>{{ district.cityName }}</td>
          <td>{{ district.countryName }}</td>
          <td>
            <button @click="ShowUpdateDistrict(district)">تعديل</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "DistrictList",
  props: {
    districts: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateDistrict"],
  methods: {
    async ShowUpdateDistrict(Data) {
      this.$emit("ShowUpdateDistrict", Data);
    },
  },
};
</script>
