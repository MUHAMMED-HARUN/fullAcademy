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
          <th>الطالب / الخطة</th>
          <th>المبلغ</th>
          <th>تاريخ الدفع</th>
          <th>طريقة الدفع</th>
          <th>المرجع</th>
          <th>ملاحظات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in items" :key="item.paymentID">
          <td class="checkbox-col">
            <input
              type="checkbox"
              :checked="selectedIds.includes(item.paymentID)"
              @change="$emit('toggle-select-row', item)"
            />
          </td>
          <td>{{ item.invoiceLabel }}</td>
          <td>{{ item.amount }}</td>
          <td>{{ item.paymentDate }}</td>
          <td>{{ getEnumLabel(paymentMethodOptions, item.paymentMethod) }}</td>
          <td>{{ item.referenceNumber }}</td>
          <td>{{ item.notes }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { PaymentMethodOptions, getEnumLabel } from "@/Generic/FinanceEnums";

export default {
  name: "PaymentList",
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
      paymentMethodOptions: PaymentMethodOptions,
    };
  },
  computed: {
    isAllSelected() {
      return (
        this.items.length > 0 &&
        this.items.every((item) => this.selectedIds.includes(item.paymentID))
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
