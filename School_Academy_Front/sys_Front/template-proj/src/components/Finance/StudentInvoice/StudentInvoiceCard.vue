<template>
  <div>
    <div class="card-item">
      <div class="item-name">الطالب:</div>
      <div class="item-value">{{ DataReq?.studentName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الخطة:</div>
      <div class="item-value">{{ DataReq?.feeStructureName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">تاريخ الإصدار:</div>
      <div class="item-value">{{ DataReq?.issueDate }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">تاريخ الاستحقاق:</div>
      <div class="item-value">{{ DataReq?.dueDate }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">المبلغ الأصلي:</div>
      <div class="item-value">{{ DataReq?.originalAmount }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الخصم:</div>
      <div class="item-value">{{ DataReq?.discountAmount }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">المتبقي:</div>
      <div class="item-value">{{ DataReq?.remainingAmount }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الحالة:</div>
      <div class="item-value">
        {{ getEnumLabel(statusOptions, DataReq?.status) }}
      </div>
    </div>
    <div class="card-item">
      <div class="item-name">ملاحظات:</div>
      <div class="item-value">{{ DataReq?.notes }}</div>
    </div>
  </div>
</template>

<script>
import { InvoiceStatusOptions, getEnumLabel } from "@/Generic/FinanceEnums";

export default {
  name: "StudentInvoiceCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      statusOptions: InvoiceStatusOptions,
    };
  },
  methods: {
    getEnumLabel,
  },
};
</script>
