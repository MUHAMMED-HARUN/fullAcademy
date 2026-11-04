<template>
  <div>
    <div class="card-item">
      <div class="item-name">اسم الخطة:</div>
      <div class="item-value">{{ DataReq?.name }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">نوع الرسوم:</div>
      <div class="item-value">
        {{ getEnumLabel(feeTypeOptions, DataReq?.feeType) }}
      </div>
    </div>
    <div class="card-item">
      <div class="item-name">المبلغ:</div>
      <div class="item-value">{{ DataReq?.amount }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">البرنامج:</div>
      <div class="item-value">{{ DataReq?.programName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الصف:</div>
      <div class="item-value">{{ DataReq?.className }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">المرحلة:</div>
      <div class="item-value">{{ DataReq?.stageName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">ملاحظات:</div>
      <div class="item-value">{{ DataReq?.notes }}</div>
    </div>
  </div>
</template>

<script>
import { FeeTypeOptions, getEnumLabel } from "@/Generic/FinanceEnums";

export default {
  name: "FeeStructureCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      feeTypeOptions: FeeTypeOptions,
    };
  },
  methods: {
    getEnumLabel,
  },
};
</script>
