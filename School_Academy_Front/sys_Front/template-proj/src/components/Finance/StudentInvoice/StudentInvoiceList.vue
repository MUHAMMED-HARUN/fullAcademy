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
          <th>الطالب</th>
          <th>الخطة</th>
          <th>تاريخ الإصدار</th>
          <th>تاريخ الاستحقاق</th>
          <th>المبلغ الأصلي</th>
          <th>الخصم</th>
          <th>المتبقي</th>
          <th>الحالة</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in items" :key="item.studentInvoiceID">
          <td class="checkbox-col">
            <input
              type="checkbox"
              :checked="selectedIds.includes(item.studentInvoiceID)"
              @change="$emit('toggle-select-row', item)"
            />
          </td>
          <td>{{ item.studentName }}</td>
          <td>{{ item.feeStructureName }}</td>
          <td>{{ item.issueDate }}</td>
          <td>{{ item.dueDate }}</td>
          <td>{{ item.originalAmount }}</td>
          <td>{{ item.discountAmount }}</td>
          <td>{{ item.remainingAmount }}</td>
          <td>{{ getEnumLabel(statusOptions, item.status) }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { InvoiceStatusOptions, getEnumLabel } from "@/Generic/FinanceEnums";

export default {
  name: "StudentInvoiceList",
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
      statusOptions: InvoiceStatusOptions,
    };
  },
  computed: {
    isAllSelected() {
      return (
        this.items.length > 0 &&
        this.items.every((item) =>
          this.selectedIds.includes(item.studentInvoiceID)
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
