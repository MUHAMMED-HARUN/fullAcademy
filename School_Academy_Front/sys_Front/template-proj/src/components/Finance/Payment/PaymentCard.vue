<template>
  <div>
    <div class="card-item">
      <div class="item-name">الفاتورة:</div>
      <div class="item-value">
        {{ DataReq?.invoiceLabel || DataReq?.studentInvoiceID }}
      </div>
    </div>
    <div class="card-item">
      <div class="item-name">المبلغ:</div>
      <div class="item-value">{{ DataReq?.amount }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">تاريخ الدفع:</div>
      <div class="item-value">{{ DataReq?.paymentDate }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">طريقة الدفع:</div>
      <div class="item-value">
        {{ getEnumLabel(paymentMethodOptions, DataReq?.paymentMethod) }}
      </div>
    </div>
    <div class="card-item">
      <div class="item-name">المرجع:</div>
      <div class="item-value">{{ DataReq?.referenceNumber }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">ملاحظات:</div>
      <div class="item-value">{{ DataReq?.notes }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الحالة:</div>
      <div class="item-value">{{ DataReq?.isReversed ? "معكوسة" : "سارية" }}</div>
    </div>
    <div v-if="DataReq?.isReversed" class="card-item">
      <div class="item-name">سبب العكس:</div>
      <div class="item-value">{{ DataReq?.reversalReason }}</div>
    </div>
    <div v-if="DataReq?.isReversed" class="card-item">
      <div class="item-name">تاريخ العكس:</div>
      <div class="item-value">{{ DataReq?.reversedAt }}</div>
    </div>
  </div>
</template>

<script>
import { PaymentMethodOptions, getEnumLabel } from "@/Generic/FinanceEnums";

export default {
  name: "PaymentCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      paymentMethodOptions: PaymentMethodOptions,
    };
  },
  methods: {
    getEnumLabel,
  },
};
</script>
