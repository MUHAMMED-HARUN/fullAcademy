<template>
  <div>
    <div class="card-item">
      <div class="item-name">اسم التعرفة:</div>
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
      <div class="item-name">الخطة الأكاديمية:</div>
      <div class="item-value">
        {{ DataReq?.academicPlanName }} · إصدار
        {{ DataReq?.academicPlanVersion }}
      </div>
    </div>
    <div class="card-item">
      <div class="item-name">المعهد المزوّد:</div>
      <div class="item-value">{{ DataReq?.providerInstituteName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">النطاق:</div>
      <div class="item-value">{{ DataReq?.scopeName }}</div>
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
