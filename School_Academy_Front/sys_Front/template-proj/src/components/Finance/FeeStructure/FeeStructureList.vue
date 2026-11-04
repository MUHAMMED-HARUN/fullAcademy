<template>
  <div class="table-responsive">
    <table class="data-table">
      <thead>
        <tr>
          <th class="checkbox-col">
            <input
              type="checkbox"
              :checked="isAllSelected"
              @change="$emit('toggle-select-all', $event.target.checked)"
            />
          </th>
          <th>اسم الخطة</th>
          <th>نوع الرسوم</th>
          <th>المبلغ</th>
          <th>البرنامج</th>
          <th>الصف</th>
          <th>المرحلة</th>
          <th>ملاحظات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in items" :key="item.feeStructureID">
          <td class="checkbox-col">
            <input
              type="checkbox"
              :checked="selectedIds.includes(item.feeStructureID)"
              @change="$emit('toggle-select-row', item)"
            />
          </td>
          <td>{{ item.name }}</td>
          <td>{{ getEnumLabel(feeTypeOptions, item.feeType) }}</td>
          <td>{{ item.amount }}</td>
          <td>{{ item.programName }}</td>
          <td>{{ item.className }}</td>
          <td>{{ item.stageName }}</td>
          <td>{{ item.notes }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { FeeTypeOptions, getEnumLabel } from "@/Generic/FinanceEnums";

export default {
  name: "FeeStructureList",
  props: {
    items: {
      type: Array,
      required: true,
    },
    selectedIds: {
      type: Array,
      default: () => [],
    },
  },
  data() {
    return {
      feeTypeOptions: FeeTypeOptions,
    };
  },
  computed: {
    isAllSelected() {
      return (
        this.items.length > 0 &&
        this.items.every((item) =>
          this.selectedIds.includes(item.feeStructureID)
        )
      );
    },
  },
  methods: {
    getEnumLabel,
  },
  emits: ["toggle-select-all", "toggle-select-row"],
};
</script>

<style scoped>
.table-responsive {
  width: 100%;
  overflow-x: auto;
  background: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 6px;
}
.data-table {
  width: 100%;
  border-collapse: collapse;
  text-align: right;
}
.data-table th,
.data-table td {
  padding: 12px;
  border-bottom: 1px solid #eee;
  font-size: 14px;
}
.data-table th {
  background-color: #f8f9fa;
  font-weight: 600;
  color: #333;
}
.checkbox-col {
  width: 40px;
  text-align: center;
}
</style>
