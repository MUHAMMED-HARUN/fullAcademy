<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div v-if="!isEdit">
        <div class="form-filter">
          <label>البحث عن طالب:</label>
          <select v-model="studentSearchType">
            <option :value="enStudentSearchType.NationalNumber">
              بحث بالرقم الوطني
            </option>
            <option :value="enStudentSearchType.ID">بحث بـ ID الطالب</option>
          </select>
          <input v-model="studentSearchValue" type="text" />
          <button @click.prevent.stop="onSearchStudent">بحث عن طالب</button>
        </div>
        <div class="student-card-wrapper">
          <StudentCard v-if="showStudentCard" :-data-req="studentData" />
          <ValidationError :messages="errors?.studentID" />
        </div>
      </div>
      <div v-else class="form-group">
        <label>الطالب:</label>
        <input :value="formData.studentName" type="text" disabled />
      </div>

      <div class="form-group">
        <label>خطة الرسوم:</label>
        <select
          v-model="formData.feeStructureID"
          :disabled="isEdit || lockFeeStructure"
          @change="onFeeStructureChange"
        >
          <option disabled value="">اختر الخطة</option>
          <option
            v-for="plan in feeStructures"
            :key="plan.feeStructureID"
            :value="plan.feeStructureID"
          >
            {{ plan.name }} - {{ plan.amount }}
          </option>
        </select>
        <ValidationError :messages="errors?.feeStructureID" />
      </div>

      <div class="form-group">
        <label>تاريخ الإصدار:</label>
        <input v-model="formData.issueDate" type="date" :disabled="isEdit" />
        <ValidationError :messages="errors?.issueDate" />
      </div>

      <div class="form-group">
        <label>تاريخ الاستحقاق:</label>
        <input v-model="formData.dueDate" type="date" />
        <ValidationError :messages="errors?.dueDate" />
      </div>

      <div class="form-group">
        <label>المبلغ الأصلي:</label>
        <input
          v-model.number="formData.originalAmount"
          type="number"
          min="0"
          step="0.01"
          :disabled="isEdit"
        />
        <ValidationError :messages="errors?.originalAmount" />
      </div>

      <div class="form-group">
        <label>الخصم:</label>
        <input
          v-model.number="formData.discountAmount"
          type="number"
          min="0"
          step="0.01"
        />
        <ValidationError :messages="errors?.discountAmount" />
      </div>

      <div class="form-group" v-if="isEdit">
        <label>الحالة:</label>
        <select v-model.number="formData.status">
          <option
            v-for="opt in statusOptions"
            :key="opt.value"
            :value="opt.value"
          >
            {{ opt.label }}
          </option>
        </select>
        <ValidationError :messages="errors?.status" />
      </div>

      <div class="form-group" v-if="isEdit">
        <label>المتبقي:</label>
        <input :value="formData.remainingAmount" type="text" disabled />
      </div>

      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.notes" type="text" />
        <ValidationError :messages="errors?.notes" />
      </div>

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import StudentCard from "@/components/Person/Student/StudentCard.vue";
import { useStudentInvoiceStore } from "@/store/Finance/StudentInvoiceStore";
import { InvoiceStatus, InvoiceStatusOptions } from "@/Generic/FinanceEnums";

const enStudentSearchType = Object.freeze({ ID: 1, NationalNumber: 3 });

export default {
  name: "UpsertStudentInvoiceComp",
  components: {
    ValidationError,
    StudentCard,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
    studentData: {
      type: Object,
      default: () => ({}),
    },
    feeStructures: {
      type: Array,
      default: () => [],
    },
    initialInvoice: {
      type: Object,
      default: null,
    },
    lockFeeStructure: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      StudentInvoiceStore: useStudentInvoiceStore(),
      enStudentSearchType,
      studentSearchType: enStudentSearchType.NationalNumber,
      studentSearchValue: "",
      showStudentCard: false,
      statusOptions: InvoiceStatusOptions,
      formData: {
        studentInvoiceID: "",
        studentID: "",
        studentName: "",
        feeStructureID: "",
        issueDate: new Date().toISOString().substring(0, 10),
        dueDate: new Date().toISOString().substring(0, 10),
        originalAmount: 0,
        discountAmount: 0,
        remainingAmount: 0,
        status: InvoiceStatus.Unpaid,
        notes: "",
      },
    };
  },
  emits: ["onSubmit", "onSearchStudent"],
  computed: {
    isEdit() {
      return Boolean(this.formData.studentInvoiceID);
    },
    FormTitle() {
      return this.isEdit ? "تعديل تسجيل الخطة" : "تسجيل طالب في خطة";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    onSearchStudent() {
      this.$emit(
        "onSearchStudent",
        this.studentSearchType,
        this.studentSearchValue
      );
    },
    onFeeStructureChange() {
      if (this.isEdit) return;
      const plan = (this.feeStructures || []).find(
        (item) => item.feeStructureID === this.formData.feeStructureID
      );
      if (plan) {
        this.formData.originalAmount = plan.amount;
      }
    },
    fillForm() {
      const entity =
        this.StudentInvoiceStore.selectedInvoice || this.initialInvoice || null;
      if (!entity) return;
      this.formData = {
        studentInvoiceID: entity.studentInvoiceID || "",
        studentID: entity.studentID || "",
        studentName: entity.studentName || "",
        feeStructureID: entity.feeStructureID || "",
        issueDate:
          entity.issueDate || new Date().toISOString().substring(0, 10),
        dueDate: entity.dueDate || new Date().toISOString().substring(0, 10),
        originalAmount: entity.originalAmount || 0,
        discountAmount: entity.discountAmount || 0,
        remainingAmount: entity.remainingAmount || 0,
        status: entity.status || InvoiceStatus.Unpaid,
        notes: entity.notes || "",
      };
    },
  },
  mounted() {
    this.fillForm();
  },
  watch: {
    studentData: {
      handler(data) {
        if (data && (data.studentID || data.personID)) {
          this.formData.studentID = data.studentID;
          this.showStudentCard = true;
        } else {
          this.showStudentCard = false;
        }
      },
      immediate: true,
      deep: true,
    },
  },
};
</script>
