<script setup>
import {
  AcademicPlanScopeTypeOptions,
  AcademicPlanStatusOptions,
} from "@/Generic/AcademicPlanEnums";
import { getEnumLabel } from "@/Generic/FinanceEnums";

defineProps({
  plan: {
    type: Object,
    required: true,
  },
});
</script>

<template>
  <div>
    <div class="card-item">
      <div class="item-name">اسم الخطة:</div>
      <div class="item-value">{{ plan?.name }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">المؤسسة:</div>
      <div class="item-value">{{ plan?.providerInstituteName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">نوع النطاق:</div>
      <div class="item-value">
        {{ getEnumLabel(AcademicPlanScopeTypeOptions, plan?.scopeType) }}
      </div>
    </div>
    <div class="card-item">
      <div class="item-name">النطاق:</div>
      <div class="item-value">{{ plan?.scopeName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الإصدار:</div>
      <div class="item-value">{{ plan?.version }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الحالة:</div>
      <div class="item-value">
        {{ getEnumLabel(AcademicPlanStatusOptions, plan?.status) }}
      </div>
    </div>
    <div class="card-item">
      <div class="item-name">الفترة:</div>
      <div class="item-value">
        {{ plan?.effectiveFrom }} - {{ plan?.effectiveTo || "مفتوحة" }}
      </div>
    </div>
    <div class="card-item">
      <div class="item-name">ملاحظات:</div>
      <div class="item-value">{{ plan?.notes }}</div>
    </div>

    <h3 class="items-title">عناصر الخطة ({{ plan?.items?.length || 0 }})</h3>
    <ul class="items-list">
      <li v-for="item in plan?.items || []" :key="item.stageInfoID">
        {{ item.stageName }} - {{ item.stageInfoName }}
      </li>
    </ul>
  </div>
</template>

<style scoped>
.card-item {
  display: flex;
  gap: 8px;
  padding: 6px 0;
  border-bottom: 1px solid #eee;
}
.item-name {
  font-weight: 600;
  min-width: 120px;
}
.items-title {
  margin-top: 16px;
  font-size: 16px;
}
.items-list {
  padding-inline-start: 20px;
}
</style>
