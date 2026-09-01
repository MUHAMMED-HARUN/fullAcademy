<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">الاسم الاول:</div>
      <div class="item-value">{{ this.DataReq?.firstName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم الأب:</div>
      <div class="item-value">{{ this.DataReq?.fatherName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الكنية:</div>
      <div class="item-value">{{ this.DataReq?.lastName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم الأم:</div>
      <div class="item-value">{{ this.DataReq?.motherName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">كنية الأم:</div>
      <div class="item-value">{{ this.DataReq?.motherLastName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الرقم الوطني:</div>
      <div class="item-value">{{ this.DataReq?.nationalNumber }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الجنس:</div>
      <div class="item-value">{{ this.DataReq?.gender }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">تاريخ الميلاد:</div>
      <div class="item-value">{{ this.DataReq?.birthDate }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">مكان الولادة:</div>
      <div class="item-value">{{ this.DataReq?.placeOfBirth }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">مكان الاقامة:</div>
      <div class="item-value">{{ this.DataReq?.livingPlace }}</div>
    </div>
  </div>
</template>
<script>
export default {
  name: "PersonCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
