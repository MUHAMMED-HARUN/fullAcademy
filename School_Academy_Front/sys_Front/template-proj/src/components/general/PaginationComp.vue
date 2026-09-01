<template>
  <div class="pagination-container" v-if="totalCount > 0">
    <!-- اختيار عدد العناصر بالصفحة -->
    <div class="page-size">
      <span>عرض:</span>
      <select
        :value="pageSize"
        @change="$emit('change-size', Number($event.target.value))"
      >
        <option :value="5">5</option>
        <option :value="10">10</option>
        <option :value="20">20</option>
        <option :value="50">50</option>
      </select>
      <span class="total-text">إجمالي السجلات: {{ totalCount }}</span>
    </div>

    <!-- أزرار الصفحات -->
    <div class="page-controls">
      <button
        :disabled="currentPage === 1"
        @click="$emit('change-page', currentPage - 1)"
      >
        السابق
      </button>

      <span class="page-info">
        صفحة <strong>{{ currentPage }}</strong> من
        <strong>{{ totalPages }}</strong>
      </span>

      <button
        :disabled="currentPage >= totalPages"
        @click="$emit('change-page', currentPage + 1)"
      >
        التالي
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: "PaginationComp",
  props: {
    currentPage: { type: Number, required: true },
    pageSize: { type: Number, required: true },
    totalCount: { type: Number, required: true },
    totalPages: { type: Number, required: true },
  },
  emits: ["change-page", "change-size"],
};
</script>

<style scoped>
.pagination-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px;
  background-color: #ffffff;
  border: 1px solid #e0e0e0;
  border-radius: 6px;
  margin-top: 15px;
}
.page-size,
.page-controls {
  display: flex;
  align-items: center;
  gap: 8px;
}
.page-info {
  margin: 0 8px;
}
button {
  padding: 6px 12px;
  border: 1px solid #d0d0d0;
  background-color: #f9f9f9;
  border-radius: 4px;
  cursor: pointer;
}
button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
