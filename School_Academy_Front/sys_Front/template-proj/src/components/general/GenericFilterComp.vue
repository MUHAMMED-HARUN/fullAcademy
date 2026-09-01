<template>
  <div class="generic-filter-card">
    <div class="filter-grid">
      <div v-for="field in fields" :key="field.key" class="filter-item">
        <label>{{ field.label }}</label>

        <!-- Input Text / Number -->
        <input
          v-if="field.type === 'text' || field.type === 'number'"
          :type="field.type"
          v-model="filterValues[field.key]"
          :placeholder="field.placeholder || field.label"
        />

        <!-- Input Date -->
        <input
          v-else-if="field.type === 'date'"
          type="date"
          v-model="filterValues[field.key]"
        />

        <!-- Select Dropdown -->
        <select
          v-else-if="field.type === 'select'"
          v-model="filterValues[field.key]"
        >
          <option :value="null">الكل</option>
          <option
            v-for="opt in field.options"
            :key="opt.value"
            :value="opt.value"
          >
            {{ opt.label }}
          </option>
        </select>
      </div>
    </div>

    <div class="filter-actions">
      <!-- قسم الترتيب (OrderBy) -->
      <div class="order-by-container">
        <label>ترتيب حسب:</label>
        <select v-model="selectedOrderField">
          <option :value="null">افتراضي</option>
          <option v-for="field in fields" :key="field.key" :value="field.key">
            {{ field.label }}
          </option>
        </select>

        <select v-model="selectedOrderDir" :disabled="!selectedOrderField">
          <option value="asc">تصاعدي (ASC)</option>
          <option value="desc">تنازلي (DESC)</option>
        </select>
      </div>

      <!-- الأزرار -->
      <div class="btn-group">
        <button @click="handleSearch" class="btn-search">بحث</button>
        <button @click="handleReset" class="btn-reset">إعادة ضبط</button>
      </div>
    </div>
  </div>
</template>

<script>
import { reactive, ref, onMounted } from "vue";

export default {
  name: "GenericFilterComp",
  props: {
    // مصفوفة الحقول المشكلة للفلتر
    fields: {
      type: Array,
      required: true,
      // مثال للتركيب: [{ key: 'FirstName', label: 'الاسم', type: 'text', operator: '=*' }]
    },
  },
  emits: ["search", "reset"],
  setup(props, { emit }) {
    const filterValues = reactive({});
    const selectedOrderField = ref(null);
    const selectedOrderDir = ref("asc");

    // تهيئة القيم الأولية بناءً على الشاشة
    const initFilters = () => {
      props.fields.forEach((field) => {
        filterValues[field.key] = field.defaultValue ?? null;
      });
      selectedOrderField.value = null;
      selectedOrderDir.value = "asc";
    };

    onMounted(() => {
      initFilters();
    });

    // بناء صيغة Gridify Filter أو إرسال القيم كما هي
    const handleSearch = () => {
      const activeFilters = [];

      props.fields.forEach((field) => {
        const val = filterValues[field.key];
        if (val !== null && val !== "" && val !== undefined) {
          const op = field.operator || "=";
          // بناء شرط Gridify
          activeFilters.push(`${field.key}${op}${val}`);
        }
      });

      // نجمع الشروط بـ comma (AND)
      const filterQuery = activeFilters.join(",");

      // بناء نص OrderBy التابع لـ Gridify (مثال: "FirstName desc")
      let orderByQuery = null;
      if (selectedOrderField.value) {
        orderByQuery = `${selectedOrderField.value} ${selectedOrderDir.value}`;
      }

      emit("search", {
        filter: filterQuery || null,
        orderBy: orderByQuery,
        rawValues: { ...filterValues },
      });
    };

    const handleReset = () => {
      initFilters();
      emit("reset");
    };

    return {
      filterValues,
      selectedOrderField,
      selectedOrderDir,
      handleSearch,
      handleReset,
    };
  },
};
</script>

<style scoped>
.generic-filter-card {
  background: #ffffff;
  padding: 16px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  margin-bottom: 20px;
}
.filter-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 12px;
  margin-bottom: 12px;
}
.filter-item {
  display: flex;
  flex-direction: column;
  gap: 4px;
}
.filter-item label {
  font-size: 13px;
  font-weight: 600;
  color: #555;
}
.filter-item input,
.filter-item select {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.filter-actions {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  flex-wrap: wrap;
}
.order-by-container {
  display: flex;
  align-items: center;
  gap: 8px;
}
.order-by-container label {
  font-size: 13px;
  font-weight: 600;
  color: #555;
}
.order-by-container select {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.btn-group {
  display: flex;
  gap: 8px;
}
.btn-search {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}
.btn-reset {
  background-color: #6c757d;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}
</style>
