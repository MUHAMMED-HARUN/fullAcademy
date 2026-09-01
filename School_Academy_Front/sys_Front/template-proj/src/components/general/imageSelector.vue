<template>
  <input @change="imageSelect" type="file" />
  <button
    class="show"
    @click.prevent="
      () => {
        showImage = !showImage;
      }
    "
  >
    show
  </button>
  <div class="studio" v-if="showImage">
    <img class="img" v-for="image in images" :src="image" :key="image" />
  </div>
</template>
<script>
export default {
  name: "imageSelector",
  data() {
    return { showImage: false };
  },
  props: {
    images: {
      type: Array,
      default: () => [],
    },
  },
  methods: {
    imageSelect: function (e) {
      const file = e.target.files[0];
      if (!file) return;
      this.$emit("addimage", URL.createObjectURL(file));
    },
  },
  emits: ["addimage"],
};
</script>
<style scoped lang="scss">
input[type="image"] {
  width: 100px;
  height: 100px;
  border: 1px solid #ccc;
  cursor: pointer;
}
.studio {
  .img {
    width: 100px;
  }
}
</style>
