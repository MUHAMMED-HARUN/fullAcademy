<template>
  <div>
    <div>{{ this.FormTitle }}</div>
    <form @submit.prevent.stop="onSubmit">
      <div class="form-group">
        <label>اسم شارع:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>

      <div class="form-group">
        <label>الجارة</label>
        <select v-model="formData.districtID">
          <option value="" disabled>اختر حارة</option>
          <option
            v-for="distric in this.districts"
            :key="distric.id"
            :value="distric.id"
          >
            {{ distric.name }}
          </option>
        </select>
      </div>

      <div class="form-group">
        <button>حفظ</button>
      </div>
    </form>
  </div>
</template>
<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useNeighborhoodStore } from "@/store/Address/useNeighborhoodStore";
export default {
  name: "UpsertNeighborhoodComp",
  props: {
    errors: {
      type: Object,
      required: false,
    },
    districts: {
      type: Array,
      required: true,
    },
  },

  data() {
    return {
      NeighborhoodStore: useNeighborhoodStore(),
      formData: {
        id: "",
        name: "",
        districtID: "",
      },
    };
  },
  emits: ["onSubmit"],
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    fillForm(data) {
      this.formData = {
        id: data.id || "",
        name: data.name || "",
        districtID: data.districtID || "",
      };
    },
  },
  components: {
    ValidationError,
  },
  computed: {
    FormTitle() {
      return this.formData.id ? "تعديل مدينة" : "إضافة مدينة";
    },
  },
  watch: {
    "NeighborhoodStore.selectedNeighborhood": {
      handler(data) {
        if (data) this.fillForm(data);
      },
      immediate: true,
    },
  },
};
</script>
