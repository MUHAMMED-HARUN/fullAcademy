<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>الشارع</th>
          <th>الحارة</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="neighborhood in neighborhoods" :key="neighborhood.id">
          <td>{{ neighborhood.name }}</td>
          <td>{{ neighborhood.districtName }}</td>
          <td>
            <button @click="ShowUpdateNeighborhood(neighborhood)">تعديل</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "NeighborhoodList",
  props: {
    neighborhoods: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateNeighborhood"],
  methods: {
    async ShowUpdateNeighborhood(Data) {
      this.$emit("ShowUpdateNeighborhood", Data);
    },
  },
};
</script>
